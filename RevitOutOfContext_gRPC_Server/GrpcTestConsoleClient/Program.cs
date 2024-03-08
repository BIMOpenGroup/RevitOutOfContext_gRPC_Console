using Grpc.Core;
using Grpc.Net.Client;
using RevitOutOfContext_gRPC_ProtosF;


//The port number must match the port of the gRPC server.
Console.WriteLine("Write some text to send");
Console.WriteLine("Or press Q key to exit...");

while (true)
{
    using var channel = GrpcChannel.ForAddress("http://localhost:5064");
    var client = new Greeter.GreeterClient(channel);
    var userName = Environment.UserName;

    var text = Console.ReadLine();
    CallOptions optionss = new CallOptions();
    CommandReply reply = client.SayHello(new HelloRequest { Name = userName, Text = text }, optionss);
    Console.WriteLine(reply.Command);
    var test = Console.ReadKey();
    var test2 = test.Key.ToString();
    if (test2 == "Q")
    {
        return;
    }
}
