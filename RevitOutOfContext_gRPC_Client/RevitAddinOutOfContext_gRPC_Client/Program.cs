using Grpc.Net.Client;
using Grpc.Net.Client.Web;
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
                //var loggerFactory = LoggerFactory.Create(logging =>
                //{
                //    logging.SetMinimumLevel(LogLevel.Debug);
                //});
                var options = new GrpcChannelOptions
                {
                    HttpHandler = handler,
                    //LoggerFactory = loggerFactory
                    //Credentials = ChannelCredentials.Create(ChannelCredentials.Insecure, credentials)
                };
                options.UnsafeUseInsecureChannelCallCredentials = true;
                var channel = GrpcChannel.ForAddress("http://localhost:5064", options);

                var client = new Greeter.GreeterClient(channel);
                //var reply = client.HearHello(new Empty());
                //Console.WriteLine(reply.Message);
                var userName = Environment.UserName;

                var text = Console.ReadLine();
                var helloRequest = new HelloRequest { Name = userName, Text = text };
                var reply2 = client.SayHello(helloRequest);
                Console.WriteLine(reply2.Message);
                var readKey = Console.ReadKey();
                var key = readKey.Key.ToString();
                if (key == "Q")
                {
                    return;
                }
            }
        }

        static async Task<HelloReply> GetReply(Greeter.GreeterClient client, HelloRequest helloRequest)
        {
            var reply = await client.SayHelloAsync(helloRequest);
            return reply;
        }
    }
}
