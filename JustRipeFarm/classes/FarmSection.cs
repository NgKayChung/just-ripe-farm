using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class FarmSection
    {
        private string sectionID;
        private string cropID;
        private string cropName;
        private string status;
        private DateTime sowDate;
        private DateTime expHarvestDate;

        public FarmSection()
        {

        }

        public FarmSection(string section_id, string crop_id, string crop_name, string stat, DateTime sow_date, DateTime exp_harvest_date)
        {
            sectionID = section_id;
            CropID = crop_id;
            cropName = crop_name;
            status = stat;
            sowDate = sow_date;
            expHarvestDate = exp_harvest_date;
        }

        public override string ToString()
        {
            return "Section " + sectionID + " - " + status + " (" + cropName + ")";
        }

        public string SectionID { get => sectionID; set => sectionID = value; }
        public string CropID { get => cropID; set => cropID = value; }
        public string CropName { get => cropName; set => cropName = value; }
        public string Status { get => status; set => status = value; }
        public DateTime SowDate { get => sowDate; set => sowDate = value; }
        public DateTime ExpHarvestDate { get => expHarvestDate; set => expHarvestDate = value; }
    }
}