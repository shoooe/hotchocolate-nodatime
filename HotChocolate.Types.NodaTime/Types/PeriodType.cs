
using System.Globalization;
using NodaTime;
using NodaTime.Text;

namespace HotChocolate.Types.NodaTime
{
    public class PeriodType : StringBaseType<Period>
    {
        public PeriodType()
            : base("Period")
        {
        }

        protected override string DoFormat(Period val)
            => PeriodPattern.Roundtrip
                .Format(val);

        protected override Period DoParse(string str)
            => PeriodPattern.Roundtrip
                .Parse(str).GetValueOrThrow();
    }
}