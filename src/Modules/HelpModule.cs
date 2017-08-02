using System;
using System.Linq;
using System.Threading.Tasks;
using HockeyApp.Models;
using HockeyApp.Utils;
using Resx = HockeyApp.Properties.Resources;

namespace HockeyApp.Modules
{
    public class HelpModule : Module
    {
        public HelpModule() : base(Resx.Help_ModuleKeyword, Resx.Help_ModuleDescription)
        {
        }

        public override bool CanHandle(string command, string[] args)
        {
            var canHandle = base.CanHandle(command, args);
            var noArgs = !args.Any();
            var invalidCommand = !Commands.ContainsKey(command);
            return canHandle || noArgs || invalidCommand;
        }

        public override Task<IResponseObject> Handle()
        {
            Console.WriteLine();
            Console.WriteLine(Resx.Help_UsageTitle);
            Console.WriteLine(Resx.Help_CommandExample, Program.ExecutableName);
            Console.WriteLine();
            var builder = new TableBuilder();
            builder.AddTitleRow(Resx.Common_Command, Resx.Common_Description);
            foreach (var command in Commands)
                builder.AddRow(command.Key, command.Value);

            builder.WriteToConsole(Console.Out);
            return Task.FromResult(default(IResponseObject));
        }
    }
}