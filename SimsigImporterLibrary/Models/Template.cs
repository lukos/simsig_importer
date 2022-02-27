using System.Collections.Generic;

namespace SimsigImporterLib.Models
{
    public class Template
    {
        /// <summary>
        /// Gets or sets the groups of locations from the template to create sheets from
        /// </summary>
        public Dictionary<string,TemplateLocation[]> Locations { get; set; }
    }
}
