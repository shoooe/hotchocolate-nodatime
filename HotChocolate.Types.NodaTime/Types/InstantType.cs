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
            Description = "Represents an instant on the global timeline, with nanosecond resolution.";
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