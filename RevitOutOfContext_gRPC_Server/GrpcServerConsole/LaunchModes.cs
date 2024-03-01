using RSNiniManager;
using Spectre.Console;
using System.Diagnostics;

namespace GrpcServerConsole
{
    public class LaunchModes
    {
        public void RunRevits() 
        {
            string path = "C:\\Program Files\\Autodesk\\Revit *\\Revit.exe";
            Process.Start(path.Replace("*", "2022"));
            Process.Start(path.Replace("*", "2023"));
        }
        public void CurrentRevits() {
            AnsiConsole.Markup($"[bold {Colors.mainColor}]CurrentRevits[/]");
            AnsiConsole.Status()
                .AutoRefresh(true)
                .Spinner(Spinner.Known.Star)
                .SpinnerStyle(Style.Parse("green bold"))
                .Start("Thinking...", ctx =>
                {
                    while (true)
                    {
                        AnsiConsole.Clear();
                        var allClients = Program._clientCollection.GetCollection();
                        foreach (var client in allClients)
                        {
                            AnsiConsole.MarkupLine($"{client.Key} {client.Value}\n");
                        }
                        // Simulate some work
                        //AnsiConsole.MarkupLine("Doing some work...");
                        ctx.Status("Thinking some more");
                        ctx.Spinner(Spinner.Known.Star);
                        ctx.SpinnerStyle(Style.Parse("green"));
                        Thread.Sleep(2000);
                        ctx.Refresh();
                    }
                });
            //AnsiConsole.Progress()
            //.Start(ctx =>
            //{
            //    var allClients = Program._clientCollection.GetCollection();
            //    // Define tasks
            //    var task1 = ctx.AddTask("[green]Reticulating splines[/]");
            //    var task2 = ctx.AddTask("[green]Folding space[/]");

            //    while (!ctx.IsFinished)
            //    {
            //        task1.Increment(1.5);
            //        task2.Increment(0.5);
            //    }
            //});
        }
        public void RemoveRevits() { }
        public void Commands() { }
    }
}
