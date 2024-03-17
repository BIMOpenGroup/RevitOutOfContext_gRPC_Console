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
            string res = $"{request.Name} {request.Text}";
            Common.clientCollection.TryAdd(unicId, res);
            var clientCond = Common.clientCollection.Get(unicId);
            if (!string.IsNullOrEmpty(clientCond.command) && clientCond.commandCondition)
            {
                Common.clientCollection.Update(unicId, false);
                return Task.FromResult(new CommandReply
                {
                    Command = clientCond.command
                });
            }
            return Task.FromResult(new CommandReply
            {
                Command = ""
            });
        }
    }
}