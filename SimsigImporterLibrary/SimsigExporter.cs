using SimsigImporterLib.Models;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SimsigImporterLib
{
    public class SimsigExporter
    {
        public void Export(SimSigTimetable timetable, string fileName)
        {
            if ( File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            var archive = ZipFile.Open(fileName, ZipArchiveMode.Create);

            using (var headerStream = new MemoryStream())
            {
                using (TextWriter writer = new StreamWriter(headerStream))
                {
                    writer.Write(ToXml(timetable));
                    writer.Flush();

                    var entry = archive.CreateEntry("SavedTimetable.xml", CompressionLevel.Optimal);
                    headerStream.Position = 0;
                    using (var s = entry.Open())
                    {
                        headerStream.CopyTo(s);
                    }
                }
            }

            // Nullify these to remove them from the serialization
            timetable.SeedGroups = null;
            timetable.TrainCategories = null;
            timetable.Timetables = null;

            using (var headerStream = new MemoryStream())
            {
                using (TextWriter writer = new StreamWriter(headerStream))
                {
                    writer.Write(ToXml(timetable));
                    writer.Flush();

                    var entry = archive.CreateEntry("TimetableHeader.xml", CompressionLevel.Optimal);
                    headerStream.Position = 0;
                    using (var s = entry.Open())
                    {
                        headerStream.CopyTo(s);
                    }
                }
            }

            archive.Dispose();
        }

        internal static string ToXml(object obj)
        {
            string retval = null;
            if (obj != null)
            {
                StringBuilder sb = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(sb, new XmlWriterSettings() { OmitXmlDeclaration = true, Indent = true }))
                {
                    new XmlSerializer(obj.GetType()).Serialize(writer, obj);
                }
                retval = sb.ToString();
            }
            return retval;
        }
    }
}
