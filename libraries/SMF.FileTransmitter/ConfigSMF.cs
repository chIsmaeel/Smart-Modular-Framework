namespace SMF.FileTransmitter;
/// <summary>
/// The config s m f file.
/// </summary>

public class ConfigSMF
{
    private string? _gENERATOR_PROJECT_BASE_PATH;
    private string? _sOLUTION_BASE_PATH;
    private string[]? _configFileLines;
    private string? _gENERATED_DIR_PATH;
    private string? _sOLUTION_NAME;
    private string? _gENERATOR_PROJECT_NAME;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigSMF"/> class.
    /// </summary>
    /// <param name="configFileLines">The config file lines.</param>
    public ConfigSMF(string configFile = "../../config.smf")
    {
        AddProjectBasePath(configFile);

    }

    /// <summary>
    /// Gets the key values.
    /// </summary>
    public Dictionary<string, string> KeyValues { get; } = new();

    /// <summary>
    /// Gets the base path.
    /// </summary>
    public string BasePath { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the s o l u t i o n_ b a s e_ p a t h.
    /// </summary>
    public string SOLUTION_BASE_PATH => _sOLUTION_BASE_PATH ??= GetValueFromKeyValuesDict(nameof(SOLUTION_BASE_PATH), @"C:\SmartModularFramework");
    /// <summary>
    /// Gets the s o l u t i o n_ n a m e.
    /// </summary>
    public string SOLUTION_NAME => _sOLUTION_NAME ??= GetValueFromKeyValuesDict(nameof(SOLUTION_NAME), "SMF");

    /// <summary>
    /// Gets the g e n e r a t o r_ p r o j e c t_ n a m e.
    /// </summary>
    public string GENERATOR_PROJECT_NAME => _gENERATOR_PROJECT_NAME ??= GetValueFromKeyValuesDict(nameof(GENERATOR_PROJECT_NAME), "SmartModularFramework");

    /// <summary>
    /// Gets or sets the g e n e r a t o r_ p r o j e c t_ b a s e_ p a t h.
    /// </summary>
    public string GENERATOR_PROJECT_BASE_PATH => _gENERATOR_PROJECT_BASE_PATH ??= GetValueFromKeyValuesDict(nameof(GENERATOR_PROJECT_BASE_PATH), "SMFGenerator");

    /// <summary>
    /// Gets the g e n e r a t e d_ d i r_ p a t h.                                                                                       
    /// </summary>
    public string GENERATED_DIR_PATH => _gENERATED_DIR_PATH ??= GetValueFromKeyValuesDict(nameof(GENERATED_DIR_PATH), Path.Combine(SOLUTION_BASE_PATH, "src", "gen", GENERATOR_PROJECT_NAME, "obj", "Generated"));
    /// <summary>
    /// Adds the project base path.
    /// </summary>
    /// <param name="configFile">The config file.</param>
    private void AddProjectBasePath(string configFile)
    {
        if (File.Exists(configFile))
        {
            BasePath = "../../";
            _configFileLines = File.ReadAllLines(configFile);
            AddValuesInDictionary();
            return;
        }
        var basePath = new DirectoryInfo(Environment.CurrentDirectory)!.Parent!.Parent!.Parent!.Parent!.Parent!.FullName;
        if (File.Exists(Path.Combine(basePath, "config.smf")))
        {
            BasePath = basePath;
            _configFileLines = File.ReadAllLines(Path.Combine(basePath, "config.smf"));
            AddValuesInDictionary();
        }
    }

    /// <summary>
    /// Adds the values in dictionary.
    /// </summary>
    private void AddValuesInDictionary()
    {
        foreach (var keyValue in _configFileLines!)
            if (keyValue.Contains('='))
            {
                var r = keyValue.Split('=');
                if (r.Length != 2) continue;
                var value = r[1].Trim();
                if (!string.IsNullOrEmpty(value))
                    KeyValues[r[0].Trim()] = value;
            }
    }

    /// <summary>
    /// Gets the value from key values dict.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="dafaultValue">The dafault value.</param>
    /// <returns>A string.</returns>
    private string GetValueFromKeyValuesDict(string key, string dafaultValue)
    {
        if (KeyValues.ContainsKey(key))
            return KeyValues[key];
        return dafaultValue;
    }

}
