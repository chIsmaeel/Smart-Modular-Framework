namespace SMF.FileTransmitter;
/// <summary>
/// The templates.
/// </summary>

public static class Templates
{
    /// <summary>
    /// Lanches the settings template.
    /// </summary>
    /// <returns>A string.</returns>
    public static string LanchSettingsTemplate()
    {
        return
$$""" 
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:64299",
      "sslPort": 44397
    }
  },
  "profiles": {
    "TestUI": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "launchUrl": "swagger",
      "applicationUrl": "https://localhost:7294;http://localhost:5294",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "swagger",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
""";
    }

    /// <summary>
    /// Apps the settings template.
    /// </summary>
    /// <returns>A string.</returns>
    public static string AppSettingsTemplate()
    {
        return
$$""" 
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

""";
    }
}
