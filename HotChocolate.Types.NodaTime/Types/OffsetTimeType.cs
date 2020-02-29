using System.Globalization;
using NodaTime;
using NodaTime.Text;

namespace HotChocolate.Types.NodaTime
{
    public class OffsetTimeType : StringBaseType<OffsetTime>
    {
        public OffsetTimeType()
            : base("OffsetTime")
        {
            Description =
                "A combination of a LocalTime and an Offset, " +
                    "to represent a time-of-day at a specific offset from UTC " +
                    "but without any date information.";
        }

        protected override string DoFormat(OffsetTime val)
            => OffsetTimePattern.GeneralIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Format(val);

        protected override OffsetTime DoParse(string str)
            => OffsetTimePattern.GeneralIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Parse(str).GetValueOrThrow();
    }
}