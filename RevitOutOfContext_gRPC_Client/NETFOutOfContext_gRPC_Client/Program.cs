using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using GrpcDotNetNamedPipes;
using RevitOutOfContext_gRPC_ProtosF;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NETFOutOfContext_gRPC_Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Write some text to send");
            Console.WriteLine("Or press Q key to exit...");
            Console.WriteLine("Target framework name: " + AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName);
            while (true)
            {
                var handler = new GrpcWebHandler(new HttpClientHandler());
                var options = new GrpcChannelOptions
                {
                    HttpHandler = handler,
                };
                //options.UnsafeUseInsecureChannelCallCredentials = true;
                var channel = GrpcChannel.ForAddress("http://localhost:5064", options);

                //var channel = new NamedPipeChannel(".", "MY_PIPE_NAME");

                var client = new Greeter.GreeterClient(channel);
                var userName = Environment.UserName;

                //var text = Console.ReadLine();
                var helloRequest = new HelloRequest { Name = userName, Text = $"NETFOutOfContext_gRPC_Client {DateTime.Now}" };
                var reply2 = client.SayHello(helloRequest);
                Console.WriteLine(reply2.Command);
                var readKey = Console.ReadKey();
                var key = readKey.Key.ToString();
                if (key == "Q")
                {
                    return;
                }
            }
        }
    }
}
