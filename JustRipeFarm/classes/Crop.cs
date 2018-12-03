using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class Crop
    {
        private string cropID;
        private string cropName;
        private decimal minTemperature;
        private decimal maxTemperature;
        private int harvestDays;
        private string containerType;

        public Crop()
        {

        }

        public Crop(string crop_id, string crop_name, decimal min_temp, decimal max_temp, int harvest_days, string container_type)
        {
            CropID = crop_id;
            CropName = crop_name;
            MinTemperature = min_temp;
            MaxTemperature = max_temp;
            HarvestDays = harvest_days;
            ContainerType = container_type;
        }

        public override string ToString()
        {
            return cropName;
        }

        public string CropID { get => cropID; set => cropID = value; }
        public string CropName { get => cropName; set => cropName = value; }
        public decimal MinTemperature { get => minTemperature; set => minTemperature = value; }
        public decimal MaxTemperature { get => maxTemperature; set => maxTemperature = value; }
        public int HarvestDays { get => harvestDays; set => harvestDays = value; }
        public string ContainerType { get => containerType; set => containerType = value; }
    }
}