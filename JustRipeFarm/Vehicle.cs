using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    using System;

    public class Vehicle
    {

        string macID, macName, macMan, macModal, macDesc, macType, macStatus;

        public Vehicle()
        { }

        public Vehicle(string mid, string mname, string macman,string macmod, string macdes, string mactype, string macstatus)
        {
            macID = mid;
            macName = mname;
            macModal = macmod;
            macMan = macman;
            macDesc = macdes;
            macType = mactype;
            macStatus = macstatus;
        }


        public string mac_id { get => macID; set => macID = value; }
        public string mac_name { get => macName; set => macName = value; }
        public string mac_model { get => macModal; set => macModal = value; }
        public string mac_desc { get => macDesc; set => macDesc = value; }
        public string mac_type { get => macType; set => macType = value; }
        public string mac_status { get => macStatus; set => macStatus = value; }
        public string mac_man { get => macMan; set => macMan = value; }


    }

}
