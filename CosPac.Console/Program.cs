using CommandLine;
using CosPac.Console.Options;
using System.Threading.Tasks;

namespace CosPac.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var launcher = new Launcher();
            await Parser.Default.ParseArguments<SauvegardeOptions>(args)
                    .MapResult(
                        (SauvegardeOptions opts) => launcher.Backup(opts),
                      errs => Task.FromResult(1));
        }
    }
}
