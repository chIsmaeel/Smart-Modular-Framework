using SMF.FileTransmitter;

ConfigSMF _configSMF = new();

//StaticMethods.AddProjectDirectoriesIfNotExist(_configSMF);
StaticMethods.DeleteSMFAddonsSourceGenerator(_configSMF);

StaticMethods.MoveProjectFiles(_configSMF, "Domain");
StaticMethods.MoveProjectFiles(_configSMF, "Application");
StaticMethods.MoveProjectFiles(_configSMF, "Infrastructure");
//StaticMethods.MoveProjectFiles(_configSMF, "API");


StaticMethods.AddCSProjFileIfNotExist(_configSMF, "Domain", CSProjConfig.DomainCsProjConfig(_configSMF));
StaticMethods.AddCSProjFileIfNotExist(_configSMF, "Application", CSProjConfig.ApplicationCsProjConfig(_configSMF));
StaticMethods.AddCSProjFileIfNotExist(_configSMF, "Infrastructure", CSProjConfig.InfrastructureCsProjConfig(_configSMF));
StaticMethods.AddCSProjFileIfNotExist(_configSMF, "API", CSProjConfig.MinimalApiCsProjConfig(_configSMF), "net7.0");
ProgramFileGenerator.Generate(_configSMF);


// API Json File

StaticMethods.WriteFileIfNotExist(
    _configSMF,
    Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".API", "Properties", "launchSettings.json"),
    Templates.LanchSettingsTemplate());

StaticMethods.WriteFileIfNotExist(
    _configSMF,
    Path.Combine(_configSMF.SOLUTION_BASE_PATH, _configSMF.SOLUTION_NAME, "src", _configSMF.SOLUTION_NAME + ".API", "appsettings.json"),
    Templates.AppSettingsTemplate());

StaticMethods.AddSolutionFileIfNotExist(_configSMF);


