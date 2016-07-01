using System;
using System.Collections.Generic;

namespace PricingAndHedging
{
    public static class BmfCalendar
    {
        private static List<DateTime> Holidays = new List<DateTime>()
        {            
            new DateTime(2014, 1, 1),
            new DateTime(2014, 1, 25),
            new DateTime(2014, 6, 12),
            new DateTime(2014, 7, 9),
            new DateTime(2014, 11, 20),
            new DateTime(2014, 12, 24),
            new DateTime(2014, 12, 31),
            new DateTime(2015, 1, 1),
            new DateTime(2015, 1, 25),
            new DateTime(2015, 7, 9),
            new DateTime(2015, 11, 20),
            new DateTime(2015, 12, 24),
            new DateTime(2015, 12, 31),
            new DateTime(2016, 1, 1),
            new DateTime(2016, 1, 25),
            new DateTime(2016, 7, 9),
            new DateTime(2016, 11, 20),
            new DateTime(2016, 12, 24),
            new DateTime(2016, 12, 31)
        };

        public static bool IsBusinessDate(DateTime date)
        {
            if ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday))
                return false;

            if (Holidays.Contains(date))
                return false;

            return true;
        }

        //public static DateTime PlusBusinessDays(int days)
        //{
        //    int i = 0;
        //    while (i != days)
        //    {
        //        if (days > 0)
        //        {

        //        }
        //    }
        //}
    }
}
