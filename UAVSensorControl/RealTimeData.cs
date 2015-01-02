using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Data.OleDb;
using OSDDataLib;
using System.Linq;
using VisualControlLib;

namespace UAVSensorControl
{
	/// <summary>
	/// Summary description for RealTimeData
	/// </summary>
	public class RealTimeData : System.Windows.Forms.Form
	{

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Timer timerRealTimeData;
		private System.Windows.Forms.ComboBox comboBoxDisplayedTime;
        private System.Windows.Forms.Label label3;
        private Button btPlayPause;
		private System.ComponentModel.IContainer components;
        private TreeView treeVDisplayedData;


        //node strings
        private const String label_OSD = "OSD Daten";
        private const String label_OSD_altitude = "Altitude (OSD)";
        private const String label_OSD_nick = "Nick (OSD)";
        private const String label_OSD_roll = "Roll (OSD)";
        private const String label_OSD_gas = "Gas (OSD)";

        private const String label_Video = "Video Daten";
        private const String label_Video_altitude = "Altitude (Video)";
        private const String label_Video_angle = "Angle (Video)";
        private const String label_Video_relAltitude = "RelativeAltitude (Video)";
        private const String label_Video_OffsetX = "OffsetX (Video)";
        private const String label_Video_OffsetY = "OffsetY (Video)";

        private const String label_US = "Ultraschall Daten";
        private const String label_US_down = "Down (Ultraschall)";
        private const String label_US_front = "Front (Ultraschall)";
        private const String label_US_left = "Left (Ultraschall)";
        private const String label_US_rear = "Rear (Ultraschall)";
        private const String label_US_right = "Right (Ultraschall)";
        private const String label_US_up = "Up (Ultraschall)";



        private CopterData replayData = null;

        public RealTimeData(CopterData replayData) : this()
        {
            this.replayData = replayData;
            btPlayPause.Visible = true;
            if(replayData.osdBuffer.Count>0)
            {
                //Time;Altitude;Nick;Roll;Gas;
                TreeNode OSDNode = new TreeNode(label_OSD);
                OSDNode.Nodes.Add(label_OSD_altitude);
                OSDNode.Nodes.Add(label_OSD_nick);
                OSDNode.Nodes.Add(label_OSD_roll);
                OSDNode.Nodes.Add(label_OSD_gas);
                treeVDisplayedData.Nodes.Add(OSDNode);
            }

            if (replayData.visualBuffer.Count > 0)
            {
                //add video  data node
                TreeNode VisualNode = new TreeNode(label_Video);
                VisualNode.Nodes.Add(label_Video_altitude);
                VisualNode.Nodes.Add(label_Video_angle);
                VisualNode.Nodes.Add(label_Video_relAltitude);
                VisualNode.Nodes.Add(label_Video_OffsetX);
                VisualNode.Nodes.Add(label_Video_OffsetY);
                treeVDisplayedData.Nodes.Add(VisualNode);
            }

            if (replayData.radarBuffer.Count > 0)
            {
                //add radar data node
                TreeNode radarNode = new TreeNode(label_US);
                radarNode.Nodes.Add(label_US_down);
                radarNode.Nodes.Add(label_US_front);
                radarNode.Nodes.Add(label_US_left);
                radarNode.Nodes.Add(label_US_rear);
                radarNode.Nodes.Add(label_US_right);
                radarNode.Nodes.Add(label_US_up);
                treeVDisplayedData.Nodes.Add(radarNode);
            }
            treeVDisplayedData.AfterCheck += treeVDisplayedData_AfterCheck;
            

        }

        void treeVDisplayedData_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                foreach (TreeNode childNode in e.Node.Nodes)
                {
                    childNode.Checked = e.Node.Checked;
                }
            }

