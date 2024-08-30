using RSNiniManager;
using Spectre.Console;
using System.Diagnostics;
using static GrpcServerConsole.Common;


namespace GrpcServerConsole
{
    public class LaunchModes
    {

        public void CurrentRevits()
        {
            AnsiConsole.Markup($"[bold {Colors.mainColor}]CurrentRevits[/]");
            AnsiConsole.Status()
                .AutoRefresh(true)
                .Spinner(Spinner.Known.Star)
                .SpinnerStyle(Style.Parse("green bold"))
                .Start("Search clients...", ctx =>
                {
                    while (true)
                    {
                        ctx.Refresh();
                        AnsiConsole.Clear();
                        AnsiConsole.Markup($"[{Colors.infoColor}]press [{Colors.attentionColor}]<esc>[/] to return[/]\n");
                        Thread.Sleep(1000);
                        var allClients = Common.clientCollection.GetCollection();
                        foreach (var client in allClients)
                        {
                            AnsiConsole.MarkupLine($"[{Colors.infoColor}]client ID: {client.Key}[/] client status: {client.Value.response} last command send: {client.Value.commandCondition}");
                        }
                        // Simulate some work
                        //AnsiConsole.MarkupLine("Doing some work...");
                        ctx.Status("To refresh clients status press any key");
                        ctx.Spinner(Spinner.Known.Star);
                        ctx.SpinnerStyle(Style.Parse("green"));

                        try
                        {
                            var cts = new CancellationTokenSource(100);
                            ConsoleKeyInfo readKeyResult = Task.Run(() => Console.ReadKey(true), cts.Token).Result;
                            if (readKeyResult.Key == ConsoleKey.Escape)
                            {
                                break;
                            }
                        }
                        catch (System.AggregateException ex)
                        {

                        }
                    }
                });
        }
        public void RunRevits()
        {
            string path = "C:\\Program Files\\Autodesk\\Revit *\\Revit.exe";
            Process.Start(path.Replace("*", "2022"));
            Process.Start(path.Replace("*", "2023"));
            return;
        }
        public void RemoveRevits()
        {
            Common.ChangeCommand("ExitRevit");
        }
        public void ExportDataFromRevits()
        {
            if (Common.clientCollection != null && Common.clientCollection.GetCollection().Count > 0)
            {
                AnsiConsole.Markup($"[{Colors.infoColor}]press [{Colors.attentionColor}]<esc>[/] to return[/]\n");
                Common.ChangeCommand("ExportFileData");
                Common.progressUpdater = new ProgressUpdater();
                var progresDescription = new TaskDescriptionColumn();
                progresDescription.Alignment = Justify.Left;
                AnsiConsole.Progress()
                //.AutoRefresh(false) // Turn off auto refresh
                .AutoClear(false)   // Do not remove the task list when done
                .HideCompleted(false)   // Hide tasks as they are completed
                .Columns(new ProgressColumn[]
                {
                    progresDescription,    // Task description
                    //new ProgressBarColumn(),        // Progress bar
                    //new PercentageColumn(),         // Percentage
                    //new RemainingTimeColumn(),      // Remaining time
                    //new SpinnerColumn(),            // Spinner
                })
                .Start(ctx =>
                {
                    var task1 = ctx.AddTask("[green]Обработка категорий[/]");
                    var task2 = ctx.AddTask("[yellow]Обработка елементов[/]");

                    progressUpdater.OnCatCounterUpdated += (currentItem, catName) =>
                    {
                        task1.Increment(1);
                        task1.Description = $"[green]Обработка категории {catName}: {currentItem} из {progressUpdater.catCount}[/]";
                    };

                    progressUpdater.OnElemCounterUpdated += (currentItem, elemName) =>
                    {
                        task2.Increment(1);
                        task2.Description = $"[blue]Обработка элемента {elemName} {currentItem} из {progressUpdater.elemCount}[/]";
                    };
                    while (true)
                    {
                        Thread.Sleep(500);
                        try
                        {
                            var cts = new CancellationTokenSource(100);
                            ConsoleKeyInfo readKeyResult = Task.Run(() => Console.ReadKey(true), cts.Token).Result;
                            if (readKeyResult.Key == ConsoleKey.Escape)
                            {
                                break;
                            }
                        }
                        catch (System.AggregateException ex)
                        {

                        }
                    }
                });
            }
        }
        public void Commands()
        {
            //AnsiConsole.Clear();
            AnsiConsole.Markup($"[bold {Colors.mainColor}]Commands[/] [{Colors.attentionColor}]https://www.revitapidocs.com/2017/f6ccdc1b-6ac3-9c49-d0bb-8a7d1877eab0.htm[/]\n");
            AnsiConsole.Markup($"[{Colors.infoColor}]Promt command from PostableCommand [{Colors.attentionColor}]<esc>[/] to return[/]\n");
            while (true)
            {
                string newCommand = Utils.ReadLineOrEsc();
                if (string.IsNullOrEmpty(newCommand))
                {
                    break;
                }
                else
                {
                    Common.ChangeCommand(newCommand);
                    AnsiConsole.Markup($"Command addet: [{Colors.selectionColor}]{newCommand}[/]\n");
                }
            }
            Console.WriteLine("\n");
        }
    }
}
