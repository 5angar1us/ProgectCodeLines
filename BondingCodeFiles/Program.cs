using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProgectCodeLines
{
    class Program
    {
        static void Main(string[] args)
        {
            MainWork();

            Console.ReadKey();
        }

        private static void MainWork()
        {
            string folderPath = $@"C:\1.Developing\С#\ИнстрСредства ИС\PrimaryDataAnalysis7";
            string outputFileName = "Code of progect.rtf";
            string outputPath = Path.Combine(folderPath, outputFileName);
            string extensionPattern = "*.cs";

            List<string> ignoreList = new List<string>()
            {
                @"\Debug\",
                "Properties",
                ".Designer.",
            };

            var files = Directory.GetFiles(folderPath, extensionPattern, SearchOption.AllDirectories).ToList();

            files.RemoveAll(file => ignoreList.Exists(ignore => file.Contains(ignore)));

            FindCodeLinesCount(files);
        }

        private static void FindCodeLinesCount(List<string> files)
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
            Console.WriteLine($"В проекте {progectCodeLines} строк");
        }
    }
}
