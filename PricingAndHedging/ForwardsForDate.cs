using System;

namespace PricingAndHedging.FinalExam
{
    public class ForwardsForDate
    {
        #region Constructor

        public ForwardsForDate(DateTime referenceDate, double spot, double forward1M, double forward3M, double forward12M)
        {
            this.ReferenceDate = referenceDate;
            this.Spot = spot;
            this.Fwd1M = forward1M;
            this.Fwd3M = forward3M;
            this.Fwd12M = forward12M;
        }

        #endregion

        #region Properties

        public DateTime ReferenceDate { get; private set; }

        public double Spot { get; private set; }

        public double Fwd1M { get; private set; }

        public double Fwd3M { get; private set; }

        public double Fwd12M { get; private set; }

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

        #region Methods to get linear interpolated forward

        private double GetInterpolatedForward(DateTime targetDate, DateTime leftDate, double leftFwd, DateTime rightDate, double rightFwd)
        {
            double numerator = rightFwd * (targetDate - leftDate).TotalDays + leftFwd * (rightDate - targetDate).TotalDays;

            double denominator = (rightDate - leftDate).TotalDays;

            return (numerator / denominator);
        }

        public double GetFwd(DateTime expiry)
        {
            if (expiry <= this.Tenor1M)
            {
                return this.GetInterpolatedForward(expiry, this.ReferenceDate, this.Spot, this.Tenor1M, this.Fwd1M);
            }
            else if (expiry <= this.Tenor3M)
            {
                return this.GetInterpolatedForward(expiry, this.Tenor1M, this.Fwd1M, this.Tenor3M, this.Fwd3M);
            }
            else
            {
                return this.GetInterpolatedForward(expiry, this.Tenor3M, this.Fwd3M, this.Tenor12M, this.Fwd12M);
            }
        }

        #endregion
    }
}
