using System;

namespace PricingAndHedging.Exercise02.Exercises
{
    public class  PremiumTest
    {
        public double EvaluateCallOption(double initialAssetPrice, double strike, double interestRate, double volatility, double timeToMaturity, int numberOfPaths, int numberOfSteps)
        {
            var payoffs = new double[numberOfPaths];

            for (int i = 0; i < numberOfPaths; i++)
            {
                var path = new AssetPath(initialAssetPrice, interestRate, volatility, timeToMaturity, numberOfSteps);

                payoffs[i] = Math.Max(0.0, path[numberOfSteps].AssetValue - strike);
            }

            double averagePayoff = 0.0;
            foreach (double payoff in payoffs)
            {
                averagePayoff += (payoff / numberOfPaths);
            }

            return averagePayoff * Math.Exp(-interestRate * timeToMaturity);
        }
    }
}
