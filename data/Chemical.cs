using Newtonsoft.Json;

namespace SignalGenerator.Data
{
    public class Chemical
    {
        public int Uid { get; set; }
        public string ElementName { get; set; }
        public string ElementSign { get; set; }
        public double ElementValue { get; set; }

        public Chemical()
        {
            Uid = 0;
            ElementName = "";
            ElementSign = "";
            ElementValue = 0;
        }

        public Chemical(int id)
        {
            Uid = id;
            ElementName = "";
            ElementSign = "";
            ElementValue = 0;
        }

        public Chemical(int id, string name, string sign, double value)
        {
            Uid = id;
            ElementName = name;
            ElementSign = sign;
            ElementValue = value;
        }

        /// <summary>
        /// Преобразование объекта в строку JSON
        /// </summary>
        /// <returns>JSON-файл, содержащий химический состав материала</returns>
        public override string ToString()
        {
            string res = JsonConvert.SerializeObject(this, Formatting.None);

            return res;
        }
    }
}