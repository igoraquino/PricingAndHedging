using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void HedgedPortfolio()
        {
            var tradeDate = new DateTime(2015, 06, 30);
            var maturity = tradeDate.AddMonths(1);

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
            this.InterpolationExperiments();
            this.Experiments();
        }
    }
}
