using SimsigImporterLib;
using SimsigImporterLib.Helpers;
using SimsigImporterLib.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SimsigImporter
{
    public partial class Form1 : Form
    {
        private ImporterSettings settings = new ImporterSettings();
        private SimSigTimetable timeTable = new SimSigTimetable();
        private ProgressDialog progress = new ProgressDialog();
        private Guid saveDialogGuid = Guid.NewGuid();
        private Guid openDialogGuid = Guid.NewGuid();

        public Form1()
        {
            InitializeComponent();
            PopulateSimDropdown();
        }

        /// <summary>
        /// Handle the export button click by attempting to write the current timetable to user-selected file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            timeTable.ID = comboSim.SelectedValue.ToString();
            timeTable.Version = textVersion.Text;
            timeTable.Name = textName.Text;
            timeTable.Description = textDesc.Text;
            timeTable.StartTime = textStart.Text.ToSimsigTime();
            timeTable.FinishTime = textEnd.Text.ToSimsigTime();
            timeTable.TrainDescriptionTemplate = textTemplate.Text;

            string path;
            using (var fileChooser = new SaveFileDialog())
            {
                fileChooser.Filter = "Simsig files (*.wtt)|*.wtt|All files (*.*)|*.*";
                fileChooser.RestoreDirectory = true;
                fileChooser.AddExtension = true;
                fileChooser.DefaultExt = "wtt";
                fileChooser.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
                fileChooser.OverwritePrompt = true;

                if (fileChooser.ShowDialog() == DialogResult.OK)
                {
                    path = fileChooser.FileName;
                }
                else
                {
                    return;
                }
            }

            var exporter = new SimsigExporter();

            if ( comboDays.SelectedItem.ToString() == "All - single TT" )
            {
                exporter.Export(timeTable, path);
                MessageBox.Show("Timetable exported", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (comboDays.SelectedItem.ToString() == "All - separate TTs")
            {
                new List<string> { "SUN", "M", "T", "W", "Th", "F", "S" }.ForEach(day =>
                    ExportTimetableDay(exporter, timeTable, day, path.Replace(".wtt", " - " + day + ".wtt"))
                );
                MessageBox.Show("Timetables exported", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Specific day so clone and filter the timetables from the main timetable
                var dayCode = comboDays.SelectedItem.ToString().ToDayCode();
                ExportTimetableDay(exporter, timeTable, dayCode, path);
                MessageBox.Show("Timetable exported", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ExportTimetableDay(SimsigExporter exporter, SimSigTimetable timetable, string dayCode, string path)
        {
            var filteredTt = timeTable.Clone();
            filteredTt.Timetables = new List<Timetable>(timeTable.Timetables.Where(tt => tt.Days.ContainsDay(dayCode)));
            if (filteredTt.Timetables.Count == 0)
            {
                MessageBox.Show($"No workings to export for day {dayCode}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                exporter.Export(filteredTt, path);
            }
        }

        /// <summary>
        /// Click handler for adding an input spreadsheet with simplified timetable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSpreadsheet_Click(object sender, EventArgs e)
        {
            // Show file chooser and send to importer
            string path;
            using (var fileChooser = new OpenFileDialog())
            {
                if (fileChooser.ShowDialog() == DialogResult.OK)
                {
                    path = fileChooser.FileName;
                }
                else
                {
                    return;
                }
            }

            var importer = new SpreadsheetHelper(LogInfo, LogWarning, LogError);

            progress = new ProgressDialog();
            progress.Show();

            var result = importer.Import(path);

            LogInfo("=========================");
            LogInfo("...All finished...");
            LogInfo("=========================");
            progress.EnableButton();
            if ( result == null )
            {

            }
            else
            {
                timeTable = result;
            }
        }

        /// <summary>
        /// Delegate for passing messages between the worker and the display dialog
        /// </summary>
        /// <param name="message"></param>
        public void LogWarning(string message)
        {
            progress.AddMessage(message, Color.DarkOrange);
        }

        /// <summary>
        /// Delegate for passing messages between the worker and the display dialog
        /// </summary>
        public void LogError(string message)
        {
            progress.AddMessage(message, Color.DarkRed);
        }

        /// <summary>
        /// Delegate for passing messages between the worker and the display dialog
        /// </summary>
        public void LogInfo(string message)
        {
            progress.AddMessage(message, Color.Black);
        }

        private void PopulateSimDropdown()
        {
            var itemsDict = new Dictionary<string, string>(100);

            itemsDict.Add("Aston", "aston");
            itemsDict.Add("Birmingham New Stree", "birmingham new street");
            itemsDict.Add("Brighton", "brighton");
            itemsDict.Add("Cardiff", "cardiff");
            itemsDict.Add("Cardiff Valleys", "cardiffvalleys");
            itemsDict.Add("Cardiff Vale of Glamorgan", "cardiffvog");
            itemsDict.Add("Carlisle", "carlisle");
            itemsDict.Add("Cathcart", "cathcart");
            itemsDict.Add("Cheshire Lines", "cheshirelines");
            itemsDict.Add("Chester", "chester");
            itemsDict.Add("Chicago Loop", "chicagoloop");
            itemsDict.Add("Coventry", "coventry");
            itemsDict.Add("Cowlairs", "cowlairs");
            itemsDict.Add("Central Scotland", "cscot");
            itemsDict.Add("Derby", "derby");
            itemsDict.Add("Doncaster North", "doncasternorth");
            itemsDict.Add("East Coastway", "eastcoastway");
            itemsDict.Add("Edge Hill", "edgehill");
            itemsDict.Add("Edinburgh", "edinburgh");
            itemsDict.Add("Exeter", "exeter");
            itemsDict.Add("Feltham", "feltham");
            itemsDict.Add("Fenchurch Street", "fenchurch");
            itemsDict.Add("Hereford", "hereford");
            itemsDict.Add("Hong Kong East", "hongkongeast");
            itemsDict.Add("Horsham", "horsham");
            itemsDict.Add("Huddersfield", "huddersfield");
            itemsDict.Add("Hunts Cross", "huntscross");
            itemsDict.Add("Huyton", "huyton");
            itemsDict.Add("Kings Cross", "kingsx");
            itemsDict.Add("Lancing", "lancing");
            itemsDict.Add("Leamington Spa", "leamington");
            itemsDict.Add("Leeds Ardsley", "leedsa");
            itemsDict.Add("Leeds East/West", "leedsew");
            itemsDict.Add("Leeds Northwest", "leedsnw");
            itemsDict.Add("Liverpool Lime Street", "limest");
            itemsDict.Add("Liverpool Street", "livst");
            itemsDict.Add("Llangollen", "llangollen");
            itemsDict.Add("London Bridge", "londonbridgeasc");
            itemsDict.Add("London Bridge Mini", "londonbridgemini");
            itemsDict.Add("London, Tilbury and Southend", "lts");
            itemsDict.Add("Maidstone East", "maideast");
            itemsDict.Add("Manchester East", "manchestereast");
            itemsDict.Add("Manchester North", "manchesternorth");
            itemsDict.Add("Manchester Piccadilly", "manchesterpiccadilly");
            itemsDict.Add("Manchester South", "manchestersouth");
            itemsDict.Add("Marylebone", "marylebone");
            itemsDict.Add("Moss Vale NSW", "moss_vale_nsw");
            itemsDict.Add("Motherwell", "motherwell");
            itemsDict.Add("North East Scotland", "nescot");
            itemsDict.Add("North East Wales", "newales");
            itemsDict.Add("Newport", "newport");
            itemsDict.Add("North Wales Coast", "north_wales_coast");
            itemsDict.Add("Oxford", "oxford");
            itemsDict.Add("Oxted", "oxted");
            itemsDict.Add("Paisley", "paisley");
            itemsDict.Add("Peak District", "peakdistrict");
            itemsDict.Add("Penzance", "penzance");
            itemsDict.Add("Peterborough", "peterborough");
            itemsDict.Add("Plymouth", "plymouth");
            itemsDict.Add("Portsmouth", "portsmouth");
            itemsDict.Add("Post Talbot", "porttalbot");
            itemsDict.Add("Royston", "royston");
            itemsDict.Add("Rugby Centre", "rugbycentre");
            itemsDict.Add("Rugby North", "rugbynorth");
            itemsDict.Add("Rugby South", "rugbysouth");
            itemsDict.Add("Salisbury", "salisbury");
            itemsDict.Add("Saltley", "saltley");
            itemsDict.Add("Sandhills", "sandhills");
            itemsDict.Add("Sheffield", "sheffield");
            itemsDict.Add("Shrewsbury", "shrewsbury");
            itemsDict.Add("Stafford", "stafford");
            itemsDict.Add("Staffordshire", "staffordshire");
            itemsDict.Add("Stockport", "stockport");
            itemsDict.Add("Strathfield", "strathfield");
            itemsDict.Add("Swindon", "swindid");
            itemsDict.Add("Three Bridges", "threebridges");
            itemsDict.Add("Tyneside", "tyneside");
            itemsDict.Add("Victoria", "victoria");
            itemsDict.Add("Victoria Central", "victoriac");
            itemsDict.Add("Victoria South Eastern", "victoriaeast");
            itemsDict.Add("West Anglia", "warm");
            itemsDict.Add("Warrington", "warringtonpsb");
            itemsDict.Add("Waterloo", "waterloo");
            itemsDict.Add("Watford Junction", "watfordjn");
            itemsDict.Add("Wembley", "wembley");
            itemsDict.Add("Wembley Sub", "wembleysub");
            itemsDict.Add("Westbury", "westbury");
            itemsDict.Add("West Hampstead", "westhampstead");
            itemsDict.Add("West Yorkshire", "westyorkshire");
            itemsDict.Add("Wigan Wallgate", "wigan");
            itemsDict.Add("Wimbledon", "wimbledon");
            itemsDict.Add("Wolverhampton", "wolverhampton");
            itemsDict.Add("York North/South", "yorkns");

            comboSim.DataSource = new BindingSource(itemsDict, null);
            comboSim.DisplayMember = "Key";
            comboSim.ValueMember = "Value";
            if (settings.SelectedSim.IsPresent() && itemsDict.ContainsValue(settings.SelectedSim))
            {
                comboSim.SelectedValue = settings.SelectedSim;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var value = ((KeyValuePair<string, string>)comboSim.SelectedItem).Value;
            settings.SelectedSim = value;
            settings.Save();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will remove any imported spreadsheets. Do you want to continue?", "Confirm", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                timeTable = new SimSigTimetable();
            }
        }
    }
}
