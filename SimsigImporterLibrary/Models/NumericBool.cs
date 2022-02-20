using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SimsigImporterLib.Models
{
    /// <summary>
    /// A special boolean type that maps to the values used in SimSig XML
    /// </summary>
    [Serializable]
    public class NumericBool : IXmlSerializable
    {
        public static NumericBool True => new NumericBool(true);
        public static NumericBool False => new NumericBool(false);

        private bool value;

        /// <summary>
        /// Needed for Serializer even though it won't be used!
        /// </summary>
        public NumericBool()
        {

        }

        /// <summary>
        /// Constructor from boolean source value
        /// </summary>
        /// <param name="value">The underlying boolean value</param>
        public NumericBool(bool value)
        {
            this.value = value;
        }

        /// <summary>
        /// Allow implicit casting from boolean to numeric bool
        /// </summary>
        /// <param name="v"></param>
        public static implicit operator NumericBool(bool v) => new NumericBool(v);

        /// <summary>
        /// Serialize this value to string
        /// </summary>
        /// <returns>A string representation of this value</returns>
        public override string ToString()
        {
            return value ? "-1" : "0";
        }

        /// <summary>
        /// Required for xml interface. Doesn't do anything
        /// </summary>
        /// <returns>An optional schema</returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Used when deserializing XML. Not used
        /// </summary>
        /// <param name="reader">The reader pointing to the XML document</param>
        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Used when serializing this value to XML
        /// </summary>
        /// <param name="writer">The writer to write to</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(ToString());
        }
    }
}
