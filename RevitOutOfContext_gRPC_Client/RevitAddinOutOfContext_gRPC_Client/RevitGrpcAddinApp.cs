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
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Threading;
//using RevitOutOfContext_gRPC_ProtosLocal;

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
        bool _requestRun = true;
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
                var handler = new GrpcWebHandler(new HttpClientHandler());
                var options = new GrpcChannelOptions
                {
                    HttpHandler = handler,
                };
                options.UnsafeUseInsecureChannelCallCredentials = true;
                var channel = GrpcChannel.ForAddress("http://localhost:5064", options);
                //var channel = new NamedPipeChannel(".", "MY_PIPE_NAME");
                _client = new Greeter.GreeterClient(channel);

                HelloToServer();
                _uiControlApplication.Idling += OnIdling;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Exception", ex.Message);
            }
        }

        async void HelloToServer()
        {
            try
            {
                new Thread(async () =>
                {
                    _requestRun = false;
                    System.Threading.Thread.Sleep(1000);

                    var procsId = Process.GetCurrentProcess().Id;
                    var helloRequest = new HelloRequest
                    {
                        Name = $"{_userName} {_versionName} {procsId}",
                        Text = $"RevitAddinOutOfContext_gRPC_Client {DateTime.Now}",
                        RevitVersion = _versionNum,
                        ProcesId = procsId.ToString(),
                        IpAdress = GetLocalIPAddress()
                    };
                    var reply2 = await _client.SayHelloAsync(helloRequest);
                    //var reply = client.HearHello(new Empty());
                    //Console.WriteLine(reply.Message);
                    string test = "";
                    _requestRun = true;
                }).Start();
            }
            catch (Exception ex)
            {
                _requestRun = true;
                //TaskDialog.Show("Exception", ex.Message);
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
                if (_requestRun)
                {
                    HelloToServer();
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Exception", ex.Message);
            }
        }
    }
}
