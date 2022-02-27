using System.Collections.Generic;
using System.Xml.Serialization;

namespace SimsigImporterLib.Models
{
    /// <summary>
    /// A decision is a randomnly selected choice which can drive whether trains enter or what activities they do e.g. only change
    /// loco on Saturdays
    /// </summary>
    public class Decision
    {
        /// <summary>
        /// Gets or sets the id/name of the decision
        /// </summary>
        [XmlAttribute]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the description of this decision set
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the list of choices for this decision
        /// </summary>
        public List<Choice> Choices { get; set; }
    }
}
