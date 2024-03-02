using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RevitOutOfContext_gRPC_ProtosF;

namespace GrpcServerConsole.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        //private readonly ILogger<GreeterService> _logger;
        //private string _name;
        private readonly Greeter.GreeterClient _client;

        public GreeterService(Greeter.GreeterClient client)
        {
            _client = client;
        }

        //public GreeterService(ILogger<GreeterService> logger)
        //{
        //    _logger = logger;
        //    _name = "test";
        //}

        //public GreeterService(string name)
        //{
        //    _name = name;
        //}

        public override Task<CommandReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            string unicId = $"{request.RevitVersion} {request.ProcesId} {request.IpAdress}";
            string text = $"{request.Name} {request.Text}";
            Common.clientCollection.Add(unicId, text);
            //Console.WriteLine($"{request.Name}: {request.Text}");
            //var test = Console.ReadLine(); {request.RevitVersion} {request.ProcesId} {request.IpAdress}
            if (!string.IsNullOrEmpty(Common.currentCommand) && !Common.clientCollection.Get(unicId).commandCondition)
            {
                Common.clientCollection.Update(unicId, true);
                return Task.FromResult(new CommandReply
                {
                    Command = Common.currentCommand
                });
            }
            return Task.FromResult(new CommandReply
            {
                Command = ""
            });
        }

        //public override Task<HelloReply> HearHello(Empty request, ServerCallContext context)
        //{
        //    //Console.WriteLine($"{request.Name}: {request.Text}");
        //    var test = Console.ReadLine();
        //    return Task.FromResult(new HelloReply
        //    {
        //        Message = test
        //    });
        //}

        public override Task<CommandReply> HearCommands(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new CommandReply
            {
                //Message = test
            });
        }
    }
}