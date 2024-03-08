using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RevitOutOfContext_gRPC_ProtosF;

namespace GrpcServerConsole.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly Greeter.GreeterClient _client;

        public GreeterService(Greeter.GreeterClient client)
        {
            _client = client;
        }

        public override Task<CommandReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            string unicId = $"{request.RevitVersion} {request.ProcesId} {request.IpAdress}";
            string text = $"{request.Name} {request.Text}";
            Common.clientCollection.Add(unicId, text);
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
    }
}