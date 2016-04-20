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

            var pathsCountRange = new[] { 10000, 20000, 30000, 40000, 50000, 60000, 70000, 80000, 90000, 100000 };
            var maximumDiscretizationLevelRange = new[] { 6, 7, 8, 9, 10, 11, 12 };

            var koPutOption = new KnockOutPutOption(spot, strike, highBarrier, lowBarrier, interestRate, dividendYield, volatility, timeToMaturity);
                        
            var header = "Paths Count,Max Discretization Level,KO Put Premium,KI Put Premium,Touched Paths in Percentage,Calculated Points Count,Calculation Time (s)";
            Console.WriteLine(header);

            foreach (int pathsCount in pathsCountRange)
            {
                foreach (int maximumDiscretizationLevel in maximumDiscretizationLevelRange)
                {
                    var exercise = new Exercise01(pathsCount, maximumDiscretizationLevel, koPutOption);
                    Console.WriteLine(exercise.GetResults());
                }
            }
        }
    }
}
