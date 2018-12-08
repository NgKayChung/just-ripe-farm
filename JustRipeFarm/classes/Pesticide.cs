using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class Pesticide : Stock
    {
        private decimal litre;

        public Pesticide() : base()
        {

        }

        public Pesticide(string id, string name, string brand, int capacity_use, string type, int quantity, decimal litre)
            : base(id, name, brand, capacity_use, type, quantity)
        {
            this.litre = litre;
        }

        public override string ToString()
        {
            return Name + " - " + Brand + " (" + litre.ToString() + ")";
        }

        public decimal Litre { get => litre; set => litre = value; }
    }
}