/*
 *>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
 *>
 *> Christian Pirner        2012
 *>
 *> Steuerungssoftware für Mikrokopter-Drohne
 *> Version 3.1
 *>
 *> Masterarbeit zum Thema:
 *> Machbarkeitsanalyse und Entwicklung einer prototypischen Software
 *> für Sensordatenerfassung und –Verarbeitung zur Flugsteuerung eines Quadrokopters
 *>
 *> Hauptprogramm
 *>
 *>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
 */

#region Namespace Inclusions

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;

using System.Threading;

using WI232Lib;


using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using VisualControlLib;

#endregion

namespace UAVSensorControl
{
    public struct str_ExternControl
    {
        public byte[] Digital;          //Zwei Buttons deren Funktion nicht bekannt ist
        public byte RemoteButtons;      //Funktion nicht bekannt
        public byte Nick;               //Kippen Vorne-Hinten (Wertebereich: -127 bis +127)
        //Nick wurde nach Test von sbyte zu byte geändert, da anscheinend negative Werte immer als "Volle Leistung" auf alle Motoren interpretiert wird
        public sbyte Roll;              //Kippen Links-Rechts (Wertebereich: -127 bis +127)     
        public sbyte Gier;              //Drehung in Horizontalebene (Wertebereich: -100 bis +100 -> aus Formel in Wiiuse.h abgeleitet)
        //Test hat ergeben, dass Werte für Gier invers zu Fernsteuerungswerten interpretiert werden
        public byte Gas;                //Energiezufuhr für alle Propeller (Wertebereich: 0 bis +240 Achtung Limitierung! Gefahr von overflow bei < 0)
        public sbyte Height;             //Wird in Vorlage nicht verwendet, deshalb ist umrechnung in Meter nicht möglich
        public byte Free;               //Funktion nicht bekannt
        public byte Frame;              //Rückantwort von FlghtCtl, Wird in Vorlage nicht ausgewertet, deshalb inhalt unbekannt
        public byte Config;             //1: ExternControls-Werte werden angewendet, 0: ExternControls-Werte werden ignoriert

        public str_ExternControl(byte a)    //Wenn ein neues ExternControls-Strukt angelegt wird, werden alle Werte neutral auf Null gesetzt
        {
            Digital = new byte[2] { 0, 0 };
            RemoteButtons = 0;
            Nick = 0;
            Roll = 0;
            Gier = 0;
            Gas = 0;
            Height = 0;
            Free = 0;
            Frame = 0;
            Config = a;                     //Config = 1 : Werte im Strukt dürfen vom Quadrokopter verwendet werden
                                            //Config = 0 : Alle Werte in Strukt werden vom Quadrokopter ignoriert
        }

    }

    public struct str_RadarData
    {
        //Variablen für Sensormessungen (Distanzen)
        public int distFront;
        public int distRear;
        public int distLeft;
        public int distRight;
        public int distUp;
        public int distDown;
    }

    public partial class frmUAVSensorControl : Form
    {

#region Constants

        const int asciiQuestionmark = 63;
        const int invinitDistance = 1000;
        const int labelLength = 126;
        const int labelGasLength = 240;
        const int labelTopDownLength = 120;
        const int labelWidth = 25;
        const int labelTopOffset = 52;
        const int labelLeftOffset = 47;

        const string settingsFile = "Settings.csv";
        const string loggingFile = "DataLog.csv";

        const String entryAltitudePLatformFC = "PlatformEntfernung Aktuell";
        const String entryAltitudePLatformGP = "PlatformEntfernung Referenz";
        const String entryOSD = "OSDHeight";
        const String entryUltraSonic = "UltraSonic";

#endregion

#region Global Variables
        private OSDDataLib.NaviControl control = new OSDDataLib.NaviControl();
        private VisualControlWrapper visualControl=null;
        private VisualControlWrapper visualControl2= null;
        private RealTimeData graph = new RealTimeData();
        private frmCalibrate callibration = null;
        private bool requestOSDDone = true;
        private str_RadarData radarData;
        private CopterData copterData = new CopterData(50);
        private str_CalibrationData calibrationData_FlyCam= new str_CalibrationData(0.0064,38.112,5);
        private str_CalibrationData calibrationData_GoPro = new str_CalibrationData(0.0064, 29, 3);
        private str_CalibrationData actualCalibration;

        public str_ExternControl ExternControl = new str_ExternControl(0);  //Strukt zum speichern der Werte die gesendet werden sollen
        public Char[] arr_ExternControl = new Char[11];                     //Zwischenspeicher um Daten als Char-Array senden zu können ohne Pointer zu verwenden
                
        //public SerialPort comPort = new SerialPort();                       //COM-Port für die Übermittlung der Steuerbefehle zur Drohne

        char[] tx_buffer = new char[150];	//Ausgangspuffer für Daten
        //char[] rx_buffer = new char[150];	//Eingangspuffer (Wird nicht verwendet)
        string rx_buffer;                   //Eingangsstring um Antwort der Drohne zu empfangen. (Wird nicht ausgewertet)

        private bool settingsLoaded = false;    //Verriegelung, sodass beim Laden der Settings das Saving nicht ausgelöst wird
        private bool autoControl = false;       //Regelung auf Automatik oder Manuel gestellt
        private bool controlConnected = false;  //Verbindung zu einem COM-Port für Drohnensteuerung hergestellt
        
        private bool dataWrite = false;         //Data-File mitschreiben       

        private int NickTemp = 0;               //Variable für Umrechnung der Nick-Eingabewerte
        
        //Arrays für Median-Filterung der 6 Sensoren
        private Int32[] arr_distFront = new Int32[3] { 0, 0, 0 };
        private Int32[] arr_distRear = new Int32[3] { 0, 0, 0 };
        private Int32[] arr_distLeft = new Int32[3] { 0, 0, 0 };
        private Int32[] arr_distRight = new Int32[3] { 0, 0, 0 };
        private Int32[] arr_distUp = new Int32[3] { 0, 0, 0 };
        private Int32[] arr_distDown = new Int32[3] { 0, 0, 0 };
        private int counterArr = 0; //Zähler zum durchlaufen der Arrays

        private uint sendCounter = 0;


        private int tempDistFront, tempDistRear, tempDistLeft, tempDistRight, tempDistUp, tempDistDown;     //temporäre Variablen für Sensor-Logging
        private int controlStartValueX, control100PercentValueX, controlDeltaXX;                            //Variablen für Regelungseingangsgrenzen in cm X-Achse = Roll
        private int controlStartValueY, control100PercentValueY, controlDeltaXY;                            //Variablen für Regelungseingangsgrenzen in cm Y-Achse = Nick
        private int controlStartValueZ, control100PercentValueZ, controlDeltaXZ;                            //Variablen für Regelungseingangsgrenzen in cm Z-Achse = Gas
        private int hoverGas;                                                                               //Gas-Wert bei dem die Drohne schwebt
        private int maximumX, maximumY, maximumZ;                                                           //Erlaubte Maximalwerte für X-Y-Z-Achsen
        private float tempValue = 0;                                                                        //Zwischenspeicher für Berechnungen

        //Datei für Logging anlegen
        StreamWriter DataFile = new StreamWriter(loggingFile);        //Anlegen der Datei zum mitschreiben der Sensor- und Steuerungsdaten
        private int writeCounter = 0;                                   //Zähler, wie viele Datensätze geschrieben wurden

        private IWI232Connector _connector = null;  //Anlegen des Verbindungs-Objekts für die Sensordatenübertragung
        int messageCounter = 0;                     //Zählvariable für Unterscheidung der Sensoren innerhalb des empfangenen Datenobjektes
        int highByteBuffer;                         //Zwischenspeicher für das übertragene Highbyte des aktuellen Sensors
        Stopwatch stopwatchPing = new Stopwatch();  //Timer anlegen für Pingmessungen

        ToolTip UserInfoToolTip = new ToolTip();    //Initialisierung für den ToolTip-Handler

#endregion

#region Initialisation

