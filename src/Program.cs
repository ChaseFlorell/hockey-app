using System.Reflection;
using HockeyApp.Modules;

namespace HockeyApp
{
    internal class Program
    {
        private static string _executableName;
        public static string ExecutableName => _executableName ?? (_executableName = $"{Assembly.GetExecutingAssembly().GetName().Name}.exe");

        private static void Main(string[] args)
        {
            var chain = new ChainBuilder()
                .AddModule<HelpModule>()
                .AddModule<UploadModule>()
                .Build();
            chain.Run(args).Wait(); // one day we will be allowed to await in Main, but not today :(
        }
    }
}