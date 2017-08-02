using System;
using System.Threading;
using System.Threading.Tasks;
// ReSharper disable LocalizableElement

namespace HockeyApp.Utils
{
    public static class Progress
    {
        public static Task Dots(CancellationToken token)
        {
            return Dots(token, 2000);
        }

        public static async Task Dots(CancellationToken token, int msDelay)
        {
            try
            {
                await Task.Run(async () =>
                {
                    var dots = ".";
                    while (!token.IsCancellationRequested)
                    {
                        Console.Write($"\r{dots}");
                        var trimmedLength = dots.Replace("\r", "").Replace("\n", "").Length;
                        if (trimmedLength%Console.WindowWidth == 0)
                        {
                            Console.WriteLine();
                            dots = "";
                        }
                        dots += ".";
                        await Task.Delay(msDelay, token);
                    }
                },
                    token);
            }
            catch { /* gulp - task cancelled*/ }
            finally { Console.WriteLine();} 
        }

        public static Task Spinner(CancellationToken token)
        {
            return Spinner(token, 100);
        }

        public static async Task Spinner(CancellationToken token, int msDelay)
        {
            try
            {
                await Task.Run(async () =>
                {
                    while (!token.IsCancellationRequested)
                    {
                        var arr = new[] {"\\", "|", "/", "-"};
                        foreach (var a in arr)
                        {
                            Console.Write($"\r {a}");
                            await Task.Delay(msDelay, token);
                        }
                    }
                },
                    token);
            }
            catch { /* gulp - task cancelled */ }
            finally { Console.Write("\r   "); }
        }
    }
}