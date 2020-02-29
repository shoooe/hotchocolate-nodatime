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
            Description =
                "An offset from UTC in seconds.\n" +
                "A positive value means that the local time is ahead of UTC (e.g. for Europe); " +
                    "a negative value means that the local time is behind UTC (e.g. for America).";
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