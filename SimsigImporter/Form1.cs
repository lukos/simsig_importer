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
        private SimSigTimetable timeTable = new SimSigTimetable();
        private ProgressDialog progress = new ProgressDialog();
        private Guid saveDialogGuid = Guid.NewGuid();
        private Guid openDialogGuid = Guid.NewGuid();

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle the export button click by attempting to write the current timetable to user-selected file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_Click(object sender, EventArgs e)
        {
            timeTable.ID = comboSim.SelectedItem.ToString();
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

            }
            else
            {
                // Specific day so clone and filter the timetables from the main timetable
                var dayCode = comboDays.SelectedItem.ToString().ToDayCode();
                var filteredTt = timeTable.Clone();
                filteredTt.Timetables = new List<Timetable>(timeTable.Timetables.Where(tt => tt.Days.ContainsDay(dayCode)));
                if (filteredTt.Timetables.Count == 0)
                {
                    MessageBox.Show("No workings to export for selected day", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    exporter.Export(filteredTt, path);
                    MessageBox.Show("Timetable exported", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
    }
}
