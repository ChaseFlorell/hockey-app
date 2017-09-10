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
        private bool _invalidCommand;
        private string _attemptedCommand;

        public HelpModule() : base(Resx.Help_ModuleKeyword, Resx.Help_ModuleDescription)
        {
        }

        public override bool CanHandle(string command, string[] args)
        {
            _attemptedCommand= command;
            var canHandle = base.CanHandle(command, args);
            var noArgs = !args.Any();
            _invalidCommand = !Commands.ContainsKey(command);
            return canHandle || noArgs || _invalidCommand;
        }

        public override Task<IResponseObject> Handle()
        {
            if (_invalidCommand)
            {
                var defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Invalid Command: {_attemptedCommand}");
                Console.ForegroundColor = defaultColor;
            }

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