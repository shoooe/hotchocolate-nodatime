using System.Globalization;
using NodaTime;
using NodaTime.Text;

namespace HotChocolate.Types.NodaTime
{
    public class OffsetDateTimeType : StringBaseType<OffsetDateTime>
    {
        public OffsetDateTimeType()
            : base("OffsetDateTime")
        {
            Description = "A local date and time in a particular calendar system, combined with an offset from UTC.";
        }

        protected override string DoFormat(OffsetDateTime val)
            => OffsetDateTimePattern.GeneralIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Format(val);

        protected override OffsetDateTime DoParse(string str)
            => OffsetDateTimePattern.ExtendedIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Parse(str).GetValueOrThrow();
    }
}