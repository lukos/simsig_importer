namespace SimsigImporterLib.Models
{
    public class TemplateLocation
    {
        /// <summary>
        /// Gets or sets the tiploc code for this location
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the display name for this location
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets whether this location only applies in one direction can be "Up" or "Down"
        /// </summary>
        public string Direction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this location should be split into two rows on the spreadsheet for
        /// separate arr and dep times
        /// </summary>
        public bool ArrDep { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to include a platform row for this location in the spreadsheet
        /// </summary>
        public bool Plat { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to include a path (incoming line) for the location in the spreadsheet
        /// </summary>
        public bool Path { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to include a line (outgoing line) for the location in the spreadsheet
        /// </summary>
        public bool Line { get; set; } = false;
    }
}
