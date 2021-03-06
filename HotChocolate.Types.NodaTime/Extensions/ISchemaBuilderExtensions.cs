namespace HotChocolate.Types.NodaTime.Extensions
{
    public static class ISchemaBuilderExtensions
    {
        public static ISchemaBuilder AddNodaTime(this ISchemaBuilder schemaBuilder)
        {
            return schemaBuilder
                .AddType<DateTimeZoneType>()
                .AddType<DurationType>()
                .AddType<InstantType>()
                .AddType<IsoDayOfWeekType>()
                .AddType<LocalDateTimeType>()
                .AddType<LocalDateType>()
                .AddType<LocalTimeType>()
                .AddType<OffsetDateTimeType>()
                .AddType<OffsetDateType>()
                .AddType<OffsetTimeType>()
                .AddType<OffsetType>()
                .AddType<PeriodType>()
                .AddType<ZonedDateTimeType>();
        }
    }
}