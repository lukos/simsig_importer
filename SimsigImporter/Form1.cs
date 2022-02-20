using SimsigImporterLib;
using SimsigImporterLib.Helpers;
using SimsigImporterLib.Models;
using System;
using System.Drawing;
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
            exporter.Export(timeTable, path);
            MessageBox.Show("Timetable exported", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

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

        public void LogWarning(string message)
        {
            progress.AddMessage(message, Color.DarkOrange);
        }

        public void LogError(string message)
        {
            progress.AddMessage(message, Color.DarkRed);
        }

        public void LogInfo(string message)
        {
            progress.AddMessage(message, Color.Black);
        }
    }
}
