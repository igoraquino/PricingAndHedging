using MathNet.Numerics.Distributions;
using PricingAndHedging.BrownianMotion;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PricingAndHedging.PartialPresentationSeptember
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
            this.GeneratePath();
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
                return this.assetValues[i];
            }
        }

        private void GeneratePath()
        {
            var s = new Stopwatch();
            s.Start();
            this.assetValues = new List<BrownianMotionPoint>(this.NumberOfSteps + 1);

            var normalRandomNumbers = new double[this.NumberOfSteps];
            Console.Write(",Instantiation: " + s.ElapsedTicks.ToString());
            s.Restart();

            Normal.Samples(normalRandomNumbers, 0, 1);
            Console.Write(",NormalGen: " + s.ElapsedTicks.ToString());
            s.Restart();

            double dt = this.TimeToMaturity / this.NumberOfSteps;

            double assetPrice = this.InitialPrice;
            this.assetValues.Add(new BrownianMotionPoint(0.0, 0.0, this.InitialPrice));
            Console.Write(",InitialParam: " + s.ElapsedTicks.ToString());
            s.Restart();

            for (int calculationStep = 1; calculationStep <= this.NumberOfSteps; calculationStep++)
            {
                double randomValue = normalRandomNumbers[calculationStep - 1];

                assetPrice = assetPrice * Math.Exp((this.InterestRate - (Math.Pow(this.Volatility, 2.0)) / 2) * dt + this.Volatility * Math.Sqrt(dt) * randomValue);

                this.assetValues.Add(new BrownianMotionPoint(calculationStep * dt, randomValue, assetPrice));
            }
            Console.Write(",PathBuild: " + s.ElapsedTicks.ToString());
        }

        #endregion
    }

    public class BestOfTimeTest
    {
        public void Evaluate(double initialAssetPrice, double strike, double interestRate, double volatility, double timeToMaturity, int numberOfPaths, int numberOfSteps, double barrierValue)
        {
            var payoffs = new double[numberOfPaths];

            for (int i = 0; i < numberOfPaths; i++)
            {
                var globalWatch = new Stopwatch();
                globalWatch.Start();

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var path = new AssetPath(initialAssetPrice, interestRate, volatility, timeToMaturity, numberOfSteps);
                Console.Write(",TotalPathCreation: " + stopwatch.ElapsedTicks.ToString());
                stopwatch.Restart();

                double max = 0.0;
                for (int j = 0; j < numberOfSteps; j++)
                {
                    if (path[j].AssetPrice > max)
                    {
                        max = path[j].AssetPrice;
                    }
                }

                payoffs[i] = max;

                Console.Write(",Payoff: " + stopwatch.ElapsedTicks.ToString());

                Console.WriteLine(",TOTAL: " + globalWatch.ElapsedTicks.ToString());
            }
        }
    }
}
