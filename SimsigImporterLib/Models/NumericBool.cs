namespace SimsigImporterLib.Models
{
    public class NumericBool
    {
        public static NumericBool True => new NumericBool(true);
        public static NumericBool False => new NumericBool(false);

        private bool value;

        public NumericBool(bool value)
        {
            this.value = value;
        }

        public static implicit operator NumericBool(bool v) => new NumericBool(v);

        public override string ToString()
        {
            return value ? "-1" : null;
        }
    }
}
