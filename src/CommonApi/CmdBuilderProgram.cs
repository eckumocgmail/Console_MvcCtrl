using eckumoc.Services;

using eckumoc_netcore_cmd_builder.ConsoleCmdBuilder;

using Newtonsoft.Json;

using System;
using System.Reflection;
public class CommandBuilderProgram
{


    public static void Run(string[] args)
    {

        if (args.Length == 0)
        {
            Run(new string[] {
                "/S",
                "/K",
                @"-USERNAME=""ADMIN""",
                @"-PASSWORD=""ADMIN"""
            });
        }
        else
        {
            Run(CommandBuilder.Get("todo", args));
        }
    }

    public static void Run(ICommandBuilder commandBuilder)
    {
        var cmd = new CMD();
        Console.WriteLine(cmd.CmdExec(commandBuilder.ToString()));
    }
}









