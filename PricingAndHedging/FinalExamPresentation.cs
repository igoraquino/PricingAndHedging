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

        private void button1_Click(object sender, EventArgs e)
        {
            var vols = new VOLS();
            var vola = vols[new DateTime(2014, 7, 1)];
            MessageBox.Show(vola.GetVol(new DateTime(2014, 9, 10)).ToString());


            var fwds = new FWDS();
            MessageBox.Show("fwds " + fwds[new DateTime(2014, 7, 1)].GetFwd(new DateTime(2014, 7, 10)).ToString());

            var f = new ForwardsForDate(new DateTime(2014, 7, 1), 2.2018, 2.2199, 2.2557, 2.4148);
            MessageBox.Show(f.GetFwd(new DateTime(2014, 7, 10)).ToString());

            var rates = new RATES();
            var r = rates[new DateTime(2014, 7, 1)];

            var ir = new InterestRatesForDate(new DateTime(2014, 7, 1), 0.1081, 0.107741, 0.108398);
            MessageBox.Show("rates: " + r.GetCDI(new DateTime(2014, 7, 10)).ToString() + "\t\t" + ir.GetCDI(new DateTime(2014, 7, 10)).ToString());
            MessageBox.Show("rates: " + r.GetCDI(new DateTime(2014, 9, 10)).ToString() + "\t\t" + ir.GetCDI(new DateTime(2014, 9, 10)).ToString());
            MessageBox.Show("rates: " + r.GetCDI(new DateTime(2014, 10, 1)).ToString() + "\t\t" + ir.GetCDI(new DateTime(2014, 10, 1)).ToString());
            MessageBox.Show("rates: " + r.GetCDI(new DateTime(2015, 5, 10)).ToString() + "\t\t" + ir.GetCDI(new DateTime(2015, 5, 10)).ToString());
        }
    }
}
