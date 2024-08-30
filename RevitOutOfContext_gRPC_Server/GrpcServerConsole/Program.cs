using GrpcServerConsole.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RevitOutOfContext_gRPC_ProtosF;
using RSNiniManager;
using Spectre.Console;
using System.Threading.Tasks;
using static GrpcServerConsole.Common;

namespace GrpcServerConsole
{
    class Program
    {
        static void Main()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddGrpc();
            builder.Services.AddGrpcClient<Greeter.GreeterClient>(o =>
            {
                o.Address = new Uri("https://localhost:5064");
            });
            builder.Services.AddGrpcHealthChecks()
                .AddCheck("Sample", () => HealthCheckResult.Healthy());

            var app = builder.Build();

            Common.clientCollection.CommandSend += PromtCommandSendEvent;

            new Thread(() =>
            {
                app.UseRouting();
                app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
                // Configure the HTTP request pipeline.
                app.MapGrpcService<GreeterService>();
                app.MapGrpcHealthChecksService();
                app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. " +
                            "To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                app.Run();
            }).Start();

            string dbPath = Path.Combine(@"C:\temp\sqlite_from_server", "RevitElements.db");
            dbHelper = new SQLiteHelper(dbPath);
            //var server = new NamedPipeServer("MY_PIPE_NAME");
            //Greeter.BindService(server.ServiceBinder, new GreeterService("MY_PIPE_NAME"));
            //server.Start();
            //Console.WriteLine(server.ToString());
            Console.ReadLine();
            LaunchModes launchModes = new LaunchModes();
            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.Markup($"[bold {Colors.mainColor}]GrpcServerConsole[/]\n");
                Console.WriteLine("\n");
                var launchMode = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title($"[underline {Colors.mainColor}]Select launch mode[/]\n")
                        .HighlightStyle(Colors.selectionStyle)
                        .PageSize(7)
                        .AddChoices(new[] { "Current Revits", "Run Revits", "Commands", "Export Data From Revits", "Exit Revits", "Server off" })
                    );
                if (launchMode == "Current Revits")
                {
                    launchModes.CurrentRevits();
                }
                else if(launchMode == "Run Revits")
                {
                    launchModes.RunRevits();
                }
                else if (launchMode == "Commands")
                {
                    launchModes.Commands();
                }
                else if (launchMode == "Export Data From Revits")
                {
                    launchModes.ExportDataFromRevits();
                }
                else if (launchMode == "Exit Revits")
                {
                    launchModes.RemoveRevits();
                }
                else if (launchMode == "Server off")
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
            Common.clientCollection.CommandSend -= PromtCommandSendEvent;
        }

        static void PromtCommandSendEvent(string message)
        {
            AnsiConsole.Markup($" {message}\n");
        }
    }
}