            if (e.Node.Parent!= null)
            {
                if (e.Node.Checked)
                {
                    addSeries(e.Node.Text);
                }
                else
                {
                    deleteSeries(e.Node.Text);
                }
            }
        }

		public RealTimeData()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            comboBoxDisplayedTime.SelectedIndex = 2;
            chart1.Series.Clear();
            chart1.Legends["Default"].CustomItems.Clear();
            chart1.ChartAreas["Default"].AxisX.LabelStyle.Format = "HH:mm:ss";
            chart1.ChartAreas["Default"].AxisY.Interval = 50;
            chart1.ChartAreas["Default"].AxisY.MajorTickMark.Interval = 100;
            chart1.ChartAreas["Default"].AxisY.MajorGrid.Interval = 100;
            chart1.ChartAreas["Default"].AxisY.MinorTickMark.Interval = 50;
            chart1.ChartAreas["Default"].AxisY.MinorGrid.Interval = 100;
            chart1.Legends["Default"].Enabled = true;

		}
		

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.comboBoxDisplayedTime = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.timerRealTimeData = new System.Windows.Forms.Timer(this.components);
            this.btPlayPause = new System.Windows.Forms.Button();
            this.treeVDisplayedData = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chart1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(223)))), ((int)(((byte)(193)))));
            this.chart1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            this.chart1.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(64)))), ((int)(((byte)(1)))));
            this.chart1.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            this.chart1.BorderlineWidth = 2;
            this.chart1.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Emboss;
            chartArea1.Area3DStyle.Inclination = 15;
            chartArea1.Area3DStyle.IsClustered = true;
            chartArea1.Area3DStyle.IsRightAngleAxes = false;
            chartArea1.Area3DStyle.Perspective = 10;
            chartArea1.Area3DStyle.Rotation = 10;
            chartArea1.Area3DStyle.WallWidth = 0;
            chartArea1.AxisX.IsLabelAutoFit = false;
            chartArea1.AxisX.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.IsLabelAutoFit = false;
            chartArea1.AxisY.LabelStyle.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            chartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.AxisY.Maximum = 100D;
            chartArea1.AxisY.Minimum = -50D;
            chartArea1.BackColor = System.Drawing.Color.OldLace;
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            chartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.Name = "Default";
            chartArea1.ShadowColor = System.Drawing.Color.Transparent;
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.Enabled = false;
            legend1.Font = new System.Drawing.Font("Trebuchet MS", 8.25F, System.Drawing.FontStyle.Bold);
            legend1.IsTextAutoFit = false;
            legend1.Name = "Default";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(16, 12);
            this.chart1.Name = "chart1";
            series1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(26)))), ((int)(((byte)(59)))), ((int)(((byte)(105)))));
            series1.ChartArea = "Default";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Default";
            series1.Name = "Default";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(390, 294);
            this.chart1.TabIndex = 1;
            // 
            // comboBoxDisplayedTime
            // 
            this.comboBoxDisplayedTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxDisplayedTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDisplayedTime.Items.AddRange(new object[] {
            "10",
            "20",
            "30",
            "40"});
            this.comboBoxDisplayedTime.Location = new System.Drawing.Point(128, 312);
            this.comboBoxDisplayedTime.Name = "comboBoxDisplayedTime";
            this.comboBoxDisplayedTime.Size = new System.Drawing.Size(80, 22);
            this.comboBoxDisplayedTime.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Location = new System.Drawing.Point(13, 312);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "&Time to display:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timerRealTimeData
            // 
            this.timerRealTimeData.Interval = 50;
            this.timerRealTimeData.Tick += new System.EventHandler(this.timerRealTimeData_Tick);
            // 
            // btPlayPause
            // 
            this.btPlayPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btPlayPause.Location = new System.Drawing.Point(353, 311);
            this.btPlayPause.Name = "btPlayPause";
            this.btPlayPause.Size = new System.Drawing.Size(75, 23);
            this.btPlayPause.TabIndex = 4;
            this.btPlayPause.Text = "Play";
            this.btPlayPause.UseVisualStyleBackColor = true;
            this.btPlayPause.Visible = false;
            this.btPlayPause.Click += new System.EventHandler(this.btPlayPause_Click);
            // 
            // treeVDisplayedData
            // 
            this.treeVDisplayedData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.treeVDisplayedData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeVDisplayedData.CheckBoxes = true;
            this.treeVDisplayedData.Location = new System.Drawing.Point(412, 12);
            this.treeVDisplayedData.Name = "treeVDisplayedData";
            this.treeVDisplayedData.Size = new System.Drawing.Size(171, 293);
            this.treeVDisplayedData.TabIndex = 5;
            // 
            // RealTimeData
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(587, 347);
            this.Controls.Add(this.treeVDisplayedData);
            this.Controls.Add(this.btPlayPause);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.comboBoxDisplayedTime);
            this.Controls.Add(this.label3);
            this.Font = new System.Drawing.Font("Verdana", 9F);
            this.Name = "RealTimeData";
            this.Load += new System.EventHandler(this.RealTimeData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        public void AddXY(String name,DateTime time, double value)
        {
             if (value > chart1.ChartAreas["Default"].AxisY.Maximum)
             {
                 chart1.ChartAreas["Default"].AxisY.Maximum = 50 * (((int)value / 50) + 2);
             }

             if (value < chart1.ChartAreas["Default"].AxisY.Minimum)
             {
                 chart1.ChartAreas["Default"].AxisY.Minimum =-1*( 50 * (((int)-1*value / 50) + 1));
             }
                 

             chart1.Series[name].Points.AddXY(time.ToOADate(), value); 
        }

        public int addSeries(String name)
        {
            try
            {
                int test =chart1.Series[name].Points.Count;
                return chart1.Series.Count - 1;
            }
            catch(Exception e)
            {
                chart1.Series.Add(new Series(name)
                {
                    ChartType = SeriesChartType.Line
                });

                chart1.Series[name].XValueType = ChartValueType.DateTime;

                LegendItem legItem = new LegendItem()
                {
                    Tag = chart1.Series[name]
                };

                var oldEntrys = chart1.Legends["Default"].CustomItems;

                chart1.Legends["Default"].CustomItems.Clear();
                foreach (var entry in oldEntrys)
                {
                    chart1.Legends["Default"].CustomItems.Add(entry);
                }

                chart1.Legends["Default"].CustomItems.Add(legItem);
                return chart1.Series.Count - 1;    
            }

            
        }

        public int deleteSeries(String name)
        {
            if (chart1 == null)
                return -1;

            Series toRemove= chart1.Series[name];
            if( toRemove != null)
            {
                chart1.Series.Remove(toRemove);


                LegendItem item = new LegendItem()
                {
                    Tag = name
                };

                if(chart1.Legends["Default"].CustomItems.Contains(item))
                {
                    chart1.Legends["Default"].CustomItems.Remove(item);
                }

                var oldEntrys = chart1.Legends["Default"].CustomItems;

                chart1.Legends["Default"].CustomItems.Clear();
                foreach (var entry in oldEntrys)
                {
                    chart1.Legends["Default"].CustomItems.Add(entry);
                }

               
                return 0;
            }
            else
            {
                return -1;
            }
        }

		public void updateGraph(DateTime upperBorder)
        {
            if (chart1.Series.Count == 0)
            {
                return;
            }

            // Define some variables
            int displayedTime = int.Parse(comboBoxDisplayedTime.Text);
            
            foreach (var series in chart1.Series)
            {

                //while (series.Points.Count > 0 && series.Points[0].XValue < DateTime.Now.AddSeconds(displayedTime * -1).ToOADate())
                //{
                //    series.Points.RemoveAt(0);
                //}

                foreach (var point in series.Points)
                {
                    if(point.Label != String.Empty)
                    {
                        point.Label = String.Empty;
                    }
                }

                if (series.Points.Count > 0)
                {
                    series.Points[series.Points.Count - 1].Label = "Y = " + series.Points[series.Points.Count - 1].YValues[0].ToString("0.0000") + "\nX = " + DateTime.FromOADate(series.Points[series.Points.Count - 1].XValue).ToString("HH:mm:ss");
                    chart1.ApplyPaletteColors();
                    series.Points[series.Points.Count - 1].LabelForeColor = series.Color;

                }
                
            }
            chart1.ChartAreas["Default"].AxisX.Maximum = upperBorder.AddSeconds(5).ToOADate();

            chart1.ChartAreas["Default"].AxisX.Minimum = upperBorder.AddSeconds(displayedTime * -1).ToOADate();

			// Redraw chart
            chart1.Update();
            chart1.Invalidate();
		}




		private void RealTimeData_Load(object sender, System.EventArgs e)
		{
			
			comboBoxDisplayedTime.SelectedIndex = 2;			
		}

        private int lastVideoIndex = 0;
        private int lastOSDIndex = 0;
        private int lastRadarIndex = 0;


        private void timerRealTimeData_Tick(object sender, EventArgs e)
        {
            foreach (TreeNode ParentNode in treeVDisplayedData.Nodes)
            {
                switch (ParentNode.Text)
                {
                        //osd values /////////////////////////////////////////////////////////////////////
                    case label_OSD:
                        {
                            if(HasCheckedChildNodes(ParentNode))
                            {
                                DateTime offsetTime = replayData.osdBuffer.First().Key;
                                var upper = lastUpdateTime.AddTicks(-1 * startTime.Ticks);


                                List<KeyValuePair<DateTime, OSDData>> newEntrys = new List<KeyValuePair<DateTime, OSDData>>();
                                var osdBuffArr = replayData.osdBuffer.ToArray();
                                for (int i = lastOSDIndex; i < osdBuffArr.Count(); i++)
                                {
                                    if ((osdBuffArr[i].Key.AddTicks(-1 * offsetTime.Ticks) <= upper))
                                    {
                                        newEntrys.Add(osdBuffArr[i]);
                                    }
                                    else
                                    {
                                        if (i != 0)
                                        {
                                            lastOSDIndex = i;
                                            break;
                                        }
                                    }
                                }

                                foreach (var osdEntry in newEntrys)
	                            {
                                    foreach (TreeNode childNode in ParentNode.Nodes)
                                    {
                                        if(childNode.Checked)
                                        {
                                            switch (childNode.Text)
                                            {
                                                case label_OSD_altitude:
                                                    AddXY(label_OSD_altitude, osdEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1*offsetTime.Ticks), osdEntry.Value.Altimeter);
                                                    break;

                                                case label_OSD_gas:
                                                    AddXY(label_OSD_gas, osdEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1*offsetTime.Ticks), osdEntry.Value.Gas);
                                                    break;

                                                case label_OSD_nick:
                                                    AddXY(label_OSD_nick, osdEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1*offsetTime.Ticks), osdEntry.Value.AngleNick);
                                                    break;

                                                case label_OSD_roll:
                                                    AddXY(label_OSD_roll, osdEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1*offsetTime.Ticks), osdEntry.Value.AngleRoll);
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                    }
	                            }
                            }
                        }
                        break;

                        //Video Values //////////////////////
                    case label_Video:
                        {
                            if (HasCheckedChildNodes(ParentNode))
                            {
                                DateTime offsetTime = replayData.visualBuffer.First().Key;
                                var upper = lastUpdateTime.AddTicks(-1 * startTime.Ticks);
   

                                List<KeyValuePair<DateTime, str_VisualData>> newEntrys = new List<KeyValuePair<DateTime,str_VisualData>>();
                                var visBuffArr = replayData.visualBuffer.ToArray();
                                for (int i = lastVideoIndex; i < visBuffArr.Count(); i++)
			                    {
			                         if((visBuffArr[i].Key.AddTicks(-1 * offsetTime.Ticks) <= upper))
                                    {
                                        newEntrys.Add(visBuffArr[i]);
                                    }
                                    else
                                    {
                                        if (i != 0)
                                         {
                                             lastVideoIndex = i;
                                             break;
                                         }                                        
                                    }
			                    }

                                foreach (var visEntry in newEntrys)
                                {

                                    foreach (TreeNode childNode in ParentNode.Nodes)
                                    {
                                        if (childNode.Checked)
                                        {
                                            switch (childNode.Text)
                                            {
                                                case label_Video_altitude:
                                                    AddXY(label_Video_altitude, visEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1 * offsetTime.Ticks), visEntry.Value.Altitude);
                                                    break;

                                                case label_Video_angle:
                                                    AddXY(label_Video_angle, visEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1 * offsetTime.Ticks), visEntry.Value.Angle);
                                                    break;

                                                case label_Video_OffsetX:
                                                    AddXY(label_Video_OffsetX, visEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1 * offsetTime.Ticks), visEntry.Value.Offset.X);
                                                    break;

                                                case label_Video_OffsetY:
                                                    AddXY(label_Video_OffsetY, visEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1 * offsetTime.Ticks), visEntry.Value.Offset.Y);
                                                    break;

                                                case label_Video_relAltitude:
                                                    AddXY(label_Video_relAltitude, visEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1 * offsetTime.Ticks), visEntry.Value.RelativeAltitude);
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case label_US:
                        {
                            if (HasCheckedChildNodes(ParentNode))
                            {
                                DateTime offsetTime = replayData.radarBuffer.First().Key;
                                var upper = lastUpdateTime.AddTicks(-1 * startTime.Ticks);


                                List<KeyValuePair<DateTime, str_RadarData>> newEntrys = new List<KeyValuePair<DateTime, str_RadarData>>();
                                var visBuffArr = replayData.radarBuffer.ToArray();
                                for (int i = lastRadarIndex; i < visBuffArr.Count(); i++)
                                {
                                    if ((visBuffArr[i].Key.AddTicks(-1 * offsetTime.Ticks) <= upper))
                                    {
                                        newEntrys.Add(visBuffArr[i]);
                                    }
                                    else
                                    {
                                        if (i != 0)
                                        {
                                            lastRadarIndex = i;
                                            break;
                                        }
                                    }
                                }

                                foreach (var usEntry in newEntrys)
                                {

                                    foreach (TreeNode childNode in ParentNode.Nodes)
                                    {
                                        if (childNode.Checked)
                                        {
                                            switch (childNode.Text)
                                            {
                                                case label_US_down:
                                                    AddXY(label_US_down, usEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1 * offsetTime.Ticks), usEntry.Value.distDown);
                                                    break;

                                                case label_US_front:
                                                    AddXY(label_US_front, usEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1 * offsetTime.Ticks), usEntry.Value.distFront);
                                                    break;

                                                case label_US_left:
                                                    AddXY(label_US_left, usEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1 * offsetTime.Ticks), usEntry.Value.distLeft);
                                                    break;

                                                case label_US_rear:
                                                    AddXY(label_US_rear, usEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1 * offsetTime.Ticks), usEntry.Value.distRear);
                                                    break;

                                                case label_US_right:
                                                    AddXY(label_US_right, usEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1 * offsetTime.Ticks), usEntry.Value.distRight);
                                                    break;

                                                case label_US_up:
                                                    AddXY(label_US_up, usEntry.Key.AddTicks(startTime.Ticks).AddTicks(-1 * offsetTime.Ticks), usEntry.Value.distUp);
                                                    break;

                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            //lastUpdateTime = DateTime.Now;
            lastUpdateTime = lastUpdateTime.AddTicks(timerRealTimeData.Interval * 10000);
            updateGraph(lastUpdateTime);
        }

        // Returns a value indicating whether the specified  
        // TreeNode has checked child nodes. 
        private bool HasCheckedChildNodes(TreeNode node)
        {
            if (node.Nodes.Count == 0) return false;
            foreach (TreeNode childNode in node.Nodes)
            {
                if (childNode.Checked) return true;
                // Recursively check the children of the current child node. 
                if (HasCheckedChildNodes(childNode)) return true;
            }
            return false;
        }

        private DateTime lastUpdateTime = new DateTime(0);
        private DateTime startTime = new DateTime(0);
       // private DateTime offsetTime;

        private void btPlayPause_Click(object sender, EventArgs e)
        {
            if(!timerRealTimeData.Enabled)
            {
                if (startTime.Ticks == 0)
                    startTime = DateTime.Now;
                
                if(lastUpdateTime.Ticks == 0)
                    lastUpdateTime = DateTime.Now;
                timerRealTimeData.Enabled = true;
                btPlayPause.Text = "Pause";
            }
            else
            {
                timerRealTimeData.Enabled = false;
                btPlayPause.Text = "Play";
            }
        }
	}
}
