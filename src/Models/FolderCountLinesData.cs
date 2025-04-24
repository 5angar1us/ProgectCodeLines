using System.Collections.Generic;
using System.Linq;

namespace ProgectCodeLines.Models
{

    public enum TargetType
    {
        File,
        Directory,
        Project,
        Unknown
    }

    public record FolderCountLinesData(TargetType Type, string Path, int RootLines, IReadOnlyList<FolderCountLinesData> Children)
    {
        public int SubdirectoryLines => Children
            .Where(d => d.Type == TargetType.Directory)
            .Sum(d => d.RootLines + d.SubdirectoryLines);
    }
}