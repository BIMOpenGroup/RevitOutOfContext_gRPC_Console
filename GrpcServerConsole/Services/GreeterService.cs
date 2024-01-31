using Grpc.Core;
using GrpcService1;
using Microsoft.Extensions.Logging;

namespace GrpcService1.Services
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
            var test = Console.ReadLine(); 
            return Task.FromResult(new HelloReply
            {
                Message = test
            });
        }
    }
}