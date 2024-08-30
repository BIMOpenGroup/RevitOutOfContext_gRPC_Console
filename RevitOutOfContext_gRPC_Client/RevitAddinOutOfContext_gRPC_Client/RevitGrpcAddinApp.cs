using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Grpc.Health.V1;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using RevitOutOfContext_gRPC_ProtosF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading;
using static Autodesk.AdvanceSteel.Modelling.ProjectData;
using static Grpc.Health.V1.HealthCheckResponse.Types;

namespace RevitAddinOutOfContext_gRPC_Client
{
    [Transaction(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class RevitGrpcAddinApp : IExternalApplication
    {
        public static UIControlledApplication _uiControlApplication;
        public string _userName;
        public string _versionName;
        string _versionNum;
        bool _canRunRequest = true;
        Health.HealthClient _clientHealth;
        Greeter.GreeterClient _client;

        public Result OnStartup(UIControlledApplication uiControlApplication)
        {
            try
            {

                _uiControlApplication = uiControlApplication;
                _uiControlApplication.ControlledApplication.ApplicationInitialized += OnInitialized;

                _userName = Environment.UserName;
                _versionName = _uiControlApplication.ControlledApplication.VersionName;
                _versionNum = _uiControlApplication.ControlledApplication.VersionNumber;
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("fail", ex.Message);
                return Result.Cancelled;
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "No network adapters with an IPv4 address in the system!";
        }

        public async void OnInitialized(object sender, ApplicationInitializedEventArgs e)
        {
            try
            {
                UIApplication uiapp = sender as UIApplication;
                var handler = new GrpcWebHandler(new HttpClientHandler());
                var options = new GrpcChannelOptions
                {
                    HttpHandler = handler,
                };
                options.UnsafeUseInsecureChannelCallCredentials = true;
                var channel = GrpcChannel.ForAddress("http://localhost:5064", options);
                //var channel = new NamedPipeChannel(".", "MY_PIPE_NAME");
                _clientHealth = new Health.HealthClient(channel);
                _client = new Greeter.GreeterClient(channel);

                HelloToServer(uiapp);
                _uiControlApplication.Idling += OnIdling;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Exception", ex.Message);
            }
        }

        async void HelloToServer(UIApplication uiapp)
        {
            try
            {
                CommandReply commandReply = new CommandReply();
                //System.Threading.Thread.Sleep(1000);
                new Thread(async () =>
                {
                    _canRunRequest = false;
                    System.Threading.Thread.Sleep(1000);

                    var procsId = Process.GetCurrentProcess().Id;
                    var helloRequest = new HelloRequest
                    {
                        Name = $"{_userName}",
                        Text = $"{DateTime.Now}",
                        RevitVersion = _versionNum,
                        ProcesId = procsId.ToString(),
                        IpAdress = GetLocalIPAddress()
                    };
                    try
                    {
                        var response = await _clientHealth.CheckAsync(new HealthCheckRequest());
                        var status = response.Status;
                        if (status == ServingStatus.Serving)
                        {
                            commandReply = await _client.SayHelloAsync(helloRequest);
                        }
                    }
                    catch (Grpc.Core.RpcException ex)
                    {

                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Exception", ex.Message);
                    }
                    if (!string.IsNullOrEmpty(commandReply.Command))
                    {
                        if (commandReply.Command == "ExportFileData")
                        {
                            ExportFileData(uiapp);
                        }
                        else
                        {
                            PostableCommand postCommand;
                            bool isFind = System.Enum.TryParse(commandReply.Command, out postCommand);
                            if (isFind)
                            {
                                RevitCommandId id_addin = RevitCommandId.LookupPostableCommandId(postCommand);
                                if (id_addin != null)
                                {
                                    uiapp.PostCommand(id_addin);
                                }
                            }
                        }
                    }
                    //var reply = client.HearHello(new Empty());
                    //Console.WriteLine(reply.Message);
                    string test = "";
                    _canRunRequest = true;
                }).Start();
            }
            catch (Exception ex)
            {
                _canRunRequest = true;
                TaskDialog.Show("Exception", ex.Message);
            }

        }

        private async void ExportFileData(UIApplication uiapp)
        {
            try
            {
                //string dbPath = Path.Combine(@"C:\temp\sqlite", "RevitElements.db");
                //SQLiteHelper dbHelper = new SQLiteHelper(dbPath);
                //dbHelper.InitializeDatabase();

                Document doc = uiapp.ActiveUIDocument.Document;
                Categories cats = doc.Settings.Categories; //ฅ≽^•⩊•^≼ฅ mya
                string catExcept = "Материалы,Виды,Сведения о проекте,Листы";
                //List<ElementId> catsId = new List<ElementId>();
                int catsCount = cats.Size;
                int catCounter = 0;
                foreach (Category cat in cats)
                {
                    catCounter += 1;
                    if (!(catExcept.Contains(cat.Name)))
                    {
                        ICollection<ElementId> elementsIds = collectorByCatId(doc, cat.Id);
                        int elemCount = elementsIds.Count;
                        int elemCounter = 0;
                        if (elemCount > 0)
                        {
                            foreach (ElementId elemId in elementsIds)
                            {
                                Element elem = doc.GetElement(elemId);
                                string unicElemGuid = elem.UniqueId;
                                (string geomParamsString, string dataParamsString) = paramsParser(elem);
                                if (!string.IsNullOrEmpty(geomParamsString))
                                {
                                    //dbHelper.InsertElement(cat.Name, elem.Name, unicElemGuid, geomParamsString, dataParamsString);
                                    var response = await _clientHealth.CheckAsync(new HealthCheckRequest());
                                    var status = response.Status;
                                    if (status == ServingStatus.Serving)
                                    {
                                        elemCounter += 1;
                                        _client.SendElementInfo(new ElementInfoRequest()
                                        {
                                            FileName = doc.Title,
                                            CategoryName = cat.Name,
                                            ElemName = elem.Name,
                                            ElemGuid = unicElemGuid,
                                            GeomParameters = geomParamsString,
                                            DataParameters = dataParamsString,
                                            CatCount = catsCount,
                                            CatCounter = catCounter,
                                            ElemCount = elemCount,
                                            ElemCounter = elemCounter
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
                _client.SendElementInfo(new ElementInfoRequest()
                {
                    FileName = doc.Title,
                    CategoryName = "task complite",
                    ElemName = "task complite",
                    ElemGuid = "",
                    GeomParameters = "",
                    DataParameters = "",
                    CatCount = catsCount,
                    CatCounter = catCounter,
                    ElemCount = 0,
                    ElemCounter = 0
                });
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Exception", ex.Message);
            }
        }

        public static List<ElementId> collectorByCatId(Document doc, ElementId catId) //колектор элементов по категории с отчисткой 
        {
            FilteredElementCollector fcol = new FilteredElementCollector(doc);
            ICollection<ElementId> ids_coll = fcol.OfCategoryId(catId).WhereElementIsNotElementType().ToElementIds();
            List<ElementId> ids_coll_clear = elementLocationTest(ids_coll, doc);
            return ids_coll_clear;
        }

        public static List<ElementId> elementLocationTest(ICollection<ElementId> elementIdCollection, Document doc) //тестируем элементы на физическое присутсвие в модели 
        {
            List<ElementId> testedElements = new List<ElementId>();
            foreach (ElementId elemId in elementIdCollection)
            {
                if (doc.GetElement(elemId).Location != null)
                {
                    testedElements.Add(elemId);
                }
            }
            return testedElements;
        }

        public static (string, string) paramsParser(Element elem)
        {
            List<Parameter> geomParamsList = new List<Parameter>();
            List<Parameter> dataParamsList = new List<Parameter>();
            ParameterSet parameters = elem.Parameters;
            foreach (Parameter p in parameters)
            {
                if (p.Definition.ParameterGroup == BuiltInParameterGroup.PG_GEOMETRY)
                {
                    geomParamsList.Add(p);
                }
                else
                {
                    dataParamsList.Add(p);
                }
            }
            string geomParamsString = paramSToString(geomParamsList);
            string dataParamsString = paramSToString(dataParamsList);
            return (geomParamsString, dataParamsString);
        }

        public static string paramSToString(List<Parameter> param_list)
        {
            var params_dictionary = new Dictionary<string, Parameter>();
            List<string> paramnames = new List<string>();
            List<Parameter> sorted_params = new List<Parameter>();
            List<string> sorted_params_names = new List<string>();
            List<string> sorted_params_values = new List<string>();
            List<ParamForSort> dicts = new List<ParamForSort>();
            foreach (Parameter p in param_list)
            {
                dicts.Add(new ParamForSort { Name = p.Definition.Name, Param = p });
            }
            IEnumerable<ParamForSort> query = dicts.OrderBy(pet => pet.Name);
            foreach (ParamForSort p in query)
            {
                sorted_params_names.Add(p.Name);
                sorted_params_values.Add(p.Param.AsValueString());
            }
            params_dictionary.Clear();
            paramnames.Clear();
            sorted_params.Clear();
            string full_string = string.Join(", ", sorted_params_names.Zip(sorted_params_values, (sort_n, sort_p) => sort_n + ": " + sort_p));
            sorted_params_names.Clear();
            sorted_params_values.Clear();
            return full_string;
        }

        class ParamForSort
        {
            public string Name { get; set; }
            public Parameter Param { get; set; }
        }

        public Result OnShutdown(UIControlledApplication uiControlApplication)
        {
            uiControlApplication.Idling -= OnIdling;
            return Result.Succeeded;
        }

        private void OnIdling(object sender, IdlingEventArgs e)
        {
            try
            {
                UIApplication uiapp = sender as UIApplication;
                if (_canRunRequest)
                {
                    HelloToServer(uiapp);
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Exception", ex.Message);
            }
        }
    }
}
