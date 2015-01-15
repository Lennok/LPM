using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VisualControlLib;

namespace UAVSensorControl
{
    public partial class frmCalibrate : Form
    {
        private VisualControlWrapper visualControl = null;
        private str_CalibrationData data = new str_CalibrationData(0.0064,38.112,5, 0);

        public frmCalibrate(VisualControlWrapper visualControl, str_CalibrationData calibrationData)
        {
            InitializeComponent();
            this.visualControl = visualControl;
            tbParam1.Text = calibrationData.param1.ToString();
            tbParam2.Text = calibrationData.param2.ToString();
            tbParam3.Text = calibrationData.param3.ToString();
            data.param4 = calibrationData.param4;
        }

        public void updateParams(double param1, double param2, double param3)
        {
            if (visualControl != null)
            {
                data.param1 = param1;
                data.param2 = param2;
                data.param3 = param3;
                visualControl.setCalculationParams(data);
            }                
        }

        private void tbParam1_TextChanged(object sender, EventArgs e)
        {
            if (tbParam1.Text != String.Empty)
                updateParams(Convert.ToDouble(tbParam1.Text), Convert.ToDouble(tbParam2.Text), Convert.ToDouble(tbParam3.Text));
        }

        private void tbParam2_TextChanged(object sender, EventArgs e)
        {
            if (tbParam2.Text != String.Empty)
                updateParams(Convert.ToDouble(tbParam1.Text), Convert.ToDouble(tbParam2.Text), Convert.ToDouble(tbParam3.Text));
        }

        private void tbParam3_TextChanged(object sender, EventArgs e)
        {
            if (tbParam3.Text != String.Empty)
                updateParams(Convert.ToDouble(tbParam1.Text), Convert.ToDouble(tbParam2.Text), Convert.ToDouble(tbParam3.Text));
        }

        private void btIncParam1_Click(object sender, EventArgs e)
        {
            tbParam1.Text = (Convert.ToDouble(tbParam1.Text) + 0.0001).ToString();
        }

        private void btDecParam1_Click(object sender, EventArgs e)
        {
            tbParam1.Text = (Convert.ToDouble(tbParam1.Text) - 0.0001).ToString();
        }

        private void btIncParam2_Click(object sender, EventArgs e)
        {
            tbParam2.Text = (Convert.ToDouble(tbParam2.Text) + 0.001).ToString();
        }

        private void btDecParam2_Click(object sender, EventArgs e)
        {
            tbParam2.Text = (Convert.ToDouble(tbParam2.Text) - 0.001).ToString();
        }

        private void btIncParam3_Click(object sender, EventArgs e)
        {
            tbParam3.Text = (Convert.ToDouble(tbParam3.Text) + 1).ToString();
        }

        private void btDecParam3_Click(object sender, EventArgs e)
        {
            tbParam3.Text = (Convert.ToDouble(tbParam3.Text) - 1).ToString();
        }
    }
}
