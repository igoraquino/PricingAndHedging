using PricingAndHedging.FinalExam.DataProviders;
using System;
using System.Collections.Generic;
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
            this.HedgedPortfolio();
            MessageBox.Show(BmfCalendar.IsBusinessDate(DateTime.Today.AddDays(2)).ToString());
            MessageBox.Show(BmfCalendar.IsBusinessDate(DateTime.Today).ToString());
            MessageBox.Show(BmfCalendar.IsBusinessDate(new DateTime(1991, 01, 01)).ToString());            
        }

        private void HedgedPortfolio()
        {
            var tradeDate = new DateTime(2015, 06, 30);
            var maturity = tradeDate.AddMonths(12);

            var call = new BlackEuropeanCallOption(tradeDate, maturity, FWDS.GetFwd(tradeDate, maturity));

            MessageBox.Show(call.Price + "\t" + call.Delta);
            
            var portfolioByDate = new Dictionary<DateTime, Portfolio>();

            var currentDay = tradeDate;
            while ((currentDay < maturity))
            {
                if (!BmfCalendar.IsBusinessDate(currentDay))
                {
                    currentDay.AddDays(1);
                    continue;
                }


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tradeDate = new DateTime(2015, 06, 30);
            var maturity = tradeDate.AddMonths(12);

            double forward = FWDS.GetFwd(tradeDate, maturity);
            double strike = FWDS.GetFwd(tradeDate, maturity);
            double timeToMaturity = TAU.Act365(tradeDate, maturity);
            double interestRate = RATES.GetRate(tradeDate, maturity);
            double volatility = VOLS.GetVol(tradeDate, maturity);

            var call = new BlackEuropeanCallOption(tradeDate, maturity, strike);
            MessageBox.Show(call.Price + "\t" + call.Delta);

            MessageBox.Show("discounted delta: "+ Math.Exp(-interestRate * timeToMaturity) * call.Delta);
        }
    }
}
