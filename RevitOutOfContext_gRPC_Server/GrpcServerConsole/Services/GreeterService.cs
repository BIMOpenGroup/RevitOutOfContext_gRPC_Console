using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RevitOutOfContext_gRPC_ProtosF;

namespace GrpcServerConsole.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private string _name;

        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
            _name = "test";
        }

        public GreeterService(string name)
        {
            _name = name;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            Console.WriteLine($"{request.Name}: {request.Text}");
            //var test = Console.ReadLine();
            return Task.FromResult(new HelloReply
            {
                Message = $"server replay test {DateTime.Now}"
            });
        }

        public override Task<HelloReply> HearHello(Empty request, ServerCallContext context)
        {
            //Console.WriteLine($"{request.Name}: {request.Text}");
            var test = Console.ReadLine();
            return Task.FromResult(new HelloReply
            {
                Message = test
            });
        }
    }
}