namespace SimsigImporterLib.Models
{
    /// <summary>
    /// Optional dwell times, which are minimum times for various operations
    /// </summary>
    public class DwellTimes
    {
        /// <summary>
        /// Gets or sets the minimum time in seconds that this train takes to pull away from a red signal
        /// </summary>
        public int RedSignalMoveOff { get; set; }

        /// <summary>
        /// Gets or sets the minimum time for the train to move away from a station
        /// </summary>
        public int StationForward { get; set; }

        /// <summary>
        /// Gets or sets the minimum time for the train to reverse at a station
        /// </summary>
        public int StationReverse { get; set; }

        /// <summary>
        /// Gets or sets the minimum time in seconds that the train takes to terminate/unload
        /// </summary>
        public int TerminateForward { get; set; }

        /// <summary>
        /// Gets or sets the minimum time in seconds that the train takes to terminate/unload
        /// </summary>
        public int TerminateReverse { get; set; }

        /// <summary>
        /// Gets or sets the minimum time to join this train
        /// </summary>
        public int Join { get; set; }

        /// <summary>
        /// Gets or sets the minimum time to divide this train
        /// </summary>
        public int Divide { get; set; }

        /// <summary>
        /// Gets or sets the minimum time it takes for a crew change
        /// </summary>
        public int CrewChange { get; set; }
    }
}
