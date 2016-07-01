using MathNet.Numerics.Distributions;
using PricingAndHedging.FinalExam.DataProviders;
using System;

namespace PricingAndHedging.FinalExam
{
    public class Portfolio
    {
        #region Fields

        private DateTime currentDate;
        private DateTime maturity;
        private double strike;
        private double forward;
        private double interestRateInYears;
        private double volatility;
        private double initialHedgingNotional;
        private double initialCash;
        private double timeStepSize;

        private double timeToMaturityInYears;
        private bool isHedged;

        private double portfolioValue;
        private double hedgingNotional;
        private double cash;

        #endregion

        #region Constructor

        public Portfolio(DateTime currentDate, DateTime maturity, double strike, double initialHedgingNotional, double initialCash, double timeStepSize)
        {
            this.currentDate = currentDate;
            this.maturity = maturity;
            this.strike = strike;
            this.initialHedgingNotional = initialHedgingNotional;
            this.initialCash = initialCash;
            this.timeStepSize = timeStepSize;

            this.forward = FWDS.GetFwd(this.currentDate, this.maturity);
            this.interestRateInYears = RATES.GetRate(this.currentDate, this.maturity);
            this.volatility = VOLS.GetVol(this.currentDate, this.maturity);
            this.timeToMaturityInYears = TAU.Act365(currentDate, maturity);
            this.isHedged = false;
        }

        #endregion

        #region Methods to delta hedge the option

        private void Hedge(double currentForwardPrice)
        {
            this.isHedged = true;

            var option = new BlackEuropeanCallOption(this.currentDate, this.maturity, this.strike);

            //if (this.currentDate == this.maturity)
            //{
            //    this.hedgingNotional = 0.0;

            //    double initialCashAdjusted = this.initialCash * Math.Exp(this.interestRateInYears * this.timeStepSize);

            //    double ndfAdjust = (PTAX.GetValue(BmfCalendar.PlusBusinessDays(this.maturity, -1)) - this.forward);

            //    this.cash = initialCashAdjusted + ndfAdjust;

            //    this.portfolioValue = (option.Price) - this.initialHedgingNotional * (currentForwardPrice) + this.cash;
            //}
            //else
            //{
            //    this.hedgingNotional = option.Delta;
            //    double notionalVariation = this.hedgingNotional - this.initialHedgingNotional;

            //    double cashUsedForHedging = assetAmountVariation * currentAssetPrice;

            //    double rolledInitialCash = this.initialCash * Math.Exp(this.interestRateInYears * this.timeStepSize);

            //    this.cash = (cashUsedForHedging + rolledInitialCash);

            //    bool isFirstInteration = ((this.initialAssetAmount < 1e-6) && (this.initialCash < 1e-6));
            //    if (isFirstInteration)
            //        this.cash = this.cash - pricingOption.Price;

            //    this.portfolioValue = (pricingOption.Price) - (this.assetAmountForHedging * currentAssetPrice) + this.cash;
            //}                                 
        }

        #endregion

        #region Properties

        public double Value(double currentForwardPrice)
        {
            if (!this.isHedged)
                this.Hedge(currentForwardPrice);

            return this.portfolioValue;
        }

        public double HedgingNotional(double currentForwardPrice)
        {
            if (!this.isHedged)
                this.Hedge(currentForwardPrice);

            return this.hedgingNotional;
        }

        public double Cash(double currentForwardPrice)
        {
            if (!this.isHedged)
                this.Hedge(currentForwardPrice);

            return this.cash;
        }

        #endregion
    }
}
