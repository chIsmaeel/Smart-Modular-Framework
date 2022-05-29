namespace SMF.FileTransmitter;

using SMF.FileTransmitter.CSProjFile;
using System;
using System.Diagnostics;
using System.Text;

/// <summary>
/// The static methods.
/// </summary>

internal static class StaticMethods
{

    /// <summary>
    /// Deletes the s m f addons source generator.
    /// </summary>
    /// <param name="_configSMF">The _config s m f.</param>
    public static void DeleteSMFAddonsSourceGenerator(ConfigSMF _configSMF)
    {
        var addonGeneratedDirPath = Path.Combine(_configSMF.GENERATED_DIR_PATH, "SMF.Addons.SourceGenerator");
        if (Directory.Exists(addonGeneratedDirPath))
        {
            Directory.Delete(addonGeneratedDirPath, true);
            Console.WriteLine();
            Console.WriteLine("Deleted: " + addonGeneratedDirPath);
            Console.WriteLine();
            Console.WriteLine($"---------------------------------------------------------------------------------------------------------------------");
        }
    }

    /// <summary>
    /// Writes the file if not exist.
    /// </summary>
    /// <param name="configSMF">The config s m f.</param>
    /// <param name="filePath">The file path.</param>
    public static void WriteFileIfNotExist(ConfigSMF configSMF, string filePath, string fileContent)
    {
        Console.WriteLine();
        var file = new FileInfo(filePath);
        if (!Directory.Exists(file.Directory!.FullName))
            Directory.CreateDirectory(file.Directory.FullName);
        if (!file.Exists)
        {
            using var fs = file.Create();
            using var ws = new StreamWriter(fs);
            ws.Write(fileContent);
            Console.WriteLine("Created: " + filePath.Replace(Path.Combine(configSMF.SOLUTION_BASE_PATH, configSMF.SOLUTION_NAME, "src"), ""));

        }
        Console.WriteLine("---------------------------------------");
        Console.WriteLine();
    }

