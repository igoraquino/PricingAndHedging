using System;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using PricingAndHedging.Options;
using PricingAndHedging.Statistics;

namespace PricingAndHedging.BrownianMotion
{
    public class BrownianBridge
    {
        private SortedList<double, BrownianMotionPoint> path;
        private int currentDiscretizationLevel;
        private int maximumDiscretizationLevel;

        #region Constructor

        public BrownianBridge(BrownianMotionPoint start, BrownianMotionPoint end, int maximumDiscretizationLevel)
        {
            this.path = new SortedList<double, BrownianMotionPoint>();
            this.path.Add(start.Time, start);
            this.path.Add(end.Time, end);
            this.End = end;
            this.currentDiscretizationLevel = 0;
            this.maximumDiscretizationLevel = maximumDiscretizationLevel;
        }

        #endregion

        private bool MaximumDiscretizationHasBeenReached
        {
            get { return (this.currentDiscretizationLevel >= this.maximumDiscretizationLevel); }
        }

        private BrownianMotionPoint End { get; set; }

        public double Evaluate(KnockOutPutOption knockOutPutOption, out Exercise01Stats stats)
        {
            bool barrierHasBeenTouched = false;
            double payoff = 0.0;
            stats = new Exercise01Stats(0, false, payoff);

            while (!this.MaximumDiscretizationHasBeenReached)
            {
                this.currentDiscretizationLevel++;

                int previousPathCount = this.path.Count;
                double[] previousPathTimes = new double[this.path.Keys.Count];
                this.path.Keys.CopyTo(previousPathTimes, 0);

                var normal = new Normal();

                for (int leftIndex = 0; leftIndex < previousPathCount - 1; leftIndex++)
                {
                    double leftTime = previousPathTimes[leftIndex];
                    double leftRandomValue = this.path[leftTime].RandomValue;
                    double leftAssetValue = this.path[leftTime].AssetValue;

                    double rightTime = previousPathTimes[leftIndex + 1];
                    double rightRandomValue = this.path[rightTime].RandomValue;

                    double newTime = (leftTime + rightTime) / 2.0;

                    double mean = ((rightTime - newTime) * leftRandomValue + (newTime - leftTime) * rightRandomValue) / (rightTime - leftTime);
                    double variance = (rightTime - newTime) * (newTime - leftTime) / (rightTime - leftTime);
                    double newRandomValue = mean + Math.Sqrt(variance) * normal.Sample();

                    double newAssetValue = leftAssetValue * Math.Exp((knockOutPutOption.InterestRate - Math.Pow(knockOutPutOption.Volatility, 2.0) / 2.0) * (newTime - leftTime) + knockOutPutOption.Volatility * (newRandomValue - leftRandomValue));

                    this.path.Add(newTime, new BrownianMotionPoint(newTime, newRandomValue, newAssetValue));

                    if (newAssetValue <= knockOutPutOption.LowBarrier)
                    {
                        barrierHasBeenTouched = true;
                        stats = new Exercise01Stats(this.path.Count - 2, true, payoff);
                        break;
                    }
                }
            }

            if (!barrierHasBeenTouched)
            {
                payoff = Math.Max(knockOutPutOption.Strike - this.End.AssetValue, 0.0);
                stats = new Exercise01Stats(this.path.Count - 2, false, payoff);
            }

            return payoff;
        }

    }
}
