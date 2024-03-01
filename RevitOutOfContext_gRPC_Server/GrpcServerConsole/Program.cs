using GrpcDotNetNamedPipes;
using GrpcServerConsole;
using GrpcServerConsole.Services;
using RevitOutOfContext_gRPC_ProtosF;
using RSNiniManager;
using Spectre.Console;

namespace GrpcServerConsole
{
    class Program
    {
        static LaunchModes _launchModes;
        public static ClientCollection _clientCollection;

        static void Main()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddGrpc();
            builder.Services.AddGrpcClient<Greeter.GreeterClient>(o =>
            {
                o.Address = new Uri("https://localhost:5064");
            });
            var app = builder.Build();
            // Additional configuration is required to successfully run gRPC on macOS.
            // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

            // Add services to the container.


            new Thread(() =>
            {
                app.UseRouting();
                app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
                // Configure the HTTP request pipeline.
                app.MapGrpcService<GreeterService>();
                app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. " +
                            "To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                app.Run();
            }).Start();

            //var server = new NamedPipeServer("MY_PIPE_NAME");
            //Greeter.BindService(server.ServiceBinder, new GreeterService("MY_PIPE_NAME"));
            //server.Start();
            //Console.WriteLine(server.ToString());

            Console.ReadLine();

            _launchModes = new LaunchModes();
            _clientCollection = new ClientCollection();
            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.Markup($"[bold {Colors.mainColor}]GrpcServerConsole[/]\n");
                Console.WriteLine("\n");
                var launchMode = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[underline {Colors.mainColor}]Select launch mode[/]\n")
                        .HighlightStyle(Colors.selectionStyle)
                        .PageSize(5)
                        .AddChoices(new[] { "Run new Revits", "Current Revits", "Remove Revits", "Commands", "Off" })
                    );
                if (launchMode == "Run new Revits")
                {
                    _launchModes.RunRevits();
                }
                else if (launchMode == "Current Revits")
                {
                    _launchModes.CurrentRevits();
                }
                else if (launchMode == "Remove Revits")
                {
                    _launchModes.RemoveRevits();
                }
                else if (launchMode == "Commands")
                {
                    _launchModes.Commands();
                }
                else if (launchMode == "Off")
                {
                    app.StopAsync();
                    app.DisposeAsync();
                    break;
                }
                else
                {
                    Console.WriteLine("no one will see this message if the program is working properly");
                }
            }
        }
    }
}

