namespace SMF.FileTransmitter.CSProjFile;
public record References(string IncludingReference, ReferenceType ReferenceType, params (string key, string value)[] ExtraInfo);
