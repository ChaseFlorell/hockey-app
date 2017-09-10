using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using HockeyApp.Models;
using Mono.Options;
using Resx = HockeyApp.Properties.Resources;

namespace HockeyApp.Modules
{
    public abstract class Module : IModule
    {
        protected static readonly Dictionary<string, string> Commands = new Dictionary<string, string>();
        private readonly string _keyWord;
        private bool _showHelp;

        protected Module(string command, string description)
        {
            _keyWord = command;
            Commands.Add(command.ToLower(), description);
            OptionSet = new OptionSet();
        }

        protected OptionSet OptionSet { get; set; }

        public IModule Successor { get; set; }

        public abstract Task<IResponseObject> Handle();

        public virtual bool CanHandle(string command, string[] args)
        {
            return command.ToLower() == _keyWord;
        }

        public IModule SetSuccessor(IModule successor)
        {
            if (Successor == null)
                Successor = successor;
            else
                Successor.SetSuccessor(successor);

            return this;
        }

        public async Task Run(string[] args)
        {
            var key = args.FirstOrDefault() ?? "";
            try
            {
                if (CanHandle(key, args))
                {
                    await HandleInternal(args.Skip(1).ToArray());
                }
                else
                {
                    if (Successor != null)
                    {
                        await Successor.Run(args);
                    }
                }
            }
            catch (Exception ex)
            {
                var defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = defaultColor;
                Console.WriteLine(Resx.Help_Title);
                Console.WriteLine(Resx.Help_ModuleHelp, Program.ExecutableName, key);
                Console.WriteLine();
                Environment.ExitCode = ExitCodes.CommandFailed;
            }
        }

        protected void AddDefaultOptions()
        {
            // Defaults for all option sets.
            OptionSet.Add(Resx.Help_Keyword, Resx.Help_Description, v => _showHelp = v != null);
        }

        private async Task HandleInternal(IEnumerable<string> args)
        {
            OptionSet.Parse(args);
            if (_showHelp)
            {
                ShowHelpForHandler();
            }
            else
            {
                var timer = new Stopwatch();
                if (_keyWord != Resx.Help_ModuleKeyword)
                {
                    timer.Start();
                    Console.WriteLine(Resx.Module_Begin, _keyWord);
                }
                var response = await Handle();
                response?.ToConsole();
                if (_keyWord != Resx.Help_ModuleKeyword)
                {
                    timer.Stop();
                    Console.WriteLine(Resx.Common_Divider);
                    Console.WriteLine(Resx.Module_Duration, timer.Elapsed);
                }
            }
        }

        private void ShowHelpForHandler()
        {
            Console.WriteLine();
            Console.WriteLine(Resx.Help_UsageTitle);
            Console.WriteLine(Resx.Help_ModuleUsage, Program.ExecutableName, _keyWord);
            Console.WriteLine(Resx.Help_OptionsTitle);
            OptionSet.WriteOptionDescriptions(Console.Out);
        }
    }
}