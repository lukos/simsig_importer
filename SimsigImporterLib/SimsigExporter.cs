using SimsigImporterLib.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SimsigImporterLib
{
    public class SimsigExporter
    {
        public void Export(SimSigTimetable data, string fileName)
        {

        }

        private void SerializeElements(SimSigTimetable timetable)
        {
            var headerStream = new MemoryStream();
            TextWriter writer = new StreamWriter(headerStream);
            writer.Write(ToXml(timetable));
            writer.Flush();

            var archive = ZipFile.Open(@"C:\temp\Wolverhampton.wtt", ZipArchiveMode.Create);
            var entry = archive.CreateEntry("TimetableHeader.xml", CompressionLevel.Optimal);
            headerStream.Position = 0;
            headerStream.CopyTo(entry.Open());

            archive.Dispose();
            writer.Close();
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
