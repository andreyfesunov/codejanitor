namespace CodeJanitor.Initializer.Helpers;

public sealed class CommandExecutor(MigrationExecutor migrationExecutor)
{
    public async Task RunAsync(string[] args)
    {
        var parsedArgs = CommandArgs.Parse(args);

        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();
        Console.WriteLine("Welcome to initializer!");

        if (!string.IsNullOrEmpty(parsedArgs.Command))
        {
            var displayCommand = parsedArgs.CommandArg != string.Empty
                ? $"{parsedArgs.Command} {parsedArgs.CommandArg}"
                : $"{parsedArgs.Command}";

            try
            {
                ConsoleHelper.WriteLine(ConsoleColor.DarkGreen, $"Command '{displayCommand}' started.");
                await Execute(parsedArgs);
                ConsoleHelper.WriteLine(ConsoleColor.DarkGreen, $"Command '{displayCommand}' completed.");
            }
            catch (Exception e)
            {
                ConsoleHelper.WriteLine(ConsoleColor.DarkRed, $"Exception during command '{displayCommand}': {e}.");

                throw;
            }

            return;
        }

        while (await ProcessCommandInput())
        {
        }

        Console.Write("Goodbye...");
    }

    private async Task<bool> ProcessCommandInput()
    {
        WriteCommandDescriptions();

        string? commandText = null;

        while (string.IsNullOrWhiteSpace(commandText))
        {
            Console.Write("> ");
            commandText = Console.ReadLine();
        }

        var parts = commandText.Split(" ");

        var parsedArgs = new CommandArgs
        {
            Command = parts[0]
        };

        if (parts.Length > 1) parsedArgs.CommandArg = parts[1];

        var displayCommand = string.IsNullOrEmpty(parsedArgs.CommandArg)
            ? $"{parsedArgs.Command}"
            : $"{parsedArgs.Command} {parsedArgs.CommandArg}";

        try
        {
            var continueRun = await Execute(parsedArgs);
            ConsoleHelper.WriteLine(ConsoleColor.DarkGreen, $"Command '{displayCommand}' completed.");

            return continueRun;
        }
        catch (Exception e)
        {
            ConsoleHelper.WriteLine(ConsoleColor.DarkRed, $"Exception during command '{displayCommand}': {e}.");
        }

        return true;
    }

    private static void WriteCommandDescriptions()
    {
        Console.WriteLine("---------------------###---------------------");
        Console.WriteLine();
        ConsoleHelper.Write(ConsoleColor.DarkGreen, " migrate ");
        Console.WriteLine("-> Migrate database to latest version");
        Console.WriteLine();
        ConsoleHelper.Write(ConsoleColor.DarkGreen, " 0 ");
        Console.WriteLine("-> Exit");
    }


    private async Task<bool> Execute(CommandArgs args)
    {
        switch (args.Command)
        {
            case "migrate":
                await migrationExecutor.RunAsync();
                return true;
            case "0":
                return false;
            default:
                ConsoleHelper.WriteLine(ConsoleColor.DarkYellow, $"Unknown command '{args.Command}'.");
                return true;
        }
    }
}