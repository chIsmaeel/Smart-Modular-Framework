namespace SMF.Common.SourceGenerator.Abstractions;

public record ModuleWithRegisteredModelCTs(ModuleCT RegisteringModule, IEnumerable<ModelCT?>? RegisteredModelCTs);