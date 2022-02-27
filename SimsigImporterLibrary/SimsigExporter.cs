using SimsigImporterLib.Models;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SimsigImporterLib
{
    /// <summary>
    /// Helper class for exporting timetables
    /// </summary>
    public class SimsigExporter
    {
        /// <summary>
        /// Creates the SimSig timetable file which is a zip containing two files
        /// </summary>
        /// <param name="timetable">The timetable data to write</param>
        /// <param name="fileName">The filename to write to</param>
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
            timetable.Decisions = null;

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

        /// <summary>
        /// Helper method which removes the XML header when serializing which is not required for SimSig
        /// </summary>
        /// <param name="obj">The item hierarchy to serialize</param>
        /// <returns>The XML string to write to file</returns>
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

        /// <summary>
        /// Create the data structure to use for our day of the week decision.
        /// </summary>
        /// <returns>A decision containing the DOTW</returns>
        public static Decision CreateDotwDecision()
        {
            return 
                new Decision{ ID = "DOTW", Description = "Day of the week", Choices = new List<Choice>{
                    new Choice{ID = "MON", Announcement = "It is Monday" },
                    new Choice{ID = "TUE", Announcement = "It is Tuesday" },
                    new Choice{ID = "WED", Announcement = "It is Wednesday" },
                    new Choice{ID = "THU", Announcement = "It is Thursday" },
                    new Choice{ID = "FRI", Announcement = "It is Friday" },
                    new Choice{ID = "SAT", Announcement = "It is Saturday" },
                    new Choice{ID = "SUN", Announcement = "It is Sunday" }
                }};
        }
    }
}
