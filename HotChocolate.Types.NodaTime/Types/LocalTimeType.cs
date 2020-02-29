using System.Globalization;
using NodaTime;
using NodaTime.Text;

namespace HotChocolate.Types.NodaTime
{
    public class LocalTimeType : StringBaseType<LocalTime>
    {
        public LocalTimeType()
            : base("LocalTime")
        {
            Description = "LocalTime is an immutable struct representing a time of day, with no reference to a particular calendar, time zone or date.";
        }

        protected override string DoFormat(LocalTime val)
            => LocalTimePattern.ExtendedIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Format(val);

        protected override LocalTime DoParse(string str)
            => LocalTimePattern.ExtendedIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Parse(str).GetValueOrThrow();
    }
}