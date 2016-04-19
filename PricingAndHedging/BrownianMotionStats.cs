using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PricingAndHedging.Statistics
{
    public class BrownianMotionStats
    {
        public BrownianMotionStats(int calculatedPointsCount, bool barrierHasTouched, double payoff)
        {
            this.CalculatedPointsCount = calculatedPointsCount;
            this.BarrierHasTouched = barrierHasTouched;
            this.Payoff = payoff;
        }

        public int CalculatedPointsCount { get; private set; }

        public bool BarrierHasTouched { get; private set; }

        public double Payoff { get; private set; }
    }
}
