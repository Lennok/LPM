/*
 *>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
 *>
 *> Christian Pirner        2012/2013
 *>
 *> Steuerungssoftware für Mikrokopter-Drohne
 *> Version 3.1
 *>
 *> Masterarbeit zum Thema:
 *> Machbarkeitsanalyse und Entwicklung einer prototypischen Software
 *> für Sensordatenerfassung und –Verarbeitung zur Flugsteuerung eines Quadrokopters
 *>
 *> Designer Code
 *>
 *>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
 */
namespace UAVSensorControl
{
    partial class frmUAVSensorControl
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gBoxStruct = new System.Windows.Forms.GroupBox();
            this.tBoxStructConfig = new System.Windows.Forms.TextBox();
            this.lblStructConfig = new System.Windows.Forms.Label();
            this.tBoxStructFrame = new System.Windows.Forms.TextBox();
            this.lblStructFrame = new System.Windows.Forms.Label();
            this.tBoxStructFree = new System.Windows.Forms.TextBox();
            this.lblStructFree = new System.Windows.Forms.Label();
            this.tBoxStructHeight = new System.Windows.Forms.TextBox();
            this.lblStructHeight = new System.Windows.Forms.Label();
            this.tBoxStructGas = new System.Windows.Forms.TextBox();
            this.lblStructGas = new System.Windows.Forms.Label();
            this.tBoxStructGier = new System.Windows.Forms.TextBox();
            this.lblStructGier = new System.Windows.Forms.Label();
            this.tBoxStructRoll = new System.Windows.Forms.TextBox();
            this.lblStructRoll = new System.Windows.Forms.Label();
            this.tBoxStructNick = new System.Windows.Forms.TextBox();
            this.lblStructNick = new System.Windows.Forms.Label();
            this.tBoxStructRemoteButtons = new System.Windows.Forms.TextBox();
            this.lblStructRemoteButtons = new System.Windows.Forms.Label();
            this.tBoxStructDigital = new System.Windows.Forms.TextBox();
            this.lblStructDigital = new System.Windows.Forms.Label();
            this.butClose = new System.Windows.Forms.Button();
            this.gBoxSettingManual = new System.Windows.Forms.GroupBox();
            this.lblButtons1and2 = new System.Windows.Forms.Label();
            this.gBoxLeftStick = new System.Windows.Forms.GroupBox();
            this.tBoxGier = new System.Windows.Forms.TextBox();
            this.tBoxGas = new System.Windows.Forms.TextBox();
            this.tBarGas = new System.Windows.Forms.TrackBar();
            this.lblGier = new System.Windows.Forms.Label();
            this.lblGas = new System.Windows.Forms.Label();
            this.tBarGier = new System.Windows.Forms.TrackBar();
            this.cBoxBut1 = new System.Windows.Forms.CheckBox();
            this.tBoxHeight = new System.Windows.Forms.TextBox();
            this.lblInterval2 = new System.Windows.Forms.Label();
            this.rButSendManual = new System.Windows.Forms.RadioButton();
            this.tBoxInterval = new System.Windows.Forms.TextBox();
            this.cBoxBut2 = new System.Windows.Forms.CheckBox();
            this.lblInterval1 = new System.Windows.Forms.Label();
            this.butSend = new System.Windows.Forms.Button();
            this.tBarHeight = new System.Windows.Forms.TrackBar();
            this.rButSendAuto = new System.Windows.Forms.RadioButton();
            this.tBoxRemoteButtons = new System.Windows.Forms.TextBox();
            this.lblHight = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gBoxRightStick = new System.Windows.Forms.GroupBox();
            this.tBoxRoll = new System.Windows.Forms.TextBox();
            this.tBoxNick = new System.Windows.Forms.TextBox();
            this.tBarNick = new System.Windows.Forms.TrackBar();
            this.lblRoll = new System.Windows.Forms.Label();
            this.lblNick = new System.Windows.Forms.Label();
            this.tBarRoll = new System.Windows.Forms.TrackBar();
            this.cBoxEnable = new System.Windows.Forms.CheckBox();
            this.gBoxConfig = new System.Windows.Forms.GroupBox();
            this.lblConnectionLost = new System.Windows.Forms.Label();
            this.butSensorStart = new System.Windows.Forms.Button();
            this.lblDataCount = new System.Windows.Forms.Label();
            this.cBoxDataWright = new System.Windows.Forms.CheckBox();
            this.lblSensorsConnected = new System.Windows.Forms.Label();
            this.lblData = new System.Windows.Forms.Label();
            this.rButAutomaticControl = new System.Windows.Forms.RadioButton();
            this.rButManualControl = new System.Windows.Forms.RadioButton();
            this.lblControlConnected = new System.Windows.Forms.Label();
            this.butConnectControl = new System.Windows.Forms.Button();
            this.lblCOMPort1 = new System.Windows.Forms.Label();
            this.butConnectSensors = new System.Windows.Forms.Button();
            this.tBoxCOMPort1 = new System.Windows.Forms.TextBox();
            this.tBoxCOMPort2 = new System.Windows.Forms.TextBox();
            this.lblCOMPort2 = new System.Windows.Forms.Label();
            this.lblControl2ValueX = new System.Windows.Forms.Label();
            this.tBoxControl100PercentValueX = new System.Windows.Forms.TextBox();
            this.lblControl2 = new System.Windows.Forms.Label();
            this.lblControl1ValueX = new System.Windows.Forms.Label();
            this.tBoxControlStartValueX = new System.Windows.Forms.TextBox();
            this.lblControl1 = new System.Windows.Forms.Label();
            this.lbl_Ping = new System.Windows.Forms.Label();
            this.lblPing = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timerSendInterval = new System.Windows.Forms.Timer(this.components);
            this.timerReceiveInterval = new System.Windows.Forms.Timer(this.components);
            this.gBoxSettingAuto = new System.Windows.Forms.GroupBox();
            this.lblMaximumZ = new System.Windows.Forms.Label();
            this.lblMaximumY = new System.Windows.Forms.Label();
            this.tBoxMaximumX = new System.Windows.Forms.TextBox();
            this.lblMaximumX = new System.Windows.Forms.Label();
            this.tBoxMaximumZ = new System.Windows.Forms.TextBox();
            this.tBoxMaximumY = new System.Windows.Forms.TextBox();
            this.lblMaximum = new System.Windows.Forms.Label();
            this.lblZ = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.lblX = new System.Windows.Forms.Label();
            this.lblControl2ValueY = new System.Windows.Forms.Label();
            this.tBoxControlStartValueY = new System.Windows.Forms.TextBox();
            this.tBoxControl100PercentValueY = new System.Windows.Forms.TextBox();
            this.lblControl1ValueY = new System.Windows.Forms.Label();
            this.lblControl2ValueZ = new System.Windows.Forms.Label();
            this.tBoxControlStartValueZ = new System.Windows.Forms.TextBox();
            this.tBoxControl100PercentValueZ = new System.Windows.Forms.TextBox();
            this.lblControl1ValueZ = new System.Windows.Forms.Label();
            this.lblControl3Value = new System.Windows.Forms.Label();
            this.tBoxHoverGas = new System.Windows.Forms.TextBox();
            this.lblHoverGas = new System.Windows.Forms.Label();
            this.gBoxSensors = new System.Windows.Forms.GroupBox();
            this.gBoxGraphicOut = new System.Windows.Forms.GroupBox();
            this.lblRearText = new System.Windows.Forms.Label();
            this.lblRightText = new System.Windows.Forms.Label();
            this.lblLeftText = new System.Windows.Forms.Label();
            this.lblUpText = new System.Windows.Forms.Label();
            this.lblDownText = new System.Windows.Forms.Label();
            this.lblFrontText = new System.Windows.Forms.Label();
            this.lblDown = new System.Windows.Forms.Label();
            this.lblUp = new System.Windows.Forms.Label();
            this.lblRight = new System.Windows.Forms.Label();
            this.lblLeft = new System.Windows.Forms.Label();
            this.lblRear = new System.Windows.Forms.Label();
            this.lblFront = new System.Windows.Forms.Label();
            this.lblCenterXY = new System.Windows.Forms.Label();
            this.lblCenterZ = new System.Windows.Forms.Label();
            this.butVideoStart = new System.Windows.Forms.Button();
            this.timerUpdateValuesVisual = new System.Windows.Forms.Timer(this.components);
            this.clbShowImage = new System.Windows.Forms.CheckedListBox();
            this.btStartOSDData = new System.Windows.Forms.Button();
            this.lOSDHeight = new System.Windows.Forms.Label();
            this.butShowDiagram = new System.Windows.Forms.Button();
            this.tbCameraIndex = new System.Windows.Forms.TextBox();
            this.lCameraIndex = new System.Windows.Forms.Label();
            this.timerUpdateOSDData = new System.Windows.Forms.Timer(this.components);
            this.timerUpdateGraph = new System.Windows.Forms.Timer(this.components);
            this.lblOSDInterval = new System.Windows.Forms.Label();
            this.tbOSDUpdateInterval = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbVIdeopdateInterval = new System.Windows.Forms.TextBox();
            this.lblVisUpdateInterval = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btResetOSD = new System.Windows.Forms.Button();
            this.timerUpdateValuesVisual2 = new System.Windows.Forms.Timer(this.components);
            this.btShowVideoCalibration = new System.Windows.Forms.Button();
            this.cbShowCompare = new System.Windows.Forms.CheckBox();
            this.comboCameraType = new System.Windows.Forms.ComboBox();
            this.btReplay = new System.Windows.Forms.Button();
            this.cbowriteprotocol = new System.Windows.Forms.CheckBox();
            this.gBoxStruct.SuspendLayout();
            this.gBoxSettingManual.SuspendLayout();
            this.gBoxLeftStick.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBarGas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarGier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarHeight)).BeginInit();
            this.gBoxRightStick.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBarNick)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarRoll)).BeginInit();
            this.gBoxConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.gBoxSettingAuto.SuspendLayout();
            this.gBoxSensors.SuspendLayout();
            this.gBoxGraphicOut.SuspendLayout();
            this.SuspendLayout();
            // 
            // gBoxStruct
            // 
            this.gBoxStruct.Controls.Add(this.tBoxStructConfig);
            this.gBoxStruct.Controls.Add(this.lblStructConfig);
            this.gBoxStruct.Controls.Add(this.tBoxStructFrame);
            this.gBoxStruct.Controls.Add(this.lblStructFrame);
            this.gBoxStruct.Controls.Add(this.tBoxStructFree);
            this.gBoxStruct.Controls.Add(this.lblStructFree);
            this.gBoxStruct.Controls.Add(this.tBoxStructHeight);
            this.gBoxStruct.Controls.Add(this.lblStructHeight);
            this.gBoxStruct.Controls.Add(this.tBoxStructGas);
            this.gBoxStruct.Controls.Add(this.lblStructGas);
            this.gBoxStruct.Controls.Add(this.tBoxStructGier);
            this.gBoxStruct.Controls.Add(this.lblStructGier);
            this.gBoxStruct.Controls.Add(this.tBoxStructRoll);
            this.gBoxStruct.Controls.Add(this.lblStructRoll);
            this.gBoxStruct.Controls.Add(this.tBoxStructNick);
            this.gBoxStruct.Controls.Add(this.lblStructNick);
            this.gBoxStruct.Controls.Add(this.tBoxStructRemoteButtons);
            this.gBoxStruct.Controls.Add(this.lblStructRemoteButtons);
            this.gBoxStruct.Controls.Add(this.tBoxStructDigital);
            this.gBoxStruct.Controls.Add(this.lblStructDigital);
            this.gBoxStruct.Location = new System.Drawing.Point(582, 12);
            this.gBoxStruct.Name = "gBoxStruct";
            this.gBoxStruct.Size = new System.Drawing.Size(401, 153);
            this.gBoxStruct.TabIndex = 3;
            this.gBoxStruct.TabStop = false;
            this.gBoxStruct.Text = "Ausgabewerte der Steuerung:";
            // 
            // tBoxStructConfig
            // 
            this.tBoxStructConfig.Location = new System.Drawing.Point(310, 124);
            this.tBoxStructConfig.Name = "tBoxStructConfig";
            this.tBoxStructConfig.ReadOnly = true;
            this.tBoxStructConfig.Size = new System.Drawing.Size(49, 20);
            this.tBoxStructConfig.TabIndex = 19;
            // 
            // lblStructConfig
            // 
            this.lblStructConfig.AutoSize = true;
            this.lblStructConfig.Location = new System.Drawing.Point(201, 124);
            this.lblStructConfig.Name = "lblStructConfig";
            this.lblStructConfig.Size = new System.Drawing.Size(68, 13);
            this.lblStructConfig.TabIndex = 18;
            this.lblStructConfig.Text = "sbyte Config:";
            // 
            // tBoxStructFrame
            // 
            this.tBoxStructFrame.Location = new System.Drawing.Point(310, 98);
            this.tBoxStructFrame.Name = "tBoxStructFrame";
            this.tBoxStructFrame.ReadOnly = true;
            this.tBoxStructFrame.Size = new System.Drawing.Size(49, 20);
            this.tBoxStructFrame.TabIndex = 17;
            // 
            // lblStructFrame
            // 
            this.lblStructFrame.AutoSize = true;
            this.lblStructFrame.Location = new System.Drawing.Point(201, 98);
            this.lblStructFrame.Name = "lblStructFrame";
            this.lblStructFrame.Size = new System.Drawing.Size(67, 13);
            this.lblStructFrame.TabIndex = 16;
            this.lblStructFrame.Text = "sbyte Frame:";
            // 
            // tBoxStructFree
            // 
            this.tBoxStructFree.Location = new System.Drawing.Point(310, 72);
            this.tBoxStructFree.Name = "tBoxStructFree";
            this.tBoxStructFree.ReadOnly = true;
            this.tBoxStructFree.Size = new System.Drawing.Size(49, 20);
            this.tBoxStructFree.TabIndex = 15;
            // 
            // lblStructFree
            // 
            this.lblStructFree.AutoSize = true;
            this.lblStructFree.Location = new System.Drawing.Point(201, 72);
            this.lblStructFree.Name = "lblStructFree";
            this.lblStructFree.Size = new System.Drawing.Size(59, 13);
            this.lblStructFree.TabIndex = 14;
            this.lblStructFree.Text = "sbyte Free:";
            // 
            // tBoxStructHeight
            // 
            this.tBoxStructHeight.Location = new System.Drawing.Point(310, 46);
            this.tBoxStructHeight.Name = "tBoxStructHeight";
            this.tBoxStructHeight.ReadOnly = true;
            this.tBoxStructHeight.Size = new System.Drawing.Size(49, 20);
            this.tBoxStructHeight.TabIndex = 13;
            // 
            // lblStructHeight
            // 
            this.lblStructHeight.AutoSize = true;
            this.lblStructHeight.Location = new System.Drawing.Point(201, 46);
            this.lblStructHeight.Name = "lblStructHeight";
            this.lblStructHeight.Size = new System.Drawing.Size(69, 13);
            this.lblStructHeight.TabIndex = 12;
            this.lblStructHeight.Text = "sbyte Height:";
            // 
            // tBoxStructGas
            // 
            this.tBoxStructGas.Location = new System.Drawing.Point(310, 20);
            this.tBoxStructGas.Name = "tBoxStructGas";
            this.tBoxStructGas.ReadOnly = true;
            this.tBoxStructGas.Size = new System.Drawing.Size(49, 20);
            this.tBoxStructGas.TabIndex = 11;
            // 
            // lblStructGas
            // 
            this.lblStructGas.AutoSize = true;
            this.lblStructGas.Location = new System.Drawing.Point(201, 20);
            this.lblStructGas.Name = "lblStructGas";
            this.lblStructGas.Size = new System.Drawing.Size(52, 13);
            this.lblStructGas.TabIndex = 10;
            this.lblStructGas.Text = "byte Gas:";
            // 
            // tBoxStructGier
            // 
            this.tBoxStructGier.Location = new System.Drawing.Point(116, 124);
            this.tBoxStructGier.Name = "tBoxStructGier";
            this.tBoxStructGier.ReadOnly = true;
            this.tBoxStructGier.Size = new System.Drawing.Size(49, 20);
            this.tBoxStructGier.TabIndex = 9;
            // 
            // lblStructGier
            // 
            this.lblStructGier.AutoSize = true;
            this.lblStructGier.Location = new System.Drawing.Point(7, 124);
            this.lblStructGier.Name = "lblStructGier";
            this.lblStructGier.Size = new System.Drawing.Size(57, 13);
            this.lblStructGier.TabIndex = 8;
            this.lblStructGier.Text = "sbyte Gier:";
            // 
            // tBoxStructRoll
            // 
            this.tBoxStructRoll.Location = new System.Drawing.Point(116, 98);
            this.tBoxStructRoll.Name = "tBoxStructRoll";
            this.tBoxStructRoll.ReadOnly = true;
            this.tBoxStructRoll.Size = new System.Drawing.Size(49, 20);
            this.tBoxStructRoll.TabIndex = 7;
            // 
            // lblStructRoll
            // 
            this.lblStructRoll.AutoSize = true;
            this.lblStructRoll.Location = new System.Drawing.Point(7, 98);
            this.lblStructRoll.Name = "lblStructRoll";
            this.lblStructRoll.Size = new System.Drawing.Size(56, 13);
            this.lblStructRoll.TabIndex = 6;
            this.lblStructRoll.Text = "sbyte Roll:";
            // 
            // tBoxStructNick
            // 
            this.tBoxStructNick.Location = new System.Drawing.Point(116, 72);
            this.tBoxStructNick.Name = "tBoxStructNick";
            this.tBoxStructNick.ReadOnly = true;
            this.tBoxStructNick.Size = new System.Drawing.Size(49, 20);
            this.tBoxStructNick.TabIndex = 5;
            // 
            // lblStructNick
            // 
            this.lblStructNick.AutoSize = true;
            this.lblStructNick.Location = new System.Drawing.Point(7, 72);
            this.lblStructNick.Name = "lblStructNick";
            this.lblStructNick.Size = new System.Drawing.Size(55, 13);
            this.lblStructNick.TabIndex = 4;
            this.lblStructNick.Text = "byte Nick:";
            // 
            // tBoxStructRemoteButtons
            // 
            this.tBoxStructRemoteButtons.Location = new System.Drawing.Point(116, 46);
            this.tBoxStructRemoteButtons.Name = "tBoxStructRemoteButtons";
            this.tBoxStructRemoteButtons.ReadOnly = true;
            this.tBoxStructRemoteButtons.Size = new System.Drawing.Size(49, 20);
            this.tBoxStructRemoteButtons.TabIndex = 3;
            // 
            // lblStructRemoteButtons
            // 
            this.lblStructRemoteButtons.AutoSize = true;
            this.lblStructRemoteButtons.Location = new System.Drawing.Point(7, 46);
            this.lblStructRemoteButtons.Name = "lblStructRemoteButtons";
            this.lblStructRemoteButtons.Size = new System.Drawing.Size(109, 13);
            this.lblStructRemoteButtons.TabIndex = 2;
            this.lblStructRemoteButtons.Text = "byte Remote Buttons:";
            // 
            // tBoxStructDigital
            // 
            this.tBoxStructDigital.Location = new System.Drawing.Point(116, 20);
            this.tBoxStructDigital.Name = "tBoxStructDigital";
            this.tBoxStructDigital.ReadOnly = true;
            this.tBoxStructDigital.Size = new System.Drawing.Size(49, 20);
            this.tBoxStructDigital.TabIndex = 1;
            // 
            // lblStructDigital
            // 
            this.lblStructDigital.AutoSize = true;
            this.lblStructDigital.Location = new System.Drawing.Point(7, 20);
            this.lblStructDigital.Name = "lblStructDigital";
            this.lblStructDigital.Size = new System.Drawing.Size(74, 13);
            this.lblStructDigital.TabIndex = 0;
            this.lblStructDigital.Text = "byte[2] Digital:";
            // 
            // butClose
            // 
            this.butClose.Location = new System.Drawing.Point(887, 684);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(100, 23);
            this.butClose.TabIndex = 6;
            this.butClose.Text = "&Beenden";
            this.butClose.UseVisualStyleBackColor = true;
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // gBoxSettingManual
            // 
            this.gBoxSettingManual.Controls.Add(this.lblButtons1and2);
            this.gBoxSettingManual.Controls.Add(this.gBoxLeftStick);
            this.gBoxSettingManual.Controls.Add(this.cBoxBut1);
            this.gBoxSettingManual.Controls.Add(this.tBoxHeight);
            this.gBoxSettingManual.Controls.Add(this.lblInterval2);
            this.gBoxSettingManual.Controls.Add(this.rButSendManual);
            this.gBoxSettingManual.Controls.Add(this.tBoxInterval);
            this.gBoxSettingManual.Controls.Add(this.cBoxBut2);
            this.gBoxSettingManual.Controls.Add(this.lblInterval1);
            this.gBoxSettingManual.Controls.Add(this.butSend);
            this.gBoxSettingManual.Controls.Add(this.tBarHeight);
            this.gBoxSettingManual.Controls.Add(this.rButSendAuto);
            this.gBoxSettingManual.Controls.Add(this.tBoxRemoteButtons);
            this.gBoxSettingManual.Controls.Add(this.lblHight);
            this.gBoxSettingManual.Controls.Add(this.label1);
            this.gBoxSettingManual.Controls.Add(this.gBoxRightStick);
            this.gBoxSettingManual.Location = new System.Drawing.Point(12, 309);
            this.gBoxSettingManual.Name = "gBoxSettingManual";
            this.gBoxSettingManual.Size = new System.Drawing.Size(564, 260);
            this.gBoxSettingManual.TabIndex = 2;
            this.gBoxSettingManual.TabStop = false;
            this.gBoxSettingManual.Text = "Manuelle Steuerung:";
            // 
            // lblButtons1and2
            // 
            this.lblButtons1and2.AutoSize = true;
            this.lblButtons1and2.Location = new System.Drawing.Point(146, 233);
            this.lblButtons1and2.Name = "lblButtons1and2";
            this.lblButtons1and2.Size = new System.Drawing.Size(136, 13);
            this.lblButtons1and2.TabIndex = 13;
            this.lblButtons1and2.Text = "( werden nicht verwendet! )";
            // 
            // gBoxLeftStick
            // 
            this.gBoxLeftStick.Controls.Add(this.tBoxGier);
            this.gBoxLeftStick.Controls.Add(this.tBoxGas);
            this.gBoxLeftStick.Controls.Add(this.tBarGas);
            this.gBoxLeftStick.Controls.Add(this.lblGier);
            this.gBoxLeftStick.Controls.Add(this.lblGas);
            this.gBoxLeftStick.Controls.Add(this.tBarGier);
            this.gBoxLeftStick.Location = new System.Drawing.Point(100, 73);
            this.gBoxLeftStick.Name = "gBoxLeftStick";
            this.gBoxLeftStick.Size = new System.Drawing.Size(210, 150);
            this.gBoxLeftStick.TabIndex = 9;
            this.gBoxLeftStick.TabStop = false;
            this.gBoxLeftStick.Text = "Linker Stick:";
            // 
            // tBoxGier
            // 
            this.tBoxGier.Location = new System.Drawing.Point(136, 21);
            this.tBoxGier.MaxLength = 4;
            this.tBoxGier.Name = "tBoxGier";
            this.tBoxGier.ReadOnly = true;
            this.tBoxGier.Size = new System.Drawing.Size(42, 20);
            this.tBoxGier.TabIndex = 5;
            // 
            // tBoxGas
            // 
            this.tBoxGas.Location = new System.Drawing.Point(42, 21);
            this.tBoxGas.MaxLength = 4;
            this.tBoxGas.Name = "tBoxGas";
            this.tBoxGas.ReadOnly = true;
            this.tBoxGas.Size = new System.Drawing.Size(42, 20);
            this.tBoxGas.TabIndex = 2;
            // 
            // tBarGas
            // 
            this.tBarGas.Location = new System.Drawing.Point(29, 39);
            this.tBarGas.Maximum = 240;
            this.tBarGas.Name = "tBarGas";
            this.tBarGas.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tBarGas.Size = new System.Drawing.Size(45, 104);
            this.tBarGas.TabIndex = 1;
            this.tBarGas.TickFrequency = 32;
            this.tBarGas.ValueChanged += new System.EventHandler(this.tBarGas_ValueChanged);
            // 
            // lblGier
            // 
            this.lblGier.AutoSize = true;
            this.lblGier.Location = new System.Drawing.Point(108, 24);
            this.lblGier.Name = "lblGier";
            this.lblGier.Size = new System.Drawing.Size(29, 13);
            this.lblGier.TabIndex = 3;
            this.lblGier.Text = "Gier:";
            // 
            // lblGas
            // 
            this.lblGas.AutoSize = true;
            this.lblGas.Location = new System.Drawing.Point(6, 24);
            this.lblGas.Name = "lblGas";
            this.lblGas.Size = new System.Drawing.Size(29, 13);
            this.lblGas.TabIndex = 0;
            this.lblGas.Text = "Gas:";
            // 
            // tBarGier
            // 
            this.tBarGier.Location = new System.Drawing.Point(98, 82);
            this.tBarGier.Maximum = 100;
            this.tBarGier.Minimum = -100;
            this.tBarGier.Name = "tBarGier";
            this.tBarGier.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tBarGier.Size = new System.Drawing.Size(104, 45);
            this.tBarGier.TabIndex = 4;
            this.tBarGier.TickFrequency = 20;
            this.tBarGier.ValueChanged += new System.EventHandler(this.tBarGier_ValueChanged);
            // 
            // cBoxBut1
            // 
            this.cBoxBut1.AutoSize = true;
            this.cBoxBut1.Location = new System.Drawing.Point(10, 233);
            this.cBoxBut1.Name = "cBoxBut1";
            this.cBoxBut1.Size = new System.Drawing.Size(66, 17);
            this.cBoxBut1.TabIndex = 11;
            this.cBoxBut1.Text = "Button &1";
            this.cBoxBut1.UseVisualStyleBackColor = true;
            this.cBoxBut1.CheckedChanged += new System.EventHandler(this.cBoxesForDigital_CheckedChanged);
            // 
            // tBoxHeight
            // 
            this.tBoxHeight.Location = new System.Drawing.Point(44, 94);
            this.tBoxHeight.MaxLength = 4;
            this.tBoxHeight.Name = "tBoxHeight";
            this.tBoxHeight.ReadOnly = true;
            this.tBoxHeight.Size = new System.Drawing.Size(42, 20);
            this.tBoxHeight.TabIndex = 8;
            // 
            // lblInterval2
            // 
            this.lblInterval2.AutoSize = true;
            this.lblInterval2.Location = new System.Drawing.Point(273, 48);
            this.lblInterval2.Name = "lblInterval2";
            this.lblInterval2.Size = new System.Drawing.Size(23, 13);
            this.lblInterval2.TabIndex = 5;
            this.lblInterval2.Text = "ms.";
            // 
            // rButSendManual
            // 
            this.rButSendManual.AutoSize = true;
            this.rButSendManual.Checked = true;
            this.rButSendManual.Location = new System.Drawing.Point(6, 21);
            this.rButSendManual.Name = "rButSendManual";
            this.rButSendManual.Size = new System.Drawing.Size(100, 17);
            this.rButSendManual.TabIndex = 0;
            this.rButSendManual.TabStop = true;
            this.rButSendManual.Text = "Manue&ll senden";
            this.rButSendManual.UseVisualStyleBackColor = true;
            this.rButSendManual.CheckedChanged += new System.EventHandler(this.rButSend_CheckedChanged);
            // 
            // tBoxInterval
            // 
            this.tBoxInterval.Location = new System.Drawing.Point(232, 46);
            this.tBoxInterval.MaxLength = 4;
            this.tBoxInterval.Name = "tBoxInterval";
            this.tBoxInterval.Size = new System.Drawing.Size(35, 20);
            this.tBoxInterval.TabIndex = 4;
            this.tBoxInterval.Text = "200";
            this.tBoxInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBoxInterval.TextChanged += new System.EventHandler(this.tBoxInterval_TextChanged);
            this.tBoxInterval.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxNumbercheck_KeyPress);
            // 
            // cBoxBut2
            // 
            this.cBoxBut2.AutoSize = true;
            this.cBoxBut2.Location = new System.Drawing.Point(82, 233);
            this.cBoxBut2.Name = "cBoxBut2";
            this.cBoxBut2.Size = new System.Drawing.Size(66, 17);
            this.cBoxBut2.TabIndex = 12;
            this.cBoxBut2.Text = "Button &2";
            this.cBoxBut2.UseVisualStyleBackColor = true;
            this.cBoxBut2.CheckedChanged += new System.EventHandler(this.cBoxesForDigital_CheckedChanged);
            // 
            // lblInterval1
            // 
            this.lblInterval1.AutoSize = true;
            this.lblInterval1.Location = new System.Drawing.Point(139, 48);
            this.lblInterval1.Name = "lblInterval1";
            this.lblInterval1.Size = new System.Drawing.Size(81, 13);
            this.lblInterval1.TabIndex = 3;
            this.lblInterval1.Text = "Sende Intervall:";
            // 
            // butSend
            // 
            this.butSend.Enabled = false;
            this.butSend.Location = new System.Drawing.Point(141, 19);
            this.butSend.Name = "butSend";
            this.butSend.Size = new System.Drawing.Size(75, 23);
            this.butSend.TabIndex = 1;
            this.butSend.Text = "Se&nden";
            this.butSend.UseVisualStyleBackColor = true;
            this.butSend.Click += new System.EventHandler(this.butSend_Click);
            // 
            // tBarHeight
            // 
            this.tBarHeight.Location = new System.Drawing.Point(31, 112);
            this.tBarHeight.Maximum = 126;
            this.tBarHeight.Minimum = -126;
            this.tBarHeight.Name = "tBarHeight";
            this.tBarHeight.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tBarHeight.Size = new System.Drawing.Size(45, 104);
            this.tBarHeight.TabIndex = 7;
            this.tBarHeight.TickFrequency = 32;
            this.tBarHeight.ValueChanged += new System.EventHandler(this.tBarHeight_ValueChanged);
            // 
            // rButSendAuto
            // 
            this.rButSendAuto.AutoSize = true;
            this.rButSendAuto.Location = new System.Drawing.Point(6, 47);
            this.rButSendAuto.Name = "rButSendAuto";
            this.rButSendAuto.Size = new System.Drawing.Size(121, 17);
            this.rButSendAuto.TabIndex = 2;
            this.rButSendAuto.Text = "Automatisc&h senden";
            this.rButSendAuto.UseVisualStyleBackColor = true;
            this.rButSendAuto.CheckedChanged += new System.EventHandler(this.rButSend_CheckedChanged);
            // 
            // tBoxRemoteButtons
            // 
            this.tBoxRemoteButtons.Location = new System.Drawing.Point(296, 232);
            this.tBoxRemoteButtons.MaxLength = 1;
            this.tBoxRemoteButtons.Name = "tBoxRemoteButtons";
            this.tBoxRemoteButtons.Size = new System.Drawing.Size(20, 20);
            this.tBoxRemoteButtons.TabIndex = 14;
            this.tBoxRemoteButtons.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tBoxRemoteButtons.TextChanged += new System.EventHandler(this.tBoxRemoteButtons_TextChanged);
            // 
            // lblHight
            // 
            this.lblHight.AutoSize = true;
            this.lblHight.Location = new System.Drawing.Point(4, 97);
            this.lblHight.Name = "lblHight";
            this.lblHight.Size = new System.Drawing.Size(41, 13);
            this.lblHight.TabIndex = 6;
            this.lblHight.Text = "Height:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(323, 233);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "&RemoteButtons (Funktionalität unbekannt)";
            // 
            // gBoxRightStick
            // 
            this.gBoxRightStick.Controls.Add(this.tBoxRoll);
            this.gBoxRightStick.Controls.Add(this.tBoxNick);
            this.gBoxRightStick.Controls.Add(this.tBarNick);
            this.gBoxRightStick.Controls.Add(this.lblRoll);
            this.gBoxRightStick.Controls.Add(this.lblNick);
            this.gBoxRightStick.Controls.Add(this.tBarRoll);
            this.gBoxRightStick.Location = new System.Drawing.Point(336, 73);
            this.gBoxRightStick.Name = "gBoxRightStick";
            this.gBoxRightStick.Size = new System.Drawing.Size(210, 150);
            this.gBoxRightStick.TabIndex = 10;
            this.gBoxRightStick.TabStop = false;
            this.gBoxRightStick.Text = "Rechter Stick:";
            // 
            // tBoxRoll
            // 
            this.tBoxRoll.Location = new System.Drawing.Point(136, 21);
            this.tBoxRoll.MaxLength = 4;
            this.tBoxRoll.Name = "tBoxRoll";
            this.tBoxRoll.ReadOnly = true;
            this.tBoxRoll.Size = new System.Drawing.Size(42, 20);
            this.tBoxRoll.TabIndex = 5;
            // 
            // tBoxNick
            // 
            this.tBoxNick.Location = new System.Drawing.Point(42, 21);
            this.tBoxNick.MaxLength = 4;
            this.tBoxNick.Name = "tBoxNick";
            this.tBoxNick.ReadOnly = true;
            this.tBoxNick.Size = new System.Drawing.Size(42, 20);
            this.tBoxNick.TabIndex = 2;
            // 
            // tBarNick
            // 
            this.tBarNick.Location = new System.Drawing.Point(29, 39);
            this.tBarNick.Maximum = 126;
            this.tBarNick.Minimum = -126;
            this.tBarNick.Name = "tBarNick";
            this.tBarNick.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tBarNick.Size = new System.Drawing.Size(45, 104);
            this.tBarNick.TabIndex = 1;
            this.tBarNick.TickFrequency = 32;
            this.tBarNick.ValueChanged += new System.EventHandler(this.tBarNick_ValueChanged);
            // 
            // lblRoll
            // 
            this.lblRoll.AutoSize = true;
            this.lblRoll.Location = new System.Drawing.Point(108, 24);
            this.lblRoll.Name = "lblRoll";
            this.lblRoll.Size = new System.Drawing.Size(28, 13);
            this.lblRoll.TabIndex = 3;
            this.lblRoll.Text = "Roll:";
            // 
            // lblNick
            // 
            this.lblNick.AutoSize = true;
            this.lblNick.Location = new System.Drawing.Point(6, 24);
            this.lblNick.Name = "lblNick";
            this.lblNick.Size = new System.Drawing.Size(32, 13);
            this.lblNick.TabIndex = 0;
            this.lblNick.Text = "Nick:";
            // 
            // tBarRoll
            // 
            this.tBarRoll.Location = new System.Drawing.Point(98, 82);
            this.tBarRoll.Maximum = 126;
            this.tBarRoll.Minimum = -126;
            this.tBarRoll.Name = "tBarRoll";
            this.tBarRoll.Size = new System.Drawing.Size(104, 45);
            this.tBarRoll.TabIndex = 4;
            this.tBarRoll.TickFrequency = 32;
            this.tBarRoll.ValueChanged += new System.EventHandler(this.tBarRoll_ValueChanged);
            // 
            // cBoxEnable
            // 
            this.cBoxEnable.AutoSize = true;
            this.cBoxEnable.Location = new System.Drawing.Point(7, 123);
            this.cBoxEnable.Name = "cBoxEnable";
            this.cBoxEnable.Size = new System.Drawing.Size(120, 17);
            this.cBoxEnable.TabIndex = 11;
            this.cBoxEnable.Text = "Extern&Controls aktiv";
            this.cBoxEnable.UseVisualStyleBackColor = true;
            this.cBoxEnable.CheckedChanged += new System.EventHandler(this.cBoxEnable_CheckedChanged);
            // 
            // gBoxConfig
            // 
            this.gBoxConfig.Controls.Add(this.lblConnectionLost);
            this.gBoxConfig.Controls.Add(this.butSensorStart);
            this.gBoxConfig.Controls.Add(this.lblDataCount);
            this.gBoxConfig.Controls.Add(this.cBoxDataWright);
            this.gBoxConfig.Controls.Add(this.lblSensorsConnected);
            this.gBoxConfig.Controls.Add(this.lblData);
            this.gBoxConfig.Controls.Add(this.rButAutomaticControl);
            this.gBoxConfig.Controls.Add(this.rButManualControl);
            this.gBoxConfig.Controls.Add(this.lblControlConnected);
            this.gBoxConfig.Controls.Add(this.butConnectControl);
            this.gBoxConfig.Controls.Add(this.lblCOMPort1);
            this.gBoxConfig.Controls.Add(this.butConnectSensors);
            this.gBoxConfig.Controls.Add(this.tBoxCOMPort1);
            this.gBoxConfig.Controls.Add(this.cBoxEnable);
            this.gBoxConfig.Controls.Add(this.tBoxCOMPort2);
            this.gBoxConfig.Controls.Add(this.lblCOMPort2);
            this.gBoxConfig.Location = new System.Drawing.Point(12, 12);
            this.gBoxConfig.Name = "gBoxConfig";
            this.gBoxConfig.Size = new System.Drawing.Size(564, 153);
            this.gBoxConfig.TabIndex = 0;
            this.gBoxConfig.TabStop = false;
            this.gBoxConfig.Text = "Grundlegende Einstellungen:";
            // 
            // lblConnectionLost
            // 
            this.lblConnectionLost.BackColor = System.Drawing.Color.Red;
            this.lblConnectionLost.Location = new System.Drawing.Point(447, 67);
            this.lblConnectionLost.Name = "lblConnectionLost";
            this.lblConnectionLost.Size = new System.Drawing.Size(99, 51);
            this.lblConnectionLost.TabIndex = 10;
            this.lblConnectionLost.Text = "Verbindung gestört! Drohne antwortet nicht mehr!";
            this.lblConnectionLost.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblConnectionLost.Visible = false;
            // 
            // butSensorStart
            // 
            this.butSensorStart.Enabled = false;
            this.butSensorStart.Location = new System.Drawing.Point(447, 16);
            this.butSensorStart.Name = "butSensorStart";
            this.butSensorStart.Size = new System.Drawing.Size(100, 40);
            this.butSensorStart.TabIndex = 8;
            this.butSensorStart.Text = "S&ensorabfrage starten";
            this.butSensorStart.UseVisualStyleBackColor = true;
            this.butSensorStart.Click += new System.EventHandler(this.butSensorStart_Click);
            // 
            // lblDataCount
            // 
            this.lblDataCount.AutoSize = true;
            this.lblDataCount.Location = new System.Drawing.Point(448, 124);
            this.lblDataCount.Name = "lblDataCount";
            this.lblDataCount.Size = new System.Drawing.Size(13, 13);
            this.lblDataCount.TabIndex = 14;
            this.lblDataCount.Text = "0";
            // 
            // cBoxDataWright
            // 
            this.cBoxDataWright.AutoSize = true;
            this.cBoxDataWright.Location = new System.Drawing.Point(202, 123);
            this.cBoxDataWright.Name = "cBoxDataWright";
            this.cBoxDataWright.Size = new System.Drawing.Size(96, 17);
            this.cBoxDataWright.TabIndex = 12;
            this.cBoxDataWright.Text = "&Daten Logging";
            this.cBoxDataWright.UseVisualStyleBackColor = true;
            this.cBoxDataWright.CheckedChanged += new System.EventHandler(this.cBoxDataWright_CheckedChanged);
            // 
            // lblSensorsConnected
            // 
            this.lblSensorsConnected.BackColor = System.Drawing.Color.Red;
            this.lblSensorsConnected.Location = new System.Drawing.Point(326, 67);
            this.lblSensorsConnected.Name = "lblSensorsConnected";
            this.lblSensorsConnected.Size = new System.Drawing.Size(100, 40);
            this.lblSensorsConnected.TabIndex = 7;
            this.lblSensorsConnected.Text = "Sensoren nicht verbunden";
            this.lblSensorsConnected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Location = new System.Drawing.Point(323, 124);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(125, 13);
            this.lblData.TabIndex = 13;
            this.lblData.Text = "Datensätze geschrieben:";
            // 
            // rButAutomaticControl
            // 
            this.rButAutomaticControl.AutoSize = true;
            this.rButAutomaticControl.Location = new System.Drawing.Point(7, 94);
            this.rButAutomaticControl.Name = "rButAutomaticControl";
            this.rButAutomaticControl.Size = new System.Drawing.Size(141, 17);
            this.rButAutomaticControl.TabIndex = 10;
            this.rButAutomaticControl.Text = "&Automatische Steuerung";
            this.rButAutomaticControl.UseVisualStyleBackColor = true;
            this.rButAutomaticControl.CheckedChanged += new System.EventHandler(this.rButControl_CheckedChanged);
            // 
            // rButManualControl
            // 
            this.rButManualControl.AutoSize = true;
            this.rButManualControl.Checked = true;
            this.rButManualControl.Location = new System.Drawing.Point(7, 70);
            this.rButManualControl.Name = "rButManualControl";
            this.rButManualControl.Size = new System.Drawing.Size(120, 17);
            this.rButManualControl.TabIndex = 9;
            this.rButManualControl.TabStop = true;
            this.rButManualControl.Text = "&Manuelle Steuerung";
            this.rButManualControl.UseVisualStyleBackColor = true;
            this.rButManualControl.CheckedChanged += new System.EventHandler(this.rButControl_CheckedChanged);
            // 
            // lblControlConnected
            // 
            this.lblControlConnected.BackColor = System.Drawing.Color.Red;
            this.lblControlConnected.Location = new System.Drawing.Point(202, 67);
            this.lblControlConnected.Name = "lblControlConnected";
            this.lblControlConnected.Size = new System.Drawing.Size(100, 40);
            this.lblControlConnected.TabIndex = 5;
            this.lblControlConnected.Text = "Steuerung nicht verbunden";
            this.lblControlConnected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // butConnectControl
            // 
            this.butConnectControl.Location = new System.Drawing.Point(202, 16);
            this.butConnectControl.Name = "butConnectControl";
            this.butConnectControl.Size = new System.Drawing.Size(100, 40);
            this.butConnectControl.TabIndex = 4;
            this.butConnectControl.Text = "&Verbinden Steuerung";
            this.butConnectControl.UseVisualStyleBackColor = true;
            this.butConnectControl.Click += new System.EventHandler(this.butConnectControl_Click);
            // 
            // lblCOMPort1
            // 
            this.lblCOMPort1.AutoSize = true;
            this.lblCOMPort1.Location = new System.Drawing.Point(6, 20);
            this.lblCOMPort1.Name = "lblCOMPort1";
            this.lblCOMPort1.Size = new System.Drawing.Size(125, 13);
            this.lblCOMPort1.TabIndex = 0;
            this.lblCOMPort1.Text = "Steuerung COM-Port Nr.:";
            // 
            // butConnectSensors
            // 
            this.butConnectSensors.Location = new System.Drawing.Point(326, 16);
            this.butConnectSensors.Name = "butConnectSensors";
            this.butConnectSensors.Size = new System.Drawing.Size(100, 40);
            this.butConnectSensors.TabIndex = 6;
            this.butConnectSensors.Text = "Verbinden &Sensoren";
            this.butConnectSensors.UseVisualStyleBackColor = true;
            this.butConnectSensors.Click += new System.EventHandler(this.butConnectSensors_Click);
            // 
            // tBoxCOMPort1
            // 
            this.tBoxCOMPort1.Location = new System.Drawing.Point(131, 16);
            this.tBoxCOMPort1.MaxLength = 2;
            this.tBoxCOMPort1.Name = "tBoxCOMPort1";
            this.tBoxCOMPort1.Size = new System.Drawing.Size(20, 20);
            this.tBoxCOMPort1.TabIndex = 1;
            this.tBoxCOMPort1.Text = "6";
            this.tBoxCOMPort1.TextChanged += new System.EventHandler(this.COMPort_TextChanged);
            this.tBoxCOMPort1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxNumbercheck_KeyPress);
            // 
            // tBoxCOMPort2
            // 
            this.tBoxCOMPort2.Location = new System.Drawing.Point(131, 42);
            this.tBoxCOMPort2.MaxLength = 2;
            this.tBoxCOMPort2.Name = "tBoxCOMPort2";
            this.tBoxCOMPort2.Size = new System.Drawing.Size(20, 20);
            this.tBoxCOMPort2.TabIndex = 3;
            this.tBoxCOMPort2.Text = "5";
            this.tBoxCOMPort2.TextChanged += new System.EventHandler(this.COMPort_TextChanged);
            // 
            // lblCOMPort2
            // 
            this.lblCOMPort2.AutoSize = true;
            this.lblCOMPort2.Location = new System.Drawing.Point(6, 45);
            this.lblCOMPort2.Name = "lblCOMPort2";
            this.lblCOMPort2.Size = new System.Drawing.Size(121, 13);
            this.lblCOMPort2.TabIndex = 2;
            this.lblCOMPort2.Text = "Sensoren COM-Port Nr.:";
            // 
            // lblControl2ValueX
            // 
            this.lblControl2ValueX.AutoSize = true;
            this.lblControl2ValueX.Location = new System.Drawing.Point(338, 55);
            this.lblControl2ValueX.Name = "lblControl2ValueX";
            this.lblControl2ValueX.Size = new System.Drawing.Size(21, 13);
            this.lblControl2ValueX.TabIndex = 8;
            this.lblControl2ValueX.Text = "cm";
            // 
            // tBoxControl100PercentValueX
            // 
            this.tBoxControl100PercentValueX.Location = new System.Drawing.Point(297, 52);
            this.tBoxControl100PercentValueX.MaxLength = 4;
            this.tBoxControl100PercentValueX.Name = "tBoxControl100PercentValueX";
            this.tBoxControl100PercentValueX.Size = new System.Drawing.Size(35, 20);
            this.tBoxControl100PercentValueX.TabIndex = 7;
            this.tBoxControl100PercentValueX.Text = "0";
            this.tBoxControl100PercentValueX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBoxControl100PercentValueX.TextChanged += new System.EventHandler(this.tBoxControl100PercentValueX_TextChanged);
            this.tBoxControl100PercentValueX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxNumbercheck_KeyPress);
            // 
            // lblControl2
            // 
            this.lblControl2.AutoSize = true;
            this.lblControl2.Location = new System.Drawing.Point(6, 55);
            this.lblControl2.Name = "lblControl2";
            this.lblControl2.Size = new System.Drawing.Size(294, 13);
            this.lblControl2.TabIndex = 1;
            this.lblControl2.Text = "Abstand ab welchem die Steuerung zu 100% entgegen wirkt:";
            // 
            // lblControl1ValueX
            // 
            this.lblControl1ValueX.AutoSize = true;
            this.lblControl1ValueX.Location = new System.Drawing.Point(338, 27);
            this.lblControl1ValueX.Name = "lblControl1ValueX";
            this.lblControl1ValueX.Size = new System.Drawing.Size(21, 13);
            this.lblControl1ValueX.TabIndex = 6;
            this.lblControl1ValueX.Text = "cm";
            // 
            // tBoxControlStartValueX
            // 
            this.tBoxControlStartValueX.Location = new System.Drawing.Point(297, 24);
            this.tBoxControlStartValueX.MaxLength = 4;
            this.tBoxControlStartValueX.Name = "tBoxControlStartValueX";
            this.tBoxControlStartValueX.Size = new System.Drawing.Size(35, 20);
            this.tBoxControlStartValueX.TabIndex = 5;
            this.tBoxControlStartValueX.Text = "50";
            this.tBoxControlStartValueX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBoxControlStartValueX.TextChanged += new System.EventHandler(this.tBoxControlStartValueX_TextChanged);
            this.tBoxControlStartValueX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxNumbercheck_KeyPress);
            // 
            // lblControl1
            // 
            this.lblControl1.AutoSize = true;
            this.lblControl1.Location = new System.Drawing.Point(6, 27);
            this.lblControl1.Name = "lblControl1";
            this.lblControl1.Size = new System.Drawing.Size(217, 13);
            this.lblControl1.TabIndex = 0;
            this.lblControl1.Text = "Abstand ab welchem die Steuerung einsetzt:";
            // 
            // lbl_Ping
            // 
            this.lbl_Ping.AutoSize = true;
            this.lbl_Ping.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbl_Ping.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Ping.ForeColor = System.Drawing.Color.Black;
            this.lbl_Ping.Location = new System.Drawing.Point(68, 23);
            this.lbl_Ping.Name = "lbl_Ping";
            this.lbl_Ping.Size = new System.Drawing.Size(19, 20);
            this.lbl_Ping.TabIndex = 1;
            this.lbl_Ping.Text = "0";
            // 
            // lblPing
            // 
            this.lblPing.AutoSize = true;
            this.lblPing.Location = new System.Drawing.Point(6, 26);
            this.lblPing.Name = "lblPing";
            this.lblPing.Size = new System.Drawing.Size(56, 13);
            this.lblPing.TabIndex = 0;
            this.lblPing.Text = "Ping [ms] :";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Location = new System.Drawing.Point(6, 46);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(743, 64);
            this.dataGridView1.TabIndex = 2;
            // 
            // timerSendInterval
            // 
            this.timerSendInterval.Interval = 200;
            this.timerSendInterval.Tick += new System.EventHandler(this.timerSendInterval_Tick);
            // 
            // timerReceiveInterval
            // 
            this.timerReceiveInterval.Interval = 300;
            this.timerReceiveInterval.Tick += new System.EventHandler(this.timerReceiveInterval_Tick);
            // 
            // gBoxSettingAuto
            // 
            this.gBoxSettingAuto.Controls.Add(this.lblMaximumZ);
            this.gBoxSettingAuto.Controls.Add(this.lblMaximumY);
            this.gBoxSettingAuto.Controls.Add(this.tBoxMaximumX);
            this.gBoxSettingAuto.Controls.Add(this.lblMaximumX);
            this.gBoxSettingAuto.Controls.Add(this.tBoxMaximumZ);
            this.gBoxSettingAuto.Controls.Add(this.tBoxMaximumY);
            this.gBoxSettingAuto.Controls.Add(this.lblMaximum);
            this.gBoxSettingAuto.Controls.Add(this.lblZ);
            this.gBoxSettingAuto.Controls.Add(this.lblY);
            this.gBoxSettingAuto.Controls.Add(this.lblX);
            this.gBoxSettingAuto.Controls.Add(this.lblControl2ValueY);
            this.gBoxSettingAuto.Controls.Add(this.tBoxControlStartValueY);
            this.gBoxSettingAuto.Controls.Add(this.tBoxControl100PercentValueY);
            this.gBoxSettingAuto.Controls.Add(this.lblControl1ValueY);
            this.gBoxSettingAuto.Controls.Add(this.lblControl2ValueZ);
            this.gBoxSettingAuto.Controls.Add(this.tBoxControlStartValueZ);
            this.gBoxSettingAuto.Controls.Add(this.tBoxControl100PercentValueZ);
            this.gBoxSettingAuto.Controls.Add(this.lblControl1ValueZ);
            this.gBoxSettingAuto.Controls.Add(this.lblControl3Value);
            this.gBoxSettingAuto.Controls.Add(this.tBoxHoverGas);
            this.gBoxSettingAuto.Controls.Add(this.lblHoverGas);
            this.gBoxSettingAuto.Controls.Add(this.lblControl2ValueX);
            this.gBoxSettingAuto.Controls.Add(this.lblControl1);
            this.gBoxSettingAuto.Controls.Add(this.tBoxControlStartValueX);
            this.gBoxSettingAuto.Controls.Add(this.tBoxControl100PercentValueX);
            this.gBoxSettingAuto.Controls.Add(this.lblControl1ValueX);
            this.gBoxSettingAuto.Controls.Add(this.lblControl2);
            this.gBoxSettingAuto.Location = new System.Drawing.Point(12, 171);
            this.gBoxSettingAuto.Name = "gBoxSettingAuto";
            this.gBoxSettingAuto.Size = new System.Drawing.Size(564, 132);
            this.gBoxSettingAuto.TabIndex = 1;
            this.gBoxSettingAuto.TabStop = false;
            this.gBoxSettingAuto.Text = "Einstellungen für automatische Steuerung:";
            // 
            // lblMaximumZ
            // 
            this.lblMaximumZ.AutoSize = true;
            this.lblMaximumZ.Location = new System.Drawing.Point(507, 107);
            this.lblMaximumZ.Name = "lblMaximumZ";
            this.lblMaximumZ.Size = new System.Drawing.Size(50, 13);
            this.lblMaximumZ.TabIndex = 26;
            this.lblMaximumZ.Text = "max. 240";
            // 
            // lblMaximumY
            // 
            this.lblMaximumY.AutoSize = true;
            this.lblMaximumY.Location = new System.Drawing.Point(415, 106);
            this.lblMaximumY.Name = "lblMaximumY";
            this.lblMaximumY.Size = new System.Drawing.Size(47, 13);
            this.lblMaximumY.TabIndex = 17;
            this.lblMaximumY.Text = "max.126";
            // 
            // tBoxMaximumX
            // 
            this.tBoxMaximumX.Location = new System.Drawing.Point(296, 103);
            this.tBoxMaximumX.MaxLength = 4;
            this.tBoxMaximumX.Name = "tBoxMaximumX";
            this.tBoxMaximumX.Size = new System.Drawing.Size(35, 20);
            this.tBoxMaximumX.TabIndex = 9;
            this.tBoxMaximumX.Text = "126";
            this.tBoxMaximumX.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBoxMaximumX.TextChanged += new System.EventHandler(this.tBoxMaximumX_TextChanged);
            this.tBoxMaximumX.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxNumbercheck_KeyPress);
            // 
            // lblMaximumX
            // 
            this.lblMaximumX.AutoSize = true;
            this.lblMaximumX.Location = new System.Drawing.Point(331, 107);
            this.lblMaximumX.Name = "lblMaximumX";
            this.lblMaximumX.Size = new System.Drawing.Size(47, 13);
            this.lblMaximumX.TabIndex = 10;
            this.lblMaximumX.Text = "max.126";
            // 
            // tBoxMaximumZ
            // 
            this.tBoxMaximumZ.Location = new System.Drawing.Point(462, 103);
            this.tBoxMaximumZ.MaxLength = 4;
            this.tBoxMaximumZ.Name = "tBoxMaximumZ";
            this.tBoxMaximumZ.Size = new System.Drawing.Size(35, 20);
            this.tBoxMaximumZ.TabIndex = 25;
            this.tBoxMaximumZ.Text = "240";
            this.tBoxMaximumZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBoxMaximumZ.TextChanged += new System.EventHandler(this.tBoxMaximumZ_TextChanged);
            this.tBoxMaximumZ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxNumbercheck_KeyPress);
            // 
            // tBoxMaximumY
            // 
            this.tBoxMaximumY.Location = new System.Drawing.Point(380, 103);
            this.tBoxMaximumY.MaxLength = 4;
            this.tBoxMaximumY.Name = "tBoxMaximumY";
            this.tBoxMaximumY.Size = new System.Drawing.Size(35, 20);
            this.tBoxMaximumY.TabIndex = 16;
            this.tBoxMaximumY.Text = "126";
            this.tBoxMaximumY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBoxMaximumY.TextChanged += new System.EventHandler(this.tBoxMaximumY_TextChanged);
            this.tBoxMaximumY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxNumbercheck_KeyPress);
            // 
            // lblMaximum
            // 
            this.lblMaximum.AutoSize = true;
            this.lblMaximum.Location = new System.Drawing.Point(8, 106);
            this.lblMaximum.Name = "lblMaximum";
            this.lblMaximum.Size = new System.Drawing.Size(230, 13);
            this.lblMaximum.TabIndex = 3;
            this.lblMaximum.Text = "Erlaubtes Maximum für Ansteuerung pro Achse:";
            // 
            // lblZ
            // 
            this.lblZ.AutoSize = true;
            this.lblZ.Location = new System.Drawing.Point(444, 16);
            this.lblZ.Name = "lblZ";
            this.lblZ.Size = new System.Drawing.Size(17, 13);
            this.lblZ.TabIndex = 18;
            this.lblZ.Text = "Z:";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(361, 16);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(17, 13);
            this.lblY.TabIndex = 11;
            this.lblY.Text = "Y:";
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(279, 16);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(17, 13);
            this.lblX.TabIndex = 4;
            this.lblX.Text = "X:";
            // 
            // lblControl2ValueY
            // 
            this.lblControl2ValueY.AutoSize = true;
            this.lblControl2ValueY.Location = new System.Drawing.Point(421, 55);
            this.lblControl2ValueY.Name = "lblControl2ValueY";
            this.lblControl2ValueY.Size = new System.Drawing.Size(21, 13);
            this.lblControl2ValueY.TabIndex = 15;
            this.lblControl2ValueY.Text = "cm";
            // 
            // tBoxControlStartValueY
            // 
            this.tBoxControlStartValueY.Location = new System.Drawing.Point(380, 24);
            this.tBoxControlStartValueY.MaxLength = 4;
            this.tBoxControlStartValueY.Name = "tBoxControlStartValueY";
            this.tBoxControlStartValueY.Size = new System.Drawing.Size(35, 20);
            this.tBoxControlStartValueY.TabIndex = 12;
            this.tBoxControlStartValueY.Text = "50";
            this.tBoxControlStartValueY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBoxControlStartValueY.TextChanged += new System.EventHandler(this.tBoxControlStartValueY_TextChanged);
            this.tBoxControlStartValueY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxNumbercheck_KeyPress);
            // 
            // tBoxControl100PercentValueY
            // 
            this.tBoxControl100PercentValueY.Location = new System.Drawing.Point(380, 52);
            this.tBoxControl100PercentValueY.MaxLength = 4;
            this.tBoxControl100PercentValueY.Name = "tBoxControl100PercentValueY";
            this.tBoxControl100PercentValueY.Size = new System.Drawing.Size(35, 20);
            this.tBoxControl100PercentValueY.TabIndex = 14;
            this.tBoxControl100PercentValueY.Text = "0";
            this.tBoxControl100PercentValueY.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBoxControl100PercentValueY.TextChanged += new System.EventHandler(this.tBoxControl100PercentValueY_TextChanged);
            this.tBoxControl100PercentValueY.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxNumbercheck_KeyPress);
            // 
            // lblControl1ValueY
            // 
            this.lblControl1ValueY.AutoSize = true;
            this.lblControl1ValueY.Location = new System.Drawing.Point(421, 27);
            this.lblControl1ValueY.Name = "lblControl1ValueY";
            this.lblControl1ValueY.Size = new System.Drawing.Size(21, 13);
            this.lblControl1ValueY.TabIndex = 13;
            this.lblControl1ValueY.Text = "cm";
            // 
            // lblControl2ValueZ
            // 
            this.lblControl2ValueZ.AutoSize = true;
            this.lblControl2ValueZ.Location = new System.Drawing.Point(504, 55);
            this.lblControl2ValueZ.Name = "lblControl2ValueZ";
            this.lblControl2ValueZ.Size = new System.Drawing.Size(21, 13);
            this.lblControl2ValueZ.TabIndex = 22;
            this.lblControl2ValueZ.Text = "cm";
            // 
            // tBoxControlStartValueZ
            // 
            this.tBoxControlStartValueZ.Location = new System.Drawing.Point(463, 24);
            this.tBoxControlStartValueZ.MaxLength = 4;
            this.tBoxControlStartValueZ.Name = "tBoxControlStartValueZ";
            this.tBoxControlStartValueZ.Size = new System.Drawing.Size(35, 20);
            this.tBoxControlStartValueZ.TabIndex = 19;
            this.tBoxControlStartValueZ.Text = "50";
            this.tBoxControlStartValueZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBoxControlStartValueZ.TextChanged += new System.EventHandler(this.tBoxControlStartValueZ_TextChanged);
            this.tBoxControlStartValueZ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxNumbercheck_KeyPress);
            // 
            // tBoxControl100PercentValueZ
            // 
            this.tBoxControl100PercentValueZ.Location = new System.Drawing.Point(463, 52);
            this.tBoxControl100PercentValueZ.MaxLength = 4;
            this.tBoxControl100PercentValueZ.Name = "tBoxControl100PercentValueZ";
            this.tBoxControl100PercentValueZ.Size = new System.Drawing.Size(35, 20);
            this.tBoxControl100PercentValueZ.TabIndex = 21;
            this.tBoxControl100PercentValueZ.Text = "0";
            this.tBoxControl100PercentValueZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBoxControl100PercentValueZ.TextChanged += new System.EventHandler(this.tBoxControl100PercentValueZ_TextChanged);
            this.tBoxControl100PercentValueZ.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxNumbercheck_KeyPress);
            // 
            // lblControl1ValueZ
            // 
            this.lblControl1ValueZ.AutoSize = true;
            this.lblControl1ValueZ.Location = new System.Drawing.Point(504, 27);
            this.lblControl1ValueZ.Name = "lblControl1ValueZ";
            this.lblControl1ValueZ.Size = new System.Drawing.Size(21, 13);
            this.lblControl1ValueZ.TabIndex = 20;
            this.lblControl1ValueZ.Text = "cm";
            // 
            // lblControl3Value
            // 
            this.lblControl3Value.AutoSize = true;
            this.lblControl3Value.Location = new System.Drawing.Point(504, 81);
            this.lblControl3Value.Name = "lblControl3Value";
            this.lblControl3Value.Size = new System.Drawing.Size(56, 13);
            this.lblControl3Value.TabIndex = 24;
            this.lblControl3Value.Text = "(0 bis 239)";
            // 
            // tBoxHoverGas
            // 
            this.tBoxHoverGas.Location = new System.Drawing.Point(463, 78);
            this.tBoxHoverGas.MaxLength = 3;
            this.tBoxHoverGas.Name = "tBoxHoverGas";
            this.tBoxHoverGas.Size = new System.Drawing.Size(35, 20);
            this.tBoxHoverGas.TabIndex = 23;
            this.tBoxHoverGas.Text = "120";
            this.tBoxHoverGas.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.tBoxHoverGas.TextChanged += new System.EventHandler(this.tBoxHoverGas_TextChanged);
            this.tBoxHoverGas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tBoxNumbercheck_KeyPress);
            // 
            // lblHoverGas
            // 
            this.lblHoverGas.AutoSize = true;
            this.lblHoverGas.Location = new System.Drawing.Point(7, 81);
            this.lblHoverGas.Name = "lblHoverGas";
            this.lblHoverGas.Size = new System.Drawing.Size(217, 13);
            this.lblHoverGas.TabIndex = 2;
            this.lblHoverGas.Text = "Gas-Wert für Schwebeflug (Standard = 120):";
            // 
            // gBoxSensors
            // 
            this.gBoxSensors.Controls.Add(this.dataGridView1);
            this.gBoxSensors.Controls.Add(this.lbl_Ping);
            this.gBoxSensors.Controls.Add(this.lblPing);
            this.gBoxSensors.Location = new System.Drawing.Point(13, 594);
            this.gBoxSensors.Name = "gBoxSensors";
            this.gBoxSensors.Size = new System.Drawing.Size(755, 115);
            this.gBoxSensors.TabIndex = 5;
            this.gBoxSensors.TabStop = false;
            this.gBoxSensors.Text = "Sensoren Abfrage:";
            // 
            // gBoxGraphicOut
            // 
            this.gBoxGraphicOut.Controls.Add(this.lblRearText);
            this.gBoxGraphicOut.Controls.Add(this.lblRightText);
            this.gBoxGraphicOut.Controls.Add(this.lblLeftText);
            this.gBoxGraphicOut.Controls.Add(this.lblUpText);
            this.gBoxGraphicOut.Controls.Add(this.lblDownText);
            this.gBoxGraphicOut.Controls.Add(this.lblFrontText);
            this.gBoxGraphicOut.Controls.Add(this.lblDown);
            this.gBoxGraphicOut.Controls.Add(this.lblUp);
            this.gBoxGraphicOut.Controls.Add(this.lblRight);
            this.gBoxGraphicOut.Controls.Add(this.lblLeft);
            this.gBoxGraphicOut.Controls.Add(this.lblRear);
            this.gBoxGraphicOut.Controls.Add(this.lblFront);
            this.gBoxGraphicOut.Controls.Add(this.lblCenterXY);
            this.gBoxGraphicOut.Controls.Add(this.lblCenterZ);
            this.gBoxGraphicOut.Enabled = false;
            this.gBoxGraphicOut.Location = new System.Drawing.Point(583, 171);
            this.gBoxGraphicOut.Name = "gBoxGraphicOut";
            this.gBoxGraphicOut.Size = new System.Drawing.Size(404, 398);
            this.gBoxGraphicOut.TabIndex = 4;
            this.gBoxGraphicOut.TabStop = false;
            this.gBoxGraphicOut.Text = "Graphische Anzeige der automatischen Steuerungsrichtung:";
            // 
            // lblRearText
            // 
            this.lblRearText.AutoSize = true;
            this.lblRearText.Location = new System.Drawing.Point(118, 309);
            this.lblRearText.Name = "lblRearText";
            this.lblRearText.Size = new System.Drawing.Size(33, 13);
            this.lblRearText.TabIndex = 6;
            this.lblRearText.Text = "Heck";
            // 
            // lblRightText
            // 
            this.lblRightText.AutoSize = true;
            this.lblRightText.Location = new System.Drawing.Point(261, 159);
            this.lblRightText.Name = "lblRightText";
            this.lblRightText.Size = new System.Drawing.Size(41, 13);
            this.lblRightText.TabIndex = 2;
            this.lblRightText.Text = "Rechts";
            // 
            // lblLeftText
            // 
            this.lblLeftText.AutoSize = true;
            this.lblLeftText.Location = new System.Drawing.Point(22, 159);
            this.lblLeftText.Name = "lblLeftText";
            this.lblLeftText.Size = new System.Drawing.Size(32, 13);
            this.lblLeftText.TabIndex = 0;
            this.lblLeftText.Text = "Links";
            // 
            // lblUpText
            // 
            this.lblUpText.AutoSize = true;
            this.lblUpText.Location = new System.Drawing.Point(322, 52);
            this.lblUpText.Name = "lblUpText";
            this.lblUpText.Size = new System.Drawing.Size(23, 13);
            this.lblUpText.TabIndex = 8;
            this.lblUpText.Text = "Auf";
            // 
            // lblDownText
            // 
            this.lblDownText.AutoSize = true;
            this.lblDownText.Location = new System.Drawing.Point(325, 305);
            this.lblDownText.Name = "lblDownText";
            this.lblDownText.Size = new System.Drawing.Size(20, 13);
            this.lblDownText.TabIndex = 10;
            this.lblDownText.Text = "Ab";
            // 
            // lblFrontText
            // 
            this.lblFrontText.AutoSize = true;
            this.lblFrontText.Location = new System.Drawing.Point(120, 47);
            this.lblFrontText.Name = "lblFrontText";
            this.lblFrontText.Size = new System.Drawing.Size(31, 13);
            this.lblFrontText.TabIndex = 4;
            this.lblFrontText.Text = "Front";
            // 
            // lblDown
            // 
            this.lblDown.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblDown.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDown.Location = new System.Drawing.Point(345, 197);
            this.lblDown.Name = "lblDown";
            this.lblDown.Size = new System.Drawing.Size(25, 120);
            this.lblDown.TabIndex = 11;
            // 
            // lblUp
            // 
            this.lblUp.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblUp.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUp.Location = new System.Drawing.Point(345, 52);
            this.lblUp.Name = "lblUp";
            this.lblUp.Size = new System.Drawing.Size(25, 120);
            this.lblUp.TabIndex = 9;
            this.lblUp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRight
            // 
            this.lblRight.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblRight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblRight.Location = new System.Drawing.Point(176, 172);
            this.lblRight.Name = "lblRight";
            this.lblRight.Size = new System.Drawing.Size(126, 25);
            this.lblRight.TabIndex = 3;
            // 
            // lblLeft
            // 
            this.lblLeft.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLeft.Location = new System.Drawing.Point(25, 172);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(126, 25);
            this.lblLeft.TabIndex = 1;
            // 
            // lblRear
            // 
            this.lblRear.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblRear.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblRear.Location = new System.Drawing.Point(151, 196);
            this.lblRear.Name = "lblRear";
            this.lblRear.Size = new System.Drawing.Size(25, 126);
            this.lblRear.TabIndex = 7;
            // 
            // lblFront
            // 
            this.lblFront.BackColor = System.Drawing.Color.RoyalBlue;
            this.lblFront.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblFront.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFront.Location = new System.Drawing.Point(151, 47);
            this.lblFront.Name = "lblFront";
            this.lblFront.Size = new System.Drawing.Size(25, 126);
            this.lblFront.TabIndex = 5;
            this.lblFront.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCenterXY
            // 
            this.lblCenterXY.BackColor = System.Drawing.Color.Black;
            this.lblCenterXY.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCenterXY.Location = new System.Drawing.Point(151, 172);
            this.lblCenterXY.Name = "lblCenterXY";
            this.lblCenterXY.Size = new System.Drawing.Size(25, 25);
            this.lblCenterXY.TabIndex = 12;
            // 
            // lblCenterZ
            // 
            this.lblCenterZ.BackColor = System.Drawing.Color.Black;
            this.lblCenterZ.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCenterZ.Location = new System.Drawing.Point(345, 172);
            this.lblCenterZ.Name = "lblCenterZ";
            this.lblCenterZ.Size = new System.Drawing.Size(25, 25);
            this.lblCenterZ.TabIndex = 13;
            // 
            // butVideoStart
            // 
            this.butVideoStart.Location = new System.Drawing.Point(1003, 125);
            this.butVideoStart.Name = "butVideoStart";
            this.butVideoStart.Size = new System.Drawing.Size(140, 35);
            this.butVideoStart.TabIndex = 9;
            this.butVideoStart.Text = "&Bildauswertung starten";
            this.butVideoStart.UseVisualStyleBackColor = true;
            this.butVideoStart.Click += new System.EventHandler(this.butVideoStart_Click);
            // 
            // timerUpdateValuesVisual
            // 
            this.timerUpdateValuesVisual.Tick += new System.EventHandler(this.timerUpdateValuesVisual_Tick);
            // 
            // clbShowImage
            // 
            this.clbShowImage.BackColor = System.Drawing.SystemColors.Menu;
            this.clbShowImage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbShowImage.CheckOnClick = true;
            this.clbShowImage.ForeColor = System.Drawing.SystemColors.MenuText;
            this.clbShowImage.FormattingEnabled = true;
            this.clbShowImage.Items.AddRange(new object[] {
            "Grauwert zeigen ",
            "Canny zeigen",
            "Ergebnis zeigen",
            "Einstellungen zeigen",
            "Bild auswählen",
            "Alle Figuren anzeigen"});
            this.clbShowImage.Location = new System.Drawing.Point(1149, 110);
            this.clbShowImage.Name = "clbShowImage";
            this.clbShowImage.Size = new System.Drawing.Size(121, 90);
            this.clbShowImage.TabIndex = 13;
            this.clbShowImage.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbShowImage_ItemCheck);
            // 
            // btStartOSDData
            // 
            this.btStartOSDData.Location = new System.Drawing.Point(1003, 21);
            this.btStartOSDData.Name = "btStartOSDData";
            this.btStartOSDData.Size = new System.Drawing.Size(138, 31);
            this.btStartOSDData.TabIndex = 14;
            this.btStartOSDData.Text = "OSD Daten starten";
            this.btStartOSDData.UseVisualStyleBackColor = true;
            this.btStartOSDData.Click += new System.EventHandler(this.btStartOSDData_Click);
            // 
            // lOSDHeight
            // 
            this.lOSDHeight.AutoSize = true;
            this.lOSDHeight.Location = new System.Drawing.Point(1078, 55);
            this.lOSDHeight.Name = "lOSDHeight";
            this.lOSDHeight.Size = new System.Drawing.Size(0, 13);
            this.lOSDHeight.TabIndex = 16;
            // 
            // butShowDiagram
            // 
            this.butShowDiagram.Location = new System.Drawing.Point(1003, 297);
            this.butShowDiagram.Name = "butShowDiagram";
            this.butShowDiagram.Size = new System.Drawing.Size(120, 23);
            this.butShowDiagram.TabIndex = 17;
            this.butShowDiagram.Text = "Diagramm zeigen";
            this.butShowDiagram.UseVisualStyleBackColor = true;
            this.butShowDiagram.Click += new System.EventHandler(this.butShowDiagram_Click);
            // 
            // tbCameraIndex
            // 
            this.tbCameraIndex.Location = new System.Drawing.Point(1109, 195);
            this.tbCameraIndex.Name = "tbCameraIndex";
            this.tbCameraIndex.Size = new System.Drawing.Size(37, 20);
            this.tbCameraIndex.TabIndex = 18;
            this.tbCameraIndex.Text = "0";
            // 
            // lCameraIndex
            // 
            this.lCameraIndex.AutoSize = true;
            this.lCameraIndex.Location = new System.Drawing.Point(1000, 198);
            this.lCameraIndex.Name = "lCameraIndex";
            this.lCameraIndex.Size = new System.Drawing.Size(72, 13);
            this.lCameraIndex.TabIndex = 19;
            this.lCameraIndex.Text = "KameraIndex:";
            // 
            // timerUpdateOSDData
            // 
            this.timerUpdateOSDData.Tick += new System.EventHandler(this.timerUpdateOSDData_Tick);
            // 
            // timerUpdateGraph
            // 
            this.timerUpdateGraph.Tick += new System.EventHandler(this.timerUpdateGraph_Tick);
            // 
            // lblOSDInterval
            // 
            this.lblOSDInterval.AutoSize = true;
            this.lblOSDInterval.Location = new System.Drawing.Point(1000, 61);
            this.lblOSDInterval.Name = "lblOSDInterval";
            this.lblOSDInterval.Size = new System.Drawing.Size(109, 13);
            this.lblOSDInterval.TabIndex = 20;
            this.lblOSDInterval.Text = "OSD Abfrageintervall:";
            // 
            // tbOSDUpdateInterval
            // 
            this.tbOSDUpdateInterval.Location = new System.Drawing.Point(1109, 58);
            this.tbOSDUpdateInterval.Name = "tbOSDUpdateInterval";
            this.tbOSDUpdateInterval.Size = new System.Drawing.Size(37, 20);
            this.tbOSDUpdateInterval.TabIndex = 21;
            this.tbOSDUpdateInterval.Text = "200";
            this.tbOSDUpdateInterval.TextChanged += new System.EventHandler(this.tbOSDUpdateInterval_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1146, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "ms";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1146, 223);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "ms";
            // 
            // tbVIdeopdateInterval
            // 
            this.tbVIdeopdateInterval.Location = new System.Drawing.Point(1109, 220);
            this.tbVIdeopdateInterval.Name = "tbVIdeopdateInterval";
            this.tbVIdeopdateInterval.Size = new System.Drawing.Size(37, 20);
            this.tbVIdeopdateInterval.TabIndex = 25;
            this.tbVIdeopdateInterval.Text = "100";
            this.tbVIdeopdateInterval.TextChanged += new System.EventHandler(this.tbVIdeopdateInterval_TextChanged);
            // 
            // lblVisUpdateInterval
            // 
            this.lblVisUpdateInterval.AutoSize = true;
            this.lblVisUpdateInterval.Location = new System.Drawing.Point(1000, 223);
            this.lblVisUpdateInterval.Name = "lblVisUpdateInterval";
            this.lblVisUpdateInterval.Size = new System.Drawing.Size(103, 13);
            this.lblVisUpdateInterval.TabIndex = 24;
            this.lblVisUpdateInterval.Text = "Bild Abfrageintervall:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1078, 217);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 13);
            this.label7.TabIndex = 23;
            // 
            // btResetOSD
            // 
            this.btResetOSD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btResetOSD.Location = new System.Drawing.Point(1149, 21);
            this.btResetOSD.Name = "btResetOSD";
            this.btResetOSD.Size = new System.Drawing.Size(90, 23);
            this.btResetOSD.TabIndex = 28;
            this.btResetOSD.Text = "OSD Höhe =0";
            this.btResetOSD.UseVisualStyleBackColor = true;
            this.btResetOSD.Click += new System.EventHandler(this.btResetOSD_Click);
            // 
            // timerUpdateValuesVisual2
            // 
            this.timerUpdateValuesVisual2.Tick += new System.EventHandler(this.timerUpdateValuesVisual2_Tick);
            // 
            // btShowVideoCalibration
            // 
            this.btShowVideoCalibration.Location = new System.Drawing.Point(1123, 244);
            this.btShowVideoCalibration.Name = "btShowVideoCalibration";
            this.btShowVideoCalibration.Size = new System.Drawing.Size(105, 23);
            this.btShowVideoCalibration.TabIndex = 29;
            this.btShowVideoCalibration.Text = "Kalibrierung starten";
            this.btShowVideoCalibration.UseVisualStyleBackColor = true;
            this.btShowVideoCalibration.Click += new System.EventHandler(this.btShowVideoCalibration_Click);
            // 
            // cbShowCompare
            // 
            this.cbShowCompare.AutoSize = true;
            this.cbShowCompare.Location = new System.Drawing.Point(1003, 248);
            this.cbShowCompare.Name = "cbShowCompare";
            this.cbShowCompare.Size = new System.Drawing.Size(94, 17);
            this.cbShowCompare.TabIndex = 30;
            this.cbShowCompare.Text = "Zeige Index+1";
            this.cbShowCompare.UseVisualStyleBackColor = true;
            // 
            // comboCameraType
            // 
            this.comboCameraType.FormattingEnabled = true;
            this.comboCameraType.Items.AddRange(new object[] {
            "FlyCamOne HD",
            "GoPro Hero2"});
            this.comboCameraType.Location = new System.Drawing.Point(1003, 164);
            this.comboCameraType.Name = "comboCameraType";
            this.comboCameraType.Size = new System.Drawing.Size(121, 21);
            this.comboCameraType.TabIndex = 31;
            this.comboCameraType.SelectedIndexChanged += new System.EventHandler(this.comboCameraType_SelectedIndexChanged);
            // 
            // btReplay
            // 
            this.btReplay.Location = new System.Drawing.Point(1003, 350);
            this.btReplay.Name = "btReplay";
            this.btReplay.Size = new System.Drawing.Size(163, 45);
            this.btReplay.TabIndex = 32;
            this.btReplay.Text = "Protokolldaten wiedergeben";
            this.btReplay.UseVisualStyleBackColor = true;
            this.btReplay.Click += new System.EventHandler(this.btReplay_Click);
            // 
            // cbowriteprotocol
            // 
            this.cbowriteprotocol.AutoSize = true;
            this.cbowriteprotocol.Location = new System.Drawing.Point(1003, 271);
            this.cbowriteprotocol.Name = "cbowriteprotocol";
            this.cbowriteprotocol.Size = new System.Drawing.Size(133, 17);
            this.cbowriteprotocol.TabIndex = 33;
            this.cbowriteprotocol.Text = "Html Protokoll erstellen";
            this.cbowriteprotocol.UseVisualStyleBackColor = true;
            this.cbowriteprotocol.CheckedChanged += new System.EventHandler(this.cbowriteprotocol_CheckedChanged);
            // 
            // frmUAVSensorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1282, 718);
            this.Controls.Add(this.cbowriteprotocol);
            this.Controls.Add(this.btReplay);
            this.Controls.Add(this.comboCameraType);
            this.Controls.Add(this.cbShowCompare);
            this.Controls.Add(this.btShowVideoCalibration);
            this.Controls.Add(this.btResetOSD);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbVIdeopdateInterval);
            this.Controls.Add(this.lblVisUpdateInterval);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbOSDUpdateInterval);
            this.Controls.Add(this.lblOSDInterval);
            this.Controls.Add(this.lCameraIndex);
            this.Controls.Add(this.tbCameraIndex);
            this.Controls.Add(this.butShowDiagram);
            this.Controls.Add(this.lOSDHeight);
            this.Controls.Add(this.btStartOSDData);
            this.Controls.Add(this.clbShowImage);
            this.Controls.Add(this.butVideoStart);
            this.Controls.Add(this.gBoxGraphicOut);
            this.Controls.Add(this.gBoxSettingAuto);
            this.Controls.Add(this.gBoxSensors);
            this.Controls.Add(this.gBoxStruct);
            this.Controls.Add(this.butClose);
            this.Controls.Add(this.gBoxSettingManual);
            this.Controls.Add(this.gBoxConfig);
            this.Name = "frmUAVSensorControl";
            this.Text = "Drohnen Sensorsteuerung";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUAVSensorControl_FormClosing);
            this.Load += new System.EventHandler(this.frmUAVSensorControl_Load);
            this.EnabledChanged += new System.EventHandler(this.frmUAVSensorControl_EnabledChanged);
            this.gBoxStruct.ResumeLayout(false);
            this.gBoxStruct.PerformLayout();
            this.gBoxSettingManual.ResumeLayout(false);
            this.gBoxSettingManual.PerformLayout();
            this.gBoxLeftStick.ResumeLayout(false);
            this.gBoxLeftStick.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBarGas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarGier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarHeight)).EndInit();
            this.gBoxRightStick.ResumeLayout(false);
            this.gBoxRightStick.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tBarNick)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tBarRoll)).EndInit();
            this.gBoxConfig.ResumeLayout(false);
            this.gBoxConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.gBoxSettingAuto.ResumeLayout(false);
            this.gBoxSettingAuto.PerformLayout();
            this.gBoxSensors.ResumeLayout(false);
            this.gBoxSensors.PerformLayout();
            this.gBoxGraphicOut.ResumeLayout(false);
            this.gBoxGraphicOut.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gBoxStruct;
        private System.Windows.Forms.TextBox tBoxStructConfig;
        private System.Windows.Forms.Label lblStructConfig;
        private System.Windows.Forms.TextBox tBoxStructFrame;
        private System.Windows.Forms.Label lblStructFrame;
        private System.Windows.Forms.TextBox tBoxStructFree;
        private System.Windows.Forms.Label lblStructFree;
        private System.Windows.Forms.TextBox tBoxStructHeight;
        private System.Windows.Forms.Label lblStructHeight;
        private System.Windows.Forms.TextBox tBoxStructGas;
        private System.Windows.Forms.Label lblStructGas;
        private System.Windows.Forms.TextBox tBoxStructGier;
        private System.Windows.Forms.Label lblStructGier;
        private System.Windows.Forms.TextBox tBoxStructRoll;
        private System.Windows.Forms.Label lblStructRoll;
        private System.Windows.Forms.TextBox tBoxStructNick;
        private System.Windows.Forms.Label lblStructNick;
        private System.Windows.Forms.TextBox tBoxStructRemoteButtons;
        private System.Windows.Forms.Label lblStructRemoteButtons;
        private System.Windows.Forms.TextBox tBoxStructDigital;
        private System.Windows.Forms.Label lblStructDigital;
        private System.Windows.Forms.Button butClose;
        private System.Windows.Forms.GroupBox gBoxSettingManual;
        private System.Windows.Forms.Label lblButtons1and2;
        private System.Windows.Forms.GroupBox gBoxLeftStick;
        private System.Windows.Forms.TextBox tBoxGier;
        private System.Windows.Forms.TextBox tBoxGas;
        private System.Windows.Forms.TrackBar tBarGas;
        private System.Windows.Forms.Label lblGier;
        private System.Windows.Forms.Label lblGas;
        private System.Windows.Forms.TrackBar tBarGier;
        private System.Windows.Forms.CheckBox cBoxBut1;
        private System.Windows.Forms.TextBox tBoxHeight;
        private System.Windows.Forms.CheckBox cBoxBut2;
        private System.Windows.Forms.TrackBar tBarHeight;
        private System.Windows.Forms.TextBox tBoxRemoteButtons;
        private System.Windows.Forms.Label lblHight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gBoxRightStick;
        private System.Windows.Forms.TextBox tBoxRoll;
        private System.Windows.Forms.TextBox tBoxNick;
        private System.Windows.Forms.TrackBar tBarNick;
        private System.Windows.Forms.Label lblRoll;
        private System.Windows.Forms.Label lblNick;
        private System.Windows.Forms.TrackBar tBarRoll;
        private System.Windows.Forms.Button butSend;
        private System.Windows.Forms.GroupBox gBoxConfig;
        private System.Windows.Forms.Label lblControlConnected;
        private System.Windows.Forms.Button butConnectControl;
        private System.Windows.Forms.Label lblCOMPort1;
        private System.Windows.Forms.TextBox tBoxCOMPort1;
        private System.Windows.Forms.RadioButton rButSendManual;
        private System.Windows.Forms.CheckBox cBoxEnable;
        private System.Windows.Forms.Label lblInterval2;
        private System.Windows.Forms.RadioButton rButSendAuto;
        private System.Windows.Forms.TextBox tBoxInterval;
        private System.Windows.Forms.Label lblInterval1;
        private System.Windows.Forms.Label lbl_Ping;
        private System.Windows.Forms.Label lblPing;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button butSensorStart;
        private System.Windows.Forms.Button butConnectSensors;
        private System.Windows.Forms.TextBox tBoxCOMPort2;
        private System.Windows.Forms.Label lblCOMPort2;
        private System.Windows.Forms.Timer timerSendInterval;
        private System.Windows.Forms.Timer timerReceiveInterval;
        private System.Windows.Forms.GroupBox gBoxSettingAuto;
        private System.Windows.Forms.Label lblControl1;
        private System.Windows.Forms.Label lblControl2ValueX;
        private System.Windows.Forms.TextBox tBoxControl100PercentValueX;
        private System.Windows.Forms.Label lblControl2;
        private System.Windows.Forms.Label lblControl1ValueX;
        private System.Windows.Forms.TextBox tBoxControlStartValueX;
        private System.Windows.Forms.RadioButton rButAutomaticControl;
        private System.Windows.Forms.RadioButton rButManualControl;
        private System.Windows.Forms.Label lblSensorsConnected;
        private System.Windows.Forms.GroupBox gBoxSensors;
        private System.Windows.Forms.TextBox tBoxHoverGas;
        private System.Windows.Forms.Label lblHoverGas;
        private System.Windows.Forms.GroupBox gBoxGraphicOut;
        private System.Windows.Forms.Label lblDown;
        private System.Windows.Forms.Label lblUp;
        private System.Windows.Forms.Label lblRight;
        private System.Windows.Forms.Label lblLeft;
        private System.Windows.Forms.Label lblRear;
        private System.Windows.Forms.Label lblFront;
        private System.Windows.Forms.Label lblControl3Value;
        private System.Windows.Forms.Label lblRearText;
        private System.Windows.Forms.Label lblRightText;
        private System.Windows.Forms.Label lblLeftText;
        private System.Windows.Forms.Label lblUpText;
        private System.Windows.Forms.Label lblDownText;
        private System.Windows.Forms.Label lblFrontText;
        private System.Windows.Forms.Label lblControl2ValueY;
        private System.Windows.Forms.TextBox tBoxControlStartValueY;
        private System.Windows.Forms.TextBox tBoxControl100PercentValueY;
        private System.Windows.Forms.Label lblControl1ValueY;
        private System.Windows.Forms.Label lblControl2ValueZ;
        private System.Windows.Forms.TextBox tBoxControlStartValueZ;
        private System.Windows.Forms.TextBox tBoxControl100PercentValueZ;
        private System.Windows.Forms.Label lblControl1ValueZ;
        private System.Windows.Forms.Label lblZ;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.TextBox tBoxMaximumZ;
        private System.Windows.Forms.TextBox tBoxMaximumY;
        private System.Windows.Forms.TextBox tBoxMaximumX;
        private System.Windows.Forms.Label lblMaximum;
        private System.Windows.Forms.CheckBox cBoxDataWright;
        private System.Windows.Forms.Label lblData;
        private System.Windows.Forms.Label lblDataCount;
        private System.Windows.Forms.Label lblMaximumZ;
        private System.Windows.Forms.Label lblMaximumY;
        private System.Windows.Forms.Label lblMaximumX;
        private System.Windows.Forms.Button butVideoStart;
        private System.Windows.Forms.Label lblConnectionLost;
        private System.Windows.Forms.Label lblCenterXY;
        private System.Windows.Forms.Label lblCenterZ;
        private System.Windows.Forms.Timer timerUpdateValuesVisual;
        private System.Windows.Forms.CheckedListBox clbShowImage;
        private System.Windows.Forms.Button btStartOSDData;
        private System.Windows.Forms.Label lOSDHeight;
        private System.Windows.Forms.Button butShowDiagram;
        private System.Windows.Forms.TextBox tbCameraIndex;
        private System.Windows.Forms.Label lCameraIndex;
        private System.Windows.Forms.Timer timerUpdateOSDData;
        private System.Windows.Forms.Timer timerUpdateGraph;
        private System.Windows.Forms.Label lblOSDInterval;
        private System.Windows.Forms.TextBox tbOSDUpdateInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbVIdeopdateInterval;
        private System.Windows.Forms.Label lblVisUpdateInterval;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btResetOSD;
        private System.Windows.Forms.Timer timerUpdateValuesVisual2;
        private System.Windows.Forms.Button btShowVideoCalibration;
        private System.Windows.Forms.CheckBox cbShowCompare;
        private System.Windows.Forms.ComboBox comboCameraType;
        private System.Windows.Forms.Button btReplay;
        private System.Windows.Forms.CheckBox cbowriteprotocol;
    }
}

