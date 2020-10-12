using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignalGenerator.Data
{
    public class Material
    {
        public int Uid { get; set; }
        public string Name { get; set; }
        public List<Chemical> Chemicals { get; set; }

        public Material()
        {
            Uid = 0;
            Name = "";
            Chemicals = new List<Chemical>();
        }

        public Material(int id)
        {
            Uid = id;
            Name = "";
            Chemicals = new List<Chemical>();
        }

        public Material(int id, string name)
        {
            Uid = id;
            Name = name;
            Chemicals = new List<Chemical>();
        }

        public Material(int id, string name, List<Chemical> chemicals)
        {
            Uid = id;
            Name = name;
            Chemicals = chemicals;
        }

        public void SetChemicals(List<Chemical> chemicals)
        {
            if (chemicals != null)
            {
                Chemicals = chemicals;
            }
        }

        public override string ToString()
        {
            string res = JsonConvert.SerializeObject(this, Formatting.None);

            return res;
        }
    }
}