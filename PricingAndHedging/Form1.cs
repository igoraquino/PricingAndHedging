using System;
using System.Windows.Forms;
using MathNet.Numerics.Distributions;
using PricingAndHedging.BrownianMotion;
using PricingAndHedging.Options;
using PricingAndHedging.Statistics;
using PricingAndHedging.Exercises;

namespace PricingAndHedging
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var strike = 100.0;
            var highBarrier = double.MaxValue;
            var lowBarrier = 85.0;
            var interestRate = 0.0;
            var dividendYield = 0.0;
            var volatility = 0.2;
            var spot = 100.0;
            var timeToMaturity = 128.0 / 256.0;
            var pathsCount = 50000;
            var maximumDiscretizationLevel = 8;

            var koPutOption = new KnockOutPutOption(spot, strike, highBarrier, lowBarrier, interestRate, dividendYield, volatility, timeToMaturity);

            var exercise = new Exercise01(pathsCount, maximumDiscretizationLevel, koPutOption.PutTheoreticalPremium);

            for (int i = 0; i < pathsCount; i++)
            {
                double randomValue = Math.Sqrt(timeToMaturity) * Normal.Sample(0.0, 1.0);

                double maturityValue = spot * Math.Exp((interestRate - Math.Pow(volatility, 2.0) / 2.0) * timeToMaturity + volatility * randomValue);

                var brownianBridge = new BrownianBridge(new BrownianMotionPoint(0.0, 0.0, spot), new BrownianMotionPoint(timeToMaturity, randomValue, maturityValue), maximumDiscretizationLevel);

                BrownianMotionStats stats;

                koPutOption.Evaluate(brownianBridge, out stats);

                exercise.AddStats(stats);
            }

            exercise.ShowResults();
        }
    }
}
