using System;
using System.Xml.Serialization;

namespace SimsigImporterLib.Models
{
    /// <summary>
    /// The bit field for the speed classes long
    /// </summary>
    public enum SpeedClasses
    {
        EPSE = 1,
        EPSD = 2,
        HST = 4,
        EMU = 8,
        DMU = 16,
        SP = 32,
        CL67 = 64,
        MGR = 128,
        TGV = 256,
        LocoH = 512,
        Metro = 1024,
        CL442 = 2048,
        Tripcock = 4096,
        Steam = 8192,
        Sim1 = 16777216,
        Sim2 = 33554432,
        Sim3 = 67108864,
        Sim4 = 134217728,
    }

    /// <summary>
    /// Represents a train type including length, power and speed characteristics
    /// </summary>
    public class TrainCategory
    {
        /// <summary>
        /// Gets or sets a unique id for the train category. 8 hex characters by default
        /// </summary>
        [XmlAttribute]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the id that the user used in the train types table to cross reference to trains in the timetable sheet
        /// </summary>
        [XmlIgnore]
        public string SheetId { get; set; }

        /// <summary>
        /// Gets or sets the display description for this train type
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the maximum speed in mph
        /// </summary>
        public int MaxSpeed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this train uses freight speed limits. Use -1 for true and 0 for false!
        /// </summary>
        public NumericBool IsFreight { get; set; } = NumericBool.False;

        /// <summary>
        /// Gets or sets a value indicating whether this train can use goods lines. Use -1 for true and 0 for false!
        /// </summary>
        public NumericBool CanUseGoodsLines { get; set; } = NumericBool.False;

        /// <summary>
        /// Gets or sets the train length in metres
        /// </summary>
        public int TrainLength { get; set; }

        /// <summary>
        /// Gets or sets a code to represent the electrification options
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
        /// Gets or sets the speed classes for this train. This is a bit field where normal classes start from LSB and sim-specific ones
        /// start from the MSB of nibble 7
        /// </summary>
        public long SpeedClass { get; set; }

        /// <summary>
        /// Gets or sets the acceleration rate from 0 to 4 from very low to very high
        /// </summary>
        public int AccelBrakeIndex { get; set; }

        /// <summary>
        /// Gets or sets the power to weight ratio from 0 (normal), 1 (Light) and 2 (heavy)
        /// </summary>
        public int PowerToWeightCategory { get; set; }

        /// <summary>
        /// Gets or sets any dwell times for this train
        /// </summary>
        public DwellTimes DwellTimes { get; set; }

        /// <summary>
        /// Converts the text from the spreadsheet into a simsig-specific value
        /// </summary>
        internal static string ParseElectrificationString(string value)
        {
            switch(value)
            {
                case "Overhead AC":
                    return "O";
                case "Diesel":
                    return "D";
                case "3rd Rail":
                    return "3";
                case "4th Rail":
                    return "4";
                case "Overhead DC":
                    return "V";
                default:
                    throw new NotImplementedException("No electrifican scheme that matches");
            }
        }

        /// <summary>
        /// Parses the train's power the weight as read-in from the spreadsheet
        /// </summary>
        /// <param name="value">The value to read</param>
        /// <returns>The power to weight ratio for the simsig timetable</returns>
        internal static int ParsePowerToWeight(string value)
        {
            switch (value)
            {
                case "Normal":
                    return 0;
                case "Light":
                    return 1;
                case "Heavy":
                    return 2;
                default:
                    throw new NotImplementedException("No power to weight value that matches");
            }
        }

        /// <summary>
        /// Parses the trains acceleration/braking index to convert it to the correct simsig index
        /// </summary>
        /// <param name="value">The value to parse</param>
        /// <returns>The correct index for the simsig timetable</returns>
        internal static int ParseAccelBrakeIndex(string value)
        {
            switch(value)
            {
                case "Very low (slow freight)":
                    return 0;
                case "Low (standard freight)":
                    return 1;
                case "Medium (Intercity)":
                    return 2;
                case "High (Commuter)":
                    return 3;
                case "Very high (Light loco)":
                    return 4;
                default:
                    throw new NotImplementedException("No accel/brake value that matches");
            }
        }

        /// <summary>
        /// Parses the speed class to convert it to the correct simsig bit field
        /// </summary>
        /// <param name="value">The value to parse</param>
        /// <returns>A bit field for the given value</returns>
        /// <remarks>Note that this can only handle a single value, not a combination</remarks>
        internal static long ParseSpeedClass(string value)
        {
            switch (value)
            {
                case "EPS-E":
                    return 1;
                case "EPS-D":
                    return 1 << 1;
                case "HST":
                    return 1 << 2;
                case "EMU":
                    return 1 << 3;
                case "DMU":
                    return 1 << 4;
                case "SP":
                    return 1 << 5;
                case "CS (Cl.67)":
                    return 1 << 6;
                case "MGR":
                    return 1 << 7;
                case "TGV (Cl.373)":
                    return 1 << 8;
                case "Loco-H":
                    return 1 << 9;
                case "Metro":
                    return 1 << 10;
                case "WES (Cl.442)":
                    return 1 << 11;
                case "Tripcock":
                    return 1 << 12;
                case "Steam":
                    return 1 << 13;
                case "Sim 1":
                    return 1 << 25;
                case "Sim 2":
                    return 1 << 26;
                case "Sim 3":
                    return 1 << 27;
                case "Sim 4":
                    return 1 << 28;
                default:
                    throw new NotImplementedException("Unrecognised speed class");
            }
        }
    }
}
