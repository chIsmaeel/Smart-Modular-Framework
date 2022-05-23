namespace SMF.FileTransmitter;
/// <summary>
/// This class is used to transmit files to a remote server.
/// </summary>
public class ProgramFileGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramGenerator"/> class.
    /// </summary>
    /// <param name="configSMF">The config s m f.</param>
    public static void Generate(ConfigSMF configSMF)
    {
        var programFilePath = Path.Combine(configSMF.SOLUTION_BASE_PATH, configSMF.SOLUTION_NAME, "src", configSMF.SOLUTION_NAME + ".API", "Program.cs");
        var programFileCode = new APIProgramFileTemplate(configSMF!,
               new()
               {
                   "Microsoft.Extensions.DependencyInjection" ,
               $"{configSMF.SOLUTION_NAME}.Infrastructure"
               },
               new()
               {
                  "AddSMFInfrastructureServices()",
                 $"AddMediatR(System.Reflection.Assembly.GetExecutingAssembly(), typeof({configSMF!.SOLUTION_NAME}.Application.Interfaces.ISMFDbContext).Assembly)"

               },
               new()
               {

               }).CreateTemplate();
        StaticMethods.WriteFileIfNotExist(configSMF, programFilePath, programFileCode);
    }
}
