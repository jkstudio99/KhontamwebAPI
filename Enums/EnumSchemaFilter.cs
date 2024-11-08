using System;
using System.ComponentModel;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace KhontamwebAPI.Enums;

public class EnumSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Description += "\n\nPossible values:\n";
            foreach (var enumValue in Enum.GetValues(context.Type))
            {
                var member = context.Type.GetMember(enumValue.ToString())[0];
                var description = member.GetCustomAttribute<DescriptionAttribute>()?.Description;
                schema.Description += $"\n{(int)enumValue} = {description ?? enumValue.ToString()}";
            }
        }
    }
}
