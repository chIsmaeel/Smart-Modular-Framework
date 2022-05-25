namespace Grpc;

using Humanizer;
using Microsoft.CodeAnalysis;
using SMF.Grpc.SourceGenerator.Generators;
using SMF.SourceGenerator.Core;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
/// <summary>
/// The proto file generator.
/// </summary>                    

[Generator]
internal class ProtoFileGenerator : CommonIncrementalGenerator
{
    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(ModuleWithRegisteredModelCTs.Collect(), AddProtoFile);
    }

    /// <summary>
    /// Adds the proto file.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="s">The s.</param>
    private void AddProtoFile(SourceProductionContext c, ImmutableArray<ModuleWithRegisteredModelCTs> mwm)
    {
        List<ModelCT> s = new();
        //#if DEBUG
        //        if (!System.Diagnostics.Debugger.IsAttached)
        //            System.Diagnostics.Debugger.Launch();
        //#endif
        foreach (var mrm in mwm)
        {
            s.AddRange(mrm.RegisteredModelCTs!);
        }
        SMFProductionContext context = new(c);
        var config = s.FirstOrDefault()?.ConfigSMFAndGlobalOptions.ConfigSMF;
        var generatorProjFile = Path.Combine(s.FirstOrDefault()!.ConfigSMFAndGlobalOptions.GeneratorProjectPath, "Protos", "smf.proto");
        var dProjFile = Path.Combine(config!.SOLUTION_BASE_PATH, config.SOLUTION_NAME, "src", config.SOLUTION_NAME + ".Grpc", "Protos", "smf.proto");
        var code = ProtoTemplate(config, s);
        //#if DEBUG
        //        if (!System.Diagnostics.Debugger.IsAttached)
        //            System.Diagnostics.Debugger.Launch();
        //#endif
        context.AddSource("smf.proto", code);
    }

    /// <summary>
    /// Protos the template.
    /// </summary>
    /// <returns>A string.</returns>
    public static string ProtoTemplate(ConfigSMF configSMF, IEnumerable<ModelCT> s)
    {
        return $$"""
/*

syntax = "proto3";

option csharp_namespace = "{{configSMF.SOLUTION_NAME}}.Grpc";

import "google/protobuf/timestamp.proto";
import "google/protobuf/wrappers.proto";

package smf;

// The greeting service definition.
{{GetProtoGrpcService(s)}}
message Void { }

message RequestId {
   int32 id = 1;
}
message ResponseId {
   int32 id = 1;
}
{{AddMessages(s)}}
*/
""";
    }

    /// <summary>
    /// Adds the messages.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <returns>A string.</returns>
    private static string AddMessages(IEnumerable<ModelCT> s)
    {
        var messages = new StringBuilder();
        foreach (var model in s)
        {
            // All Commands
            messages.AppendLine($"message {model.ModuleNameWithoutPostFix}_{model.IdentifierNameWithoutPostFix} {{");
            var tempModelCT = model;

            messages.AppendLine($"\tint32 Id = 1;");
            messages.AppendLine($"\tgoogle.protobuf.Timestamp CreatedOn = 2;");
            messages.AppendLine($"\tgoogle.protobuf.Timestamp LastModifiedOn = 3;");
            var i = 4;
            while (tempModelCT is not null)
            {
                StaticMethods.AddProperties(tempModelCT, messages, ref i);
                tempModelCT = tempModelCT.ParentClassType as ModelCT;
            }
            messages.AppendLine("}");
            messages.AppendLine();

            // Create Command 

            i = 1;
            messages.AppendLine($"message Create{model.ModuleNameWithoutPostFix}_{model.IdentifierNameWithoutPostFix}Command {{");
            tempModelCT = model;
            while (tempModelCT is not null)
            {
                StaticMethods.CreateCommandProperties(tempModelCT, messages, ref i);
                tempModelCT = tempModelCT.ParentClassType as ModelCT;
            }
            messages.AppendLine("}");
            messages.AppendLine();

            // Update Command

            messages.AppendLine($"message Update{model.ModuleNameWithoutPostFix}_{model.IdentifierNameWithoutPostFix}Command {{");
            tempModelCT = model;
            messages.AppendLine($"\tint32 id = 1;");
            i = 2;
            while (tempModelCT is not null)
            {
                StaticMethods.UpdateCommandProperties(tempModelCT, messages, ref i);
                tempModelCT = tempModelCT.ParentClassType as ModelCT;
            }
            messages.AppendLine("}");
            messages.AppendLine();


        }
        return messages.ToString();
    }



    /// <summary>
    /// Gets the proto grpc service.
    /// </summary>
    /// <param name="g">The g.</param>
    /// <returns>A string.</returns>
    private static string GetProtoGrpcService(IEnumerable<ModelCT> s)
    {
        var g = s.GroupBy(_ => _.ModuleNameWithoutPostFix);
        var services = new StringBuilder();
        foreach (var gi in g)
        {
            services.AppendLine($"service {gi.Key}Services {{");
            foreach (var model in gi)
            {
                services.AppendLine($"\trpc GetAll{model.IdentifierNameWithoutPostFix.Pluralize()} (Void) returns (stream {model.ModuleNameWithoutPostFix}_{model.IdentifierNameWithoutPostFix});");
                services.AppendLine($"\trpc Get{model.IdentifierNameWithoutPostFix}ById (RequestId) returns ({model.ModuleNameWithoutPostFix}_{model.IdentifierNameWithoutPostFix});");
                services.AppendLine($"\trpc Delete{model.IdentifierNameWithoutPostFix} (RequestId) returns (ResponseId);");
                services.AppendLine($"\trpc Add{model.IdentifierNameWithoutPostFix} (Create{model.ModuleNameWithoutPostFix}_{model.IdentifierNameWithoutPostFix}Command) returns (ResponseId);");
                services.AppendLine($"\trpc Update{model.IdentifierNameWithoutPostFix} (Update{model.ModuleNameWithoutPostFix}_{model.IdentifierNameWithoutPostFix}Command) returns (ResponseId);");
                services.AppendLine();
            }
            services.AppendLine("}");
        }

        services.AppendLine();
        return services.ToString();
    }
}
