namespace UAVSensorControl
{
    partial class frmCalibrate
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
            this.btDecParam3 = new System.Windows.Forms.Button();
            this.btIncParam3 = new System.Windows.Forms.Button();
            this.btDecParam2 = new System.Windows.Forms.Button();
            this.btIncParam2 = new System.Windows.Forms.Button();
            this.btDecParam1 = new System.Windows.Forms.Button();
            this.btIncParam1 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.tbParam3 = new System.Windows.Forms.TextBox();
            this.tbParam2 = new System.Windows.Forms.TextBox();
            this.tbParam1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btDecParam3
            // 
            this.btDecParam3.Location = new System.Drawing.Point(193, 57);
            this.btDecParam3.Name = "btDecParam3";
            this.btDecParam3.Size = new System.Drawing.Size(23, 19);
            this.btDecParam3.TabIndex = 57;
            this.btDecParam3.Text = "-";
            this.btDecParam3.UseVisualStyleBackColor = true;
            this.btDecParam3.Click += new System.EventHandler(this.btDecParam3_Click);
            // 
            // btIncParam3
            // 
            this.btIncParam3.Location = new System.Drawing.Point(164, 57);
            this.btIncParam3.Name = "btIncParam3";
            this.btIncParam3.Size = new System.Drawing.Size(23, 19);
            this.btIncParam3.TabIndex = 56;
            this.btIncParam3.Text = "+";
            this.btIncParam3.UseVisualStyleBackColor = true;
            this.btIncParam3.Click += new System.EventHandler(this.btIncParam3_Click);
            // 
            // btDecParam2
            // 
            this.btDecParam2.Location = new System.Drawing.Point(193, 31);
            this.btDecParam2.Name = "btDecParam2";
            this.btDecParam2.Size = new System.Drawing.Size(23, 19);
            this.btDecParam2.TabIndex = 55;
            this.btDecParam2.Text = "-";
            this.btDecParam2.UseVisualStyleBackColor = true;
            this.btDecParam2.Click += new System.EventHandler(this.btDecParam2_Click);
            // 
            // btIncParam2
            // 
            this.btIncParam2.Location = new System.Drawing.Point(164, 31);
            this.btIncParam2.Name = "btIncParam2";
            this.btIncParam2.Size = new System.Drawing.Size(23, 19);
            this.btIncParam2.TabIndex = 54;
            this.btIncParam2.Text = "+";
            this.btIncParam2.UseVisualStyleBackColor = true;
            this.btIncParam2.Click += new System.EventHandler(this.btIncParam2_Click);
            // 
            // btDecParam1
            // 
            this.btDecParam1.Location = new System.Drawing.Point(193, 6);
            this.btDecParam1.Name = "btDecParam1";
            this.btDecParam1.Size = new System.Drawing.Size(23, 19);
            this.btDecParam1.TabIndex = 53;
            this.btDecParam1.Text = "-";
            this.btDecParam1.UseVisualStyleBackColor = true;
            this.btDecParam1.Click += new System.EventHandler(this.btDecParam1_Click);
            // 
            // btIncParam1
            // 
            this.btIncParam1.Location = new System.Drawing.Point(164, 6);
            this.btIncParam1.Name = "btIncParam1";
            this.btIncParam1.Size = new System.Drawing.Size(23, 19);
            this.btIncParam1.TabIndex = 52;
            this.btIncParam1.Text = "+";
            this.btIncParam1.UseVisualStyleBackColor = true;
            this.btIncParam1.Click += new System.EventHandler(this.btIncParam1_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 97);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(130, 13);
            this.label9.TabIndex = 51;
            this.label9.Text = "0.0064*x*x + 38.112*x + 5";
            // 
            // tbParam3
            // 
            this.tbParam3.Location = new System.Drawing.Point(112, 57);
            this.tbParam3.Name = "tbParam3";
            this.tbParam3.Size = new System.Drawing.Size(46, 20);
            this.tbParam3.TabIndex = 50;
            this.tbParam3.Text = "5";
            this.tbParam3.TextChanged += new System.EventHandler(this.tbParam3_TextChanged);
            // 
            // tbParam2
            // 
            this.tbParam2.Location = new System.Drawing.Point(112, 31);
            this.tbParam2.Name = "tbParam2";
            this.tbParam2.Size = new System.Drawing.Size(46, 20);
            this.tbParam2.TabIndex = 49;
            this.tbParam2.Text = "38,112";
            this.tbParam2.TextChanged += new System.EventHandler(this.tbParam2_TextChanged);
            // 
            // tbParam1
            // 
            this.tbParam1.Location = new System.Drawing.Point(112, 6);
            this.tbParam1.Name = "tbParam1";
            this.tbParam1.Size = new System.Drawing.Size(46, 20);
            this.tbParam1.TabIndex = 48;
            this.tbParam1.Text = "0,0064";
            this.tbParam1.TextChanged += new System.EventHandler(this.tbParam1_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 13);
            this.label8.TabIndex = 47;
            this.label8.Text = "Param3: ( 5 )";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 46;
            this.label6.Text = "Param2: ( 38.112 )";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 45;
            this.label3.Text = "Param1: ( 0.0064 )";
            // 
            // formCalibrate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 125);
            this.Controls.Add(this.btDecParam3);
            this.Controls.Add(this.btIncParam3);
            this.Controls.Add(this.btDecParam2);
            this.Controls.Add(this.btIncParam2);
            this.Controls.Add(this.btDecParam1);
            this.Controls.Add(this.btIncParam1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbParam3);
            this.Controls.Add(this.tbParam2);
            this.Controls.Add(this.tbParam1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Name = "formCalibrate";
            this.Text = "Kallibrierung";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btDecParam3;
        private System.Windows.Forms.Button btIncParam3;
        private System.Windows.Forms.Button btDecParam2;
        private System.Windows.Forms.Button btIncParam2;
        private System.Windows.Forms.Button btDecParam1;
        private System.Windows.Forms.Button btIncParam1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbParam3;
        private System.Windows.Forms.TextBox tbParam2;
        private System.Windows.Forms.TextBox tbParam1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
    }
}