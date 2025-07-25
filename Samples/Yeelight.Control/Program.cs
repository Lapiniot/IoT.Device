using OOs.CommandLine;
using Yeelight.Control;

try
{
    switch ((ReadOnlySpan<string>)args)
    {
        case ["discover"] or []:
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
        case ["exec", .. var rest]:
            {
                var (options, _) = ExecCommandArgs.Parse(rest);
                var address = GetOption(options, "address", "Device address: ");
                var method = GetOption(options, "method", "Method: ");
                var @params = GetOption(options, "params", "Params: ");
                Console.WriteLine(await ExecCommand.RunAsync(address, method, @params).ConfigureAwait(false));
            }

            break;
        case ["monitor", .. var rest]:
            using (var cts = new CancellationTokenSource())
            {
                var (options, _) = MonitorCommandArgs.Parse(rest);
                var address = GetOption(options, "address", "Device address: ");
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

static string GetOption(IReadOnlyDictionary<string, string?> options, string name, string promptText)
{
    var option = options.TryGetValue(name, out var value) ? value as string : null;
    while (string.IsNullOrEmpty(option))
    {
        Console.Write(promptText);
        option = Console.ReadLine();
    }

    return option;
}

[Option<string>("address", "address", 'a')]
[Option<string>("method", "method", 'm')]
[Option<string>("params", "params", 'p')]
internal partial struct ExecCommandArgs { }

[Option<string>("address", "address", 'a')]
internal partial struct MonitorCommandArgs { }