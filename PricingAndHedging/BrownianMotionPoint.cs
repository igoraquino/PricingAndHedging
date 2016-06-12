namespace PricingAndHedging.Exercise01.BrownianMotion
{
    public class BrownianMotionPoint
    {
        public BrownianMotionPoint(double time, double randomValue, double assetValue)
        {
            this.Time = time;
            this.RandomValue = randomValue;
            this.AssetValue = assetValue;
        }

        public double Time { get; private set; }

        public double RandomValue { get; private set; }

        public double AssetValue { get; private set; }
    }
}
