using ProgectCodeLines.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProgectCodeLines
{
    internal static class LineCounter
    {
        public static (int TotalLines, List<FolderCountLinesData> Details) FindCodeLinesCount(
            List<string> files,
            string basePath,
            bool ignoreEmptyLines = true,
            bool ignoreComments = true)
        {
            // Валидация входных параметров
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (basePath == null) throw new ArgumentNullException(nameof(basePath));

            var details = new ConcurrentBag<FolderCountLinesData>();
            var totalLines = 0;

            // Параллельная обработка файлов
            Parallel.ForEach(files, file =>
            {
                try
                {
                    var lines = CountLinesInFile(file, ignoreEmptyLines, ignoreComments);
                    var relativePath = GetRelativePathSafe(file, basePath);

                    details.Add(new FolderCountLinesData(
                        Type: TargetType.File,
                        Path: relativePath,
                        RootLines: lines,
                        Children: new List<FolderCountLinesData>()
                    ));

                    Interlocked.Add(ref totalLines, lines);
                }
                catch (Exception ex)
                {
                    // Логирование ошибки (можно добавить ILogger)
                    Console.WriteLine($"Error processing {file}: {ex.Message}");
                }
            });

            return (totalLines, details.OrderBy(d => d.Path).ToList());
        }

        private static int CountLinesInFile(string filePath, bool ignoreEmptyLines, bool ignoreComments)
        {
            var lines = File.ReadLines(filePath);
            var count = 0;

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();

                if (ignoreEmptyLines && string.IsNullOrWhiteSpace(trimmedLine))
                    continue;

                if (ignoreComments && IsCommentLine(trimmedLine))
                    continue;

                count++;
            }

            return count;
        }

        private static bool IsCommentLine(string line)
        {
            return line.StartsWith("//") ||
                   line.StartsWith("/*") ||
                   line.StartsWith("*") ||
                   line.StartsWith("*/");
        }

        private static string GetRelativePathSafe(string fullPath, string basePath)
        {
            try
            {
                return Path.GetRelativePath(basePath, fullPath);
            }
            catch (ArgumentException)
            {
                return fullPath;
            }
        }
    }
}