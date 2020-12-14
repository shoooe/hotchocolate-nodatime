using NodaTime.Text;

namespace HotChocolate.Types.NodaTime.Extensions
{
    internal static class IPatternExtensions
    {
        public static bool TryParse<NodaTimeType>(this IPattern<NodaTimeType> pattern, string text, out NodaTimeType? output)
            where NodaTimeType : struct
        {
            var result = pattern.Parse(text);
            if (result.Success)
            {
                output = result.Value;
                return true;
            }
            else
            {
                output = null;
                return false;
            }
        }

        public static bool TryParse<NodaTimeType>(this IPattern<NodaTimeType> pattern, string text, out NodaTimeType? output)
            where NodaTimeType : class
        {
            var result = pattern.Parse(text);
            if (result.Success)
            {
                output = result.Value;
                return true;
            }
            else
            {
                output = null;
                return false;
            }
        }
    }
}