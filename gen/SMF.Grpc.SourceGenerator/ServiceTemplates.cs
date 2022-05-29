﻿namespace SMF.Grpc.SourceGenerator;

using Humanizer;
/// <summary>
/// The service templates.
/// </summary>

internal class ServiceTemplates
{
    /// <summary>
    /// Gets the all.
    /// </summary>
    /// <param name="modelCT">The model c t.</param>
    /// <returns>A string.</returns>
    public static string GetAll(ModelCT modelCT)
    {
        return $$"""
        public override async Task GetAll{{modelCT!.IdentifierNameWithoutPostFix.Pluralize()}}(Void request, global::Grpc.Core.IServerStreamWriter<{{modelCT.ModuleNameWithoutPostFix}}_{{modelCT.IdentifierNameWithoutPostFix}}> responseStream, global::Grpc.Core.ServerCallContext context)
    {
        var e = await _mediatR.Send(new GetAll{{modelCT.IdentifierNameWithoutPostFix.Pluralize()}}Query());
        foreach (var response in e)
        {        
            var obj = new {{modelCT.ModuleNameWithoutPostFix}}_{{modelCT.IdentifierNameWithoutPostFix}}();
            obj = response;
            await responseStream.WriteAsync(obj);
        }
    }
""";
    }

    public static string GetById(ModelCT modelCT)
    {
        return $$"""
        public override async Task<{{modelCT.ModuleNameWithoutPostFix}}_{{modelCT.IdentifierNameWithoutPostFix}}> Get{{modelCT.IdentifierNameWithoutPostFix}}ById(RequestId request, global::Grpc.Core.ServerCallContext context)
    {
        var response = await _mediatR.Send(new Get{{modelCT.IdentifierNameWithoutPostFix}}ByIdQuery(request.Id));
        if(response is null) return null;
        var obj = new {{modelCT.ModuleNameWithoutPostFix}}_{{modelCT.IdentifierNameWithoutPostFix}}(); 
        obj = response;
        return obj;
    }
""";
    }

    public static string Add(ModelCT modelCT)
    {
        return
$$"""
       public override async Task<ResponseId> Add{{modelCT.IdentifierNameWithoutPostFix}}(Create{{modelCT.ModuleNameWithoutPostFix}}_{{modelCT.IdentifierNameWithoutPostFix}}Command request, global::Grpc.Core.ServerCallContext context)
    {
        var createCommandObj = new Create{{modelCT.IdentifierNameWithoutPostFix}}Command();
        createCommandObj = request;
        var response = await _mediatR.Send(createCommandObj);
       return new ResponseId
         {
             Id = response,
         };

}
""";
    }

    public static string Update(ModelCT modelCT)
    {
        return
$$"""
    public override async Task<ResponseId> Update{{modelCT.IdentifierNameWithoutPostFix}}(Update{{modelCT.ModuleNameWithoutPostFix}}_{{modelCT.IdentifierNameWithoutPostFix}}Command request, global::Grpc.Core.ServerCallContext context)
    {
        var updateCommandObj = new Update{{modelCT.IdentifierNameWithoutPostFix}}Command(); 
        updateCommandObj = request;
        var response = await _mediatR.Send(updateCommandObj);
        return new ResponseId
        {
            Id = response,
        };

    }
""";
    }

    public static string Delete(ModelCT modelCT)
    {
        return
$$"""
    public override async Task<ResponseId> Delete{{modelCT.IdentifierNameWithoutPostFix}}(RequestId request, global::Grpc.Core.ServerCallContext context)
    {
        var response = await _mediatR.Send(new Delete{{modelCT.IdentifierNameWithoutPostFix}}Command(request.Id));

        return new ResponseId
        {
            Id = response,
        };
    }
""";
    }
}

