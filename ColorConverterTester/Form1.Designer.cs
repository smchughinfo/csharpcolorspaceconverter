namespace ColorSpaceConverterTester
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlRGBToHSL1 = new System.Windows.Forms.Panel();
            this.pnlRGBToHSL2 = new System.Windows.Forms.Panel();
            this.pnlRGBToHSL3 = new System.Windows.Forms.Panel();
            this.btnTest = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // pnlRGBToHSL1
            // 
            this.pnlRGBToHSL1.Location = new System.Drawing.Point(12, 12);
            this.pnlRGBToHSL1.Name = "pnlRGBToHSL1";
            this.pnlRGBToHSL1.Size = new System.Drawing.Size(100, 100);
            this.pnlRGBToHSL1.TabIndex = 0;
            // 
            // pnlRGBToHSL2
            // 
            this.pnlRGBToHSL2.Location = new System.Drawing.Point(118, 12);
            this.pnlRGBToHSL2.Name = "pnlRGBToHSL2";
            this.pnlRGBToHSL2.Size = new System.Drawing.Size(100, 100);
            this.pnlRGBToHSL2.TabIndex = 1;
            // 
            // pnlRGBToHSL3
            // 
            this.pnlRGBToHSL3.Location = new System.Drawing.Point(224, 12);
            this.pnlRGBToHSL3.Name = "pnlRGBToHSL3";
            this.pnlRGBToHSL3.Size = new System.Drawing.Size(100, 100);
            this.pnlRGBToHSL3.TabIndex = 2;
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(12, 118);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(312, 43);
            this.btnTest.TabIndex = 3;
            this.btnTest.Text = "Test RGB->HSL->RGB";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(12, 168);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(374, 20);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "The other conversion are \"tested\" in this color picker";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 205);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.pnlRGBToHSL3);
            this.Controls.Add(this.pnlRGBToHSL2);
            this.Controls.Add(this.pnlRGBToHSL1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlRGBToHSL1;
        private System.Windows.Forms.Panel pnlRGBToHSL2;
        private System.Windows.Forms.Panel pnlRGBToHSL3;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}

