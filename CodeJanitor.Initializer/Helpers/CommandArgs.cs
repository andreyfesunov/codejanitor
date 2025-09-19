namespace CodeJanitor.Initializer.Helpers;

public sealed class CommandArgs
{
    public string? Command { get; set; }
    public string? CommandArg { get; set; }
    public string Environment { get; set; } = "Development";

    public static CommandArgs Parse(IEnumerable<string> args)
    {
        var res = new CommandArgs();
        using var iterator = args.GetEnumerator();
        while (iterator.MoveNext())
        {
            var arg = iterator.Current;
            if (arg.StartsWith("--"))
            {
                switch (arg.ToLower())
                {
                    case "--env":
                        iterator.MoveNext();
                        res.Environment = iterator.Current;
                        break;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(res.Command))
                    res.Command = arg;
                else if (string.IsNullOrWhiteSpace(res.CommandArg)) res.CommandArg = arg;
            }
        }

        return res;
    }
}