using PricingAndHedging.Exercise02.Exercises;
using PricingAndHedging.FinalExam.DataProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace PricingAndHedging.FinalExam
{
    public partial class FinalExamPresentation : Form
    {
        public FinalExamPresentation()
        {
            InitializeComponent();
        }

        private void InterpolationExperiments()
        {
            MessageBox.Show("vols " + VOLS.GetVol(new DateTime(2014, 7, 1), new DateTime(2014, 9, 10)).ToString());

            MessageBox.Show("fwds " + FWDS.GetFwd(new DateTime(2014, 7, 1),new DateTime(2014, 7, 10)).ToString());

            var ir = new InterestRatesForDate(new DateTime(2014, 7, 1), 0.1081, 0.107741, 0.108398);
            MessageBox.Show("rates: " + RATES.GetRate(new DateTime(2014, 7, 1), new DateTime(2014, 7, 10)).ToString() + "\t\t" + ir.GetCDI(new DateTime(2014, 7, 10)).ToString());
            MessageBox.Show("rates: " + RATES.GetRate(new DateTime(2014, 7, 1), new DateTime(2014, 9, 10)).ToString() + "\t\t" + ir.GetCDI(new DateTime(2014, 9, 10)).ToString());
            MessageBox.Show("rates: " + RATES.GetRate(new DateTime(2014, 7, 1), new DateTime(2014, 10, 1)).ToString() + "\t\t" + ir.GetCDI(new DateTime(2014, 10, 1)).ToString());
            MessageBox.Show("rates: " + RATES.GetRate(new DateTime(2014, 7, 1), new DateTime(2015, 5, 10)).ToString() + "\t\t" + ir.GetCDI(new DateTime(2015, 5, 10)).ToString());
        }

        private void Experiments()
        {
            MessageBox.Show(BmfCalendar.IsBusinessDate(DateTime.Today.AddDays(2)).ToString());
            MessageBox.Show(BmfCalendar.IsBusinessDate(DateTime.Today).ToString());
            MessageBox.Show(BmfCalendar.IsBusinessDate(new DateTime(1991, 01, 01)).ToString());            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Question4();
        }

        private void Question4()
        {
            double initialAssetPrice = 3.01807818609578;
            double strike = 3.4828;
            double interestRate = 0.142825;
            double volatility = 0.15985;
            double timeToMaturity = (366/365);

            //var pathsCountRange = new[] { 70000, 70000, 80000, 80000, 90000, 90000, 100000, 100000 };
            //var stepsCount = new[] { 1000, 1000, 3000, 3000, 5000, 5000, 10000, 10000 };

            var pathsCountRange = new[] { 1000, 1000, 5000, 5000, 10000, 10000, 50000, 50000 };
            var stepsCount = new[] { 600, 600 };

            Console.WriteLine("Paths,Steps,Premium,Time,Erro");

            for (int pathIndex = 0; pathIndex < pathsCountRange.Length; pathIndex++)
            {
                for (int stepIndex = 0; stepIndex < stepsCount.Length; stepIndex++)
                {
                    var numberOfPaths = pathsCountRange[pathIndex];
                    var numberOfSteps = stepsCount[stepIndex];
                    Stopwatch s = new Stopwatch();
                    s.Start();
                    var premium = new PremiumTest().EvaluateCallOption(initialAssetPrice, strike, interestRate, volatility, timeToMaturity, numberOfPaths, numberOfSteps);
                    var error = Math.Abs(1 - premium / 0.192523547344284);
                    Console.WriteLine(numberOfPaths + "," + numberOfSteps + "," + premium + "," + s.Elapsed.TotalSeconds + "," + error);
                }
            }

        }

        private void Question5()
        {
            DateTime tradeDate = new DateTime(2015, 06, 30);
            DateTime maturity = tradeDate.AddMonths(3);
            double strike = FWDS.GetFwd(tradeDate, maturity);
            bool hasHedge = true;

            DateTime nextDate = BmfCalendar.PlusBusinessDays(tradeDate, 1);
            double timeStep = ((nextDate - tradeDate).TotalDays / 365.0);

            PortfolioSellSide previousPortfolio = new PortfolioSellSide(tradeDate, maturity, strike, 0.0, 0.0, timeStep, hasHedge, true);

            Console.WriteLine("Reference Date,Portfolio Value,Option Price,Hedging Notional,Cash,PTAX,Forward,Strike");
            LogOnConsole(previousPortfolio);

            for (DateTime currentDate = nextDate; currentDate <= maturity; currentDate = currentDate.AddDays(1))
            {
                if (!BmfCalendar.IsBusinessDate(currentDate))
                    continue;

                nextDate = BmfCalendar.PlusBusinessDays(currentDate, 1);
                timeStep = ((nextDate - currentDate).TotalDays / 365.0);
                var portfolio = new PortfolioSellSide(currentDate, maturity, strike, previousPortfolio.HedgingNotional, previousPortfolio.Cash, timeStep, hasHedge, true);
                LogOnConsole(portfolio);
                previousPortfolio = portfolio;
            }
        }

        private void Question7()
        {
            DateTime tradeDate = new DateTime(2015, 06, 30);
            DateTime maturity = tradeDate.AddMonths(12);
            double strike = FWDS.GetFwd(tradeDate, maturity);
            bool hasHedge = true;

            DateTime nextDate = BmfCalendar.PlusBusinessDays(tradeDate, 1);
            double timeStep = ((nextDate - tradeDate).TotalDays / 365.0);

            PortfolioSellSide previousPortfolio = new PortfolioSellSide(tradeDate, maturity, strike, 0.0, 0.0, timeStep, hasHedge, true);

            Console.WriteLine("Reference Date,Portfolio Value,Option Price,Hedging Notional,Cash,PTAX,Forward,Strike");
            LogOnConsole(previousPortfolio);

            int step = 1;
            for (DateTime currentDate = nextDate; currentDate <= maturity; currentDate = currentDate.AddDays(1))
            {
                if (!BmfCalendar.IsBusinessDate(currentDate))
                    continue;

                nextDate = BmfCalendar.PlusBusinessDays(currentDate, 1);
                timeStep = ((nextDate - currentDate).TotalDays / 365.0);
                hasHedge = (step % 5 == 0);
                var portfolio = new PortfolioSellSide(currentDate, maturity, strike, previousPortfolio.HedgingNotional, previousPortfolio.Cash, timeStep, hasHedge, true);
                LogOnConsole(portfolio);
                previousPortfolio = portfolio;
                step++;
            }
        }

        private void Question3()
        {
            DateTime tradeDate = new DateTime(2015, 06, 30);
            DateTime maturity = tradeDate.AddMonths(12);
            double strike = FWDS.GetFwd(tradeDate, maturity);
            bool hasHedge = true;

            DateTime nextDate = BmfCalendar.PlusBusinessDays(tradeDate, 1);
            double timeStep = ((nextDate - tradeDate).TotalDays / 365.0);

            Portfolio previousPortfolio = new Portfolio(tradeDate, maturity, strike, 0.0, 0.0, timeStep, hasHedge);

            Console.WriteLine("Reference Date,Portfolio Value,Option Price,Hedging Notional,Cash,PTAX,Forward,Strike");
            LogOnConsole(previousPortfolio);

            for (DateTime currentDate = nextDate; currentDate <= maturity; currentDate = currentDate.AddDays(1))
            {
                if (!BmfCalendar.IsBusinessDate(currentDate))
                    continue;

                nextDate = BmfCalendar.PlusBusinessDays(currentDate, 1);
                timeStep = ((nextDate - currentDate).TotalDays / 365.0);
                var portfolio = new Portfolio(currentDate, maturity, strike, previousPortfolio.HedgingNotional, previousPortfolio.Cash, timeStep, hasHedge);
                LogOnConsole(portfolio);
                previousPortfolio = portfolio;
            }
        }

        private void LogOnConsole(Portfolio portfolio)
        {
            var logLine = new StringBuilder();

            logLine.Append(portfolio.CurrentDate.ToString("dd-MMM-yyyy")).Append(",");
            logLine.Append(portfolio.Value.ToString("0.000000")).Append(",");
            logLine.Append(portfolio.Option.Price.ToString("0.000000")).Append(",");
            logLine.Append(portfolio.HedgingNotional.ToString("0.000000")).Append(",");
            logLine.Append(portfolio.Cash.ToString("0.000000")).Append(",");
            logLine.Append(PTAX.GetValue(portfolio.CurrentDate).ToString("0.000000")).Append(",");
            logLine.Append(portfolio.Option.Forward.ToString("0.000000")).Append(",");
            logLine.Append(portfolio.Option.Strike.ToString("0.000000")).Append(",");

            Console.WriteLine(logLine.ToString());
        }

        private void LogOnConsole(PortfolioSellSide portfolio)
        {
            var logLine = new StringBuilder();

            logLine.Append(portfolio.CurrentDate.ToString("dd-MMM-yyyy")).Append(",");
            logLine.Append(portfolio.Value.ToString("0.000000")).Append(",");
            logLine.Append(portfolio.Option.Price.ToString("0.000000")).Append(",");
            logLine.Append(portfolio.HedgingNotional.ToString("0.000000")).Append(",");
            logLine.Append(portfolio.Cash.ToString("0.000000")).Append(",");
            logLine.Append(PTAX.GetValue(portfolio.CurrentDate).ToString("0.000000")).Append(",");
            logLine.Append(portfolio.Option.Forward.ToString("0.000000")).Append(",");
            logLine.Append(portfolio.Option.Strike.ToString("0.000000")).Append(",");

            Console.WriteLine(logLine.ToString());
        }
    }
}
