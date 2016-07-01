using MathNet.Numerics.Distributions;
using PricingAndHedging.FinalExam.DataProviders;
using System;

namespace PricingAndHedging.FinalExam
{
    public class BlackEuropeanCallOption
    {
        #region Constructor

        public BlackEuropeanCallOption(DateTime currentDate, DateTime maturity, double strike)
        {
            this.CurrentDate = currentDate;
            this.Maturity = maturity;
            this.Strike = strike;

            this.InterestRate = RATES.GetRate(this.CurrentDate, this.Maturity);
            this.Volatility = VOLS.GetVol(this.CurrentDate, this.Maturity);
            this.TimeToMaturityInYears = TAU.Act365(this.CurrentDate, this.Maturity);
            this.Forward = FWDS.GetFwd(this.CurrentDate, this.Maturity);
            this.Ptax = PTAX.GetValue(this.CurrentDate);
        }

        #endregion

        #region Properties

        public DateTime CurrentDate { get; private set; }

        public DateTime Maturity { get; private set; }

        public double Forward { get; private set; }

        public double Strike { get; private set; }

        public double TimeToMaturityInYears { get; private set; }

        public double InterestRate { get; private set; }

        public double Volatility { get; private set; }

        public double Ptax { get; private set; }

        #endregion

        #region Black & Scholes Formulas

        private double d1 = double.NaN;
        private double D1
        {
            get
            {
                if (double.IsNaN(this.d1))
                {
                    double numerator = Math.Log(this.Forward / this.Strike) + (Math.Pow(this.Volatility, 2.0) / 2.0) * (this.TimeToMaturityInYears);
                    double denominator = this.Volatility * Math.Sqrt(this.TimeToMaturityInYears);
                    this.d1 = (numerator / denominator);
                }

                return this.d1;
            }
        }

        private double d2 = double.NaN;
        private double D2
        {
            get
            {
                if (double.IsNaN(this.d2))
                {
                    this.d2 = this.D1 - this.Volatility * Math.Sqrt(this.TimeToMaturityInYears);
                }

                return this.d2;
            }
        }

        private double price = double.NaN;
        public double Price
        {
            get
            {
                if (double.IsNaN(this.price))
                {
                    bool expired = (this.TimeToMaturityInYears < 1e-8);

                    if (expired)
                        this.price = Math.Max(0.0, (this.Ptax - this.Strike));
                    else
                        this.price = (Math.Exp(-this.InterestRate * this.TimeToMaturityInYears)) * (this.Forward * Normal.CDF(0.0, 1.0, this.D1) - this.Strike * Normal.CDF(0.0, 1.0, this.D2));
                }

                return this.price;
            }
        }

        private double delta = double.NaN;
        public double Delta
        {
            get
            {
                if (double.IsNaN(this.delta))
                {
                    bool expired = (this.TimeToMaturityInYears < 1e-8);

                    if (expired)
                        this.delta = 0.0;
                    else
                        this.delta = Normal.CDF(0, 1, this.D1);
                }

                return this.delta;
            }
        }

        #endregion
    }
}
