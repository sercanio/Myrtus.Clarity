using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Myrtus.Clarity.WebAPI.OpenApi;

internal sealed class SwaggerFileOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var consumesMultipart = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<ConsumesAttribute>()
            .Any(attr => attr.ContentTypes.Contains("multipart/form-data"));

        if (!consumesMultipart)
            return;

        var formFileParameters = context.MethodInfo.GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(IEnumerable<IFormFile>))
            .ToList();

        if (!formFileParameters.Any())
            return;

        operation.Parameters.Clear();

        operation.RequestBody = new OpenApiRequestBody
        {
            Content =
        {
            ["multipart/form-data"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties = formFileParameters.ToDictionary(
                        p => p.Name,
                        p => new OpenApiSchema
                        {
                            Type = "string",
                            Format = "binary"
                        }),
                    Required = new HashSet<string>(formFileParameters.Select(p => p.Name))
                }
            }
        }
        };
    }
}
