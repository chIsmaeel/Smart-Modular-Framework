namespace SMF.FileTransmitter;
using System;
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
        if (sDirInfo.EnumerateFiles("*.g.cs", SearchOption.AllDirectories).Count() > 0)
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
            if (dirInfo.EnumerateFiles("*", SearchOption.AllDirectories).Count() == 0)
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
}
