using System;
using NodaTime;

namespace HotChocolate.Types.NodaTime
{
    public class IsoDayOfWeekType : IntBaseType<IsoDayOfWeek>
    {
        public IsoDayOfWeekType()
            : base("IsoDayOfWeek")
        {
            Description =
                "Equates the days of the week with their numerical value according to ISO-8601.\n" +
                "Monday = 1, Tuesday = 2, Wednesday = 3, Thursday = 4, Friday = 5, Saturday = 6, Sunday = 7.";
        }

        protected override int DoFormat(IsoDayOfWeek val)
        {
            // The following check is necessary because otherwise 0
            // would be a valid value for `IsoDayOfWeek` and would be converted to
            // `IsoDayOfWeek.None`.
            if (val == IsoDayOfWeek.None)
                throw new Exception("`IsoDayOfWeek.None` is not a valid return value for this type");
            return (int)val;
        }

        protected override IsoDayOfWeek DoParse(int integer)
        {
            // The following check is necessary because otherwise 0
            // would be a valid value for `IsoDayOfWeek` and would be converted to
            // `IsoDayOfWeek.None`.
            if (integer <= 0 || integer > 7)
                throw new Exception("Integer should be within [1, 7]");
            return (IsoDayOfWeek)integer;
        }
    }
}