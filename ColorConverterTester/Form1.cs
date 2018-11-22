using CSharpColorSpaceConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            new Thread(delegate () {
                Enumerate8BitColors((r, g, b) =>
                {
                    this.Invoke(new Action(() => {
                        TestColor(r, g, b);
                    }));
                });
            }).Start();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/smchughinfo/color");
        }

        public void Enumerate8BitColors(Action<int, int, int> onColor)
        {
            for(var r = 0; r <= 255; r++)
            {
                for (var g = 0; g <= 255; g++)
                {
                    for (var b = 0; b <= 255; b++)
                    {
                        Thread.Sleep(1);
                        onColor(r, g, b);
                    }
                }
            }
        }

        private void TestColor(int r, int g, int b)
        {
            var hslVals = ColorSpaceConverter.RGBToHSL(r, g, b);
            var rgbVals = ColorSpaceConverter.HSLToRGB(hslVals[0], hslVals[1], hslVals[2]);
            var color = Color.FromArgb(rgbVals[0], rgbVals[1], rgbVals[2]);

            if(r != rgbVals[0] || g != rgbVals[1] || b != rgbVals[2])
            {
                throw new Exception("CONVERSION ERROR!");
            }

            pnlRGBToHSL1.BackColor = color;
            pnlRGBToHSL2.BackColor = color;
            pnlRGBToHSL3.BackColor = color;
            lblRGB.Text = $"RGB ({rgbVals[0]}, {rgbVals[1]}, {rgbVals[2]})";
            lblHSL.Text = $"HSL ({Math.Round(hslVals[0], 1)}, {Math.Round(hslVals[1], 1)}, {Math.Round(hslVals[2], 1)})";
        }
    }
}
