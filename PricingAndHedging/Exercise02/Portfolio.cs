using System;

namespace PricingAndHedging.Exercise02
{
    public class Portfolio
    {
        #region Fields

        private double strike;
        private double timeToMaturityInYears;
        private double interestRateInYears;
        private double pricingVolatility;
        private double hedgingVolatility;
        private double initialAssetAmount;
        private double initialCash;
        private double timeStepSize;
        private double portfolioValue;
        private double assetAmountForHedging;
        private double cash;
        private bool isHedged;

        #endregion

        #region Constructor

        public Portfolio(double strike, double timeToMaturityInYears, double interestRateInYears, double pricingVolatility, double hedgingVolatility, double initialAssetAmount, double initialCash, double timeStepSize)
        {
            this.strike = strike;
            this.timeToMaturityInYears = timeToMaturityInYears;
            this.interestRateInYears = interestRateInYears;
            this.pricingVolatility = pricingVolatility;
            this.hedgingVolatility = hedgingVolatility;
            this.initialAssetAmount = initialAssetAmount;
            this.initialCash = initialCash;
            this.timeStepSize = timeStepSize;

            this.isHedged = false;
        }

        #endregion

        #region Methods to delta hedge the option

        private void Hedge(double currentAssetPrice)
        {
            this.isHedged = true;

            var pricingOption = new EuropeanCallOption(currentAssetPrice, strike, timeToMaturityInYears, interestRateInYears, pricingVolatility);

            if (this.timeToMaturityInYears < 1e-8)
            {
                this.assetAmountForHedging = this.initialAssetAmount;

                this.cash = this.initialCash * Math.Exp(this.interestRateInYears * this.timeStepSize);

                this.portfolioValue = (pricingOption.Price) - (this.initialAssetAmount * currentAssetPrice) + this.cash;
            }
            else
            {
                var hedgingOption = new EuropeanCallOption(currentAssetPrice, strike, timeToMaturityInYears, interestRateInYears, hedgingVolatility);

                this.assetAmountForHedging = hedgingOption.Delta;
                double assetAmountVariation = this.assetAmountForHedging - this.initialAssetAmount;

                double cashUsedForHedging = assetAmountVariation * currentAssetPrice;

                double rolledInitialCash = this.initialCash * Math.Exp(this.interestRateInYears * this.timeStepSize);

                this.cash = (cashUsedForHedging + rolledInitialCash);

                bool isFirstInteration = ((this.initialAssetAmount < 1e-6) && (this.initialCash < 1e-6));
                if (isFirstInteration)
                    this.cash = this.cash - pricingOption.Price;

                this.portfolioValue = (pricingOption.Price) - (this.assetAmountForHedging * currentAssetPrice) + this.cash;
            }
            //Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
            //                        currentAssetPrice.ToString("0.00000"),
            //                        pricingOption.Price.ToString("0.00000"),
            //                        this.assetAmountForHedging.ToString("0.00000"),
            //                        this.cash.ToString("0.00000"),
            //                        this.portfolioValue.ToString("0.00000"),
            //                        this.timeToMaturityInYears.ToString("0.00000"));                                    
        }

        #endregion

        #region Properties

        public double Value(double currentAssetPrice)
        {
            if (!this.isHedged)
                this.Hedge(currentAssetPrice);

            return this.portfolioValue;
        }

        public double AssetAmountForHedging(double currentAssetPrice)
        {
            if (!this.isHedged)
                this.Hedge(currentAssetPrice);

            return this.assetAmountForHedging;
        }

        public double Cash(double currentAssetPrice)
        {
            if (!this.isHedged)
                this.Hedge(currentAssetPrice);

            return this.cash;
        }

        #endregion
    }
}
