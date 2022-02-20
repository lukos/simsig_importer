using System.Collections.Generic;

namespace SimsigImporterLib.Models
{
    /// <summary>
    /// Represents a single Simsig working. In general this equates to a single train but in many cases, a single train
    /// could enter as one working and change to another inside the sim. If a train leaves the sim and re-enters later,
    /// these need to be represented by two separate "Timetables" even though they might be the same train IRL with the same
    /// headcode.
    /// </summary>
    public class Timetable
    {
        /// <summary>
        /// Gets or sets the ID of this working e.g. the headcode 1M77
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the unique ID of this working, which can be used to distinguish multiple workings with the same headcode/ID e.g. 1M77-3
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// Gets or sets the display text for this working
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The departure time in seconds from midnight
        /// </summary>
        public int DepartTime { get; set; }

        /// <summary>
        /// Gets or sets the TIPLOC entry point code if not seeded or if it doesn't start from a previous working
        /// </summary>
        public string EntryPoint { get; set; }

        /// <summary>
        /// The locations data. Note the locations does not include the entry point
        /// </summary>
        public List<Trip> Trips { get; set; } = new List<Trip>();

        /// <summary>
        /// Gets or sets the unique code that links this working to its train type
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the optional seeding point for the train
        /// </summary>
        public string SeedPoint { get; set; }

        /// <summary>
        /// If seeded, this is the distance before the seeded signal in metres. All timetables have this, however and
        /// it defaults to 15m.
        /// </summary>
        public int SeedingGap { get; set; } = 15;

        public int AccelBrakeIndex { get; set; }

        /// <summary>
        /// Gets or sets the precentage of time that this train runs, if service options says it only runs "as required"
        /// </summary>
        public int AsRequiredPercent { get; set; } = 50;

        /// <summary>
        /// Gets or sets the max speed in mph as copied from the train type (which can be overridden here)
        /// </summary>
        public int MaxSpeed { get; set; }

        /// <summary>
        /// Gets or sets the speed class options. This is a bit field where normal classes start from LSB and sim-specific ones
        /// start from the MSB of nibble 7. Copied from train type and can be overridden
        /// </summary>
        public long SpeedClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this train uses freight speed limits. Use -1 for true and 0 for false!
        /// Copied from train type and can be overridden
        /// </summary>
        public NumericBool IsFreight { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether this train can use goods lines. Use -1 for true and 0 for false!
        /// Copied from train type and can be overridden
        /// </summary>
        public NumericBool CanUseGoodsLines { get; set; } = false;

        /// <summary>
        /// Gets or sets the train length in metres. Copied from train type and can be overridden
        /// </summary>
        public int TrainLength { get; set; }

        /// <summary>
        /// Gets or sets a code to represent the electrification options. Copied from train type and can be overridden
        /// </summary>
        /// <remarks>This is a string made up from the following characters:
        /// O = overhead AC
        /// 3 = 3rd rail DC
        /// 4 = 4th rail DC
        /// D = diesel
        /// V = overhead DC
        /// T = Tramway
        /// X1/2/3/4 = sim settings</remarks>
        public string Electrification { get; set; }

        /// <summary>
        /// The type of traction as copied from the train type, that this working enters with (since it might swap in-sim)
        /// </summary>
        public string StartTraction { get; set; }

        #region Origin and destination - used for descriptions

        /// <summary>
        /// Gets or sets the name of the original location e.g. Euston
        /// </summary>
        public string OriginName { get; set; }

        /// <summary>
        /// Gets or sets the name of the destination location e.g. Glasgow Central
        /// </summary>
        public string DestinationName { get; set; }

        /// <summary>
        /// Gets or sets the original departure time for this working
        /// </summary>
        public int? OriginTime { get; set; }

        /// <summary>
        /// Gets or sets the ultimate destination time for this working
        /// </summary>
        public int? DestinationTime { get; set; }

        /// <summary>
        /// Gets or sets the operator code for this service
        /// </summary>
        public string OperatorCode { get; set; }

        /// <summary>
        /// Gets or sets free-form notes for this working
        /// </summary>
        public string Notes { get; set; }

        #endregion
    }
}
