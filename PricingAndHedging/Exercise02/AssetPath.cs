using MathNet.Numerics.Distributions;
using PricingAndHedging.BrownianMotion;
using System;
using System.Collections.Generic;

namespace PricingAndHedging.Exercise02
{
    public class AssetPath
    {
        #region Fields

        private List<BrownianMotionPoint> assetValues;

        #endregion

        #region Constructor

        public AssetPath(double initialPrice, double interestRate, double volatility, double timeToMaturity, int numberOfSteps)
        {
            this.InitialPrice = initialPrice;
            this.InterestRate = interestRate;
            this.Volatility = volatility;
            this.TimeToMaturity = timeToMaturity;
            this.NumberOfSteps = numberOfSteps;
        }

        #endregion

        #region Properties

        public double InitialPrice { get; private set; }

        public double InterestRate { get; private set; }

        public double Volatility { get; private set; }

        public double TimeToMaturity { get; private set; }

        public int NumberOfSteps { get; private set; } 

        #endregion

        #region Methods to generate and retrieve asset value

        public BrownianMotionPoint this[int i]
        {
            get
            {
                if (this.assetValues == null)
                {
                    this.GeneratePath();
                }

                return this.assetValues[i];
            }
        }

        private void GeneratePath()
        {
            this.assetValues = new List<BrownianMotionPoint>(this.NumberOfSteps + 1);

            var normalRandomNumbers = new double[this.NumberOfSteps];
            Normal.Samples(normalRandomNumbers, 0, 1);
            
            double dt = this.TimeToMaturity / this.NumberOfSteps;

            double assetPrice = this.InitialPrice;
            this.assetValues.Add(new BrownianMotionPoint(0.0, 0.0, this.InitialPrice));
            
            for (int calculationStep = 1; calculationStep <= this.NumberOfSteps; calculationStep++)
            {
                double randomValue = normalRandomNumbers[calculationStep - 1];

                assetPrice = assetPrice * Math.Exp((this.InterestRate - (Math.Pow(this.Volatility, 2.0)) / 2) * dt + this.Volatility * Math.Sqrt(dt) * randomValue);

                this.assetValues.Add(new BrownianMotionPoint(calculationStep * dt, randomValue, assetPrice));
            }
        }

        #endregion
    }
}
