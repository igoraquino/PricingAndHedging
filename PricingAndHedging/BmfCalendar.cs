using System;
using System.Collections.Generic;

namespace PricingAndHedging
{
    public static class BmfCalendar
    {
        private static List<DateTime> Holidays = new List<DateTime>()
        {            
            new DateTime(2014, 1, 1),
            new DateTime(2014, 3, 3),
            new DateTime(2014, 3, 4),
            new DateTime(2014, 4, 18),
            new DateTime(2014, 4, 21),
            new DateTime(2014, 5, 1),
            new DateTime(2014, 6, 19),
            new DateTime(2014, 9, 7),
            new DateTime(2014, 10, 12),
            new DateTime(2014, 11, 2),
            new DateTime(2014, 11, 15),
            new DateTime(2014, 12, 25),
            new DateTime(2015, 1, 1),
            new DateTime(2015, 2, 16),
            new DateTime(2015, 2, 17),
            new DateTime(2015, 4, 3),
            new DateTime(2015, 4, 21),
            new DateTime(2015, 5, 1),
            new DateTime(2015, 6, 4),
            new DateTime(2015, 9, 7),
            new DateTime(2015, 10, 12),
            new DateTime(2015, 11, 2),
            new DateTime(2015, 11, 15),
            new DateTime(2015, 12, 25),
            new DateTime(2016, 1, 1),
            new DateTime(2016, 2, 8),
            new DateTime(2016, 2, 9),
            new DateTime(2016, 3, 25),
            new DateTime(2016, 4, 21),
            new DateTime(2016, 5, 1),
            new DateTime(2016, 5, 26),
            new DateTime(2016, 9, 7),
            new DateTime(2016, 10, 12),
            new DateTime(2016, 11, 2),
            new DateTime(2016, 11, 15),
            new DateTime(2016, 12, 25),
            new DateTime(2014, 1, 25),
            new DateTime(2014, 6, 12),
            new DateTime(2014, 7, 9),
            new DateTime(2014, 11, 20),
            new DateTime(2014, 12, 24),
            new DateTime(2014, 12, 31),
            new DateTime(2015, 1, 25),
            new DateTime(2015, 7, 9),
            new DateTime(2015, 11, 20),
            new DateTime(2015, 12, 24),
            new DateTime(2015, 12, 31),
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

        public static DateTime PlusBusinessDays(DateTime date, int businessDays)
        {
            DateTime targetDate = date;
            int i = 0;

            while (i != businessDays)
            {
                if (businessDays > 0)
                {
                    targetDate = targetDate.AddDays(1);
                    if (BmfCalendar.IsBusinessDate(targetDate))
                        i++;
                }
                else
                {
                    targetDate = targetDate.AddDays(-1);
                    if (BmfCalendar.IsBusinessDate(targetDate))
                        i--;
                }
            }

            return targetDate;
        }
    }
}
