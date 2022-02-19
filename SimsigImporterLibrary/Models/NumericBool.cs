using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SimsigImporterLib.Models
{
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

        public NumericBool(bool value)
        {
            this.value = value;
        }

        public static implicit operator NumericBool(bool v) => new NumericBool(v);

        public override string ToString()
        {
            return value ? "-1" : "0";
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(ToString());
        }
    }
}
