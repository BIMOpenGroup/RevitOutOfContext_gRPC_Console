using GrpcDotNetNamedPipes;
using GrpcServerConsole.Services;
using RevitOutOfContext_gRPC_ProtosF;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.

//builder.Services.AddGrpc();

//var app = builder.Build();
//app.UseRouting();
//app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
//// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();
//app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. " +
//            "To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
//app.Run();

var server = new NamedPipeServer("MY_PIPE_NAME");
Greeter.BindService(server.ServiceBinder, new GreeterService("MY_PIPE_NAME"));
server.Start();
Console.WriteLine(server.ToString());

Console.ReadLine();