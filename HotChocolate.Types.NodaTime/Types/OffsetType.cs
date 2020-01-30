using System.Globalization;
using NodaTime;
using NodaTime.Text;

namespace HotChocolate.Types.NodaTime
{
    public class OffsetType : StringBaseType<Offset>
    {
        public OffsetType()
            : base("Offset")
        {
        }

        protected override string DoFormat(Offset val)
            => OffsetPattern.GeneralInvariantWithZ
                .WithCulture(CultureInfo.InvariantCulture)
                .Format(val);

        protected override Offset DoParse(string str)
            => OffsetPattern.GeneralInvariantWithZ
                .WithCulture(CultureInfo.InvariantCulture)
                .Parse(str).GetValueOrThrow();
    }
}