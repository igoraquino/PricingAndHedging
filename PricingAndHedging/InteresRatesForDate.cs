using System;

namespace PricingAndHedging.FinalExam
{
    public class InterestRatesForDate
    {
        #region Constructor

        public InterestRatesForDate(DateTime referenceDate, double cdi1M, double cdi3M, double cdi12M)
        {
            this.ReferenceDate = referenceDate;
            this.CDI1M = cdi1M;
            this.CDI3M = cdi3M;
            this.CDI12M = cdi12M;
        }

        #endregion

        #region Properties

        public DateTime ReferenceDate { get; private set; }

        public double CDI1M { get; private set; }

        public double CDI3M { get; private set; }

        public double CDI12M { get; private set; }

        public DateTime Tenor1M
        {
            get { return this.ReferenceDate.AddMonths(1); }
        }

        public DateTime Tenor3M
        {
            get { return this.ReferenceDate.AddMonths(3); }
        }

        public DateTime Tenor12M
        {
            get { return this.ReferenceDate.AddMonths(12); }
        }

        #endregion

        #region Methods to get linear interpolated cdi

        private double GetInterpolatedCDI(DateTime targetDate, DateTime leftDate, double leftCDI, DateTime rightDate, double rightCDI)
        {
            double numerator = rightCDI * (targetDate - leftDate).TotalDays + leftCDI * (rightDate - targetDate).TotalDays;

            double denominator = (rightDate - leftDate).TotalDays;

            return (numerator / denominator);
        }

        public double GetCDI(DateTime expiry)
        {
            if (expiry <= this.Tenor1M)
            {
                return this.CDI1M;
            }
            else if (expiry <= this.Tenor3M)
            {
                return this.GetInterpolatedCDI(expiry, this.Tenor1M, this.CDI1M, this.Tenor3M, this.CDI3M);
            }
            else
            {
                return this.GetInterpolatedCDI(expiry, this.Tenor3M, this.CDI3M, this.Tenor12M, this.CDI12M);
            }
        }

        #endregion
    }
}