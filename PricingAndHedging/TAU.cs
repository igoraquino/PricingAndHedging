using System;

namespace PricingAndHedging.FinalExam.DataProviders
{
    public class TAU
    {
        public static double Act365(DateTime from, DateTime to)
        {
            double totalDays = (to - from).TotalDays;
            return (totalDays / 365.0);
        }
    }
}
