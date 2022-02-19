using System.Xml;
using System.Xml.Serialization;

namespace SimsigImporterLib.Models
{
    public class TimetableHeader
    {
        [XmlAttribute]
        public string ID { get; set; }

        [XmlAttribute]
        public string Version { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int StartTime { get; set; }

        public int FinishTime { get; set; }

        public int VMajor { get; set; }

        public int VMinor { get; set; } = 1;

        public int VBuild { get; set; }

        public string TrainDescriptionTemplate { get; set; }

        public string SeedGroupSummary { get; set; } = "";

        public string ScenarioOptions { get; set; } = "NSIMERA,1";
    }
}
