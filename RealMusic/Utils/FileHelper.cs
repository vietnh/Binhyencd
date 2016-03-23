using System;
using System.IO;

namespace RealMusic.Utils
{
    public static class FileHelper
    {
        public static string GetRelativePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            var currentDir = Environment.CurrentDirectory;
            var directory = new DirectoryInfo(currentDir);
            var file = new FileInfo(path);

            var fullDirectory = directory.FullName;
            var fullFile = file.FullName;

            return fullFile.StartsWith(fullDirectory) ? fullFile.Substring(fullDirectory.Length + 1) : string.Empty;
        }
    }
}
