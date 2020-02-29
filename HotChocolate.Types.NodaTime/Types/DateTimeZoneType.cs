using System;
using NodaTime;

namespace HotChocolate.Types.NodaTime
{
    public class DateTimeZoneType : StringBaseType<DateTimeZone>
    {
        public DateTimeZoneType()
            : base("DateTimeZone")
        {
            Description =
                "Represents a time zone - a mapping between UTC and local time.\n" +
                "A time zone maps UTC instants to local times - or, equivalently, " +
                    "to the offset from UTC at any particular instant.";
        }

        protected override string DoFormat(DateTimeZone val)
            => val.Id;

        protected override DateTimeZone DoParse(string str)
        {
            var result = DateTimeZoneProviders.Tzdb.GetZoneOrNull(str);
            if (result == null)
                throw new Exception();
            return result;
        }
    }
}