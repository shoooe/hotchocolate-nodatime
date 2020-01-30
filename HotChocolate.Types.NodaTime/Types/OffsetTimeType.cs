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