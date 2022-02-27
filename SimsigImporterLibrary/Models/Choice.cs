using System.Xml.Serialization;

namespace SimsigImporterLib.Models
{
    /// <summary>
    /// One of the choices available in a decision
    /// </summary>
    public class Choice
    {
        /// <summary>
        /// Gets or sets the id/name of this choice
        /// </summary>
        [XmlAttribute]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the optional annoucement made in the console when a choice is selected
        /// </summary>
        public string Announcement { get; set; }
    }
}
