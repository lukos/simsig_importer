using System.Collections.Generic;

namespace SimsigImporterLib.Models
{
    /// <summary>
    /// Represents an entry in the location tab. There are many things that can be set here but at its simplest,
    /// it will be a TIPLOC code, departure/pass time and some flags related to adjacent locations that help the
    /// simulator know whether to reverse
    /// </summary>
    public class Trip
    {
        /// <summary>
        /// Gets or sets the TIPLOC code for this location see http://www.railwaycodes.org.uk/crs/crs0.shtm
        /// </summary>
        public string Location { get; set; } 

        /// <summary>
        /// Gets or sets the arrival time for this location. Not used if only passing
        /// </summary>
        public int? ArrTime { get; set; }

        /// <summary>
        /// Gets or sets the departure or pass time. When importing from spreadsheet, the times might not be split
        /// out in a bold time entry and so the arrival time will need computing.
        /// </summary>
        public int? DepPassTime { get; set; }

        /// <summary>
        /// Gets or sets the path into the location, simulation specific e.g. Main or relief
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the line taken out of the location e.g. Main or Relief
        /// </summary>
        public string Line { get; set; }

        /// <summary>
        /// Gets or sets whether the time is only for passing and not departing.
        /// </summary>
        public NumericBool IsPassTime { get; set; }

        /// <summary>
        /// Gets or sets the optional "platform" for a location, which might be e.g. 1 but could also be UGL, location specific
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets any optional activities that take place at this location
        /// </summary>
        public List<Activity> Activities { get; set; }
    }
}
