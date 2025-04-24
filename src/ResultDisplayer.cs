using ProgectCodeLines.Models;
using Spectre.Console;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProgectCodeLines
{
    internal static class ResultDisplayer
    {
       
        public static void DisplayHierarchy(IEnumerable<FolderCountLinesData> roots)
        {
            foreach (var root in roots)
            {
                // Печатать корневую папку без отступа
                PrintDirectory(root, indent: "");
            }
        }

        private static void PrintDirectory(FolderCountLinesData dirNode, string indent)
        {
            // Название папки
            var dirName = Path.GetFileName(dirNode.Path) ?? dirNode.Path;
            // Суммарно: строки в файлах корня и строки в подпапках
            var totalInSubdirs = dirNode.SubdirectoryLines;
            AnsiConsole.MarkupLine(
                $"{indent}[bold]{dirName}[/] " +
                $"(files: {dirNode.RootLines} lines, subdirs: {totalInSubdirs} lines)");

            // Вывести файлы
            foreach (var f in dirNode.Children.Where(d => d.Type == TargetType.File)
                                              .OrderByDescending(f => f.RootLines))
            {
                var fileName = Path.GetFileName(f.Path);
                AnsiConsole.MarkupLine($"{indent} - {fileName} — {f.RootLines} lines");
            }

            // Рекурсивно для подпапок
            foreach (var sub in dirNode.Children.Where(d => d.Type == TargetType.Directory)
                                               .OrderByDescending(d => d.RootLines + d.SubdirectoryLines))
            {
                PrintDirectory(sub, indent + "  ");
            }
        }
    }
}