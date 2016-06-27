using System;
using MathNet.Numerics.Distributions;

namespace PricingAndHedging.Exercise02
{
    public class EuropeanCallOption
    {
        #region Constructor

        public EuropeanCallOption(double assetPrice, double strike, double timeToMaturityInYears, double interestRateInYears, double volatility)
        {
            this.AssetPrice = assetPrice;
            this.Strike = strike;
            this.TimeToMaturityInYears = timeToMaturityInYears;
            this.InterestRateInYears = interestRateInYears;
            this.Volatility = volatility;
        }

        #endregion

        #region Properties

        public double AssetPrice { get; private set; }

        public double Strike { get; private set; }

        public double TimeToMaturityInYears { get; private set; }

        public double InterestRateInYears { get; private set; }

        public double Volatility { get; private set; }

        #endregion

        #region Black & Scholes Formulas

        private double d1 = double.NaN;
        private double D1
        {
            get
            {
                if (double.IsNaN(this.d1))
                {
                    double numerator = Math.Log(this.AssetPrice / this.Strike) + ((this.InterestRateInYears) + (Math.Pow(this.Volatility, 2.0) / 2.0)) * (this.TimeToMaturityInYears);
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
                        this.price = Math.Max(0.0, (this.AssetPrice - this.Strike));
                    else
                        this.price = this.AssetPrice * Normal.CDF(0.0, 1.0, this.D1) - Normal.CDF(0.0, 1.0, this.D2) * this.Strike * Math.Exp(-this.InterestRateInYears * this.TimeToMaturityInYears);
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
