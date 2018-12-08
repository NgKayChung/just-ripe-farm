using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class Truck : Vehicle
    {
        private int totalQuantity;
        private int quantityInUse;
        private int containerQuantity;
        private DateTime timeRequired;

        public Truck(string machine_id, string machine_name, string manufacturer, string model, string description, string machine_type, string status, int total_quantity, int quantity_in_use, int container_quantity, DateTime time_required)
            : base(machine_id, machine_name, manufacturer, model, description, machine_type, status)
        {
            TotalQuantity = total_quantity;
            QuantityInUse = quantity_in_use;
            ContainerQuantity = container_quantity;
            TimeRequired = time_required;
        }

        public override string ToString()
        {
            return mac_name;
        }

        public int TotalQuantity { get => totalQuantity; set => totalQuantity = value; }
        public int QuantityInUse { get => quantityInUse; set => quantityInUse = value; }
        public int QuantityAvailable
        {
            get
            {
                return TotalQuantity - QuantityInUse;
            }
        }
        public int ContainerQuantity { get => containerQuantity; set => containerQuantity = value; }
        public DateTime TimeRequired { get => timeRequired; set => timeRequired = value; }
    }
}