using System;
using System.Windows.Forms;
using MathNet.Numerics.Distributions;
using PricingAndHedging.Exercise01.BrownianMotion;
using PricingAndHedging.Exercise01.Options;
using PricingAndHedging.Exercise01.Statistics;
using PricingAndHedging.Exercise01.Exercises;
using System.Threading;
using System.IO;
using PricingAndHedging.Exercise02.Exercises;
using System.Text;
using System.Diagnostics;
using PricingAndHedging.Exercise02;
using System.Collections.Generic;

namespace PricingAndHedging.Exercise01
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Exercise 01 - Brownian Bridge

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

            var header = "Type,Paths Count,Max Discretization Level,KO Put Premium,KI Put Premium,Touched Paths in Percentage,Calculated Points Count,Calculation Time (s)";

            var fileName = @"%TEMP%\" + DateTime.Now.ToString("yyyyMMdd HHmmss") + "_BrownianBridge.csv";
            fileName = Environment.ExpandEnvironmentVariables(fileName);
            StreamWriter logFileStream = File.AppendText(fileName);
            logFileStream.WriteLine(header);
            logFileStream.Flush();

            foreach (int pathsCount in pathsCountRange)
            {
                foreach (int maximumDiscretizationLevel in maximumDiscretizationLevelRange)
                {
                    var exercise = new PricingAndHedging.Exercise01.Exercises.Exercise01(pathsCount, maximumDiscretizationLevel, koPutOption);
                    logFileStream.WriteLine(exercise.GetResults());
                    logFileStream.Flush();
                }
            }

            logFileStream.Close();
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

            var header = "Type,Paths Count,Max Discretization Level,KO Put Premium,KI Put Premium,Touched Paths in Percentage,Calculated Points Count,Calculation Time (s)";

            var fileName = @"%TEMP%\" + DateTime.Now.ToString("yyyyMMdd HHmmss") + "_BrownianMotion.csv";
            fileName = Environment.ExpandEnvironmentVariables(fileName);
            StreamWriter logFileStream = File.AppendText(fileName);
            logFileStream.WriteLine(header);
            logFileStream.Flush();

            foreach (int pathsCount in pathsCountRange)
            {
                foreach (int maximumDiscretizationLevel in maximumDiscretizationLevelRange)
                {
                    int numberOfSteps = int.Parse(Math.Pow(2, maximumDiscretizationLevel).ToString());
                    var result = new BrownianMotion.BrownianMotion().Evaluate(pathsCount, numberOfSteps, koPutOption);
                    logFileStream.WriteLine(result);
                    logFileStream.Flush();
                }
            }

            logFileStream.Close();
        }

        private void parallelRun_Click(object sender, EventArgs e)
        {
            var brownianBridgeTask = new Thread(this.RunBrownianBridge);
            brownianBridgeTask.Start();

            var brownianMotionTask = new Thread(this.RunBrownianMotion);
            brownianMotionTask.Start();
        }

        #endregion

        #region Exercise 02 - Delta Hedging

        private void exercise02_Click(object sender, EventArgs e)
        {
            PrintHedgePortfolioStepByStep();
        }

        private void PrintHedgePortfolioStepByStep()
        {
            int pathCount = 500;
            int stepsCount = 50000;

            double initialPrice = 100.0;
            double interestRate = 0.0;
            double actualVolatility = 0.30;
            double impliedVolatility = 0.20;
            double timeToMaturity = 1.0;
            double strike = 100.0;

            double pricingVolatility = impliedVolatility;
            //double hedgingVolatility = actualVolatility;
            double hedgingVolatility = impliedVolatility;

            var text = new StringBuilder();
            var portfolioPricesByPathIndex = new Dictionary<int, IList<double>>();
            for (int pathIndex = 0; pathIndex < pathCount; pathIndex++)
            {
                var path = new AssetPath(initialPrice, interestRate, actualVolatility, timeToMaturity, stepsCount);

                double timeStepSize = (timeToMaturity / stepsCount);
                
                var previousPortfolio = new Portfolio(strike, timeToMaturity, interestRate, pricingVolatility, hedgingVolatility, 0.0, 0.0, timeStepSize);

                //Console.WriteLine("currentAssetPrice,pricingOption.Price,assetAmountForHedging,cash,portfolioValue");
            
                var portfolioPrices = new List<double>();
                portfolioPrices.Add(previousPortfolio.Value(path[0].AssetPrice));

                for (int hedgeIndex = 1; hedgeIndex <= stepsCount; hedgeIndex++)
                {
                    double currentTimeToMaturity = 1.0 - (hedgeIndex * timeStepSize);
                    double previousAssetAmount = previousPortfolio.AssetAmountForHedging(path[hedgeIndex - 1].AssetPrice);
                    double previousCash = previousPortfolio.Cash(path[hedgeIndex - 1].AssetPrice);
                    
                    var currentPortfolio = new Portfolio(strike, currentTimeToMaturity, interestRate, pricingVolatility, hedgingVolatility, previousAssetAmount, previousCash, timeStepSize);

                    portfolioPrices.Add(currentPortfolio.Value(path[hedgeIndex].AssetPrice));
                }

                //Console.WriteLine(portfolioPrices[0].ToString("0.0000"));
                //for (int i = 1; i < portfolioPrices.Count; i++)
                //{
                //    double profit = portfolioPrices[i] - portfolioPrices[i - 1];
                //    Console.WriteLine(portfolioPrices[i].ToString("0.0000") + "\t\t" + profit);
                //}

                //portfolioPricesByPathIndex[pathIndex] = portfolioPrices;

                //Console.WriteLine(portfolioPrices[stepsCount].ToString("0.0000"));
                text.AppendLine(portfolioPrices[stepsCount].ToString("0.0000"));
            }

            //for (int i = 0; i <= stepsCount; i++)
            //{
            //    var line = new StringBuilder();

            //    foreach (IList<double> portfolioPrices in portfolioPricesByPathIndex.Values)
            //        line.Append(portfolioPrices[i].ToString("0.0000")).Append(",");

            //    text.AppendLine(line.ToString());
            //}

            var fileName = @"%TEMP%\" + DateTime.Now.ToString("HHmmss") + "_Hedge.csv";
            fileName = Environment.ExpandEnvironmentVariables(fileName);
            StreamWriter logFileStream = File.AppendText(fileName);
            logFileStream.WriteLine(text.ToString());
            logFileStream.Flush();
            logFileStream.Close();

            var excel = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excel.Workbooks.Open(fileName);
            excel.Visible = true;
        }

        private void CalculateEuropeanCall()
        {
            var call = new EuropeanCallOption(100.0, 102.0, 1.7, 0.08, .20);
            MessageBox.Show(call.Price + "\n" + call.Delta);
        }

        private void EvaluateCallViaMonteCarlo()
        {
            //var pathsCountRange = new[] {20000, 20000, 30000, 30000, 40000, 40000, 50000, 50000, 60000, 60000, 70000, 70000};
            var pathsCountRange = new[] { 70000, 70000, 80000, 80000, 90000, 90000, 100000, 100000, 1000000, 10000000, 10000000, 20000000, 20000000, 30000000, 30000000, 50000000, 100000000 };

            StringBuilder text = new StringBuilder();
            text.AppendLine("r = 0.0");
            var stopwatch = new Stopwatch();
            for (int i = 0; i < pathsCountRange.Length; i++)
            {
                stopwatch.Restart();
                double premium = new PremiumTest().EvaluateCallOption(100.0, 100.0, 0.0, 0.2, 0.5, pathsCountRange[i], 1);
                text.AppendLine(pathsCountRange[i] + ": " + premium);
                Console.WriteLine(text.ToString() + "\t" + stopwatch.ElapsedMilliseconds / 1000.0 + "\n");
            }

            text.AppendLine("\n\nr = 2.0, T = 0.5");
            for (int i = 0; i < pathsCountRange.Length; i++)
            {
                double premium = new PremiumTest().EvaluateCallOption(100.0, 100.0, 0.02, 0.2, 0.5, pathsCountRange[i], 5000);
                text.AppendLine(pathsCountRange[i] + ": " + premium);
            }

            MessageBox.Show(text.ToString());
        }

        #endregion
    }
}
