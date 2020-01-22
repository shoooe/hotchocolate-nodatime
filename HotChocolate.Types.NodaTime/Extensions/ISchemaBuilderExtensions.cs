namespace HotChocolate.Types.NodaTime.Extensions
{
    public static class ISchemaBuilderExtensions
    {
        public static ISchemaBuilder AddNodaTime(this ISchemaBuilder schemaBuilder)
        {
            return schemaBuilder.AddType<DurationType>();
        }
    }
}