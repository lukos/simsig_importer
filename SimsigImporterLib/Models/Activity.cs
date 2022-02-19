using System.Xml.Serialization;

namespace SimsigImporterLib.Models
{
    public enum Activities
    {
        Next = 0,
        DividesNewRear = 1,
        DividesNewFront = 2,
        Join = 3,
        DetachEngineRear = 4,
        DetachEngineFront = 5,
        DropCoachesRear = 6,
        DropCoachesFront = 7,
        PlatformShare = 9,
        CrewChange = 10,
    }

    /// <summary>
    /// Gets or sets an activity that takes place at a location like a crew change, "next" or split/join
    /// </summary>
    public class Activity
    {
        /// <summary>
        /// Gets or sets the code of the activity
        /// </summary>
        [XmlElement("Activity")]
        public int ActivityCode { get; set; }

        /// <summary>
        /// Gets the code of the train that relates to this specific activity
        /// </summary>
        public string AssociatedTrain { get; set; }

        /// <summary>
        /// Gets or sets the duration of the activity in seconds
        /// </summary>
        public int? ActivityDuration { get; set; }
    }
}
