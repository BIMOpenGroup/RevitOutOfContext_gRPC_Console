using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System.Net.Http;
using Autodesk.Revit.DB.Events;
using RevitOutOfContext_gRPC_ProtosF;
using System.Reflection;
using System.IO;
using GrpcDotNetNamedPipes;
using static System.Net.Mime.MediaTypeNames;
using Google.Protobuf.WellKnownTypes;
using static System.Net.Http.WinHttpHandler;
//using RevitOutOfContext_gRPC_ProtosLocal;

namespace RevitAddinOutOfContext_gRPC_Client
{
    [Transaction(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class RevitGrpcAddinApp : IExternalApplication
    {
        public static UIControlledApplication _uiControlApplication;
        public string _userName;
        public string _version;
        public Result OnStartup(UIControlledApplication uiControlApplication)
        {
            try
            {
                //uiControlApplication.Idling += OnIdling;
                uiControlApplication.ControlledApplication.ApplicationInitialized += OnInitialized;
                _uiControlApplication = uiControlApplication;
                _userName = Environment.UserName;
                _version = _uiControlApplication.ControlledApplication.VersionName;
                //System.AppDomain currentDomain = System.AppDomain.CurrentDomain;
                //Assembly.LoadFrom("C:\\code\\RevitOutOfContext_gRPC\\RevitOutOfContext_gRPC_Client\\RevitAddinOutOfContext_gRPC_Client\\bin\\Debug\\System.Diagnostics.DiagnosticSource.dll");
                //AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += CurrentDomain_AssemblyResolve;
                //AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                //currentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("fail", ex.Message);
                return Result.Cancelled;
            }
        }

        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender,ResolveEventArgs args)
        {
            if (args.Name.Contains("DiagnosticSource"))
            {
                string filename = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                filename = Path.Combine(filename, "System.Diagnostics.DiagnosticSource.dll");

                if (File.Exists(filename))
                {
                    return System.Reflection.Assembly.LoadFrom(filename);
                }
            }
            return null;
        }

        System.Reflection.Assembly currentDomain_AssemblyResolve(object sender,ResolveEventArgs args)
        {
            return Assembly.LoadFrom("C:\\code\\RevitOutOfContext_gRPC\\RevitOutOfContext_gRPC_Client\\RevitAddinOutOfContext_gRPC_Client\\bin\\Debug\\System.Diagnostics.DiagnosticSource.dll");
        }

        public async void OnInitialized(object sender, ApplicationInitializedEventArgs e)
        {
            try
            {
                var handler = new GrpcWebHandler(new HttpClientHandler());
                var options = new GrpcChannelOptions
                {
                    HttpHandler = handler,
                };
                options.UnsafeUseInsecureChannelCallCredentials = true;
                var channel = GrpcChannel.ForAddress("http://localhost:5064", options);
                //var channel = new NamedPipeChannel(".", "MY_PIPE_NAME");
                var client = new Greeter.GreeterClient(channel);
                var helloRequest = new HelloRequest { Name = $"{_userName} {_version}", Text = $"RevitAddinOutOfContext_gRPC_Client {DateTime.Now}" };
                var reply2 = await client.SayHelloAsync(helloRequest);
                //var reply = client.HearHello(new Empty());
                //Console.WriteLine(reply.Message);
                string test = "";
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Exception", ex.Message);
            }
        }

        public Result OnShutdown(UIControlledApplication uiControlApplication)
        {
            uiControlApplication.Idling -= OnIdling;
            //_serverDispatcher.ReciveData -= OnReciveData;
            return Result.Succeeded;
        }

        private void OnIdling(object sender, IdlingEventArgs e)
        {
            try
            {
                //var channel = GrpcChannel.ForAddress("https://localhost:5064", new GrpcChannelOptions
                //{
                //    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                //});

                //var client = new Greeter.GreeterClient(channel);

                //var userName = Environment.UserName;

                //var text = "Console.ReadLine()";
                //var reply = client.SayHello(
                //                  new HelloRequest { Name = userName, Text = text });

                //var uiapp = (UIApplication)sender;
                //var uidoc = uiapp.ActiveUIDocument;
                //var selection = uidoc?.Selection;
                //var doc = uidoc?.Document;
                //if (selection != null)
                //{
                //    var elemsIds = selection.GetElementIds();
                //    if (elemsIds.Count > 0)
                //    {

                //    }
                //}
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Exception", ex.Message);
            }
        }
    }
}
