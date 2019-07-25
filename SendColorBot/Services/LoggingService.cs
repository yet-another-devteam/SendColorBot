using System;
using System.IO;

namespace SendColorBot.Services
{
    class LoggingService
    {
        private enum LogType { Info, Warn, Error }
        private static string logDirectory { get; set; }
        private static string logFile => Path.Combine(logDirectory, $"{DateTime.UtcNow.ToString("yyyy-MM-dd")}.log");

        public static void StartLoggingService()
        {
            logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }


        public static void LogInfo(string text) => Log(text, LogType.Info);

        public static void LogWarn(string text) => Log(text, LogType.Warn);

        public static void LogError(string text) => Log(text, LogType.Error);


        private static void Log(string text, LogType type)
        {
            CheckFile();

            string logText = $"[{DateTime.UtcNow.ToString("HH:mm:ss")}z] [{type.ToString("F").ToUpper()}]: {text}";
            File.AppendAllText(logFile, logText + "\n");

            Console.Out.WriteLine(logText);
        }

        private static void CheckFile()
        {
            if (!Directory.Exists(logDirectory))
                Directory.CreateDirectory(logDirectory);
            if (!File.Exists(logFile))
                File.Create(logFile).Dispose();
        }
    }
}
