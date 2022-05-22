namespace SMF.FileTransmitter;
using System;
using System.Diagnostics;

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
        var sourceDirPath = Path.Combine(generatedFilesDirInfo.FullName, $"SMF.{projectName}Layer.SourceGenerator");
        var sDirInfo = new DirectoryInfo(sourceDirPath);

        var dDirInfo = new DirectoryInfo(dDirPath);
        if (!dDirInfo.Exists)
        {
            dDirInfo.Create();
            Console.WriteLine("Created: " + dDirInfo.FullName.Replace(Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src"), ""));
            Console.WriteLine("---------------------------------------");
            Console.WriteLine();
        }
        if (sDirInfo.EnumerateFiles("*.g.cs", SearchOption.AllDirectories).Any())
        {
            Console.WriteLine();
            foreach (var di in dDirInfo.EnumerateDirectories("*", SearchOption.AllDirectories))
            {
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
    public static void AddCSProjFileIfNotExist(ConfigSMF configSMF, string projectName, CSProjConfig cSProjConfig)
    {
        Console.WriteLine();
        var (properties, references) = cSProjConfig;
        var csProjFilePath = Path.Combine(configSMF.SOLUTION_BASE_PATH, configSMF.SOLUTION_NAME, "src", configSMF.SOLUTION_NAME + "." + projectName, $"{configSMF.SOLUTION_NAME}.{projectName}.csproj");
        if (!File.Exists(csProjFilePath))
        {
            using var fs = File.Create(csProjFilePath);
            using var ws = new StreamWriter(fs);
            ws.Write(CSProjGenerator.Template(CSProjGenerator.GetProperties(properties), CSProjGenerator.GetReferences(references)));
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

            sourceDirPath = Path.Combine(sourceDirPath, $"SMF.{projectName}Layer.SourceGenerator", projectName);

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
