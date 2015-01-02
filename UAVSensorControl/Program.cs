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
 *> Main - Haupteinstiegspunkt
 *>
 *>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace UAVSensorControl
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Inside Main!\n");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmUAVSensorControl());
        }
    }
}
