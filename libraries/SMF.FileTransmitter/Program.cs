using SMF.FileTransmitter;


ConfigSMF _configSMF = new();

//StaticMethods.AddProjectDirectoriesIfNotExist(_configSMF);
StaticMethods.DeleteSMFAddonsSourceGenerator(_configSMF);
StaticMethods.MoveProjectFiles(_configSMF, "Domain");
StaticMethods.MoveProjectFiles(_configSMF, "Application");
StaticMethods.MoveProjectFiles(_configSMF, "Infrastructure");


//StaticMethods.MoveProjectFiles(_configSMF, "Infrastructure");



