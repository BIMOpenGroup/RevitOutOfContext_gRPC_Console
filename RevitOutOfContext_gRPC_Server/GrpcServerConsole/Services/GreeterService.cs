using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using RevitOutOfContext_gRPC_ProtosF;
using Spectre.Console;

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

        public override Task<CommandReply> SendElementInfo(ElementInfoRequest request, ServerCallContext context)
        {
            if (request.CategoryName != "task complite")
            {
                Common.dbHelper.InsertElement(request.FileName, request.CategoryName, request.ElemName, request.ElemGuid, request.GeomParameters, request.DataParameters);
            }
            Common.progressUpdater.catCount = request.CatCount;
            Common.progressUpdater.elemCount = request.ElemCount;
            Common.progressUpdater.UpdateCatCounter(request.CatCounter, request.CategoryName);
            Common.progressUpdater.UpdateElemCounter(request.ElemCounter, request.ElemName);
            //Common.catCount = request.CatCount;
            //Common.elemCount = request.ElemCount;
            //Common.catCounter = request.CatCounter;
            //Common.elemCounter = request.ElemCounter;

            return Task.FromResult(new CommandReply
            {
                Command = ""
            });
        }
    }
}