        public frmUAVSensorControl()
        {
            InitializeComponent();

            //Die ToolTips konfigurieren
            UserInfoToolTip.UseFading = true;
            UserInfoToolTip.UseAnimation = true;
            UserInfoToolTip.IsBalloon = true;
            UserInfoToolTip.SetToolTip(cBoxEnable, "Wenn dieser Haken nicht gesetzt ist, wird\ndie Drohne alle Steuerungsdaten ignorieren.");
            UserInfoToolTip.SetToolTip(cBoxDataWright, "Wird dieser Haken gesetzt, werden die Daten der Sensoren und der Steuerung in die Datei 'LogFile.csv' geschrieben.\nWird der Haken wieder entfernt, wird das Schreiben unterbrochen, kann aber jederzeit fortgesetzt werden.\nDie Datei wird erst beim beenden der Anwendung abgeschlossen!");
            UserInfoToolTip.SetToolTip(rButManualControl, "Hiermit können die Steuerungswerte über die Regler\nder Manuellen Steuerung eingestellt werden.");
            UserInfoToolTip.SetToolTip(rButAutomaticControl, "Hiermit wird die Steuerung über die\nHinderniserkennung aktiviert.");
            UserInfoToolTip.SetToolTip(lblData, "Anzahl der in die Logging-Datei\ngeschriebenen Datensätze.");
            UserInfoToolTip.SetToolTip(lblDataCount, "Anzahl der in die Logging-Datei\ngeschriebenen Datensätze.");
            UserInfoToolTip.SetToolTip(rButSendManual, "Die Steuerungsdaten werden nur durch klicken\nauf den Senden-Button übertragen.");
            UserInfoToolTip.SetToolTip(rButSendAuto, "Die Steuerungsdaten werden im eingestellten\nZeitinterval gesendet.");
            UserInfoToolTip.SetToolTip(cBoxBut1, "Checked = 1\nUnchecked = 0");
            UserInfoToolTip.SetToolTip(cBoxBut2, "Checked = 1\nUnchecked = 0");
            UserInfoToolTip.SetToolTip(tBoxRemoteButtons, "Der Byte-Wert des eingegebenen\nZeichens wird übertragen.");

            //Settings der letzten Sitzung wieder herstellen
            try
            {
                FileStream SettingsFile = new FileStream(@settingsFile, FileMode.Open);
                using (StreamReader SettingsReader = new StreamReader(SettingsFile, System.Text.Encoding.Default))
                {
                    string strLine = SettingsReader.ReadLine();
                    string[] arr_Settings = strLine.Split(';');
                    tBoxCOMPort1.Text = arr_Settings[0];
                    tBoxCOMPort2.Text = arr_Settings[1];
                    tBoxControlStartValueX.Text = arr_Settings[2];
                    tBoxControl100PercentValueX.Text = arr_Settings[3];
                    tBoxMaximumX.Text = arr_Settings[4];
                    tBoxControlStartValueY.Text = arr_Settings[5];
                    tBoxControl100PercentValueY.Text = arr_Settings[6];
                    tBoxMaximumY.Text = arr_Settings[7];
                    tBoxControlStartValueZ.Text = arr_Settings[8];
                    tBoxControl100PercentValueZ.Text = arr_Settings[9];
                    tBoxHoverGas.Text = arr_Settings[10];
                    tBoxMaximumZ.Text = arr_Settings[11];
                    tBoxInterval.Text = arr_Settings[12];
                    SettingsFile.Close();
                    SettingsReader.Close();
                    settingsLoaded = true;
                }
            }
            catch
            {
                MessageBox.Show("Die Settings der letzten Sitzung konnten nicht aus der Datei gelesen werden!\nStellen Sie sicher, dass die Datei 'Settings.csv' gelesen werden kann und nicht beschädigt ist.\n\nAchtung! Es können in dieser Sitzung auch keine Settings gespeichert werden!", "Datei kann nicht gelesen werden!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
            
            //Vorbelegung der Regelungsvariablen mit den Standardwerten der Textboxen X-Achse
            controlStartValueX = Convert.ToInt32(tBoxControlStartValueX.Text); 
            control100PercentValueX = Convert.ToInt32(tBoxControl100PercentValueX.Text);
            controlDeltaXX = controlStartValueX - control100PercentValueX;
            //Vorbelegung der Regelungsvariablen mit den Standardwerten der Textboxen Y-Achse
            controlStartValueY = Convert.ToInt32(tBoxControlStartValueY.Text);
            control100PercentValueY = Convert.ToInt32(tBoxControl100PercentValueY.Text);
            controlDeltaXY = controlStartValueY - control100PercentValueY;
            //Vorbelegung der Regelungsvariablen mit den Standardwerten der Textboxen Z-Achse
            controlStartValueZ = Convert.ToInt32(tBoxControlStartValueZ.Text);
            control100PercentValueZ = Convert.ToInt32(tBoxControl100PercentValueZ.Text);
            controlDeltaXZ = controlStartValueZ - control100PercentValueZ;
            hoverGas = Convert.ToInt32(tBoxHoverGas.Text);
            //Vorbelegung der Maxima
            maximumX = Convert.ToInt32(tBoxMaximumX.Text);
            maximumY = Convert.ToInt32(tBoxMaximumY.Text);
            maximumZ = Convert.ToInt32(tBoxMaximumZ.Text);
                        
            // Anzeigetabelle für Sensoren anlegen und konfigurieren 
            dataGridView1.ColumnCount = 7;
            dataGridView1.ColumnHeadersVisible = true;
            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();
            columnHeaderStyle.BackColor = Color.Beige;
            columnHeaderStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;
            dataGridView1.ReadOnly = true;
            dataGridView1.Rows.Add("ID - Richtung", "E0 - Rechts", "E2 - Links", "E4 - Oben", "E6 - Unten", "E8 - Heck", "EA - Front");
            dataGridView1.Rows.Add("Abstand [cm]");
            dataGridView1[0, 1].Style.BackColor = Color.Khaki;
            foreach (DataGridViewColumn col in dataGridView1.Columns) col.SortMode = DataGridViewColumnSortMode.NotSortable;

            control.FlightCTRLDataReceived += new OSDDataLib.FlightCTRLDataHandler(port_DataReceived);  //Eventhandler zum Empfang der Antwortdaten von der Rohne. Werden nicht ausgewertet

            //initialize to GoPro
            comboCameraType.SelectedIndex = 0;
            actualCalibration = calibrationData_FlyCam;
        }

        private void frmUAVSensorControl_Load(object sender, EventArgs e) //Korrekte Stellungen der Schieber und des Sende-Timers anzeigen
        {
            timerSendInterval.Interval = Convert.ToInt32(tBoxInterval.Text);
            tBoxHeight.Text = tBarHeight.Value.ToString();
            tBoxGas.Text = tBarGas.Value.ToString();
            tBoxGier.Text = tBarGier.Value.ToString();
            tBoxNick.Text = tBarNick.Value.ToString();
            tBoxRoll.Text = tBarRoll.Value.ToString();

            //arr_ExternControl mit Anfangswerten vorbelegen
            arr_ExternControl[0] = (char)ExternControl.Digital[0];
            arr_ExternControl[1] = (char)ExternControl.Digital[1];
            tBoxStructDigital.Text = ExternControl.Digital[0].ToString() + ", " + ExternControl.Digital[1].ToString();

            tBoxStructRemoteButtons.Text = ExternControl.RemoteButtons.ToString();
            arr_ExternControl[2] = (char)ExternControl.RemoteButtons;

            tBoxStructNick.Text = ExternControl.Nick.ToString();
            arr_ExternControl[3] = (char)ExternControl.Nick;

            tBoxStructRoll.Text = ExternControl.Roll.ToString();
            arr_ExternControl[4] = (char)ExternControl.Roll;

            tBoxStructGier.Text = ExternControl.Gier.ToString();
            arr_ExternControl[5] = (char)ExternControl.Gier;

            tBoxStructGas.Text = ExternControl.Gas.ToString();
            arr_ExternControl[6] = (char)ExternControl.Gas;

            tBoxStructHeight.Text = ExternControl.Height.ToString();
            arr_ExternControl[7] = (char)ExternControl.Height;

            tBoxStructFree.Text = ExternControl.Free.ToString();
            arr_ExternControl[8] = (char)ExternControl.Free;

            tBoxStructFrame.Text = ExternControl.Frame.ToString();
            arr_ExternControl[9] = (char)ExternControl.Frame;

            tBoxStructConfig.Text = ExternControl.Config.ToString();
            arr_ExternControl[10] = (char)ExternControl.Config;

            try
            {
                //DataLog-File vorformatieren
                DataFile.Write("Rechts;");
                DataFile.Write("Links;");
                DataFile.Write("Oben;");
                DataFile.Write("Unten;");
                DataFile.Write("Heck;");
                DataFile.Write("Front;");
                DataFile.Write("Roll;");
                DataFile.Write("Nick;");
                DataFile.WriteLine("Gas;");
            }
            catch
            {
                MessageBox.Show("Die Logging-Datei konnte nicht initialisiert werden.\n Stellen Sie sicher, dass Sie Schreibrechte in dem Programmordner haben.", "Auf Logging-Datei kann nicht zugegriffen werden!", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
        }

#endregion

#region SensorCode
        //COM-Port Verbindung für die Sensoren herstellen
        private void butConnectSensors_Click(object sender, EventArgs e)
        {
            if (_connector == null)
            {
                //Herstellen WI232 Verbindung
                _connector = new WI232Lib.WI232Connector();
                _connector.PortName = ("COM" + tBoxCOMPort2.Text);
                _connector.MessageReceived += new EventHandler<WI232MessageReceivedEventArgs>(_connector_MessageReceived);
                if (_connector.Connect())
                {
                    lblSensorsConnected.BackColor = System.Drawing.Color.LightGreen;
                    lblSensorsConnected.Text = "Sensoren verbunden";
                    //Buttons anpassen
                    butSensorStart.Enabled = true;
                }
                                
                _connector.StartListening(OperatingMode.ReadBytes);

                butConnectSensors.Enabled = false;
            }
            else
            {
                //Wi232 Verbindung beenden
                _connector.StopListening();
                _connector.Disconnect();
                _connector.MessageReceived -= _connector_MessageReceived;
                _connector = null;

                //Buttons anpassen
                butSensorStart.Enabled = false;
            }
        }


        private int radarDownStart = Int16.MinValue;
        //Funktion die aufgerufen wird, sobald eine Nachricht vom Mikrokontroller empfangen wird
        private void _connector_MessageReceived(object sender, WI232MessageReceivedEventArgs e)
        {
            //Empfangene Nachricht (immer ein Byte)
            String message = e.Message;

            //Wenn Startzeichen gesendet
            if (message == "?")
                //Counter um zu Wissen welches Byte gesendet wurde
                messageCounter = 0;

            switch (messageCounter)
            {
                //Case 0 : Startzeichen
                case 0: break;
                //Highbyte Sensor E0 
                case 1: highByteBuffer = int.Parse(message); break;
                //Lowbyte Sensor E0, Berechnung der Entfernung (in cm) und anzeigen in der Anzeigetabelle
                case 2: radarData.distRight = highByteBuffer * 256 + int.Parse(message);
                    tempDistRight = radarData.distRight;
                    if (radarData.distRight == 0) radarData.distRight = invinitDistance;
                    
                    arr_distRight[counterArr % 3] = radarData.distRight;
                    Array.Sort<int>(arr_distRight);
                    radarData.distRight = arr_distRight[1];

                    dataGridView1.Invoke((MethodInvoker)delegate { dataGridView1.Rows[1].Cells[1].Value = radarData.distRight.ToString(); });
                    break;
                //Highbyte Sensor E2
                case 3: highByteBuffer = int.Parse(message); break;
                //Lowbyte Sensor E2, Berechnung der Entfernung (in cm) und anzeigen in der Anzeigetabelle
                case 4: radarData.distLeft = highByteBuffer * 256 + int.Parse(message);
                    tempDistLeft = radarData.distLeft;
                    if (radarData.distLeft == 0) radarData.distLeft = invinitDistance;
                    
                    arr_distLeft[counterArr % 3] = radarData.distLeft;
                    Array.Sort<int>(arr_distLeft);
                    radarData.distLeft = arr_distLeft[1];

                    dataGridView1.Invoke((MethodInvoker)delegate { dataGridView1.Rows[1].Cells[2].Value = radarData.distLeft.ToString(); });
                    break;
                //Highbyte Sensor E4
                case 5: highByteBuffer = int.Parse(message); break;
                //Lowbyte Sensor E4, Berechnung der Entfernung (in cm) und anzeigen in der Anzeigetabelle
                case 6: radarData.distUp = highByteBuffer * 256 + int.Parse(message);
                    tempDistUp = radarData.distUp;
                    if (radarData.distUp == 0) radarData.distUp = invinitDistance;
                    
                    arr_distUp[counterArr % 3] = radarData.distUp;
                    Array.Sort<int>(arr_distUp);
                    radarData.distUp = arr_distUp[1];

                    dataGridView1.Invoke((MethodInvoker)delegate { dataGridView1.Rows[1].Cells[3].Value = radarData.distUp.ToString(); });
                    break;
                //Highbyte Sensor E6
                case 7: highByteBuffer = int.Parse(message); break;
                //Lowbyte Sensor E6, Berechnung der Entfernung (in cm) und anzeigen in der Anzeigetabelle
                case 8: radarData.distDown = highByteBuffer * 256 + int.Parse(message);
                    tempDistDown = radarData.distDown;
                    if (radarData.distDown == 0) radarData.distDown = invinitDistance;
                    
                    arr_distDown[counterArr % 3] = radarData.distDown;
                    Array.Sort<int>(arr_distDown);
                    radarData.distDown = arr_distDown[1];

                    dataGridView1.Invoke((MethodInvoker)delegate { dataGridView1.Rows[1].Cells[4].Value = radarData.distDown.ToString(); });
                    break;
                //Highbyte Sensor E8
                case 9: highByteBuffer = int.Parse(message); break;
                //Lowbyte Sensor E8, Berechnung der Entfernung (in cm) und anzeigen in der Anzeigetabelle
                case 10: radarData.distRear = highByteBuffer * 256 + int.Parse(message);
                    tempDistRear = radarData.distRear;
                    if (radarData.distRear == 0) radarData.distRear = invinitDistance;
                    
                    arr_distRear[counterArr % 3] = radarData.distRear;
                    Array.Sort<int>(arr_distRear);
                    radarData.distRear = arr_distRear[1];

                    dataGridView1.Invoke((MethodInvoker)delegate { dataGridView1.Rows[1].Cells[5].Value = radarData.distRear.ToString(); });
                    break;
                //Highbyte Sensor EA
                case 11: highByteBuffer = int.Parse(message); break;
                //Lowbyte Sensor EA, Berechnung der Entfernung (in cm) und anzeigen in der Anzeigetabelle
                case 12: radarData.distFront = highByteBuffer * 256 + int.Parse(message);
                    tempDistFront = radarData.distFront;
                    if (radarData.distFront == 0) radarData.distFront = invinitDistance;

                    arr_distFront[counterArr % 3] = radarData.distFront;
                    Array.Sort<int>(arr_distFront);
                    radarData.distFront = arr_distFront[1];

                    dataGridView1.Invoke((MethodInvoker)delegate { dataGridView1.Rows[1].Cells[6].Value = radarData.distFront.ToString(); });

                    //Messung abgeschlossen--> Timer stoppen und Ping anzeigen
                    stopwatchPing.Stop();
                    lbl_Ping.Invoke((MethodInvoker)delegate { lbl_Ping.Text = stopwatchPing.ElapsedMilliseconds.ToString(); });

                    counterArr++;

                      
                    if (autoControl) controlling();

                    //Sensordaten in das Logging-File schreiben
                   // if (dataWrite) FileWriting(); //Hier verwenden wenn Sensordaten Priorität haben 
                    
                    if(radarDownStart == Int16.MinValue)
                    {
                        radarDownStart = radarData.distDown;
                    }

                    radarData.distDown -= radarDownStart;

                    copterData.radarBuffer.Enqueue(new KeyValuePair<DateTime, str_RadarData>(DateTime.Now, radarData));

                    break;

                //wenn Endzeichen erhalten oder Counter überschritten
                default: //Counter zurücksetzen
                    messageCounter = -1;
                    
                    break;

            }

            //Messagecounter erhöhen
            messageCounter++; 
        }

        //Funktion die ausgeführt wird, sobald man den Start-Button betätigt
        private void butSensorStart_Click(object sender, EventArgs e)
        {
            //neue Messung veranlassen (Startzeichen "?" senden)
            List<byte> message = new List<byte>();
            message.Add(asciiQuestionmark);
            _connector.SendMsg(message.ToArray());

            //Timer für Ping starten
            stopwatchPing.Start();

            timerReceiveInterval.Enabled = true;
            timerReceiveInterval.Start();

            //add to graph
            graph.addSeries(entryUltraSonic);
        }

        private void timerReceiveInterval_Tick(object sender, EventArgs e)
        {
            //Nach Ablauf des Timers, nächste Messung veranlassen
            List<byte> message2 = new List<byte>();
            message2.Add(asciiQuestionmark);
            _connector.SendMsg(message2.ToArray());

            //Pingmessung zuerst zurücksetzten, dann neu starten
            stopwatchPing.Reset();
            
            //if (graph.Visible)
            //    graph.AddXY(entryUltraSonic, radarData.distDown);

            stopwatchPing.Start();
        }

#endregion        

#region Buttons

        //COM-Port Verbindung für die Steuerung herstellen
        private void butConnectControl_Click(object sender, EventArgs e)
        {
            bool error = false; //Variable zur Fehleridentifikation
            

            try //Fehlerbehandlung
            {
                // connect the port
                if (!control.IsConnected())
                {
                    control.connect("COM" + tBoxCOMPort1.Text);
                }   
            }
            catch (UnauthorizedAccessException) { error = true; }
            catch (IOException) { error = true; }
            catch (ArgumentException) { error = true; }

            if (error) MessageBox.Show("Com-Port für Regelung konnte nicht geöffnet werden. Wahrscheinlich wird der Port bereits verwendet, wurde entfernt oder steht nicht zur Verfügung.", "COM Port kann nicht geöffnet werden", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            else
            {
                //Wenn kein Fehler aufgetreten ist, bestehende Verbindung anzeigen und abhängige Funktionen freigeben
                lblControlConnected.BackColor = System.Drawing.Color.LightGreen;
                lblControlConnected.Text = "Steuerung Verbunden";
                controlConnected = true;
                butSend.Enabled = true;
                butConnectControl.Enabled = false;
            }            
        }

        //Aufforderung manuel eingestellte Steuerungsdaten zu übertragen
        private void butSend_Click(object sender, EventArgs e)
        {
            sending();
        }

        private void rButSend_CheckedChanged(object sender, EventArgs e) //Senden durch Klick oder per Timer auswählen
        {
            //automatisch senden und Verbindung besteht
            if (!rButSendManual.Checked && controlConnected)
            {
                timerSendInterval.Enabled = true;                                   //Timer aktiv
                timerSendInterval.Interval = Convert.ToInt32(tBoxInterval.Text);    //eingestelltes Interval übernehmen
            }
            //automatisch senden aber keine Verbindung besteht
            else if (!rButSendManual.Checked && !controlConnected)
            {
                //Fehlermeldung ausgeben und Automtik Sendemodus beenden
                MessageBox.Show("Es besteht keine Verbindung zu einem Com-Port für die Steuerung!", "Keine Verbindung zu COM-Port!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                rButSendManual.Checked = true;
            }
            //manuel senden
            else timerSendInterval.Enabled = false; //Timer inaktiv
        }

        private void butVideoStart_Click(object sender, EventArgs e)
        {
            if (!timerUpdateValuesVisual.Enabled)
            { 
               if(visualControl == null)
               {
                   visualControl = new VisualControlWrapper(Convert.ToInt16(tbCameraIndex.Text));
                   visualControl.setCalculationParams(actualCalibration);
               }

                timerUpdateValuesVisual.Enabled = true;
                tbCameraIndex.Enabled = false;
                comboCameraType.Enabled = false;
                graph.addSeries(entryAltitudePLatformFC);
                butVideoStart.Text = "Bildauswertung beenden";

                if(cbShowCompare.Checked)
                {
                    if (visualControl2 == null)
                    {
                        visualControl2 = new VisualControlWrapper(Convert.ToInt16(tbCameraIndex.Text) + 1);
                        if(actualCalibration.Equals(calibrationData_FlyCam))
                        {
                            visualControl2.setCalculationParams(calibrationData_GoPro);
                        }
                        else if (actualCalibration.Equals(calibrationData_GoPro))
                        {
                            visualControl2.setCalculationParams(calibrationData_FlyCam);
                        }
                    }
                    //Test 2 Cameras
                    timerUpdateValuesVisual2.Enabled = true;
                    graph.addSeries(entryAltitudePLatformGP);
                }
                cbShowCompare.Enabled = false;
            }
            else
            {
                butVideoStart.BackColor = SystemColors.Control;
                timerUpdateValuesVisual.Enabled = false;
                tbCameraIndex.Enabled = true;
                comboCameraType.Enabled = true;
                copterData.resetvisualBuffer();
                graph.deleteSeries(entryAltitudePLatformFC);                
                //create a new visual control
                visualControl.end();
                visualControl = null;                
                butVideoStart.Text = "Bildauswertung starten";

                if (cbShowCompare.Checked)
                {
                    //Test 2 Camers
                    timerUpdateValuesVisual2.Enabled = false;
                    visualControl2 = new VisualControlWrapper(Convert.ToInt16(tbCameraIndex.Text) + 1);
                    copterData.resetvisualBuffer2();
                    graph.deleteSeries(entryAltitudePLatformGP);
                }
                cbShowCompare.Enabled = true;
            }            
        }

        //Anwendung schließen
        private void butClose_Click(object sender, EventArgs e)
        {
            control = null;
            if (visualControl != null)
            {
                visualControl.end();
                visualControl = null;
            }            
            visualControl2= null;
            graph = null;
            this.Close();
            Environment.Exit(0);
        }        

#endregion

#region User Inputs

        //Speichern der Settings für beide COMPort Textboxen
        private void COMPort_TextChanged(object sender, EventArgs e)
        {
            if (settingsLoaded) saveSettings();
        }

        //Umschalten zwischen automatischer und manueller Steuerung
        private void rButControl_CheckedChanged(object sender, EventArgs e)
        {
            //Anfangssettings nur für automatische Steuerung
            if (rButAutomaticControl.Checked == true)
            {
                if (userInputCheck() == false)   //Prüfen ob alle Einstellungen für die automatische Steuerung gültig sind
                {
                    cBoxEnable.Checked = false;
                }

                autoControl = true;
                gBoxSettingManual.Enabled = false;
                gBoxGraphicOut.Enabled = true;

                timerSendInterval.Enabled = false;
            }
            //Anfangssettings nur für manuelle Steuerung
            else if (rButManualControl.Checked == true)
            {
                autoControl = false;
                gBoxSettingManual.Enabled = true;
                gBoxGraphicOut.Enabled = false;
                cBoxEnable.Checked = false;
                tBarHeight.Value = 0;
                tBarGas.Value = 0;
                tBarGier.Value = 0;
                tBarNick.Value = 0;
                tBarRoll.Value = 0;

                //Wenn nötig Sende-Timer einschalten
                if (rButSendAuto.Checked == true) timerSendInterval.Enabled = true;

                //Balkenvisualisierung für automatische Steuerung auf Grundzustand zurücksetzten um Fehlinterpretation zu vermeiden
                //Die untenstehenden Berechnungen der Label-Positionen beziehten sich auf die Größenverhältnisse in der GroupBox und können beliebig verändert werden
                lblFront.Height = labelLength;
                lblFront.Top = labelLeftOffset;
                lblRear.Height = labelLength;
                lblRear.Top = labelLeftOffset + labelLength + labelWidth;
                lblLeft.Width = labelLength;
                lblLeft.Left = labelWidth;
                lblRight.Width = labelLength;
                lblRight.Left = labelWidth + labelLength + labelWidth;
                lblUp.Height = labelTopDownLength;
                lblUp.Top = labelTopOffset;
                lblDown.Height = labelTopDownLength;
                lblDown.Top = labelTopOffset + labelTopDownLength + labelWidth;
            }
            //Anfangssettings für beide Steuerungen
            cBoxBut1.Checked = false;
            cBoxBut2.Checked = false;
            tBoxRemoteButtons.Text = "";
        }

        private void cBoxEnable_CheckedChanged(object sender, EventArgs e) //ExternControls Anwenden(1) oder nicht(0)
        {
            if (cBoxEnable.Checked == true && rButAutomaticControl.Checked == true && userInputCheck() == false)   //Prüfen ob alle Einstellungen für die automatische Steuerung gültig sind
            {
                cBoxEnable.Checked = false;
                return;
            }

            //Abhängig davon ob ExternControls aktiv sind oder nicht sollen unterschiedliche Oberflächenelemente verfügbar und konfiguriert sein
            if (cBoxEnable.Checked == true && rButAutomaticControl.Checked == true) gBoxSettingAuto.Enabled = false;
            else gBoxSettingAuto.Enabled = true;

            if (!cBoxEnable.Checked)
            {
                rButAutomaticControl.Enabled = true;
                rButManualControl.Enabled = true;                
                ExternControl.Config = 0;
            }
            else
            {
                if (rButAutomaticControl.Checked == true) rButManualControl.Enabled = false;
                else rButAutomaticControl.Enabled = false;
                ExternControl.Config = 1;
            }

            //Einstellung für ExternControls in das Strukt und das Array eintragen
            tBoxStructConfig.Text = ExternControl.Config.ToString();
            arr_ExternControl[10] = (char)ExternControl.Config;
        }

        //Prüfen ob Daten in das Logging-File geschrieben werden sollen
        private void cBoxDataWright_CheckedChanged(object sender, EventArgs e)
        {
            if (cBoxDataWright.Checked == true)
            {
                dataWrite = true;
            }
            else
            {
                dataWrite = false;                
            }
        }        

        //Prüffunktion für div. Textboxen. Es können nur Nummern eingegben werden
        private void tBoxNumbercheck_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) e.Handled = true;
        }

        //Wenn ein negatives Timerintaval eingegeben wird, dies korrigieren
        private void tBoxInterval_TextChanged(object sender, EventArgs e) 
        {
            if (rButSendAuto.Checked)
            {
                if (!(Convert.ToInt32(tBoxInterval.Text) < 1)) timerSendInterval.Interval = Convert.ToInt32(tBoxInterval.Text);
                else timerSendInterval.Interval = 200;
            }

            if (settingsLoaded) saveSettings();
        }

        //Setzen gültiger Werte für die Digital Buttons des Strukts
        private void cBoxesForDigital_CheckedChanged(object sender, EventArgs e) 
        {
            //Benutzereingaben auswerten und in die nötigen Datenformate ransferieren
            byte first = 0, second = 0;

            if (cBoxBut1.Checked) first = 1;
            else first = 0;

            if (cBoxBut2.Checked) second = 1;
            else second = 0;

            //Erstellte Werte in das Strukt und das Array eintragen
            ExternControl.Digital[0] = first;
            ExternControl.Digital[1] = second;            
            arr_ExternControl[0] = (char)ExternControl.Digital[0];
            arr_ExternControl[1] = (char)ExternControl.Digital[1];
        }

        //Setzen gültiger Werte für die RemoteButtons des Strukts
        private void tBoxRemoteButtons_TextChanged(object sender, EventArgs e)  
        {
            if (tBoxRemoteButtons.Text == "") ExternControl.RemoteButtons = 0;
            else ExternControl.RemoteButtons = Convert.ToByte(Convert.ToChar(tBoxRemoteButtons.Text));            
            arr_ExternControl[2] = (char)ExternControl.RemoteButtons;
        }

        //Manuelles Setzen des Höhenwerts
        private void tBarHeight_ValueChanged(object sender, EventArgs e) 
        {
            tBoxHeight.Text = tBarHeight.Value.ToString();
            ExternControl.Height = Convert.ToSByte(tBarHeight.Value);            
            arr_ExternControl[7] = (char)ExternControl.Height;
        }

        //Manuelles Setzen des Gaswerts
        private void tBarGas_ValueChanged(object sender, EventArgs e) 
        {
            tBoxGas.Text = tBarGas.Value.ToString();
            ExternControl.Gas = Convert.ToByte(tBarGas.Value);            
            arr_ExternControl[6] = (char)ExternControl.Gas;
        }

        //Manuelles Setzen des Gierwerts
        private void tBarGier_ValueChanged(object sender, EventArgs e) 
        {
            tBoxGier.Text = tBarGier.Value.ToString();
            ExternControl.Gier = Convert.ToSByte(tBarGier.Value);            
            arr_ExternControl[5] = (char)ExternControl.Gier;
        }

        //Manuelles Setzen des Nickwerts
        private void tBarNick_ValueChanged(object sender, EventArgs e) 
        {
            tBoxNick.Text = tBarNick.Value.ToString();
            //Umrechnung nötig, da Nickachse keine negativen Werte akzeptiert
            if (tBarNick.Value >= 0)
            {
                NickTemp = tBarNick.Value;
            }
            else
            {
                NickTemp = tBarNick.Value + 254;
            }
            ExternControl.Nick = Convert.ToByte(NickTemp);            
            arr_ExternControl[3] = (char)ExternControl.Nick;
        }

        //Manuelles Setzen des Rollwerts
        private void tBarRoll_ValueChanged(object sender, EventArgs e)  
        {
            tBoxRoll.Text = tBarRoll.Value.ToString();
            ExternControl.Roll = Convert.ToSByte(-tBarRoll.Value);      //Negierung nötig, da Rollachse spiegelverkehrt interpretiert wird            
            arr_ExternControl[4] = (char)ExternControl.Roll;
        }

        private bool userInputCheck()
        {
            bool checkResult = true;


            //Plausibilitätsprüfung, Benutzinformation und Korrektur für die Regelungs-Anfangsgrenze
            if (Convert.ToInt32(tBoxControlStartValueX.Text) > 500)
            {
                MessageBox.Show("Es können keine Entfernungen über 500cm eingetragen werden!", "Ungültiger Eingabewert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tBoxControlStartValueX.Text = "500";

                checkResult = false;
            }
            if (Convert.ToInt32(tBoxControlStartValueX.Text) < 2)
            {
                MessageBox.Show("Die Entfernung darf nicht kleiner als 2cm sein!", "Ungültiger Eingabewert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tBoxControlStartValueX.Text = "2";

                checkResult = false;
            }

            if (Convert.ToInt32(tBoxControlStartValueY.Text) > 500)
            {
                MessageBox.Show("Es können keine Entfernungen über 500cm eingetragen werden!", "Ungültiger Eingabewert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tBoxControlStartValueY.Text = "500";

                checkResult = false;
            }
            if (Convert.ToInt32(tBoxControlStartValueY.Text) < 2)
            {
                MessageBox.Show("Die Entfernung darf nicht kleiner als 2cm sein!", "Ungültiger Eingabewert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tBoxControlStartValueY.Text = "2";

                checkResult = false;
            }

            if (Convert.ToInt32(tBoxControlStartValueZ.Text) > 500)
            {
                MessageBox.Show("Es können keine Entfernungen über 500cm eingetragen werden!", "Ungültiger Eingabewert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tBoxControlStartValueZ.Text = "500";

                checkResult = false;
            }
            if (Convert.ToInt32(tBoxControlStartValueZ.Text) < 2)
            {
                MessageBox.Show("Die Entfernung darf nicht kleiner als 2cm sein!", "Ungültiger Eingabewert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tBoxControlStartValueZ.Text = "2";

                checkResult = false;
            }


            //Plausibilitätsprüfung und Korrektur für die Regelungs-Endgrenze
            if (Convert.ToInt32(tBoxControl100PercentValueX.Text) > Convert.ToInt32(tBoxControlStartValueX.Text))
            {
                MessageBox.Show("Der Wert für 100% Ansteuerung muss kleiner sein als der Startwert für die Steuerung", "Ungültiger Eingabewert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tBoxControl100PercentValueX.Text = "1";

                checkResult = false;
            }

            if (Convert.ToInt32(tBoxControl100PercentValueY.Text) > Convert.ToInt32(tBoxControlStartValueY.Text))
            {
                MessageBox.Show("Der Wert für 100% Ansteuerung muss kleiner sein als der Startwert für die Steuerung", "Ungültiger Eingabewert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tBoxControl100PercentValueY.Text = "1";

                checkResult = false;
            }

            if (Convert.ToInt32(tBoxControl100PercentValueZ.Text) > Convert.ToInt32(tBoxControlStartValueZ.Text))
            {
                MessageBox.Show("Der Wert für 100% Ansteuerung muss kleiner sein als der Startwert für die Steuerung", "Ungültiger Eingabewert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tBoxControl100PercentValueZ.Text = "1";

                checkResult = false;
            }


            //Plausibilitätsprüfung und Korrektur für die Regelungs-Maximalwerte
            if (Convert.ToInt32(tBoxMaximumX.Text) > 126)
            {
                MessageBox.Show("Es können keine Werte über 126 eingetragen werden!", "Ungültiger Eingabewert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tBoxMaximumX.Text = "126";

                checkResult = false;
            }

            if (Convert.ToInt32(tBoxMaximumY.Text) > 126)
            {
                MessageBox.Show("Es können keine Werte über 126 eingetragen werden!", "Ungültiger Eingabewert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tBoxMaximumY.Text = "126";

                checkResult = false;
            }

            if (Convert.ToInt32(tBoxMaximumZ.Text) > 240)
            {
                MessageBox.Show("Es können keine Werte über 240 eingetragen werden!", "Ungültiger Eingabewert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tBoxMaximumZ.Text = "240";

                checkResult = false;
            }


            //Überprüfen ob Einstellungen für maximalen Gaswert und Schwebegaswert NICHT plausiebel sind
            if (Convert.ToInt32(tBoxHoverGas.Text) > 239)
            {
                MessageBox.Show("Werte Größer als 239 sind nicht zulässig!", "Ungültige Eingabe!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tBoxHoverGas.Text = "239";

                checkResult = false;
            }   
                       
            if (Convert.ToInt32(tBoxMaximumZ.Text) < Convert.ToInt32(tBoxHoverGas.Text))
            {
                MessageBox.Show("Das Maximum der Z-Achse darf nicht kleiner als das Schwebe-Gas sein!", "Ungültiger Eingabewert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tBoxMaximumZ.Text = (Convert.ToInt32(tBoxHoverGas.Text) + 1).ToString();

                checkResult = false;
            }

            return (checkResult);
        }

        //Eingabefunktionen für die Einstellungen der automatischen Steuerung

        private void tBoxControlStartValueX_TextChanged(object sender, EventArgs e)  
        {
            controlStartValueX = Convert.ToInt32(tBoxControlStartValueX.Text); //Distanz-Wert einlesen, bei dem die Steuerung einsetzten soll

            controlDeltaXX = controlStartValueX - control100PercentValueX; //Neuberechnung für die Steigung der Linearen Regelungskurve

            if (settingsLoaded) saveSettings(); //Einstellungen in Datei speichern
        }
        
        private void tBoxControl100PercentValueX_TextChanged(object sender, EventArgs e) 
        {
            control100PercentValueX = Convert.ToInt32(tBoxControl100PercentValueX.Text); //Distanz-Wert einlesen, bei dem die Steuerung den maximalen Steuerungswert liefern soll

            controlDeltaXX = controlStartValueX - control100PercentValueX; //Neuberechnung für die Steigung der Linearen Regelungskurve

            if (settingsLoaded) saveSettings(); //Einstellungen in Datei speichern
        }

        private void tBoxControlStartValueY_TextChanged(object sender, EventArgs e)  
        {
            controlStartValueY = Convert.ToInt32(tBoxControlStartValueY.Text); //Distanz-Wert einlesen, bei dem die Steuerung einsetzten soll

            controlDeltaXY = controlStartValueY - control100PercentValueY; //Neuberechnung für die Steigung der Linearen Regelungskurve

            if (settingsLoaded) saveSettings(); //Einstellungen in Datei speichern
        }

        private void tBoxControl100PercentValueY_TextChanged(object sender, EventArgs e)
        {
            control100PercentValueY = Convert.ToInt32(tBoxControl100PercentValueY.Text); //Distanz-Wert einlesen, bei dem die Steuerung den maximalen Steuerungswert liefern soll

            controlDeltaXY = controlStartValueY - control100PercentValueY; //Neuberechnung für die Steigung der Linearen Regelungskurve

            if (settingsLoaded) saveSettings(); //Einstellungen in Datei speichern
        }

        private void tBoxControlStartValueZ_TextChanged(object sender, EventArgs e)
        {
            controlStartValueZ = Convert.ToInt32(tBoxControlStartValueZ.Text); //Distanz-Wert einlesen, bei dem die Steuerung einsetzten soll

            controlDeltaXZ = controlStartValueZ - control100PercentValueZ; //Neuberechnung für die Steigung der Linearen Regelungskurve

            if (settingsLoaded) saveSettings();
        }

        private void tBoxControl100PercentValueZ_TextChanged(object sender, EventArgs e)
        {
            control100PercentValueZ = Convert.ToInt32(tBoxControl100PercentValueZ.Text); //Distanz-Wert einlesen, bei dem die Steuerung den maximalen Steuerungswert liefern soll

            controlDeltaXZ = controlStartValueZ - control100PercentValueZ; //Neuberechnung für die Steigung der Linearen Regelungskurve

            if (settingsLoaded) saveSettings(); //Einstellungen in Datei speichern
        }

        private void tBoxMaximumX_TextChanged(object sender, EventArgs e)
        {
            maximumX = Convert.ToInt32(tBoxMaximumX.Text); //Maximalwert für die Steuerung einlesen

            if (settingsLoaded) saveSettings(); //Einstellungen in Datei speichern
        }

        private void tBoxMaximumY_TextChanged(object sender, EventArgs e)
        {
            maximumY = Convert.ToInt32(tBoxMaximumY.Text); //Maximalwert für die Steuerung einlesen

            if (settingsLoaded) saveSettings(); //Einstellungen in Datei speichern
        }

        private void tBoxMaximumZ_TextChanged(object sender, EventArgs e)
        {
            maximumZ = Convert.ToInt32(tBoxMaximumZ.Text); //Maximalwert für die Steuerung einlesen

            if (settingsLoaded) saveSettings(); //Einstellungen in Datei speichern
        } 

        private void tBoxHoverGas_TextChanged(object sender, EventArgs e)
        {
            hoverGas = Convert.ToInt32(tBoxHoverGas.Text); //Schwebegaswert einlesen

            if (settingsLoaded) saveSettings(); //Einstellungen in Datei speichern

            lblCenterZ.Top = labelTopOffset + labelGasLength - hoverGas;
        }

        //Settings nach Änderungen durch den Benutzer für nächsten Programmstart sichern
        private void saveSettings()
        {
            try
            {
                StreamWriter Settings = new StreamWriter(settingsFile);
                Settings.Write(tBoxCOMPort1.Text + ";" + tBoxCOMPort2.Text + ";" +
                    tBoxControlStartValueX.Text + ";" + tBoxControl100PercentValueX.Text + ";" + tBoxMaximumX.Text + ";" +
                    tBoxControlStartValueY.Text + ";" + tBoxControl100PercentValueY.Text + ";" + tBoxMaximumY.Text + ";" +
                    tBoxControlStartValueZ.Text + ";" + tBoxControl100PercentValueZ.Text + ";" + tBoxHoverGas.Text + ";" + tBoxMaximumZ.Text + ";" +
                    tBoxInterval.Text);
                Settings.Close();
            }
            catch
            {
                MessageBox.Show("Die Änderungen an den Einstellungen konnten nicht gespeichert werden!\nStellen Sie sicher, dass Schreibzugriff auf die Datei 'Settings.csv' besteht.", "Datei kann nicht geschrieben werden!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Beim Beenden der Anwendung muss die Logging-Datei geschlossen werden
        private void frmUAVSensorControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Es soll sichergestellt sein, dass die letzte Übertragung zur Drohne die ExternControl-Funktion deaktiviert
            try
            {
                if (control.IsConnected())
                {
                    arr_ExternControl[10] = '0';
                    //SendOutData('b', 1, arr_ExternControl, (byte)arr_ExternControl.Length);
                    requestSendExtControlData(arr_ExternControl);
                }
            }
            catch { }

            DataFile.Close();
        }
       
#endregion

#region Functions
        private void controlling()
        {
            /*
             * Die Formeln für die Berechnung der linearen Regelung beruhen auf der Formel für eine Gerade durch zwei Punkte. y = m * x + t
             * Die X-Koordinaten dieser zwei Punkte stellen die beiden Grenzabstände der Regelung. (Standard: Obere Grenze 450cm; Untere Grenze 50cm) Also x1 = 50; x2 = 450
             * Die Y-Koordinaten Stellen die beiden Grenzpunkte der Aussteuerung, also maximale und minimale Aussteuerung für eine Bewegungsrichtung. (z.B. minimal = 0; maximal = 126 bzw. -126) !Für GIER gilt max = 100 bzw. -100!
             * (Achtung: Da Gas keine negativen Werte kennt, gilt hier: Für Abwärtsmessung -> min = Schwebegas; max = 240 / für Aufwärtsmessung -> min = 0; max = Schwebegas)
             * Somit erhält man für jede Bewegungsrichtung die Steigung "m" der gesuchten Gerade. m = deltaY / deltaX. (Im Standardfall ist DeltaY = 126 und deltaX ist 450 - 50 = 400)
             * Es fehlt jedoch noch der Schnittpunkt dieser Geraden mit der Y-Achse, hier "t" genannt.
             * t kann durch umstellen der Geradengleichung y = mx + t errechnet werden -> t = y - mx. Die Steigung m wurde bereits im Schritt zuvor bestimmt. Für y und x können beliebige Punkte der Geraden eingesetzt werden (z.B. der Punkt an dem Y sein Maximum hat).
             * Somit ergibt sich schlussendlich für die Steuerauslenkung y = mx + t: y = deltaY / deltaX * x +/- MaximaleAuslenkung +/- deltaY / deltaX * UntereGrenze
             * -> y = deltaY / deltaX * (x - UntereGrenze) +/- MaximaleAuslenkung
             * (Die Vorzeichen +/- ergeben sich abhängig von der gesuchten Bewegungsrichtung und der dafür notwendigen Variablenvorzeichen) 
            */
            try
            {
                if (radarData.distFront > controlStartValueY && radarData.distRear > controlStartValueY)  //Herstellen neutraler Werte wenn keine Regelung Forne-Hinten nötig ist
                {
                    tempValue = 0;
                    tBoxStructNick.Invoke((MethodInvoker)delegate { tBoxStructNick.Text = Convert.ToInt32(tempValue).ToString(); });
                    ExternControl.Nick = Convert.ToByte((int)tempValue);
                    arr_ExternControl[3] = (char)ExternControl.Nick;

                    lblFront.Invoke((MethodInvoker)delegate { lblFront.Height = 0; lblFront.Top = labelLeftOffset + labelLength; });
                    lblRear.Invoke((MethodInvoker)delegate { lblRear.Height = 0; });
                }

                else if (radarData.distFront <= controlStartValueY && radarData.distRear <= controlStartValueY && radarData.distFront <= radarData.distRear)       //Vorne und Hinten gleichzeitig ein Hindernis erkannt; vorderes Hindernis ist näher oder gleich weit entfernt
                {
                    radarData.distFront = controlStartValueY - (radarData.distRear - radarData.distFront);   //Unterschied zwischen beiden Abständen errechnen. Je größer der Unterschied desto stärker muss Lenkbewegung sein
                    radarData.distRear = controlStartValueY + 1;                       //Verhindern, dass weiter entferntes Hindernis eine zweite, entgegengesetzte Lenkbewegung auslöst
                }

                else if (radarData.distFront <= controlStartValueY && radarData.distRear <= controlStartValueY && radarData.distFront > radarData.distRear)   //Vorne und Hinten gleichzeitig ein Hindernis erkannt; vorderes Hindernis ist weiter entfernt
                {
                    radarData.distRear = controlStartValueY - (radarData.distFront - radarData.distRear);    //Unterschied zwischen beiden Abständen errechnen. Je größer der Unterschied desto stärker muss Lenkbewegung sein
                    radarData.distFront = controlStartValueY + 1;                      //Verhindern, dass weiter entferntes Hindernis eine zweite, entgegengesetzte Lenkbewegung auslöst
                }

                if (radarData.distFront <= controlStartValueY) //Hindernis Vorne oder Vorne näher als Hinten
                {
                    //Errechnen des Regelwertes nach linearer Geradengleichung und Eintragung in das Array und das Strukt
                    tempValue = maximumY / (float)controlDeltaXY * (radarData.distFront - control100PercentValueY) + 254 - maximumY;
                    if (tempValue < 254 - maximumY) tempValue = 254 - maximumY;
                    tBoxStructNick.Invoke((MethodInvoker)delegate { tBoxStructNick.Text = Convert.ToInt32(tempValue).ToString(); });
                    ExternControl.Nick = Convert.ToByte((int)tempValue);
                    arr_ExternControl[3] = (char)ExternControl.Nick;

                    //Graphische Darstellung entsprechend der Regelung darstellen
                    lblFront.Invoke((MethodInvoker)delegate { lblFront.Height = 0; lblFront.Top = labelLeftOffset + labelLength; });
                    lblRear.Invoke((MethodInvoker)delegate { lblRear.Height = 254 - (int)tempValue; });
                }

                if (radarData.distRear <= controlStartValueY)  //Hindernis Hinten oder Hinten näher als Vorne
                {
                    //Errechnen des Regelwertes nach linearer Geradengleichung und Eintragung in das Array und das Strukt
                    tempValue = -maximumY / (float)controlDeltaXY * (radarData.distRear - control100PercentValueY) + maximumY;
                    if (tempValue > maximumY) tempValue = maximumY;
                    tBoxStructNick.Invoke((MethodInvoker)delegate { tBoxStructNick.Text = Convert.ToInt32(tempValue).ToString(); });
                    ExternControl.Nick = Convert.ToByte((int)tempValue);
                    arr_ExternControl[3] = (char)ExternControl.Nick;

                    //Graphische Darstellung entsprechend der Regelung darstellen
                    lblRear.Invoke((MethodInvoker)delegate { lblRear.Height = 0; });
                    lblFront.Invoke((MethodInvoker)delegate { lblFront.Height = (int)tempValue; lblFront.Top = labelLeftOffset + labelLength - (int)tempValue; });
                }

                if (radarData.distLeft > controlStartValueX && radarData.distRight > controlStartValueX)  //Herstellen neutraler Werte wenn keine Regelung Links-Rechts nötig ist
                {
                    tempValue = 0;
                    tBoxStructRoll.Invoke((MethodInvoker)delegate { tBoxStructRoll.Text = Convert.ToInt32(tempValue).ToString(); });
                    ExternControl.Roll = Convert.ToSByte((int)tempValue);
                    arr_ExternControl[4] = (char)ExternControl.Roll;

                    lblRight.Invoke((MethodInvoker)delegate { lblRight.Width = 0; });
                    lblLeft.Invoke((MethodInvoker)delegate { lblLeft.Left = labelWidth + labelLength; lblLeft.Width = 0; });
                }

                else if (radarData.distLeft <= controlStartValueX && radarData.distRight <= controlStartValueX && radarData.distLeft <= radarData.distRight)       //Links und Rechts gleichzeitig ein Hindernis erkannt; linkes Hindernis ist näher oder gleich weit entfernt
                {
                    radarData.distLeft = controlStartValueX - (radarData.distRight - radarData.distLeft);   //Unterschied zwischen beiden Abständen errechnen. Je größer der Unterschied desto stärker muss Lenkbewegung sein
                    radarData.distRight = controlStartValueX + 1;                       //Verhindern, dass weiter entferntes Hindernis eine zweite, entgegengesetzte Lenkbewegung auslöst
                }

                else if (radarData.distLeft <= controlStartValueX && radarData.distRight <= controlStartValueX && radarData.distLeft > radarData.distRight)   //Vorne und Hinten gleichzeitig ein Hindernis erkannt; linkes Hindernis ist weiter entfernt
                {
                    radarData.distRight = controlStartValueX - (radarData.distLeft - radarData.distRight);    //Unterschied zwischen beiden Abständen errechnen. Je größer der Unterschied desto stärker muss Lenkbewegung sein
                    radarData.distLeft = controlStartValueX + 1;                      //Verhindern, dass weiter entferntes Hindernis eine zweite, entgegengesetzte Lenkbewegung auslöst
                }

                if (radarData.distLeft <= controlStartValueX)  //Hindernis Links oder Links näher als Rechts
                {
                    //Errechnen des Regelwertes nach linearer Geradengleichung und Eintragung in das Array und das Strukt
                    tempValue = maximumX / (float)controlDeltaXX * (radarData.distLeft - control100PercentValueX) - maximumX;
                    if (tempValue < -maximumX) tempValue = -maximumX;
                    tBoxStructRoll.Invoke((MethodInvoker)delegate { tBoxStructRoll.Text = Convert.ToInt32(tempValue).ToString(); });
                    ExternControl.Roll = Convert.ToSByte((int)tempValue);
                    arr_ExternControl[4] = (char)ExternControl.Roll;

                    //Graphische Darstellung entsprechend der Regelung darstellen
                    lblRight.Invoke((MethodInvoker)delegate { lblRight.Width = (int)-tempValue; });
                    lblLeft.Invoke((MethodInvoker)delegate { lblLeft.Left = labelWidth + labelLength; lblLeft.Width = 0; });
                }

                if (radarData.distRight <= controlStartValueX) //Hindernis Rechts oder Rechts näher als Links
                {
                    //Errechnen des Regelwertes nach linearer Geradengleichung und Eintragung in das Array und das Strukt
                    tempValue = -maximumX / (float)controlDeltaXX * (radarData.distRight - control100PercentValueX) + maximumX;
                    if (tempValue > maximumX) tempValue = maximumX;
                    tBoxStructRoll.Invoke((MethodInvoker)delegate { tBoxStructRoll.Text = Convert.ToInt32(tempValue).ToString(); });
                    ExternControl.Roll = Convert.ToSByte((int)tempValue);
                    arr_ExternControl[4] = (char)ExternControl.Roll;

                    //Graphische Darstellung entsprechend der Regelung darstellen
                    lblLeft.Invoke((MethodInvoker)delegate { lblLeft.Width = (int)tempValue; lblLeft.Left = labelWidth + labelLength - (int)tempValue; });
                    lblRight.Invoke((MethodInvoker)delegate { lblRight.Width = 0; });
                }

                //Gier ist nicht Teil der Regelung, muss aber trotzdem gesetzt und übertragen werden
                tBoxStructGier.Invoke((MethodInvoker)delegate { tBoxStructGier.Text = "0"; });
                ExternControl.Gier = 0;
                arr_ExternControl[5] = (char)ExternControl.Gier;

                if (radarData.distUp > controlStartValueZ && radarData.distDown > controlStartValueZ) //Herstellen neutraler Werte wenn keine Regelung Oben-Unten nötig is
                {
                    tempValue = hoverGas;
                    tBoxStructGas.Invoke((MethodInvoker)delegate { tBoxStructGas.Text = Convert.ToInt32(tempValue).ToString(); });
                    ExternControl.Gas = Convert.ToByte((int)tempValue);
                    arr_ExternControl[6] = (char)ExternControl.Gas;

                    lblDown.Invoke((MethodInvoker)delegate { lblDown.Height = 0; lblDown.Top = labelTopOffset + labelWidth + labelGasLength - hoverGas;});
                    lblUp.Invoke((MethodInvoker)delegate { lblUp.Height = 0; lblUp.Top = labelTopOffset + labelGasLength - hoverGas; });
                }

                else if (radarData.distUp <= controlStartValueZ && radarData.distDown <= controlStartValueZ && radarData.distUp <= radarData.distDown)       //Oben und Unten gleichzeitig ein Hindernis erkannt; oberes Hindernis ist näher oder gleich weit entfernt
                {
                    radarData.distUp = controlStartValueZ - (radarData.distDown - radarData.distUp);   //Unterschied zwischen beiden Abständen errechnen. Je größer der Unterschied desto stärker muss Lenkbewegung sein
                    radarData.distDown = controlStartValueZ + 1;                       //Verhindern, dass weiter entferntes Hindernis eine zweite, entgegengesetzte Lenkbewegung auslöst
                }

                else if (radarData.distUp <= controlStartValueZ && radarData.distDown <= controlStartValueZ && radarData.distUp > radarData.distDown)   //Oben und Unten gleichzeitig ein Hindernis erkannt; oberes Hindernis ist weiter entfernt
                {
                    radarData.distDown = controlStartValueZ - (radarData.distUp - radarData.distDown);    //Unterschied zwischen beiden Abständen errechnen. Je größer der Unterschied desto stärker muss Lenkbewegung sein
                    radarData.distUp = controlStartValueZ + 1;                      //Verhindern, dass weiter entferntes Hindernis eine zweite, entgegengesetzte Lenkbewegung auslöst
                }

                if (radarData.distUp <= controlStartValueZ)
                {
                    //Errechnen des Regelwertes nach linearer Geradengleichung und Eintragung in das Array und das Strukt
                    tempValue = (float)(maximumZ - hoverGas) / (float)controlDeltaXZ * (radarData.distUp - control100PercentValueZ) + (hoverGas - (maximumZ - hoverGas)); //(2 * hoverGas - maximumZ) sorgt dafür dass auch für Sinkflug der Gaswert nicht um mehr als (maximumZ - hoverGas) gesenkt wird
                    if (tempValue < hoverGas - (maximumZ - hoverGas)) tempValue = hoverGas - (maximumZ - hoverGas);
                    tBoxStructGas.Invoke((MethodInvoker)delegate { tBoxStructGas.Text = Convert.ToInt32(tempValue).ToString(); });
                    ExternControl.Gas = Convert.ToByte((int)tempValue);
                    arr_ExternControl[6] = (char)ExternControl.Gas;

                    //Graphische Darstellung entsprechend der Regelung darstellen
                    lblDown.Invoke((MethodInvoker)delegate { lblDown.Height = hoverGas - (int)tempValue; lblDown.Top = labelTopOffset + labelWidth + labelGasLength - hoverGas; });
                    lblUp.Invoke((MethodInvoker)delegate { lblUp.Height = 0; lblUp.Top = labelTopOffset + labelGasLength - hoverGas; });
                }

                if (radarData.distDown <= controlStartValueZ)
                {
                    //Errechnen des Regelwertes nach linearer Geradengleichung und Eintragung in das Array und das Strukt
                    tempValue = -(maximumZ - (float)hoverGas) / (float)controlDeltaXZ * (radarData.distDown - control100PercentValueZ) + maximumZ;
                    if (tempValue > maximumZ) tempValue = maximumZ;
                    tBoxStructGas.Invoke((MethodInvoker)delegate { tBoxStructGas.Text = Convert.ToInt32(tempValue).ToString(); });
                    ExternControl.Gas = Convert.ToByte((int)tempValue);
                    arr_ExternControl[6] = (char)ExternControl.Gas;

                    //Graphische Darstellung entsprechend der Regelung darstellen
                    lblDown.Invoke((MethodInvoker)delegate { lblDown.Height = 0; lblDown.Top = labelTopOffset + labelWidth + labelGasLength - hoverGas; });
                    lblUp.Invoke((MethodInvoker)delegate { lblUp.Height = (int)tempValue - hoverGas; lblUp.Top = labelTopOffset + labelGasLength - (int)tempValue; });
                }

                //Sensordaten in das Logging-File schreiben
                //if (dataWrite) FileWriting(); //Hier verwenden, wenn Priorität auf Steuerung liegt   
                //SendOutData('b', 1, arr_ExternControl, (byte)arr_ExternControl.Length);
                
                //TODO Fabian 
                control.requestSendExtControlData(arr_ExternControl);
                
                
            }
            catch { }
        }

        //Distanzwerte aller 6 Sensoren und die Ergebnisse der Steuerung für die drei Achsen in die Logging-Datei schreiben und die Datensätze zählen und anzeigen
        private void FileWriting()
        {
            try
            {                
                DataFile.Write(tempDistRight.ToString() + ";");
                DataFile.Write(tempDistLeft.ToString() + ";");
                DataFile.Write(tempDistUp.ToString() + ";");
                DataFile.Write(tempDistDown.ToString() + ";");
                DataFile.Write(tempDistRear.ToString() + ";");
                DataFile.Write(tempDistFront.ToString() + ";");
                DataFile.Write(ExternControl.Roll.ToString() + ";");
                DataFile.Write(ExternControl.Nick.ToString() + ";");
                DataFile.WriteLine(ExternControl.Gas.ToString() + ";");
                lblDataCount.Invoke((MethodInvoker)delegate { lblDataCount.Text = (writeCounter++).ToString(); });
            }
            catch
            {
                MessageBox.Show("Die Daten konnten nicht in die Logging-Datei geschrieben werden.\n Stellen Sie sicher, dass die Datei 'Datalog.csv' existiert und Sie Schreibrechte in dem Programmordner haben.", "Auf Logging-Datei kann nicht zugegriffen werden!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cBoxDataWright.Checked = false;
            }
        }

        //Manuel erstellte Steuerungswerte an die Drohne senden
        private void sending()
        {
            if (controlConnected)
            {
                //Anzeigen der tatsächlich gesendeten Regelungswerte in der Strukt-GroupBox
                tBoxStructDigital.Text = ExternControl.Digital[0].ToString() + ", " + ExternControl.Digital[1].ToString();
                tBoxStructRemoteButtons.Text = ExternControl.RemoteButtons.ToString();
                tBoxStructHeight.Text = ExternControl.Height.ToString();
                tBoxStructGas.Text = ExternControl.Gas.ToString();
                tBoxStructGier.Text = ExternControl.Gier.ToString();
                tBoxStructNick.Text = ExternControl.Nick.ToString();
                tBoxStructRoll.Text = ExternControl.Roll.ToString();



                //Übergeben des Strukts an die COM-Port Sendefunktion
                // 'b' und 1 entsprechen der gewünschten Adresse und dem Komando an die FlghtCtl und sind vorgegeben
                //SendOutData('b', 1, arr_ExternControl, (byte)arr_ExternControl.Length);
                requestSendExtControlData(arr_ExternControl);
            }
        }

        private void requestSendExtControlData(char[] data)
        {
            if (controlConnected)
            {
                try
                {
                    control.requestSendExtControlData(arr_ExternControl);

                    sendCounter++;                              //Es wurde ein Datensatz mehr versendet, also sollte auch eine Antwort mehr zurückkommen
                }
                //Fehlermeldung falls Senden nicht möglich
                catch
                {
                    timerSendInterval.Enabled = false;
                    MessageBox.Show("Steuerbefehle konnten nicht gesendet werden. Wahrscheinlich besteht keine Verbindung zu einem COM-Port.", "Steuerbefehle Senden ist fehlgeschlagen!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    controlConnected = false;
                    lblControlConnected.BackColor = System.Drawing.Color.Red;
                    lblControlConnected.Text = "Regelung nicht verbunden";
                }

                if (sendCounter > 2)
                {
                    lblConnectionLost.Invoke((MethodInvoker)delegate { lblConnectionLost.Visible = true; });
                }
                else
                {
                    lblConnectionLost.Invoke((MethodInvoker)delegate { lblConnectionLost.Visible = false; });
                }
            }
            else
            {
                lblConnectionLost.Invoke((MethodInvoker)delegate { lblConnectionLost.Visible = true; });
            }
        }

        //Manuel erstellte Steuerungswerte an die Drohne nach ablauf des automatischen Timers senden
        private void timerSendInterval_Tick(object sender, EventArgs e) //Senden Event auslösen durch Timer
        {
            sending();
        }

        //Generieren der CRC Informationen für das Senden nach Vorgaben
        void AddCRC(uint frame_length) 
        { //length of #,adr,cmd,data

            uint tmpCRC = 0;
            uint i;

            for (i = 0; i < frame_length; i++)
            {
                tmpCRC += tx_buffer[i];
            }

            tmpCRC %= 4096;
            tx_buffer[i++] = Convert.ToChar('=' + tmpCRC / 64);
            tx_buffer[i++] = Convert.ToChar('=' + tmpCRC % 64);
            tx_buffer[i++] = '\r';

        }
        

        //Funktion zum Empfang der Antwortdaten von der Drohne
        void port_DataReceived(object sender, String raw_data)
        {
            if (!control.IsConnected()) return;        //Abbruch falls kein COM-Port geöffnet ist

            sendCounter = 0;                    //Den SendeCounter zurücksetzen (Eine Antwort von der Drohne ist eingetroffen, also besteht die Verbindung noch

            //Da weder die nötigen Variablen für den Aufruf der Dekodierfunktion bekannt sind noch die Interpretation der Ergebnisse, wird dies hier nicht ausgeführt
            //Decode64(unsigned char *ptrOut, unsigned char len, unsigned char ptrIn,unsigned char max)
        }

        private void timerUpdateValuesVisual_Tick(object sender, EventArgs e)
        {
            str_VisualData visData =visualControl.doDetection();

            if(!visData.DataVlid)
            {
                butVideoStart.Invoke((MethodInvoker)delegate{butVideoStart.BackColor=Color.Yellow;});
            }
            else
            {
                butVideoStart.Invoke((MethodInvoker)delegate { butVideoStart.BackColor = SystemColors.Control; });
            }

            copterData.visualBuffer.Enqueue(new KeyValuePair<DateTime, str_VisualData>(DateTime.Now, visData));
        }

        private void frmUAVSensorControl_EnabledChanged(object sender, EventArgs e)
        {
           
        }

        private void clbShowImage_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // quick an dirty, because it was annoying as 
            if (visualControl == null)
            {
                butVideoStart.PerformClick(); 
            }
            switch (e.Index)
            {
                case 0:
                    //show/ hide gray image
                    visualControl.setShowGrayImage(e.NewValue == CheckState.Checked ? true : false);
                    if (cbShowCompare.Checked)
                    {
                        visualControl2.setShowGrayImage(e.NewValue == CheckState.Checked ? true : false);
                    }
                    break;
                case 1:
                    // show/ hide canny
                    visualControl.setShowCannyImage(e.NewValue == CheckState.Checked ? true : false);
                    if (cbShowCompare.Checked)
                    {
                        visualControl2.setShowCannyImage(e.NewValue == CheckState.Checked ? true : false);
                    }
                    break;
                case 2:
                    //show result
                    visualControl.setShowResultImage(e.NewValue == CheckState.Checked ? true : false);
                    if (cbShowCompare.Checked)
                    {
                        visualControl2.setShowResultImage(e.NewValue == CheckState.Checked ? true : false);
                    }
                    break;
                case 3:
                    //show settings
                    visualControl.setShowSettings(e.NewValue == CheckState.Checked ? true : false);
                    if (cbShowCompare.Checked)
                    {
                        visualControl2.setShowSettings(e.NewValue == CheckState.Checked ? true : false);
                    }
                    break;
                case 4:
                    //show saved image
                    visualControl.setShowImage(e.NewValue == CheckState.Checked ? true : false);
                    if (cbShowCompare.Checked)
                    {
                        visualControl2.setShowImage(e.NewValue == CheckState.Checked ? true : false);
                    }
                    break;
                case 5:
                    //show saved image
                    visualControl.setShowAllShapes(e.NewValue == CheckState.Checked ? true : false);
                    if (cbShowCompare.Checked)
                    {
                        visualControl2.setShowAllShapes(e.NewValue == CheckState.Checked ? true : false);
                    }
                    break;
                default:
                    break;
            }
        }

        private void btStartOSDData_Click(object sender, EventArgs e)
        {
            if(timerUpdateOSDData.Enabled)
            {
                timerUpdateOSDData.Enabled = false;
                requestOSDDone = true;
                btStartOSDData.Text = "OSD Daten starten";
                control.OSDDataReceived -= myOSDDataHandler;
                copterData.resetOSDBuffer();
                graph.deleteSeries(entryOSD);
            }
            else
            {
                try
                {
                    if (!control.IsConnected())
                    {
                        control.connect("COM" + tBoxCOMPort1.Text);
                    }

                    control.OSDDataReceived += myOSDDataHandler;
                    graph.addSeries(entryOSD);
                    btStartOSDData.Text = "OSD Daten beenden";
                    timerUpdateOSDData.Enabled = true;
                }
                catch (Exception excep)
                {
                    MessageBox.Show(excep.Message);
                }
            }

           

            
        }

        private void butShowDiagram_Click(object sender, EventArgs e)
        {
            if(!graph.Visible)
            {
                try
                {
                    graph.Show();
                    butShowDiagram.Text = "Diagramm beenden";
                    timerUpdateGraph.Enabled = true;
                }
                catch (Exception excep)
                {
                    MessageBox.Show(excep.Message);
                }
            }
            else
            {
                try
                {
                    graph.Hide();
                    butShowDiagram.Text = "Diagramm zeigen";
                    timerUpdateGraph.Enabled = false;
                }
                catch (Exception excep)
                {
                    MessageBox.Show(excep.Message);
                }
            }
        }

        short osdAltitudeStart = short.MinValue;
        private void myOSDDataHandler(Object sender, OSDDataLib.OSDData data, Boolean dataValid)
        {            
            if(dataValid)
            {
                if (osdAltitudeStart == short.MinValue)
                {
                    osdAltitudeStart = data.Altimeter;
                }
                OSDDataLib.OSDData newData = data;

                data.Altimeter = (short)(data.Altimeter - osdAltitudeStart);

                copterData.osdBuffer.Enqueue(new KeyValuePair<DateTime, OSDDataLib.OSDData>(DateTime.Now, data));
                btStartOSDData.BackColor = SystemColors.Control;
            }
            else
            {
                btStartOSDData.BackColor = Color.Yellow;       
            }
            requestOSDDone = true;
           
        }

        private void timerUpdateOSDData_Tick(object sender, EventArgs e)
        {
            if (control != null && requestOSDDone)
            {
                if(0==control.requestReadOSDData())
                {
                    requestOSDDone = false;
                }                
            }            
        }

        private DateTime startTime = DateTime.Now;
        private DateTime lastLogTime = DateTime.Now;
        private String protocol_dir_name=null;

        private void LogData(String protocol_dir_name)
        {
            

            if(copterData.osdBuffer.Count>0)
            {
                if (!File.Exists(protocol_dir_name + "\\osdLog.csv"))
                {
                    // Create a file to write to. 
                    using (StreamWriter sw = File.CreateText(protocol_dir_name + "\\osdLog.csv"))
                    {
                        sw.WriteLine("Time;Altitude;Nick;Roll;Gas;");
                        sw.Close();
                    }

                }
                else
                {
                    using (StreamWriter sw = File.AppendText(protocol_dir_name + "\\osdLog.csv"))
                    {
                        var newOSDEntrys = copterData.osdBuffer.Where(key => key.Key > lastLogTime);
                        foreach (var newEntry in newOSDEntrys)
                        {
                            sw.WriteLine((newEntry.Key - startTime).TotalMilliseconds.ToString() + ";" +
                                        newEntry.Value.Altimeter.ToString() + ";" +
                                        newEntry.Value.AngleNick.ToString() + ";" +
                                        newEntry.Value.AngleRoll.ToString() + ";" +
                                        newEntry.Value.Gas.ToString() + ";"

                                        );
                        }

                        sw.Close();
                    }
                }
            }
           

            if(copterData.visualBuffer.Count>0)
            {
                if (!File.Exists(protocol_dir_name + "\\visualLog.csv"))
                {
                    // Create a file to write to. 
                    using (StreamWriter sw = File.CreateText(protocol_dir_name + "\\visualLog.csv"))
                    {
                        sw.WriteLine("Time;Altitude;Angle;RelativeAltitude;OffsetX;OffsetY;");
                        sw.Close();
                    }

                }
                else
                {
                    using (StreamWriter sw = File.AppendText(protocol_dir_name + "\\visualLog.csv"))
                    {
                        var newOSDEntrys = copterData.visualBuffer.Where(key => key.Key > lastLogTime);
                        foreach (var newEntry in newOSDEntrys)
                        {
                            sw.WriteLine((newEntry.Key - startTime).TotalMilliseconds.ToString() + ";" +
                                        newEntry.Value.Altitude.ToString() + ";" +
                                        newEntry.Value.Angle.ToString() + ";" +
                                        newEntry.Value.RelativeAltitude.ToString() + ";" +
                                        newEntry.Value.Offset.X.ToString() + ";" +
                                        newEntry.Value.Offset.Y.ToString() + ";"
                                        );
                        }

                        sw.Close();
                    }
                }
            }

            if(copterData.radarBuffer.Count>0)
            {
                if (!File.Exists(protocol_dir_name + "\\ultrasonicLog.csv"))
                {
                    // Create a file to write to. 
                    using (StreamWriter sw = File.CreateText(protocol_dir_name + "\\ultrasonicLog.csv"))
                    {
                        sw.WriteLine("Time;Down;Front;Left;Rear;Right;Up;");
                        sw.Close();
                    }

                }
                else
                {
                    using (StreamWriter sw = File.AppendText(protocol_dir_name + "\\ultrasonicLog.csv"))
                    {                        
                        var newOSDEntrys = copterData.radarBuffer.Where(key => key.Key > lastLogTime);
                        foreach (var newEntry in newOSDEntrys)
                        {
                            sw.WriteLine((newEntry.Key - startTime).TotalMilliseconds.ToString() + ";" +
                                        newEntry.Value.distDown.ToString() + ";" +
                                        newEntry.Value.distFront.ToString() + ";" +
                                        newEntry.Value.distLeft.ToString() + ";" +
                                        newEntry.Value.distRear.ToString() + ";" +
                                        newEntry.Value.distRight.ToString() + ";" +
                                        newEntry.Value.distUp.ToString() + ";"
                                        );
                        }

                        sw.Close();
                    }
                }
            }
            lastLogTime = DateTime.Now;
        }

        private void timerUpdateGraph_Tick(object sender, EventArgs e)
        {
            if (graph.Visible)
            {
                graph.Invoke((MethodInvoker)delegate
                {
                    if (copterData.osdBuffer.Count>0)
                    {
                        var latestOSD = copterData.osdBuffer.ToArray()[copterData.osdBuffer.Count - 1];
                        graph.AddXY(entryOSD, latestOSD.Key, latestOSD.Value.Altimeter);

                       
                    }

                    if (copterData.radarBuffer.Count > 0)
                    {
                        var latestRadar = copterData.radarBuffer.ToArray()[copterData.radarBuffer.Count - 1];
                        graph.AddXY(entryUltraSonic, latestRadar.Key, latestRadar.Value.distDown);
                    }

                    if (copterData.visualBuffer.Count > 0)
                    {
                        var latestVisual = copterData.visualBuffer.ToArray()[copterData.visualBuffer.Count-1];
                        if(latestVisual.Value.Altitude < 1000000)
                        {
                            graph.AddXY(entryAltitudePLatformFC, latestVisual.Key, latestVisual.Value.Altitude);
                        }
                        
                    }

                    if (copterData.visualBuffer2.Count > 0)
                    {
                        var latestVisual = copterData.visualBuffer2.ToArray()[copterData.visualBuffer2.Count - 1];
                        if (latestVisual.Value.Altitude < 1000000)
                        {
                            graph.AddXY(entryAltitudePLatformGP, latestVisual.Key, latestVisual.Value.Altitude);
                        }

                    }

                    graph.updateGraph(DateTime.Now);
                });

            }

            //write values to file
            if (protocol_dir_name == null)
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                protocol_dir_name = path + "Protocol" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                Directory.CreateDirectory(protocol_dir_name);
            }


            if (dataWrite)
            {

                if (visualControl != null)
                    visualControl.setVideoLogging(true, protocol_dir_name);

                LogData(protocol_dir_name);
            } 
            else
            {
                if(visualControl!=  null)
                    visualControl.setVideoLogging(false, protocol_dir_name);
            }
        }

        private void btResetOSD_Click(object sender, EventArgs e)
        {
            osdAltitudeStart = Int16.MinValue;
        }

        private void tbOSDUpdateInterval_TextChanged(object sender, EventArgs e)
        {
            if (!(Convert.ToInt32(tbOSDUpdateInterval.Text) < 1)) timerUpdateOSDData.Interval = Convert.ToInt32(tbOSDUpdateInterval.Text);
            else timerUpdateOSDData.Interval = 100;
        }

        private void tbVIdeopdateInterval_TextChanged(object sender, EventArgs e)
        {
            if (!(Convert.ToInt32(tbVIdeopdateInterval.Text) < 1)) timerUpdateValuesVisual.Interval = Convert.ToInt32(tbVIdeopdateInterval.Text);
            else timerUpdateValuesVisual.Interval = 100;
        }

        private void timerUpdateValuesVisual2_Tick(object sender, EventArgs e)
        {
            str_VisualData visData = visualControl2.doDetection();

            if (!visData.DataVlid)
            {
                butVideoStart.Invoke((MethodInvoker)delegate { butVideoStart.BackColor = Color.Yellow; });
            }
            else
            {
                butVideoStart.Invoke((MethodInvoker)delegate { butVideoStart.BackColor = SystemColors.Control; });
            }

            copterData.visualBuffer2.Enqueue(new KeyValuePair<DateTime, str_VisualData>(DateTime.Now, visData));
        }

        private void btShowVideoCalibration_Click(object sender, EventArgs e)
        {
            if(callibration== null)
            {
                callibration = new frmCalibrate(visualControl, actualCalibration);
            }

            if (!callibration.Visible)
            {
                try
                {
                    callibration.Show();
                    btShowVideoCalibration.Text = "Kalibrierung beenden";
                }
                catch (Exception excep)
                {
                    MessageBox.Show(excep.Message);
                }
            }
            else
            {
                try
                {
                    callibration.Hide();
                    btShowVideoCalibration.Text = "Kalibrierung starten";
                    callibration = null;
                }
                catch (Exception excep)
                {
                    MessageBox.Show(excep.Message);
                }
            }
        }

        private void comboCameraType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboCameraType.SelectedIndex)
            {
                case 0:
                    {
                        //FlyCam
                        actualCalibration = calibrationData_FlyCam;
                    }                    
                    break;
                case 1:
                    {
                        //GoPro
                        actualCalibration = calibrationData_GoPro;
                    }
                    break;

                default:
                    break;
            }
        }

        private void btReplay_Click(object sender, EventArgs e)
        {
            string startupPath = AppDomain.CurrentDomain.BaseDirectory;
            CopterData replayData = new CopterData(Int16.MaxValue);

            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
             
                try
                { 

                dialog.Description = "Open a folder which contains the recorded data";
                dialog.ShowNewFolderButton = false;
                dialog.SelectedPath = startupPath;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    //read osd data
                    if (File.Exists(dialog.SelectedPath + "\\osdLog.csv"))
                    {
                        string[] lines = File.ReadAllLines(dialog.SelectedPath + "\\osdLog.csv");
                        for (int i = 1; i < lines.Length; i++)
                        {
                            string[] entrys = lines[i].Split(';');

                            if (entrys.Length >= 5)
                            {
                                OSDDataLib.OSDData osd = new OSDDataLib.OSDData();
                                osd.Altimeter = short.Parse(entrys[1]);
                                osd.AngleNick = sbyte.Parse(entrys[2]);
                                osd.AngleRoll = sbyte.Parse(entrys[3]);
                                osd.Gas = byte.Parse(entrys[4]);


                                KeyValuePair<DateTime, OSDDataLib.OSDData> newEntry = new KeyValuePair<DateTime, OSDDataLib.OSDData>(new DateTime((long)double.Parse(entrys[0]) * 10000), osd);

                                replayData.osdBuffer.Enqueue(newEntry);
                            }

                        }
                    }

                    //read visual data
                    if (File.Exists(dialog.SelectedPath + "\\visualLog.csv"))
                    {
                        string[] lines = File.ReadAllLines(dialog.SelectedPath + "\\visualLog.csv");
                        for (int i = 1; i < lines.Length; i++)
                        {
                            string[] entrys = lines[i].Split(';');

                            if (entrys.Length >= 6)
                            {
                                str_VisualData visual = new str_VisualData();
                                visual.Altitude = float.Parse(entrys[1]);
                                visual.Angle = float.Parse(entrys[2]);
                                visual.RelativeAltitude = float.Parse(entrys[3]);
                                visual.Offset.X = float.Parse(entrys[4]);
                                visual.Offset.Y = float.Parse(entrys[5]);


                                KeyValuePair<DateTime, str_VisualData> newEntry = new KeyValuePair<DateTime, str_VisualData>(new DateTime((long)double.Parse(entrys[0]) * 10000), visual);

                                replayData.visualBuffer.Enqueue(newEntry);
                            }

                        }
                    }

                    //read ultrasonic data
                    if (File.Exists(dialog.SelectedPath + "\\ultrasonicLog.csv"))
                    {
                        string[] lines = File.ReadAllLines(dialog.SelectedPath + "\\ultrasonicLog.csv");
                        for (int i = 1; i < lines.Length; i++)
                        {
                            string[] entrys = lines[i].Split(';');

                            if (entrys.Length >= 7)
                            {
                                str_RadarData radar = new str_RadarData();
                                radar.distDown = int.Parse(entrys[1]);
                                radar.distFront = int.Parse(entrys[2]);
                                radar.distLeft = int.Parse(entrys[3]);
                                radar.distRear = int.Parse(entrys[4]);
                                radar.distRight = int.Parse(entrys[5]);
                                radar.distUp = int.Parse(entrys[6]);


                                KeyValuePair<DateTime, str_RadarData> newEntry = new KeyValuePair<DateTime, str_RadarData>(new DateTime((long)double.Parse(entrys[0]) * 10000), radar);

                                replayData.radarBuffer.Enqueue(newEntry);
                            }

                        }
                    }



                    graph = new RealTimeData(replayData);
                    graph.Show();
                }
                }
                catch(Exception ex)
                {
                        MessageBox.Show(ex.Message);
                    }

                
            }
        }

      

    }
#endregion
}
