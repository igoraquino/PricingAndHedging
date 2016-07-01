using PricingAndHedging.FinalExam.DataProviders;
using System;

namespace PricingAndHedging.FinalExam
{
    public class VolatilitiesForDate
    {
        #region Constructor

        public VolatilitiesForDate(DateTime referenceDate, double vol1M, double vol3M, double vol12M)
        {
            this.ReferenceDate = referenceDate;
            this.Vol1M = vol1M;
            this.Vol3M = vol3M;
            this.Vol12M = vol12M;
        }

        #endregion

        #region Properties

        public DateTime ReferenceDate { get; private set; }

        public double Vol1M { get; private set; }

        public double Vol3M { get; private set; }

        public double Vol12M { get; private set; }

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

        #region Methods to retrieve interpolated volatility

        private double GetInterpolatedVol(DateTime referenceDate, DateTime targetDate, DateTime leftDate, double leftVol, DateTime rightDate, double rightVol)
        {
            double effectiveVarianceFromReferenceDateToLeftDate = Math.Pow(leftVol, 2.0) * TAU.Act365(referenceDate, leftDate);

            double effectiveVarianceFromReferenceDateToRightDate = Math.Pow(rightVol, 2.0) * TAU.Act365(referenceDate, rightDate);

            double effectiveVarianceFromLeftDateToRightDate = effectiveVarianceFromReferenceDateToRightDate - effectiveVarianceFromReferenceDateToLeftDate;

            double volatilityFromLeftDateToRightDate = Math.Sqrt(effectiveVarianceFromLeftDateToRightDate / TAU.Act365(leftDate, rightDate));

            double effectiveVarianceFromLeftDateToTargetDate = Math.Pow(volatilityFromLeftDateToRightDate, 2.0) * TAU.Act365(leftDate, targetDate);

            double effectiveVarianceFromReferenceDateToTargetDate = effectiveVarianceFromReferenceDateToLeftDate + effectiveVarianceFromLeftDateToTargetDate;

            double volatilityFromReferenceDateToTargetDate = Math.Sqrt(effectiveVarianceFromReferenceDateToTargetDate / TAU.Act365(referenceDate, targetDate));

            return volatilityFromReferenceDateToTargetDate;
        }

        public double GetVol(DateTime expiry)
        {
            if (expiry <= this.Tenor1M)
            {
                return this.Vol1M;
            }
            else if (expiry <= this.Tenor3M)
            {
                return this.GetInterpolatedVol(this.ReferenceDate, expiry, this.Tenor1M, this.Vol1M, this.Tenor3M, this.Vol3M);
            }
            else
            {
                return this.GetInterpolatedVol(this.ReferenceDate, expiry, this.Tenor3M, this.Vol3M, this.Tenor12M, this.Vol12M);                
            }
        }

        #endregion
    }
}
