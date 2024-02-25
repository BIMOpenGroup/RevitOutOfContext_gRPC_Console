using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using RevitOutOfContext_gRPC_ProtosF;

namespace GrpcServerConsole.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            Console.WriteLine($"{request.Name}: {request.Text}");
            //var test = Console.ReadLine();
            return Task.FromResult(new HelloReply
            {
                Message = "server replay test"
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