using CSharpColorSpaceConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorSpaceConverterTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            var colorVals = ColorSpaceConverter.RGBToHSL();
            var color = Color.FromArgb(colorVals.Item1, colorVals.Item2, colorVals.Item3);
            pnlRGBToHSL1.BackColor = color;
            pnlRGBToHSL2.BackColor = color;
            pnlRGBToHSL3.BackColor = color;
        }
    }
}
