using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PricingAndHedging.PartialPresentationSeptember
{
    public partial class SeptemberPresentation : Form
    {
        public SeptemberPresentation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double initialAssetPrice = 3.01807818609578;
            double strike = 3.4828;
            double interestRate = 0.142825;
            double volatility = 0.15985;
            double timeToMaturity = (366 / 365);
            int numberOfPaths = 5;
            int numberOfSteps = 15;

            new BestOfTimeTest().Evaluate(initialAssetPrice, strike, interestRate, volatility, timeToMaturity, numberOfPaths, numberOfSteps, 3.51);
        }
    }
}
