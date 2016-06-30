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
            var v = new VolatilitiesForDate(new DateTime(2014, 7, 1), 0.09025, 0.10158, 0.1237);
            var vol = v.GetVol(new DateTime(2014, 9, 10));

            var f = new ForwardsForDate(new DateTime(2014, 7, 1), 2.2018, 2.2199, 2.2557, 2.4148);
            MessageBox.Show(f.GetFwd(new DateTime(2014, 7, 10)).ToString());
            MessageBox.Show(f.GetFwd(new DateTime(2014, 9, 10)).ToString());
            MessageBox.Show(f.GetFwd(new DateTime(2014, 10, 1)).ToString());
        }
    }
}
