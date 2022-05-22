using SMF.FileTransmitter;

ConfigSMF _configSMF = new();

//StaticMethods.AddProjectDirectoriesIfNotExist(_configSMF);
StaticMethods.DeleteSMFAddonsSourceGenerator(_configSMF);

StaticMethods.MoveProjectFiles(_configSMF, "Domain");
StaticMethods.MoveProjectFiles(_configSMF, "Application");
StaticMethods.MoveProjectFiles(_configSMF, "Infrastructure");


StaticMethods.AddCSProjFileIfNotExist(_configSMF, "Domain", DomainCsProjConfig());
StaticMethods.AddCSProjFileIfNotExist(_configSMF, "Application", ApplicationCsProjConfig());
StaticMethods.AddCSProjFileIfNotExist(_configSMF, "Infrastructure", InfrastructureCsProjConfig());


StaticMethods.AddSolutionFileIfNotExist(_configSMF);

CSProjConfig DomainCsProjConfig()
{
    List<CSProjProperties> properties = new()
    {
        new("RootNamespace",_configSMF.SOLUTION_NAME+".Domain"),
        new("AssemblyName",_configSMF.SOLUTION_NAME+".Domain"),
        new("NoWarn","CS8669"),
    };


    List<References> references = new()
    {

    };

    return new(properties, references);
}

CSProjConfig ApplicationCsProjConfig()
{
    var csprojfileName = _configSMF.SOLUTION_NAME + ".Application";
    List<CSProjProperties> properties = new()
    {
        new("RootNamespace",csprojfileName),
        new("AssemblyName",csprojfileName),
          new("NoWarn","CS8669"),

    };

    List<References> references = new()
    {
     new ("Microsoft.EntityFrameworkCore",ReferenceType.Package, ("Version","6.0.5")),
     new ("MediatR",ReferenceType.Package, ("Version","10.0.1")),
     new (@$"..\{_configSMF.SOLUTION_NAME}.Domain\{_configSMF.SOLUTION_NAME}.Domain.csproj",ReferenceType.Project),
    };

    return new(properties, references);
}

CSProjConfig InfrastructureCsProjConfig()
{
    List<CSProjProperties> properties = new()
    {
        new("RootNamespace",_configSMF.SOLUTION_NAME+".Infrastructure"),
        new("AssemblyName",_configSMF.SOLUTION_NAME+".Infrastructure"),
        new("NoWarn","CS8669"),
    };

    List<References> references = new()
    {
    new ("Microsoft.EntityFrameworkCore.Analyzers",ReferenceType.Package, ("Version","6.0.5")),
    new ("Microsoft.EntityFrameworkCore.SqlServer",ReferenceType.Package, ("Version","6.0.5")),
    new ("Microsoft.EntityFrameworkCore.Tools",ReferenceType.Package, ("Version","6.0.5")),
    new (@$"..\{_configSMF.SOLUTION_NAME}.Application\{_configSMF.SOLUTION_NAME}.Application.csproj",ReferenceType.Project),

    };

    return new(properties, references);
}

