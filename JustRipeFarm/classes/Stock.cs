using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class Stock
    {
        private string id;
        private string name;
        private string brand;
        private int capacityUse;
        private string type;
        private int quantity;

        public Stock()
        {

        }

        public Stock(string id, string stock_name, string stock_brand, int capacity_use, string stock_type, int stock_quantity)
        {
            this.id = id;
            name = stock_name;
            brand = stock_brand;
            capacityUse = capacity_use;
            type = stock_type;
            Quantity = stock_quantity;
        }

        public string ID { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Brand { get => brand; set => brand = value; }
        public int CapacityUse { get => capacityUse; set => capacityUse = value; }
        public string Type { get => type; set => type = value; }
        public int Quantity { get => quantity; set => quantity = value; }
    }
}