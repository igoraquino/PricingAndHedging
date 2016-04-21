namespace PricingAndHedging
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
            this.brownianBridge = new System.Windows.Forms.Button();
            this.brownianMotion = new System.Windows.Forms.Button();
            this.parallelRun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // brownianBridge
            // 
            this.brownianBridge.Location = new System.Drawing.Point(12, 12);
            this.brownianBridge.Name = "brownianBridge";
            this.brownianBridge.Size = new System.Drawing.Size(75, 40);
            this.brownianBridge.TabIndex = 0;
            this.brownianBridge.Text = "Brownian Bridge";
            this.brownianBridge.UseVisualStyleBackColor = true;
            this.brownianBridge.Click += new System.EventHandler(this.brownianBridge_Click);
            // 
            // brownianMotion
            // 
            this.brownianMotion.Location = new System.Drawing.Point(197, 13);
            this.brownianMotion.Name = "brownianMotion";
            this.brownianMotion.Size = new System.Drawing.Size(75, 40);
            this.brownianMotion.TabIndex = 1;
            this.brownianMotion.Text = "Brownian Motion";
            this.brownianMotion.UseVisualStyleBackColor = true;
            this.brownianMotion.Click += new System.EventHandler(this.brownianMotion_Click);
            // 
            // parallelRun
            // 
            this.parallelRun.Location = new System.Drawing.Point(101, 64);
            this.parallelRun.Name = "parallelRun";
            this.parallelRun.Size = new System.Drawing.Size(75, 23);
            this.parallelRun.TabIndex = 2;
            this.parallelRun.Text = "Parallel";
            this.parallelRun.UseVisualStyleBackColor = true;
            this.parallelRun.Click += new System.EventHandler(this.parallelRun_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.parallelRun);
            this.Controls.Add(this.brownianMotion);
            this.Controls.Add(this.brownianBridge);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button brownianBridge;
        private System.Windows.Forms.Button brownianMotion;
        private System.Windows.Forms.Button parallelRun;
    }
}

