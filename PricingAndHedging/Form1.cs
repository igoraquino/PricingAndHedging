using System;
using System.Windows.Forms;
using MathNet.Numerics.Distributions;
using PricingAndHedging.Exercise01.BrownianMotion;
using PricingAndHedging.Exercise01.Options;
using PricingAndHedging.Exercise01.Statistics;
using PricingAndHedging.Exercise01.Exercises;
using System.Threading;
using System.IO;

namespace PricingAndHedging.Exercise01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void brownianBridge_Click(object sender, EventArgs e)
        {
            this.RunBrownianBridge();
        }

        private void RunBrownianBridge()
        {
            var strike = 100.0;
            var highBarrier = double.MaxValue;
            var lowBarrier = 85.0;
            var interestRate = 0.0;
            var dividendYield = 0.0;
            var volatility = 0.2;
            var spot = 100.0;
            var timeToMaturity = 128.0 / 256.0;

            var pathsCountRange = new[] { 10000/*, 20000, 30000, 40000, 50000, 60000, 70000, 80000, 90000, 100000*/};
            var maximumDiscretizationLevelRange = new[] { 6, 7, 8, 9, 10, 11, 12 };

            var koPutOption = new KnockOutPutOption(spot, strike, highBarrier, lowBarrier, interestRate, dividendYield, volatility, timeToMaturity);

            var fileName = @"C:\Users\igor\Dropbox\Mestrado\7. Pricing and Hedging\exercicios\1. Brownian Bridge\" + DateTime.Now.ToString("yyyyMMdd HHmmss") + "_BrownianBridge.txt";

            var header = "Type,Paths Count,Max Discretization Level,KO Put Premium,KI Put Premium,Touched Paths in Percentage,Calculated Points Count,Calculation Time (s)\n";
            File.WriteAllText(fileName, header + "\n");

            foreach (int pathsCount in pathsCountRange)
            {
                foreach (int maximumDiscretizationLevel in maximumDiscretizationLevelRange)
                {
                    var exercise = new PricingAndHedging.Exercise01.Exercises.Exercise01(pathsCount, maximumDiscretizationLevel, koPutOption);
                    File.AppendAllText(fileName, exercise.GetResults() + "\n");
                }
            }
        }

        private void brownianMotion_Click(object sender, EventArgs e)
        {
            this.RunBrownianMotion();
        }

        private void RunBrownianMotion()
        {
            var strike = 100.0;
            var highBarrier = double.MaxValue;
            var lowBarrier = 85.0;
            var interestRate = 0.0;
            var dividendYield = 0.0;
            var volatility = 0.2;
            var spot = 100.0;
            var timeToMaturity = 128.0 / 256.0;

            var pathsCountRange = new[] { 10000/*, 20000, 30000, 40000, 50000, 60000, 70000, 80000, 90000, 100000 */};
            var maximumDiscretizationLevelRange = new[] { 6, 7, 8, 9, 10, 11, 12 };

            var koPutOption = new KnockOutPutOption(spot, strike, highBarrier, lowBarrier, interestRate, dividendYield, volatility, timeToMaturity);

            var fileName = @"C:\Users\igor\Dropbox\Mestrado\7. Pricing and Hedging\exercicios\1. Brownian Bridge\" + DateTime.Now.ToString("yyyyMMdd HHmmss") + "_BrownianMotion.txt";

            var header = "Type,Paths Count,Max Discretization Level,KO Put Premium,KI Put Premium,Touched Paths in Percentage,Calculated Points Count,Calculation Time (s)";
            File.WriteAllText(fileName, header + "\n");

            foreach (int pathsCount in pathsCountRange)
            {
                foreach (int maximumDiscretizationLevel in maximumDiscretizationLevelRange)
                {
                    int numberOfSteps = int.Parse(Math.Pow(2, maximumDiscretizationLevel).ToString());
                    var result = new BrownianMotion.BrownianMotion().Evaluate(pathsCount, numberOfSteps, koPutOption);
                    File.AppendAllText(fileName, result + "\n");
                }
            }
        }

        private void parallelRun_Click(object sender, EventArgs e)
        {
            var brownianBridgeTask = new Thread(this.RunBrownianBridge);
            brownianBridgeTask.Start();

            var brownianMotionTask = new Thread(this.RunBrownianMotion);
            brownianMotionTask.Start();
        }
    }
}
