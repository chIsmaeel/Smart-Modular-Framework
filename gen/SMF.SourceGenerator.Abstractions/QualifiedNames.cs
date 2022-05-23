﻿namespace SMF.SourceGenerator.Abstractions;
/// <summary>
/// This class represents a method.
/// </summary>
public static class QualifiedNames
{
    /// <summary>
    /// Gets the s m f db context.
    /// </summary>
    /// <param name="configSMF">The config s m f.</param>
    /// <returns>A string.</returns>
    public static string GetSMFDbContext(ConfigSMF configSMF)
    {
        return $"{configSMF.SOLUTION_NAME}.Infrastructure.Data.SMFDbContext";
    }

    /// <summary>
    /// Gets the i s m f db context.
    /// </summary>
    /// <param name="configSMF">The config s m f.</param>
    /// <returns>A string.</returns>
    public static string GetISMFDbContext(ConfigSMF configSMF)
    {
        return $"{configSMF.SOLUTION_NAME}.Application.Interfaces.ISMFDbContext";
    }

    /// <summary>
    /// Gets the register model repository interface.
    /// </summary>
    /// <param name="configSMF">The config s m f.</param>
    /// <param name="modelCT">The model c t.</param>
    /// <returns>A string.</returns>
    public static string GetRegisterModelRepositoryInterface(ConfigSMF configSMF, ModelCT modelCT)
    {
        return $"{configSMF.SOLUTION_NAME}.Application.{modelCT.ContainingModuleName}.Repositories.Interfaces.I{modelCT.IdentifierNameWithoutPostFix}Repository";
    }

    /// <summary>
    /// Gets the register model repository.
    /// </summary>
    /// <param name="configSMF">The config s m f.</param>
    /// <param name="modelCT">The model c t.</param>
    /// <returns>A string.</returns>
    public static string GetRegisterModelRepository(ConfigSMF configSMF, ModelCT modelCT)
    {
        return $"{configSMF.SOLUTION_NAME}.Infrastructure.{modelCT.ContainingModuleName}.Repositories.{modelCT.IdentifierNameWithoutPostFix}Repository";
    }

    /// <summary>
    /// Gets the unit of work.
    /// </summary>
    /// <param name="configSMF">The config s m f.</param>
    /// <returns>A string.</returns>
    public static string GetUnitOfWork(ConfigSMF configSMF)
    {
        return $"{configSMF.SOLUTION_NAME}.Infrastructure.UnitOfWork";
    }

    /// <summary>
    /// Gets the i unit of work.
    /// </summary>
    /// <param name="configSMF">The config s m f.</param>
    /// <returns>A string.</returns>
    public static string GetIUnitOfWork(ConfigSMF configSMF)
    {
        return $"{configSMF.SOLUTION_NAME}.Application.Interfaces.IUnitOfWork";
    }
}
