using ProgectCodeLines.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProgectCodeLines
{
    internal static class DirectoryProcessor
    {

        public static FolderCountLinesData ProcessDirectory(
             string directoryPath,
             HashSet<string> ignoredFolders,
             HashSet<string> ignoredFiles,
             HashSet<string> extensions)
        {
            var normalizedRoot = Path.GetFullPath(directoryPath).TrimEnd(Path.DirectorySeparatorChar);

            // --- Файлы в корне ---
            var rootFiles = Directory
              .EnumerateFiles(normalizedRoot, "*.*", SearchOption.TopDirectoryOnly)
              .Where(file =>
                 extensions.Contains(Path.GetExtension(file)) &&
                 !ignoredFiles.Contains(Path.GetFileName(file)))
              .ToList();

            var (rootLines, rootDetails) = LineCounter.FindCodeLinesCount(rootFiles, normalizedRoot, ignoreComments: false);

            // --- Подпапки рекурсивно ---
            var subResults = new List<FolderCountLinesData>();
            foreach (var dir in Directory.EnumerateDirectories(normalizedRoot))
            {
                var dirName = Path.GetFileName(dir);
                if (ignoredFolders.Contains(dirName)) continue;
                subResults.Add(ProcessDirectory(dir, ignoredFolders, ignoredFiles, extensions));
            }

            return new FolderCountLinesData(
                Type: TargetType.Directory,
                Path: normalizedRoot,
                RootLines: rootLines,
                Children: rootDetails.Concat(subResults).ToList()
            );
        }
    }
}