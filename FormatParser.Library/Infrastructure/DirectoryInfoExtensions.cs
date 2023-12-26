namespace FormatParser.Library.Infrastructure;

public static class DirectoryInfoExtensions
{
    public static FileInfo[] SearchFiles(this DirectoryInfo directoryInfo, string searchPattern)
    {
        FileInfo[] files = directoryInfo.GetFiles(searchPattern);
        if (files.Length == 0)
        {
            Queue<DirectoryInfo> queue = new();
            foreach (DirectoryInfo subdirectory in directoryInfo.GetDirectories())
                queue.Enqueue(subdirectory);

            while (queue.Count > 0)
            {
                DirectoryInfo subdirectory = queue.Dequeue();
                foreach (DirectoryInfo subsubdirectory in subdirectory.GetDirectories())
                    queue.Enqueue(subsubdirectory);

                files = subdirectory.GetFiles(searchPattern);
                if (files.Length > 0)
                    return files;
            }
        }

        return files;
    }
}
