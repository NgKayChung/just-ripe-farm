using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class Seed : Stock
    {
        public Seed() : base()
        {

        }

        public Seed(string id, string name, string brand, int capacity_use, string type, int quantity)
            : base(id, name, brand, capacity_use, type, quantity)
        {
            
        }

        public override string ToString()
        {
            return Name + " - " + Brand;
        }
    }
}