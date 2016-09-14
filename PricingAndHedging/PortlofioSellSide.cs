using MathNet.Numerics.Distributions;
using PricingAndHedging.FinalExam.DataProviders;
using System;

namespace PricingAndHedging.FinalExam
{
    public class PortfolioSellSide
    {
        #region Fields

        private DateTime currentDate;
        private DateTime maturity;
        private double strike;
        private double interestRateInYears;
        private double volatility;
        private double initialHedgingNotional;
        private double initialCash;
        private double timeStepSize;
        private bool hasHedge;

        private double timeToMaturityInYears;
        private bool isEvaluated;
        private bool hasFwdAdjust;
        private IOption option;

        private double portfolioValue;
        private double hedgingNotional;
        private double cash;

        #endregion

        #region Constructor

        public PortfolioSellSide(DateTime currentDate, DateTime maturity, double strike, double initialHedgingNotional, double initialCash, double timeStepSize, bool hasHedge, bool hasFwdAdjust)
        {
            this.currentDate = currentDate;
            this.maturity = maturity;
            this.strike = strike;
            this.initialHedgingNotional = initialHedgingNotional;
            this.initialCash = initialCash;
            this.timeStepSize = timeStepSize;
            this.hasHedge = hasHedge;
            this.hasFwdAdjust = hasFwdAdjust;

            this.interestRateInYears = RATES.GetRate(this.currentDate, this.maturity);
            this.volatility = VOLS.GetVol(this.currentDate, this.maturity);
            this.timeToMaturityInYears = TAU.Act365(currentDate, maturity);
            this.isEvaluated = false;
        }

        #endregion

        #region Methods to delta hedge the option

        private void Evaluate()
        {
            this.isEvaluated = true;

            this.option = new BlackEuropeanCallOption(this.currentDate, this.maturity, this.strike);

            if (this.hasFwdAdjust)
            {
                if (this.currentDate == this.maturity)
                {
                    this.hedgingNotional = 0.0;

                    double rolledInitialCash = this.initialCash * Math.Exp(this.interestRateInYears * this.timeStepSize);

                    DateTime yesterday = BmfCalendar.PlusBusinessDays(this.currentDate, -1);

                    double ndfAdjust = (PTAX.GetValue(this.currentDate) - FWDS.GetFwd(yesterday, this.maturity));

                    this.cash = rolledInitialCash + (this.initialHedgingNotional * ndfAdjust);
                }
                else
                {
                    this.hedgingNotional = this.hasHedge ? -option.Delta : this.initialHedgingNotional;

                    bool isFirstInteration = ((Math.Abs(this.initialHedgingNotional) < 1e-6) && (Math.Abs(this.initialCash) < 1e-6));

                    if (isFirstInteration)
                    {
                        this.cash = option.Price;
                    }
                    else
                    {
                        double rolledInitialCash = this.initialCash * Math.Exp(this.interestRateInYears * this.timeStepSize);

                        DateTime yesterday = BmfCalendar.PlusBusinessDays(this.currentDate, -1);

                        double ndfAdjust = (FWDS.GetFwd(this.currentDate, this.maturity) - FWDS.GetFwd(yesterday, this.maturity));

                        double discountFactor = Math.Exp(-this.interestRateInYears * this.timeToMaturityInYears);

                        this.cash = rolledInitialCash + (this.initialHedgingNotional * ndfAdjust * discountFactor);
                    }
                }
            }
            else
            {
                this.hedgingNotional = initialHedgingNotional;

                bool isFirstInteration = ((Math.Abs(this.initialHedgingNotional) < 1e-10) && (Math.Abs(this.initialCash) < 1e-10));

                if (isFirstInteration)
                {
                    this.cash = option.Price;
                }
                else
                {
                    this.cash = this.initialCash * Math.Exp(this.interestRateInYears * this.timeStepSize);
                }
            }

            this.portfolioValue = -option.Price + this.cash;
        }

        #endregion

        #region Properties

        public double Value
        {
            get
            {
                if (!this.isEvaluated)
                    this.Evaluate();

                return this.portfolioValue;
            }
        }

        public double HedgingNotional
        {
            get
            {
                if (!this.isEvaluated)
                    this.Evaluate();

                return this.hedgingNotional;
            }
        }

        public double Cash
        {
            get
            {
                if (!this.isEvaluated)
                    this.Evaluate();

                return this.cash;
            }
        }

        public IOption Option
        {
            get
            {
                if (!this.isEvaluated)
                    this.Evaluate();

                return this.option;
            }
        }

        public DateTime CurrentDate
        {
            get { return this.currentDate; }
        }

        #endregion
    }
}
