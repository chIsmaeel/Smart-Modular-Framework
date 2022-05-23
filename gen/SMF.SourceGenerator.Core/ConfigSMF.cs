namespace SMF.SourceGenerator.Core;
/// <summary>
/// The config s m f file.
/// </summary>

public class ConfigSMF
{
    private readonly string[]? _configFileLines;
    private string? _dB_NAME;
    private string? _dB_DATA_SOURCE;
    private string? _sOLUTION_NAME;
    private string? _ef_PROJECT_PARENT_PATH;
    private string? _ef_PROJECT_NAME;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigSMF"/> class.
    /// </summary>
    /// <param name="configFileLines">The config file lines.</param>
    public ConfigSMF(string configFile)
    {
        if (configFile is not null)
        {
            _configFileLines = configFile.Split('\n');
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
    /// Gets the key values.
    /// </summary>
    public Dictionary<string, string> KeyValues { get; } = new();

    /// <summary>
    /// Gets the s o l u t i o n_ n a m e.
    /// </summary>
    public string SOLUTION_NAME => _sOLUTION_NAME ??= GetValueFromKeyValuesDict(nameof(SOLUTION_NAME), "SMF");     /// <summary>
                                                                                                                   /// Gets the e f_ p r o j e c t_ n a m e.
    public string DB_DATA_SOURCE => _dB_DATA_SOURCE ??= GetValueFromKeyValuesDict(nameof(DB_DATA_SOURCE), ".");     /// <summary>

    public string DB_NAME => _dB_NAME ??= GetValueFromKeyValuesDict(nameof(DB_NAME), "SMF");
    /// <summary>

    public string EF_PROJECT_NAME => _ef_PROJECT_NAME ??= GetValueFromKeyValuesDict(nameof(EF_PROJECT_NAME), "SMF.Data");

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

    /// <summary>
    /// Gets the e f_ p r o j e c t_ p a t h.
    /// </summary>
    public string EF_PROJECT_PARENT_PATH => _ef_PROJECT_PARENT_PATH ??= GetValueFromKeyValuesDict(nameof(EF_PROJECT_PARENT_PATH), Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
}
