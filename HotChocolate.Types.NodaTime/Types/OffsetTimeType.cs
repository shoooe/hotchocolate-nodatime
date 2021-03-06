using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using HotChocolate.Types.NodaTime.Extensions;
using NodaTime;
using NodaTime.Text;

namespace HotChocolate.Types.NodaTime
{
    public class OffsetTimeType : StringToStructBaseType<OffsetTime>
    {
        public OffsetTimeType() : base("OffsetTime")
        {
            Description =
                "A combination of a LocalTime and an Offset, " +
                    "to represent a time-of-day at a specific offset from UTC " +
                    "but without any date information.";
        }

        protected override string Serialize(OffsetTime baseValue)
            => OffsetTimePattern.GeneralIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Format(baseValue);

        protected override bool TryDeserialize(string str, [NotNullWhen(true)] out OffsetTime? output)
            => OffsetTimePattern.GeneralIso
                .WithCulture(CultureInfo.InvariantCulture)
                .TryParse(str, out output);
    }
}