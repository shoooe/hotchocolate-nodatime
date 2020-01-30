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
        }

        protected override string DoFormat(ZonedDateTime val)
            => ZonedDateTimePattern.ExtendedFormatOnlyIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Format(val);

        protected override ZonedDateTime DoParse(string str)
            => ZonedDateTimePattern.ExtendedFormatOnlyIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Parse(str).GetValueOrThrow();
    }
}