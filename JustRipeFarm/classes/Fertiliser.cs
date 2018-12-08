using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class Fertiliser : Stock
    {
        private bool is_organic;

        public Fertiliser() : base()
        {

        }

        public Fertiliser(string id, string name, string brand, int capacity_use, string type, int quantity, bool isOrganic)
            : base(id, name, brand, capacity_use, type, quantity)
        {
            is_organic = isOrganic;
        }

        public override string ToString()
        {
            return Name + " - " + Brand + " (" + (is_organic ? "Organic" : "Inorganic") + ")";
        }

        public bool IsOrganic { get => is_organic; set => is_organic = value; }
    }
}