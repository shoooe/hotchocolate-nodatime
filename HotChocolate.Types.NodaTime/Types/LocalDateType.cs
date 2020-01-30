using System.Globalization;
using NodaTime;
using NodaTime.Text;

namespace HotChocolate.Types.NodaTime
{
    public class LocalDateType : StringBaseType<LocalDate>
    {
        public LocalDateType()
            : base("LocalDate")
        {
        }

        protected override string DoFormat(LocalDate val)
            => LocalDatePattern.Iso
                .WithCulture(CultureInfo.InvariantCulture)
                .Format(val);

        protected override LocalDate DoParse(string str)
            => LocalDatePattern.Iso
                .WithCulture(CultureInfo.InvariantCulture)
                .Parse(str).GetValueOrThrow();
    }
}