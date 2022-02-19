namespace SimsigImporterLib.Models
{
    public class SeedGroup
    {
        /// <summary>
        /// Guid identifier for this seed group
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Optional start time. If not set, midnight is used (00:00)
        /// </summary>
        public int? StartTime { get; set; }

        /// <summary>
        /// Optional title of the seed group
        /// </summary>
        public string Title { get; set; }
    }
}
