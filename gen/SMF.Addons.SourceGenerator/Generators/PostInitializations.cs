namespace SMF.Addons.SourceGenerator.Generators;

/// <summary>
/// The post initializations.
/// </summary>
[Generator]
internal class PostInitializations : CommonIncrementalGenerator
{
    /// <summary>
    /// Executes the.
    /// </summary>
    /// <param name="context">The context.</param>
    protected override void Execute(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(c => c.AddSource(
               "RegisterModelDelegates.g.cs",
               @$"namespace SMF.Addons;

public delegate void RegisterModel<T1>();

public delegate void RegisterModel<T1, T2>();

public delegate void RegisterModel<T1, T2, T3>();

public delegate void RegisterModel<T1, T2, T3, T4>();

public delegate void RegisterModel<T1, T2, T3, T4, T5>();

public delegate void RegisterModel<T1, T2, T3, T4, T5, T6>();

public delegate void RegisterModel<T1, T2, T3, T4, T5, T6, T7>();

public delegate void RegisterModel<T1, T2, T3, T4, T5, T6, T7, T8>();

public delegate void RegisterModel<T1, T2, T3, T4, T5, T6, T7, T8, T9>();

public delegate void RegisterModel<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();"
            ));
    }
}
