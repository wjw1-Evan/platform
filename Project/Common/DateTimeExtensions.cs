using System;

namespace Common
{
    public  class DateTimeLocal
    {
        public static string Now => GetNow();

        public static string NowDate => GetNowDate();

        public static string GetNow()
        {
            return DateTime.UtcNow.AddHours(8).ToString("yyyy/MM/dd HH:mm:ss");
        }
        public static string GetNowDate()
        {
            return DateTime.UtcNow.AddHours(8).ToString("yyyy/MM/dd");
        }
    }

}