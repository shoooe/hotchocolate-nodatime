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