    /// <summary>
    /// Adds the migration command.
    /// </summary>
    /// <param name="_configSMF">The _config s m f.</param>
    internal static void AddMigrationCommand(ConfigSMF _configSMF)
    {
        var migrationDir = new DirectoryInfo(Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", $"{_configSMF.SOLUTION_NAME}.Infrastructure", "Migrations"));
        if (!migrationDir.Exists) migrationDir.Create();
        bool foundMigrationFile = false;
        var filePath = Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "MigrationCommand.bat");
        foreach (var migrationFile in migrationDir.EnumerateFiles())
        {
            if (migrationFile.Name.EndsWith("_SMF." + _configSMF.APP_VERSION + ".cs"))
            {
                foundMigrationFile = true;
                Console.WriteLine("Already found migration file: " + migrationFile.Name);
                break;
            }
        }
        if (foundMigrationFile) return;
        var command = $@" cmd /k dotnet ef migrations add SMF.{_configSMF.APP_VERSION} --project {Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", $"{_configSMF.SOLUTION_NAME}.Infrastructure")}";

        Console.WriteLine();
        var file = new FileInfo(filePath);
        if (!Directory.Exists(file.Directory!.FullName))
            Directory.CreateDirectory(file.Directory.FullName);
        using var fs = file.Create();
        using var ws = new StreamWriter(fs);
        ws.Write(command);
        Console.WriteLine("Created: " + filePath.Replace(Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src"), ""));

        Console.WriteLine("---------------------------------------");
        Console.WriteLine();

    }

    internal static async Task AddGrpcConversionFilesAsync(ConfigSMF _configSMF)
    {
        var servicesDir = Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".Grpc", "Conversions");
        var servicesDirInfo = new DirectoryInfo(servicesDir);
        if (!servicesDirInfo.Exists) return;

        foreach (var serviceFile in servicesDirInfo.EnumerateFiles())
        {
            var serviceFileAllLines = File.ReadAllLines(serviceFile.FullName);
            if (serviceFileAllLines!.Length == 0) return;
            var sb = new StringBuilder();
            for (int i = 2; i < serviceFileAllLines.Length - 1; i++)
                sb.AppendLine(serviceFileAllLines[i]);
            using var fs = File.Create(serviceFile.FullName);
            using var ws = new StreamWriter(fs);
            await ws.WriteAsync(sb.ToString());
            ws.Dispose();
            fs.Dispose();
        }
    }

    /// <summary>
    /// Adds the extension methods file.
    /// </summary>
    /// <param name="_configSMF">The _config s m f.</param>
    /// <returns>A Task.</returns>
    internal static async Task AddExtensionMethodsFileAsync(ConfigSMF _configSMF)
    {
        var extensionMethodPath = Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".Grpc", "ExtensionMethods.g.cs");
        var extensionMethodFileInfo = new FileInfo(extensionMethodPath);
        if (!extensionMethodFileInfo.Exists) return;

        var emFileAllLines = File.ReadAllLines(extensionMethodPath);
        if (emFileAllLines!.Length == 0) return;
        var sb = new StringBuilder();
        for (int i = 2; i < emFileAllLines.Length - 1; i++)
            sb.AppendLine(emFileAllLines[i]);
        using var fs = File.Create(extensionMethodPath);
        using var ws = new StreamWriter(fs);
        await ws.WriteAsync(sb.ToString());
        ws.Dispose();
        fs.Dispose();
    }


    /// <summary>
    /// Adds the grpc proto file.
    /// </summary>
    /// <param name="_configSMF">The _config s m f.</param>
    internal static async Task AddGrpcProtoFileAsync(ConfigSMF _configSMF)
    {
        var generatorProjProtoPath = Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".Grpc", "smf.proto.g.cs");
        var generatorProjProtoFileInfo = new FileInfo(generatorProjProtoPath);
        if (!generatorProjProtoFileInfo.Exists) return;

        var protoFileText = File.ReadAllLines(generatorProjProtoPath);

        if (protoFileText!.Length == 0) return;
        var sb = new StringBuilder();
        for (int i = 2; i < protoFileText.Length - 1; i++)
        {
            sb.AppendLine(protoFileText[i]);
        }
        var dPath = Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".Grpc", "Protos");
        if (!Directory.Exists(dPath))
            Directory.CreateDirectory(dPath);
        using var fs = File.Create(Path.Combine(dPath, "smf.proto"));

        using var ws = new StreamWriter(fs);
        await ws.WriteAsync(sb.ToString());
        if (generatorProjProtoFileInfo.Exists)
            generatorProjProtoFileInfo.Delete();
    }

    /// <summary>
    /// Adds the grpc services files.
    /// </summary>
    /// <param name="_configSMF">The _config s m f.</param>
    internal static async Task AddGrpcServicesFilesAsync(ConfigSMF _configSMF)
    {
        var servicesDir = Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".Grpc", "Services");
        var servicesDirInfo = new DirectoryInfo(servicesDir);
        if (!servicesDirInfo.Exists) return;

        foreach (var serviceFile in servicesDirInfo.EnumerateFiles())
        {
            var serviceFileAllLines = File.ReadAllLines(serviceFile.FullName);
            if (serviceFileAllLines!.Length == 0) return;
            var sb = new StringBuilder();
            for (int i = 2; i < serviceFileAllLines.Length - 1; i++)
                sb.AppendLine(serviceFileAllLines[i]);
            var dPath = Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".Grpc", "Protos");
            using var fs = File.Create(serviceFile.FullName);
            using var ws = new StreamWriter(fs);
            await ws.WriteAsync(sb.ToString());
            ws.Dispose();
            fs.Dispose();
        }
    }


    /// <summary>
    /// Moves the project files.
    /// </summary>
    /// <param name="_configSMF">The _config s m f.</param>
    /// <param name="projectName">The project name.</param>
    public static void MoveProjectFiles(ConfigSMF _configSMF, string projectName)
    {
        Console.WriteLine();
        Console.WriteLine($"         ------------------------------  <<<<<  {_configSMF.SOLUTION_NAME}.{projectName}  >>>>>  -----------------------------");
        Console.WriteLine();
        Console.WriteLine($"---------------------------------------------------------------------------------------------------------------------");

        var generatedFilesDirInfo = new DirectoryInfo(_configSMF.GENERATED_DIR_PATH);
        var dDirPath = Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + "." + projectName);
        var sourceDirPath = Path.Combine(generatedFilesDirInfo.FullName, $"SMF.{projectName}.SourceGenerator");
        var sDirInfo = new DirectoryInfo(sourceDirPath);
        if (!sDirInfo.Exists) return;
        var dDirInfo = new DirectoryInfo(dDirPath);
        if (!dDirInfo.Exists)
        {
            dDirInfo.Create();
            Console.WriteLine("Created: " + dDirInfo.FullName.Replace(Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src"), ""));
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();
        }
        if (sDirInfo.EnumerateFiles("*.g.cs", SearchOption.TopDirectoryOnly).Any())
        {
            Console.WriteLine();
            foreach (var di in dDirInfo.EnumerateDirectories("*", SearchOption.AllDirectories))
            {
                if (di.Name.Contains("Migrations") || di.Name == "obj" || di.Name == "bin")
                    continue;

                di.Delete(true);
                Console.WriteLine("Deleted: " + di.FullName.Replace(Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src"), ""));
            }
            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();
        }


        MoveProjectFiles(_configSMF, projectName, generatedFilesDirInfo);
        Console.WriteLine($"---------------------------------------------------------------------------------------------------------------------");
    }

    /// <summary>
    /// Adds the c s proj file if not exist.
    /// </summary>
    /// <param name="configSMF">The config s m f.</param>
    /// <param name="projectName">The project name.</param>
    /// <param name="properties">The properties.</param>
    /// <param name="references">The references.</param>
    public static void AddCSProjFileIfNotExist(ConfigSMF configSMF, string projectName, CSProjConfig cSProjConfig, string version = "net6.0", string? extraInfo = null)
    {
        Console.WriteLine();
        var (properties, references) = cSProjConfig;
        var csProjFilePath = Path.Combine(configSMF.SOLUTION_BASE_PATH, configSMF.SOLUTION_NAME, "src", configSMF.SOLUTION_NAME + "." + projectName, $"{configSMF.SOLUTION_NAME}.{projectName}.csproj");
        var file = new FileInfo(csProjFilePath);
        if (!Directory.Exists(file.Directory!.FullName))
            Directory.CreateDirectory(file.Directory.FullName);
        if (!file.Exists)
        {
            using var fs = file.OpenWrite();
            using var ws = new StreamWriter(fs);
            ws.Write(CSProjGenerator.Template(CSProjGenerator.GetProperties(properties), CSProjGenerator.GetReferences(references), version, extraInfo));
            Console.WriteLine("Created: " + csProjFilePath.Replace(Path.Combine(configSMF.SOLUTION_BASE_PATH, configSMF.SOLUTION_NAME, "src"), ""));

        }
        Console.WriteLine("---------------------------------------");
        Console.WriteLine();
    }

    /// <summary>
    /// Moves the project files.
    /// </summary>
    /// <param name="_configSMF">The _config s m f.</param>
    /// <param name="projectName">The project name.</param>
    /// <param name="generatedFilesDirInfo">The generated files dir info.</param>
    public static void MoveProjectFiles(ConfigSMF _configSMF, string projectName, DirectoryInfo generatedFilesDirInfo)
    {
        foreach (var dirInfo in generatedFilesDirInfo.EnumerateDirectories("*", SearchOption.AllDirectories).Where(_ => _.Name.StartsWith(projectName)))
        {
            var destinationDirPath = Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + "." + projectName);
            var sourceDirPath = generatedFilesDirInfo.FullName;

            sourceDirPath = Path.Combine(sourceDirPath, $"SMF.{projectName}.SourceGenerator", projectName);
            //if (!Directory.Exists(sourceDirPath)) continue;
            if (!dirInfo.Exists) continue;
            if (!dirInfo.EnumerateFiles("*", SearchOption.AllDirectories).Any())
                continue;

            var name = dirInfo.Name;
            var nameArray = name.Split('.');

            int i = 1;
            while (i < nameArray.Length - 1)
            {
                sourceDirPath = $"{sourceDirPath}.{nameArray[i]}";
                destinationDirPath = Path.Combine(destinationDirPath, nameArray[i]);
                i++;
            }
            sourceDirPath = $"{sourceDirPath}.{nameArray[^1]}";
            var sourceDirInfo = new DirectoryInfo(sourceDirPath);
            var dest = new DirectoryInfo(destinationDirPath);
            if (!sourceDirInfo.Exists) continue;
            if (!dest.Exists)
            {
                dest.Create();
                Console.WriteLine("Created: " + dest.FullName.Replace(Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src"), ""));
                Console.WriteLine();
                Console.WriteLine("---------------------------------------");
                Console.WriteLine();
            }
            foreach (var file in sourceDirInfo.EnumerateFiles("*.g.cs", SearchOption.AllDirectories).Where(_ => _.FullName.EndsWith(".g.cs")))
            {
                var sourceFileFullName = file.FullName;
                var destinationFileFullName = sourceFileFullName.Replace(sourceDirPath, destinationDirPath);
                File.Move(sourceFileFullName, destinationFileFullName, true);
                Console.WriteLine("Moved: " + destinationFileFullName.Replace(Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src"), ""));

            }
            Console.WriteLine();
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();
            sourceDirPath = null;
        }
    }

    /// <summary>
    /// Adds the solution file.
    /// </summary>
    /// <param name="_configSMF">The _config s m f.</param>
    public static void AddSolutionFileIfNotExist(ConfigSMF _configSMF)
    {
        var path = Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, _configSMF.SOLUTION_NAME + ".sln");
        if (File.Exists(path))
            return;
        List<string> commands = new()
        {
    $"dotnet new sln --output {Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME)} --name {_configSMF.SOLUTION_NAME} ",
    $"dotnet sln {Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, _configSMF.SOLUTION_NAME)}.sln add { Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".Domain")}" ,
    $"dotnet sln { Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, _configSMF.SOLUTION_NAME)}.sln add { Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".Application")}",
    $"dotnet sln { Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, _configSMF.SOLUTION_NAME)}.sln add { Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".Infrastructure")}",
  $"dotnet sln { Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, _configSMF.SOLUTION_NAME)}.sln add { Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".API")}",
 $"dotnet sln { Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, _configSMF.SOLUTION_NAME)}.sln add { Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".Grpc")}",
};

        for (int i = 0; i < commands.Count; i++)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/C " + commands[i],
                //WindowStyle = ProcessWindowStyle.Hidden,
                //CreateNoWindow = true,
                //UseShellExecute = false
            };
            Process.Start(startInfo);
            if (i != commands.Count - 1)
                Task.Delay(5000).Wait();

        }
    }
}
