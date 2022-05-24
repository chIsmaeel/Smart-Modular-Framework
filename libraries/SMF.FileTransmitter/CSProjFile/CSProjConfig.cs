namespace SMF.FileTransmitter.CSProjFile;
public record CSProjConfig(List<CSProjProperties> CSProjProperties, List<References> References)
{
    /// <summary>
    /// Domains the cs proj config.
    /// </summary>
    /// <param name="_configSMF">The _config s m f.</param>
    /// <returns>A CSProjConfig.</returns>
    public static CSProjConfig DomainCsProjConfig(ConfigSMF _configSMF)
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

    /// <summary>
    /// Applications the cs proj config.
    /// </summary>
    /// <param name="_configSMF">The _config s m f.</param>
    /// <returns>A CSProjConfig.</returns>
    public static CSProjConfig ApplicationCsProjConfig(ConfigSMF _configSMF)
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

    /// <summary>
    /// Grpcs the cs proj config.
    /// </summary>
    /// <param name="_configSMF">The _config s m f.</param>
    /// <returns>A CSProjConfig.</returns>
    internal static CSProjConfig GrpcCsProjConfig(ConfigSMF _configSMF)
    {
        List<CSProjProperties> properties = new()
    {
new("OutputType","exe"),
        new("RootNamespace", _configSMF.SOLUTION_NAME + ".API"),
        new("AssemblyName", _configSMF.SOLUTION_NAME + ".API"),
        new("NoWarn", "CS8669"),
    };

        List<References> references = new()
    {

    new ("Microsoft.Extensions.DependencyInjection",ReferenceType.Package, ("Version","6.0.0")),
             new ("MediatR.Extensions.Microsoft.DependencyInjection",ReferenceType.Package, ("Version","10.0.1")),

            new ("Microsoft.EntityFrameworkCore.Analyzers",ReferenceType.Package, ("Version","6.0.5")),

        new ("Grpc.AspNetCore",ReferenceType.Package, ("Version","2.43.0")),


        new ("Microsoft.EntityFrameworkCore.SqlServer",ReferenceType.Package, ("Version","6.0.5")),
    new ("Microsoft.EntityFrameworkCore.Tools",ReferenceType.Package, ("Version","6.0.5")),
    new (@$"..\{_configSMF.SOLUTION_NAME}.Application\{_configSMF.SOLUTION_NAME}.Application.csproj",ReferenceType.Project),
      new (@$"..\{_configSMF.SOLUTION_NAME}.Infrastructure\{_configSMF.SOLUTION_NAME}.Infrastructure.csproj",ReferenceType.Project),

    };

        return new(properties, references);
    }

    /// <summary>
    /// Infrastructures the cs proj config.
    /// </summary>
    /// <param name="_configSMF">The _config s m f.</param>
    /// <returns>A CSProjConfig.</returns>
    public static CSProjConfig InfrastructureCsProjConfig(ConfigSMF _configSMF)
    {
        List<CSProjProperties> properties = new()
    {
        new("RootNamespace",_configSMF.SOLUTION_NAME+".Infrastructure"),
        new("AssemblyName",_configSMF.SOLUTION_NAME+".Infrastructure"),
        new("NoWarn","CS8669"),
    };

        List<References> references = new()
    {

    new ("Microsoft.Extensions.DependencyInjection",ReferenceType.Package, ("Version","6.0.0")),
         new ("Microsoft.EntityFrameworkCore.Analyzers",ReferenceType.Package, ("Version","6.0.5")),

        new ("Microsoft.EntityFrameworkCore.SqlServer",ReferenceType.Package, ("Version","6.0.5")),
    new ("Microsoft.EntityFrameworkCore.Tools",ReferenceType.Package, ("Version","6.0.5")),
    new (@$"..\{_configSMF.SOLUTION_NAME}.Application\{_configSMF.SOLUTION_NAME}.Application.csproj",ReferenceType.Project),

    };

        return new(properties, references);
    }

    /// <summary>
    /// Minimals the api cs proj config.
    /// </summary>
    /// <returns>A CSProjConfig.</returns>
    public static CSProjConfig MinimalApiCsProjConfig(ConfigSMF _configSMF)
    {
        List<CSProjProperties> properties = new()
    {
new("OutputType","exe"),
        new("RootNamespace", _configSMF.SOLUTION_NAME + ".API"),
        new("AssemblyName", _configSMF.SOLUTION_NAME + ".API"),
        new("NoWarn", "CS8669"),
    };

        List<References> references = new()
    {

    new ("Microsoft.Extensions.DependencyInjection",ReferenceType.Package, ("Version","6.0.0")),
             new ("MediatR.Extensions.Microsoft.DependencyInjection",ReferenceType.Package, ("Version","10.0.1")),

            new ("Microsoft.EntityFrameworkCore.Analyzers",ReferenceType.Package, ("Version","6.0.5")),

        new ("Swashbuckle.AspNetCore",ReferenceType.Package, ("Version","6.3.1")),
        new ("Swashbuckle.AspNetCore.Swagger",ReferenceType.Package, ("Version","6.3.1")),
        new ("Swashbuckle.AspNetCore.SwaggerGen",ReferenceType.Package, ("Version","6.3.1")),
            new ("Swashbuckle.AspNetCore.SwaggerUI",ReferenceType.Package, ("Version","6.3.1")),


        new ("Microsoft.EntityFrameworkCore.SqlServer",ReferenceType.Package, ("Version","6.0.5")),
    new ("Microsoft.EntityFrameworkCore.Tools",ReferenceType.Package, ("Version","6.0.5")),
    new (@$"..\{_configSMF.SOLUTION_NAME}.Application\{_configSMF.SOLUTION_NAME}.Application.csproj",ReferenceType.Project),
      new (@$"..\{_configSMF.SOLUTION_NAME}.Infrastructure\{_configSMF.SOLUTION_NAME}.Infrastructure.csproj",ReferenceType.Project),

    };

        return new(properties, references);
    }

}
