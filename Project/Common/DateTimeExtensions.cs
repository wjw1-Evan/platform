using System;

namespace Common
{
    public  class DateTimeLocal
    {
        public static DateTime Now => GetNow();

        public static DateTime GetNow()
        {
            return DateTime.UtcNow.AddHours(8);
        }

    }

}