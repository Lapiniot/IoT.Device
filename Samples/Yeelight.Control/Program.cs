using OOs.CommandLine;
using Yeelight.Control;
[assembly: Command("discover", IsDefault = true), Command("exec"), Command("monitor")]
[assembly: Option<string>("address", "address", 'a'), Option<string>("method", "method", 'm'), Option<string>("params", "params", 'p')]

var arguments = Arguments.Parse(args, true);

try
{
    switch (arguments.Command)
    {
        case "discover":
            using (var cts = new CancellationTokenSource())
            {
                Console.WriteLine("Searching for Yeelight devices on the LAN...");
                Console.WriteLine("Press any key to exit.");
                Console.WriteLine();
                var runTask = DiscoverCommand.RunAsync(new ReportSsdpReplyObserver(), cts.Token);
                var cancelTask = CancelOnUserInputAsync(cts);
                await Task.WhenAny(runTask, cancelTask).ConfigureAwait(false);
                await runTask.ConfigureAwait(false);
            }

            break;
        case "exec":
            {
                var address = GetArgument(arguments, "address", "Device address: ");
                var method = GetArgument(arguments, "method", "Method: ");
                var @params = GetArgument(arguments, "params", "Params: ");
                Console.WriteLine(await ExecCommand.RunAsync(address, method, @params).ConfigureAwait(false));
            }

            break;
        case "monitor":
            using (var cts = new CancellationTokenSource())
            {
                var address = GetArgument(arguments, "address", "Device address: ");
                Console.WriteLine("Listening for device prop change notifications...");
                Console.WriteLine("Press any key to exit.");
                var runTask = MonitorCommand.RunAsync(address, new ReportPropChangeObserver(), cts.Token);
                var cancelTask = CancelOnUserInputAsync(cts);
                await Task.WhenAny(runTask, cancelTask).ConfigureAwait(false);
                await runTask.ConfigureAwait(false);
            }

            break;
    }
}
catch (OperationCanceledException) { }
#pragma warning disable CA1031 // Do not catch general exception types
catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
{
    var current = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(e.Message);
    Console.ForegroundColor = current;
}

static async Task CancelOnUserInputAsync(CancellationTokenSource cts)
{
    Console.ReadKey();
    await cts.CancelAsync().ConfigureAwait(false);
}

static string GetArgument(Arguments arguments, string name, string promptText)
{
    var argument = arguments.Options.TryGetValue(name, out var value) ? value as string : null;
    while (string.IsNullOrEmpty(argument))
    {
        Console.Write(promptText);
        argument = Console.ReadLine();
    }

    return argument;
}