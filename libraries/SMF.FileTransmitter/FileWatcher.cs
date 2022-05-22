namespace SMF.FileTransmitter;
using System;
/// <summary>
/// The file watcher.
/// </summary>

internal class FileWatcher
{
    private readonly ConfigSMF _configSMF;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileWatcher"/> class.
    /// </summary>
    /// <param name="configSMF">The config s m f.</param>
    public FileWatcher(ConfigSMF configSMF)
    {
        _configSMF = configSMF;
        FileSystemWatcher watcher = WatchDirectory(configSMF.BasePath);

    }

    /// <summary>
    /// Watches the directory.
    /// </summary>
    /// <param name="dirPath">The dir path.</param>
    /// <returns>A FileSystemWatcher.</returns>
    private FileSystemWatcher WatchDirectory(string dirPath)
    {
        Console.WriteLine(_configSMF.SOLUTION_NAME);
        var watcher = new FileSystemWatcher(dirPath)
        {
            NotifyFilter = NotifyFilters.Attributes
                                         | NotifyFilters.CreationTime
                                         | NotifyFilters.DirectoryName
                                         | NotifyFilters.FileName
                                         | NotifyFilters.LastAccess
                                         | NotifyFilters.LastWrite
                                         | NotifyFilters.Security
                                         | NotifyFilters.Size
        };

        watcher.Changed += OnChanged;
        watcher.Created += OnCreated;
        watcher.Deleted += OnDeleted;
        watcher.Renamed += OnRenamed;
        watcher.Error += OnError;

        watcher.Filter = "*.g.cs";
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;

        Console.WriteLine("Press enter to exit.");
        Console.ReadLine();

        static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed: {e.FullPath}");
        }

        static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);
        }

        static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Deleted: {e.FullPath}");
        }

        static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed:");
            Console.WriteLine($"    Old: {e.OldFullPath}");
            Console.WriteLine($"    New: {e.FullPath}");
        }

        static void OnError(object sender, ErrorEventArgs e)
        {
            PrintException(e.GetException());
        }

        static void PrintException(Exception? ex)
        {
            if (ex != null)
            {
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine("Stacktrace:");
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine();
                PrintException(ex.InnerException);
            }
        }

        return watcher;
    }
}
