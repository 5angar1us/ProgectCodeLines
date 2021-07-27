using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;

namespace ProgectCodeLines
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
           
            return await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(async (CommandLineOptions opts) =>
                {
                    return Run(opts.Progect);
                },
                errs => Task.FromResult(-1)); // Invalid arguments
        }

        private static int Run(string inputFolderPath)
        {
            if (Directory.Exists(inputFolderPath) == false)
            {
                Console.WriteLine("This path does not exist");
                return -1;
            }

            const string extensionPattern = "*.cs";

            List<string> ignoredItems = new List<string>()
            {
                @"\Debug\",
                "Properties",
                ".Designer.",
            };

            List<string> files = Directory.GetFiles(inputFolderPath, extensionPattern, SearchOption.AllDirectories).ToList();

            files.RemoveAll(file => ignoredItems.Any(ignore => file.Contains(ignore)));

            int codeLinesCount = FindCodeLinesCount(files);

            Console.WriteLine($"The project contains {codeLinesCount} lines");

            return 0;
        }

        private static int FindCodeLinesCount(List<string> files)
        {
            int progectCodeLines = 0;
            foreach (var s in files)
            {
                using (var sr = new StreamReader(s))
                {

                    int linesCounter = 0;
                    string temp;
                    while (!sr.EndOfStream)
                    {
                        temp = sr.ReadLine();
                        linesCounter++;
                    }
                    progectCodeLines += linesCounter;
                }
            }
            return progectCodeLines;
        }
    }
}
