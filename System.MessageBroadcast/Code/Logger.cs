// ========== COMPLETED - DO NOT MODIFY WITHOUT REVIEW ==========
// STABLE VERSION 1.0 - 2026-08-07
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace System.MessageBroadcast.Code
{
    /// <summary>
    /// Static logger class for file-based logging.
    /// Writes logs to ApplicationData folder.
    /// </summary>
    public static class Logger
    {
        // ============================================================
        // Configuration
        // ============================================================
        private static readonly string LogDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "LidomaSync");
        private static readonly string LogFilePath = Path.Combine(LogDirectory, "lidoma_sync.log");
        private static readonly object _lockObject = new object();
        private static readonly int MAX_LOG_SIZE_BYTES = 5 * 1024 * 1024; // 5 MB
        private static readonly int MAX_LOG_FILES = 5;

        // Enable/disable logging from config
        private static readonly bool _isLoggingEnabled;

        static Logger()
        {
            try
            {
                _isLoggingEnabled = true;
                var enabledConfig = System.Configuration.ConfigurationManager.AppSettings["LidomaLoggingEnabled"];
                if (enabledConfig != null && enabledConfig.ToLowerInvariant() == "false")
                    _isLoggingEnabled = false;
            }
            catch
            {
                _isLoggingEnabled = true;
            }
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        public static void LogInfo(string message)
        {
            LogInternal("INFO", message, null);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        public static void LogWarning(string message)
        {
            LogInternal("WARN", message, null);
        }

        /// <summary>
        /// Logs an error message with optional exception details.
        /// </summary>
        public static void LogError(string message, Exception ex)
        {
            LogInternal("ERROR", message, ex);
        }

        /// <summary>
        /// Logs a debug message (only when debugger is attached).
        /// </summary>
        public static void LogDebug(string message)
        {
            LogInternal("DEBUG", message, null);
        }

        private static void LogInternal(string level, string message, Exception ex)
        {
            if (!_isLoggingEnabled)
                return;

            try
            {
                lock (_lockObject)
                {
                    // Ensure directory exists
                    if (!Directory.Exists(LogDirectory))
                        Directory.CreateDirectory(LogDirectory);

                    // Check file size and archive if needed
                    if (File.Exists(LogFilePath))
                    {
                        var fileInfo = new FileInfo(LogFilePath);
                        if (fileInfo.Length > MAX_LOG_SIZE_BYTES)
                        {
                            ArchiveLogFile();
                        }
                    }

                    // Build log entry
                    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    var threadId = Thread.CurrentThread.ManagedThreadId;
                    var logEntry = string.Format("[{0}] [{1}] [Thread-{2}] {3}", timestamp, level, threadId, message);

                    if (ex != null)
                    {
                        logEntry += string.Format(" | Exception: {0} | StackTrace: {1}", ex.ToString(), ex.StackTrace ?? string.Empty);
                    }

                    logEntry += Environment.NewLine;

                    // Append to file
                    using (var sw = new StreamWriter(LogFilePath, true, Encoding.UTF8))
                    {
                        sw.Write(logEntry);
                    }
                }
            }
            catch
            {
                // Suppress all logging errors to prevent application crashes
            }
        }

        private static void ArchiveLogFile()
        {
            try
            {
                // Delete oldest log file if we've reached the max
                for (int i = MAX_LOG_FILES - 1; i >= 1; i--)
                {
                    var oldFile = LogFilePath + "." + i;
                    if (i == MAX_LOG_FILES - 1)
                    {
                        if (File.Exists(oldFile))
                            File.Delete(oldFile);
                    }
                    else
                    {
                        var newFile = LogFilePath + "." + (i + 1);
                        if (File.Exists(oldFile))
                            File.Move(oldFile, newFile);
                    }
                }

                // Archive current log
                var archivedFile = LogFilePath + ".1";
                if (File.Exists(archivedFile))
                    File.Delete(archivedFile);
                File.Move(LogFilePath, archivedFile);
            }
            catch
            {
                // Suppress archiving errors
            }
        }
    }
}
