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
            MessageBox.Show(vol.ToString());
        }
    }
}
