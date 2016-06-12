using MathNet.Numerics.Distributions;
using PricingAndHedging.Exercise01.BrownianMotion;
using PricingAndHedging.Exercise01.Statistics;
using System;

namespace PricingAndHedging.Exercise01.Options
{
    public class KnockOutPutOption
    {
        public KnockOutPutOption(double spot, double strike, double highBarrier, double lowBarrier, double interestRate, double dividendYield, double volatility, double timeToMaturity)
        {
            this.Spot = spot;
            this.Strike = strike;
            this.HighBarrier = highBarrier;
            this.LowBarrier = lowBarrier;
            this.InterestRate = interestRate;
            this.DividendYield = dividendYield;
            this.Volatility = volatility;
            this.TimeToMaturity = timeToMaturity;
        }

        #region Public members

        public double Spot { get; set; }

        public double Strike { get; private set; }

        public double HighBarrier { get; private set; }

        public double LowBarrier { get; private set; }

        public double InterestRate { get; private set; }

        public double DividendYield { get; private set; }

        public double Volatility { get; private set; }

        public double TimeToMaturity { get; set; }

        #endregion

        public double Evaluate(BrownianBridge brownianBridge, out Exercise01Stats stats)
        {
            return brownianBridge.Evaluate(this, out stats);
        }

        public double PutTheoreticalPremium
        {
            get
            {
                double d1 = (1 / (this.Volatility * Math.Sqrt(this.TimeToMaturity))) * (Math.Log(this.Spot / this.Strike) + (this.InterestRate + Math.Pow(this.Volatility, 2.0) / 2.0) * this.TimeToMaturity);

                double d2 = d1 - this.Volatility * Math.Sqrt(this.TimeToMaturity);

                return Normal.CDF(0.0, 1.0, -d2) * this.Strike * Math.Exp(-this.InterestRate * this.TimeToMaturity) - Normal.CDF(0.0, 1.0, -d1) * this.Spot;
            }
        }
    }
}
