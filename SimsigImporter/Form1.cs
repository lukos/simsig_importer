using SimsigImporterLib;
using SimsigImporterLib.Helpers;
using SimsigImporterLib.Models;
using System;
using System.Windows.Forms;

namespace SimsigImporter
{
    public partial class Form1 : Form
    {
        private SimSigTimetable timeTable = new SimSigTimetable();
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
            
            var exporter = new SimsigExporter();
            exporter.Export(timeTable, "c:\\temp\\testexport.wtt");
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

            var importer = new SpreadsheetHelper(LogWarning, LogError);
            var result = importer.Import(path);
            if ( result == null )
            {

            }
            else
            {
                timeTable = result;
            }
        }

        public void LogWarning(string warning)
        {

        }

        public void LogError(string warning)
        {

        }
    }
}
