using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PricingAndHedging.Statistics;
using PricingAndHedging.Options;
using MathNet.Numerics.Distributions;
using PricingAndHedging.BrownianMotion;
using System.Diagnostics;

namespace PricingAndHedging.Exercises
{
    public class Exercise01
    {
        #region Fields

        private int pathsCount;
        private int maximumDiscretizationLevel;
        private KnockOutPutOption koPutOption;
        private TimeSpan calculationTime;

        #endregion

        #region Constructor

        public Exercise01(int pathsCount, int maximumDiscretizationLevel, KnockOutPutOption koPutOption)
        {
            this.pathsCount = pathsCount;
            this.maximumDiscretizationLevel = maximumDiscretizationLevel;
            this.koPutOption = koPutOption;
        }

        #endregion

        private List<Exercise01Stats> statistics = null;
        private List<Exercise01Stats> Statistics
        {
            get
            {
                if (this.statistics == null)
                    this.statistics = new List<Exercise01Stats>();

                return this.statistics;
            }
        }

        private void Evaluate()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < this.pathsCount; i++)
            {
                double randomValue = Math.Sqrt(this.koPutOption.TimeToMaturity) * Normal.Sample(0.0, 1.0);

                double maturityValue = this.koPutOption.Spot * Math.Exp((this.koPutOption.InterestRate - Math.Pow(this.koPutOption.Volatility, 2.0) / 2.0) * this.koPutOption.TimeToMaturity + this.koPutOption.Volatility * randomValue);

                var brownianBridge = new BrownianBridge(new BrownianMotionPoint(0.0, 0.0, this.koPutOption.Spot), new BrownianMotionPoint(this.koPutOption.TimeToMaturity, randomValue, maturityValue), maximumDiscretizationLevel);

                Exercise01Stats stats;

                this.koPutOption.Evaluate(brownianBridge, out stats);

                this.Statistics.Add(stats);
            }

            this.calculationTime = stopwatch.Elapsed;
        }

        public string GetResults()
        {
            this.Evaluate();

            int calculatedPointsCount = 0;
            int barrierTouchesCount = 0;
            double sumOfKnockOutPayoffs = 0.0;

            foreach (Exercise01Stats stats in this.Statistics)
            {
                calculatedPointsCount += stats.CalculatedPointsCount;

                if (stats.BarrierHasTouched) barrierTouchesCount++;

                sumOfKnockOutPayoffs += stats.Payoff;
            }

            double knockOutPutPremium = sumOfKnockOutPayoffs / this.pathsCount;
            double knockInPutPremium = koPutOption.PutTheoreticalPremium - knockOutPutPremium;
            double touchedPathsInPercentage = (barrierTouchesCount * 100.0 / this.pathsCount);

            var result = new StringBuilder();
            result.Append(this.pathsCount).Append(",");
            result.Append(this.maximumDiscretizationLevel).Append(",");
            result.Append(knockOutPutPremium).Append(",");
            result.Append(knockInPutPremium).Append(",");
            result.Append(touchedPathsInPercentage).Append(",");
            result.Append(calculatedPointsCount).Append(",");
            result.Append(this.calculationTime.TotalSeconds);

            return result.ToString();
        }
    }
}
