using CommandLine;
using ProgectCodeLines.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProgectCodeLines
{

    class Program
    {
        private static HashSet<string> ExtensionPatterns = new List<string>() { ".cs", ".ts" }.ToHashSet();

        private static HashSet<string> ignoredFolders = new List<string>()
        {
            @"Debug",
            ".git",
            "Properties",
            ".Designer.",
            "Build",
            "dist",
            "node_modules"
        }.ToHashSet();

        static async Task<int> Main(string[] args)
        {
            return await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(async (CommandLineOptions opts) =>
                {
                    var errors = new List<string>();
                    var results = new List<FolderCountLinesData>();
                    var allDetails = new List<FolderCountLinesData>();

                    if (!string.IsNullOrEmpty(opts.Directory))
                    {
                        if (Directory.Exists(opts.Directory))
                        {
                            try
                            {
                                var result = DirectoryProcessor.ProcessDirectory(opts.Directory, ignoredFolders, new HashSet<string>(), ExtensionPatterns);
                                results.Add(result);
                            }
                            catch (Exception ex)
                            {
                                errors.Add($"Error processing directory '{opts.Directory}': {ex.Message}");
                            }
                        }
                        else
                        {
                            errors.Add($"Directory path does not exist: {opts.Directory}");
                        }
                    }

                    if (results.Count > 0)
                    {
                        ResultDisplayer.DisplayHierarchy(results);
                    }

                    if (errors.Count > 0)
                    {
                        AnsiConsole.MarkupLine("[red]Errors:[/]");
                        foreach (var error in errors)
                        {
                            AnsiConsole.MarkupLine($"[red]{error}[/]");
                        }
                    }

                    if (results.Count == 0)
                    {
                        AnsiConsole.MarkupLine("[red]No valid inputs provided.[/]");
                        return -1;
                    }

                    return errors.Count > 0 ? -1 : 0;
                },
                errs => Task.FromResult(-1));
        }
    }
}