using System.Globalization;
using NodaTime;
using NodaTime.Text;

namespace HotChocolate.Types.NodaTime
{
    public class InstantType : StringBaseType<Instant>
    {
        public InstantType()
            : base("Instant")
        {
            Description =
                "An instant is defined by an integral number of 'ticks' since the Unix epoch " +
                "(typically described as January 1st 1970, midnight, UTC, ISO calendar), " +
                "where a tick is equal to 100 nanoseconds.\n" +
                "An Instant has no concept of a particular time zone or calendar: it simply " +
                "represents a point in time that can be globally agreed-upon.";
        }

        protected override string DoFormat(Instant val)
            => InstantPattern.ExtendedIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Format(val);

        protected override Instant DoParse(string str)
            => InstantPattern.ExtendedIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Parse(str).GetValueOrThrow();
    }
}