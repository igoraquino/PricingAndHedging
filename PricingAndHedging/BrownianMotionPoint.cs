namespace PricingAndHedging.BrownianMotion
{
    public class BrownianMotionPoint
    {
        public BrownianMotionPoint(double time, double randomValue, double assetValue)
        {
            this.Time = time;
            this.RandomValue = randomValue;
            this.AssetPrice = assetValue;
        }

        public double Time { get; private set; }

        public double RandomValue { get; private set; }

        public double AssetPrice { get; private set; }
    }
}
