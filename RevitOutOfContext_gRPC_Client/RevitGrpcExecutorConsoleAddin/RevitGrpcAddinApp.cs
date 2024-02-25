using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcGreeterClient;
using System.Net.Http;

namespace RevitGrpcExecutorConsoleAddin
{
    [Transaction(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class RevitGrpcAddinApp : IExternalApplication
    {
        public static UIControlledApplication _uiControlApplication;
        public string _userName;
        public Result OnStartup(UIControlledApplication uiControlApplication)
        {
            try
            {
                uiControlApplication.Idling += OnIdling;
                _uiControlApplication = uiControlApplication;
                _userName = Environment.UserName;
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("fail", ex.Message);
                return Result.Cancelled;
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
                var channel = GrpcChannel.ForAddress("https://localhost:5064", new GrpcChannelOptions
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });

                var client = new Greeter.GreeterClient(channel);

                var userName = Environment.UserName;

                var text = "Console.ReadLine()";
                var reply = client.SayHello(
                                  new HelloRequest { Name = userName, Text = text });

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
