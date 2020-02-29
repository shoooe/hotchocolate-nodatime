using System.Globalization;
using NodaTime;
using NodaTime.Text;

namespace HotChocolate.Types.NodaTime
{
    public class ZonedDateTimeType : StringBaseType<ZonedDateTime>
    {
        public ZonedDateTimeType()
            : base("ZonedDateTime")
        {
            Description =
                "A LocalDateTime in a specific time zone and with a particular offset to " +
                    "distinguish between otherwise-ambiguous instants.\n" +
                "A ZonedDateTime is global, in that it maps to a single Instant.";
        }

        protected override string DoFormat(ZonedDateTime val)
            => ZonedDateTimePattern
                .CreateWithInvariantCulture("uuuu'-'MM'-'dd'T'HH':'mm':'ss' 'z' 'o<g>", DateTimeZoneProviders.Tzdb)
                .Format(val);

        protected override ZonedDateTime DoParse(string str)
            => ZonedDateTimePattern
                .CreateWithInvariantCulture("uuuu'-'MM'-'dd'T'HH':'mm':'ss' 'z' 'o<g>", DateTimeZoneProviders.Tzdb)
                .Parse(str).GetValueOrThrow();
    }
}