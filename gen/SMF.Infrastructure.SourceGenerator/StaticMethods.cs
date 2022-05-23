namespace SMF.InfrastructureLayer.SourceGenerator;
/// <summary>
/// This class is used to generate the source code for the SMF.InfrastructureLayer.DataAccessLayer.DataAccessLayer class.
/// </summary>
public static class StaticMethods
{
    /// <summary>
    /// Adds the model methods.
    /// </summary>
    /// <param name="s">The s.</param>
    /// <param name="handlerClass">The handler class.</param>
    public static void AddModelMethods(ModelCT s, ClassTypeTemplate handlerClass)
    {
        foreach (var method in s.Methods!)
        {
            var methodString = method!.MDS!.ToString();
            methodString = methodString.Replace("private partial", "private");
            handlerClass.StringMembers.Add(methodString);
        }
    }
}
