using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MathNet.Numerics.Distributions;
using PricingAndHedging.Exercise01.Options;
using PricingAndHedging.Exercise01.Statistics;
using System.Text;

namespace PricingAndHedging.Exercise01.BrownianMotion
{
    public class BrownianMotion
    {
        public string Evaluate(int numberOfPaths, int numberOfSteps, KnockOutPutOption koPutOption)
        {
            var stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            double dt = koPutOption.TimeToMaturity / numberOfSteps;
            var assetPrice = koPutOption.Spot;
            
            var statisticsList = new List<Exercise01Stats>();
            for (int n = 0; n < numberOfPaths; n++)
            {
                var normalRandomNumbers = new double[numberOfSteps];
                Normal.Samples(normalRandomNumbers, 0, 1);

                var normalDistribution = new Normal(0, 1);

                bool barrierHasTouched = false;

                int calculationStep = 0;
                for (calculationStep = 0; calculationStep < numberOfSteps; calculationStep++)
                {
                    //assetPrice = assetPrice * Math.Exp((koPutOption.InterestRate - (Math.Pow(koPutOption.Volatility, 2.0)) / 2) * dt + koPutOption.Volatility * Math.Sqrt(dt) * normalRandomNumbers[calculationStep]);
                    assetPrice = assetPrice * Math.Exp((koPutOption.InterestRate - (Math.Pow(koPutOption.Volatility, 2.0)) / 2) * dt + koPutOption.Volatility * Math.Sqrt(dt) * normalDistribution.Sample());
                    if (assetPrice <= koPutOption.LowBarrier)
                    {
                        barrierHasTouched = true;
                        break;
                    }
                }

                double payoff = 0.0;
                if (!barrierHasTouched)
                {
                    payoff = Math.Max(0.0, koPutOption.Strike - assetPrice);
                }

                statisticsList.Add(new Exercise01Stats(calculationStep, barrierHasTouched, payoff));

                assetPrice = koPutOption.Spot;
            }
            
            int calculatedPointsCount = 0;
            int barrierTouchesCount = 0;
            double sumOfKnockOutPayoffs = 0.0;

            foreach (Exercise01Stats stats in statisticsList)
            {
                calculatedPointsCount += stats.CalculatedPointsCount;

                if (stats.BarrierHasTouched) barrierTouchesCount++;

                sumOfKnockOutPayoffs += stats.Payoff;
            }

            double knockOutPutPremium = (sumOfKnockOutPayoffs / numberOfPaths);
            double knockInPutPremium = koPutOption.PutTheoreticalPremium - knockOutPutPremium;
            double touchedPathsInPercentage = (barrierTouchesCount * 100.0 / numberOfPaths);

            var result = new StringBuilder();
            result.Append("Brownian Motion").Append(",");
            result.Append(numberOfPaths).Append(",");
            result.Append(Math.Log(numberOfSteps,2)).Append(",");
            result.Append(knockOutPutPremium).Append(",");
            result.Append(knockInPutPremium).Append(",");
            result.Append(touchedPathsInPercentage).Append(",");
            result.Append(calculatedPointsCount).Append(",");
            result.Append(stopwatch.Elapsed.TotalSeconds);

            return result.ToString();
        }
    }
}
