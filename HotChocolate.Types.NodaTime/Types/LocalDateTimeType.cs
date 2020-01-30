using System.Globalization;
using NodaTime;
using NodaTime.Text;

namespace HotChocolate.Types.NodaTime
{
    public class LocalDateTimeType : StringBaseType<LocalDateTime>
    {
        public LocalDateTimeType()
            : base("LocalDateTime")
        {
        }

        protected override string DoFormat(LocalDateTime val)
            => LocalDateTimePattern.ExtendedIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Format(val);

        protected override LocalDateTime DoParse(string str)
            => LocalDateTimePattern.ExtendedIso
                .WithCulture(CultureInfo.InvariantCulture)
                .Parse(str).GetValueOrThrow();
    }
}