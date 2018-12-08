using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class Container
    {
        private string containerID;
        private string containerType;
        private int totalCapacity;
        private string containerStatus;
        private Crop crop;

        public Container()
        {

        }

        public Container(string cont_id, string cont_type, int total_capacity, string status, Crop crop)
        {
            ContainerID = cont_id;
            ContainerType = cont_type;
            TotalCapacity = total_capacity;
            ContainerStatus = status;
            this.Crop = crop;
        }

        public string ContainerID { get => containerID; set => containerID = value; }
        public string ContainerType { get => containerType; set => containerType = value; }
        public decimal TemperatureSet
        {
            get
            {
                return (crop.MinTemperature + crop.MaxTemperature) / 2;
            }
        }
        public int TotalCapacity { get => totalCapacity; set => totalCapacity = value; }
        public string ContainerStatus { get => containerStatus; set => containerStatus = value; }
        public Crop Crop { get => crop; set => crop = value; }
    }
}