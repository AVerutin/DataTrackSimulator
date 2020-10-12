using System.Collections.Generic;
using Newtonsoft.Json;

namespace SignalGenerator.Data
{
    public class MaterialsList
    {
        public List<Material> Materials { get; set; }

        public MaterialsList()
        {
            Materials = new List<Material>();
        }

        public MaterialsList(List<Material> materials)
        {
            if (materials != null)
            {
                Materials = materials;
            }
            else
            {
                Materials = new List<Material>();
            }
        }
        
        public override string ToString()
        {
            string res = JsonConvert.SerializeObject(Materials, Formatting.None);
            return res;
        }
    }
}