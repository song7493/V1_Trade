using System;
using System.IO;
using System.Text;

namespace V1_Trade.Infrastructure.Logging
{
    public static class Logger
    {
        private static readonly object _lock = new object();
        private const long MaxSize = 100L * 1024 * 1024; // 100MB
        private const int RetentionDays = 14;
        private static readonly string LogDir = @"C:\\Log\\V1_Trade";
        private static string _currentFile = string.Empty;
        private static StreamWriter _writer;

        static Logger()
        {
            Directory.CreateDirectory(LogDir);
            Cleanup();
            RollFile();
        }

        public static void Log(string level, string message)
        {
            lock (_lock)
            {
                if (_writer == null || new FileInfo(_currentFile).Length > MaxSize)
                {
                    _writer?.Dispose();
                    RollFile();
                }

                var escaped = message.Replace("\\", "\\\\").Replace("\"", "\\\"");
                var entry = $"{{\"ts\":\"{DateTime.UtcNow:o}\",\"level\":\"{level}\",\"message\":\"{escaped}\"}}";
                _writer.WriteLine(entry);
                _writer.Flush();
            }
        }

        private static void RollFile()
        {
            var date = DateTime.UtcNow.ToString("yyyyMMdd");
            var index = 0;
            string path;
            do
            {
                path = Path.Combine(LogDir, $"{date}_{index}.log");
                index++;
            } while (File.Exists(path) && new FileInfo(path).Length > MaxSize);

            _currentFile = path;
            _writer = new StreamWriter(new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read), Encoding.UTF8);
        }

        private static void Cleanup()
        {
            foreach (var file in Directory.GetFiles(LogDir, "*.log"))
            {
                if (File.GetCreationTimeUtc(file) < DateTime.UtcNow.AddDays(-RetentionDays))
                {
                    try { File.Delete(file); } catch { }
                }
            }
        }
    }
}
