using CommandLine;

namespace ProgectCodeLines
{
   
        public class CommandLineOptions
        {
            [Value(index: 0, Required = true, HelpText = "Progect file Path to analyze.")]
            public string Progect { get; set; }
        }
    
}
