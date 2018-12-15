using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class Wholesale
    {
        private string wholesaleID;
        private DateTime startDateTime;
        private DateTime endDateTime;
        private DateTime assignedDateTime;
        private User manager;
        private List<Truck> trucks;
        private List<int> trucksQuantity;
        private List<Container> containers;

        public Wholesale()
        {

        }

        public Wholesale(string wholesale_id, DateTime start_dateTime, DateTime end_dateTime, DateTime assigned_dateTime, User assigned_manager, List<Truck> trucks, List<int> trucks_quantity, List<Container> containers)
        {
            WholesaleID = wholesale_id;
            StartDateTime = start_dateTime;
            EndDateTime = end_dateTime;
            AssignedDateTime = assigned_dateTime;
            this.Manager = assigned_manager;
            this.Trucks = trucks;
            TrucksQuantity = trucks_quantity;
            this.Containers = containers;
        }

        public string WholesaleID { get => wholesaleID; set => wholesaleID = value; }
        public DateTime StartDateTime { get => startDateTime; set => startDateTime = value; }
        public DateTime EndDateTime { get => endDateTime; set => endDateTime = value; }
        public DateTime AssignedDateTime { get => assignedDateTime; set => assignedDateTime = value; }
        public List<int> TrucksQuantity { get => trucksQuantity; set => trucksQuantity = value; }
        internal User Manager { get => manager; set => manager = value; }
        internal List<Truck> Trucks { get => trucks; set => trucks = value; }
        internal List<Container> Containers { get => containers; set => containers = value; }
    }
}