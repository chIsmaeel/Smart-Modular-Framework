using SMF.FileTransmitter;

Console.WriteLine("Ok");

ConfigSMF _configSMF = new();

var domainProjectDir = Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", "Domain");
Console.WriteLine(domainProjectDir);
if (!Directory.Exists(domainProjectDir))
    Directory.CreateDirectory(domainProjectDir);
var applicationProjectDir = Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", "Application");
Console.WriteLine(applicationProjectDir);
if (!Directory.Exists(applicationProjectDir))
    Directory.CreateDirectory(applicationProjectDir);
var grpcProjectDir = Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", "Grpc");
Console.WriteLine(grpcProjectDir);
if (!Directory.Exists(grpcProjectDir))
    Directory.CreateDirectory(grpcProjectDir);

var generatorProjectDir = Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "gen", _configSMF.GENERATOR_PROJECT_NAME);
Console.WriteLine(generatorProjectDir);
if (!Directory.Exists(generatorProjectDir))
    Directory.CreateDirectory(generatorProjectDir);

Console.WriteLine(_configSMF.SOLUTION_NAME);
using var watcher = new FileSystemWatcher(@"E:\Projects\Smart Modular Framework\sample\SMF.PointOfSale\obj\Generated");

watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

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
