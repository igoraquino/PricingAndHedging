using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PricingAndHedging.Statistics;

namespace PricingAndHedging.Exercises
{
    public class Exercise01
    {
        #region Fields

        private int pathsCount;
        private int maximumDiscretizationLevel;
        private double putOptionPremium;

        #endregion

        #region Constructor

        public Exercise01(int pathsCount, int maximumDiscretizationLevel, double putOptionPremium)
        {
            this.pathsCount = pathsCount;
            this.maximumDiscretizationLevel = maximumDiscretizationLevel;
            this.putOptionPremium = putOptionPremium;
        }

        #endregion

        private List<BrownianMotionStats> statistics = null;
        private List<BrownianMotionStats> Statistics
        {
            get
            {
                if (this.statistics == null)
                    this.statistics = new List<BrownianMotionStats>();

                return this.statistics;
            }
        }

        public void AddStats(BrownianMotionStats stats)
        {
            this.Statistics.Add(stats);
        }

        public void ShowResults()
        {
            int calculatedPointsCount = 0;
            int barrierTouchesCount = 0;
            double sumOfKnockOutPayoffs = 0.0;

            foreach (BrownianMotionStats stats in this.Statistics)
            {
                calculatedPointsCount += stats.CalculatedPointsCount;

                if (stats.BarrierHasTouched) barrierTouchesCount++;

                sumOfKnockOutPayoffs += stats.Payoff;
            }

            double knockOutPutPremium = sumOfKnockOutPayoffs / this.pathsCount;
            double knockInPutPremium = putOptionPremium - knockOutPutPremium;

            Console.WriteLine("Knock Out Put Premium:\t" + knockOutPutPremium.ToString("0.00"));
            Console.WriteLine("Knock In Put Premium:\t" + knockInPutPremium.ToString("0.00"));
            Console.WriteLine("Percentage of paths that hit the barrier (%):\t" + (barrierTouchesCount * 100.0 / this.pathsCount).ToString("0.00"));
            Console.WriteLine("Number of calculated points:\t" + calculatedPointsCount);
        }
    }
}
