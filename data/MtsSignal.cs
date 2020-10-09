using System.Collections.Generic;

namespace SignalGenerator.Data
{
    public class MtsSignal
    {
        public ushort Number { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }

        public MtsSignal()
        {
            Number = 0;
            Name = "";
            Value = 0;
        }

        public MtsSignal(ushort number, string name, double value)
        {
            Number = number;
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"[{Number}] = {Value} ({Name})";
        }
    }
}