using System.Globalization;
using NodaTime;
using NodaTime.Text;

namespace HotChocolate.Types.NodaTime
{
    public class LocalDateType : StringBaseType<LocalDate>
    {
        public LocalDateType()
            : base("LocalDate")
        {
            Description =
                "LocalDate is an immutable struct representing a date " +
                    "within the calendar, with no reference to a particular " +
                    "time zone or time of day.";
        }

        protected override string DoFormat(LocalDate val)
            => LocalDatePattern.Iso
                .WithCulture(CultureInfo.InvariantCulture)
                .Format(val);

        protected override LocalDate DoParse(string str)
            => LocalDatePattern.Iso
                .WithCulture(CultureInfo.InvariantCulture)
                .Parse(str).GetValueOrThrow();
    }
}