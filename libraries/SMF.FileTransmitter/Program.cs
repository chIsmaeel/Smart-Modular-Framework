using SMF.FileTransmitter;
using SMF.FileTransmitter.CSProjFile;
using SMF.FileTransmitter.ProgramFile;

ConfigSMF _configSMF = new();

//StaticMethods.AddProjectDirectoriesIfNotExist(_configSMF);
//StaticMethods.DeleteSMFAddonsSourceGenerator(_configSMF);

StaticMethods.MoveProjectFiles(_configSMF, "Domain");
StaticMethods.MoveProjectFiles(_configSMF, "Application");
StaticMethods.MoveProjectFiles(_configSMF, "Infrastructure");
StaticMethods.MoveProjectFiles(_configSMF, "API");
StaticMethods.MoveProjectFiles(_configSMF, "Grpc");

StaticMethods.AddCSProjFileIfNotExist(_configSMF, "Domain", CSProjConfig.DomainCsProjConfig(_configSMF));
StaticMethods.AddCSProjFileIfNotExist(_configSMF, "Application", CSProjConfig.ApplicationCsProjConfig(_configSMF));
StaticMethods.AddCSProjFileIfNotExist(_configSMF, "Infrastructure", CSProjConfig.InfrastructureCsProjConfig(_configSMF));
StaticMethods.AddCSProjFileIfNotExist(_configSMF, "API", CSProjConfig.MinimalApiCsProjConfig(_configSMF), "net7.0");
StaticMethods.AddCSProjFileIfNotExist(_configSMF, "Grpc", CSProjConfig.GrpcCsProjConfig(_configSMF), "net7.0", $@"<Protobuf Include=""Protos\smf.proto"" GrpcServices=""Both"" />");

ProgramFileGenerator.APIGenerate(_configSMF, Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".API", "Program.cs"));
ProgramFileGenerator.GrpcGenerate(_configSMF, Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".Grpc", "Program.cs"));


// API Json File

StaticMethods.WriteFileIfNotExist(
    _configSMF,
    Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".API", "Properties", "launchSettings.json"),
    Templates.LanchSettingsTemplate());

StaticMethods.WriteFileIfNotExist(
    _configSMF,
    Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".API", "appsettings.json"),
    Templates.AppSettingsTemplate());

// Add Grpc LanchSettings

StaticMethods.WriteFileIfNotExist(
    _configSMF,
    Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".Grpc", "Properties", "launchSettings.json"),
    Templates.GrpcLanchSettions());

StaticMethods.AddGrpcProtoFile(_configSMF);
StaticMethods.AddMigrationCommand(_configSMF);

StaticMethods.AddSolutionFileIfNotExist(_configSMF);


