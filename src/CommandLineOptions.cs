using CommandLine;

namespace ProgectCodeLines
{
    public class CommandLineOptions
    {
        [Option('d', "directory", HelpText = "Path to the directory to analyze.")]
        public string Directory { get; set; }
    }
}