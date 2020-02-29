using System.Globalization;
using NodaTime;
using NodaTime.Text;

namespace HotChocolate.Types.NodaTime
{
    public class OffsetDateType : StringBaseType<OffsetDate>
    {
        public OffsetDateType()
            : base("OffsetDate")
        {
            Description =
                "A combination of a LocalDate and an Offset, to represent a date " +
                    "at a specific offset from UTC but without any time-of-day information.";
        }

        protected override string DoFormat(OffsetDate val)
            => OffsetDatePattern.GeneralIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Format(val);

        protected override OffsetDate DoParse(string str)
            => OffsetDatePattern.GeneralIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Parse(str).GetValueOrThrow();
    }
}