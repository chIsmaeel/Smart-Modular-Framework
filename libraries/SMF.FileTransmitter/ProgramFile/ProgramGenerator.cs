namespace SMF.FileTransmitter.ProgramFile;
/// <summary>
/// This class is used to transmit files to a remote server.
/// </summary>
public class ProgramFileGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProgramGenerator"/> class.
    /// </summary>
    /// <param name="configSMF">The config s m f.</param>
    public static void APIGenerate(ConfigSMF configSMF, string programFilePath)
    {
        var programFileCode = new ProgramFileTemplate(configSMF!,
               new()
               {

                   "Microsoft.Extensions.DependencyInjection" ,
                   "Microsoft.AspNetCore.Builder",
               $"{configSMF.SOLUTION_NAME}.Infrastructure",
               $"{configSMF.SOLUTION_NAME}.API",
               "MediatR",
               "Microsoft.AspNetCore.Mvc",
               },
               new()
               {
                  "AddSMFInfrastructureServices()",
                  "AddEndpointsApiExplorer()",
                  "AddSwaggerGen()",

                 $"AddMediatR(System.Reflection.Assembly.GetExecutingAssembly(), typeof({configSMF!.SOLUTION_NAME}.Application.Interfaces.ISMFDbContext).Assembly)"

               },
               new()
               {
                            "AddSMFMigrations()" ,
                            "MapSMFAPIs()"  ,
                            "UseSwagger()",
                            "UseSwaggerUI()",
               }).CreateTemplate();
        StaticMethods.WriteFileIfNotExist(configSMF, programFilePath, programFileCode);
    }

    public static void GrpcGenerate(ConfigSMF configSMF, string programFilePath)
    {
        var programFileCode = new ProgramFileTemplate(configSMF!,
               new()
               {

                   "Microsoft.Extensions.DependencyInjection" ,
                   "Microsoft.AspNetCore.Builder",

               $"{configSMF.SOLUTION_NAME}.Infrastructure",
               $"{configSMF.SOLUTION_NAME}.Grpc",
               "MediatR",
               },
               new()
               {
                  "AddSMFInfrastructureServices()",

                 $"AddMediatR(System.Reflection.Assembly.GetExecutingAssembly(), typeof({configSMF!.SOLUTION_NAME}.Application.Interfaces.ISMFDbContext).Assembly)",
                     "AddGrpc()",
               },
               new()
               {
"AddSMFMigrations()" ,
"MapSMFGrpcServices()"  ,
$@"MapGet(""/"", () => ""Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909"");" ,

               }).CreateTemplate();
        StaticMethods.WriteFileIfNotExist(configSMF, programFilePath, programFileCode);
    }

}
