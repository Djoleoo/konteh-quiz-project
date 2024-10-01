using NJsonSchema.Generation;

namespace Konteh.Infrastructure;

public class CustomSwaggerSchemaNameGenerator : DefaultSchemaNameGenerator
{
    public override string Generate(Type type)
    {
        List<Type>? declaringTypes = new List<Type>();
        GetAllDeclaringTypes(declaringTypes, type);
        return declaringTypes.Count > 0
            ? $"{string.Join("", declaringTypes.Select(t => t.Name))}{type.Name}"
            : base.Generate(type);
    }

    private static void GetAllDeclaringTypes(List<Type> declaringTypes, Type type)
    {
        while (true)
        {
            if (type.DeclaringType != null)
            {
                declaringTypes.Insert(0, type.DeclaringType);
                type = type.DeclaringType;
                continue;
            }

            break;
        }
    }
}