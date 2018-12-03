using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class CropMethod
    {
        private int methodID;
        private string methodName;
        private string methodType;
        private DateTime timeRequired;
        private string machineID;
        private string machineName;

        public CropMethod()
        {

        }

        public CropMethod(int method_id, string method_name, string method_type, DateTime time_required, string machine_id, string machine_name)
        {
            methodID = method_id;
            methodName = method_name;
            methodType = method_type;
            timeRequired = time_required;
            machineID = machine_id;
            machineName = machine_name;
        }

        public override string ToString()
        {
            return methodName + (machineName.Equals("") ? "" : " - " + machineName);
        }

        public int MethodID { get => methodID; set => methodID = value; }
        public string MethodName { get => methodName; set => methodName = value; }
        public string MethodType { get => methodType; set => methodType = value; }
        public DateTime TimeRequired { get => timeRequired; set => timeRequired = value; }
        public string MachineID { get => machineID; set => machineID = value; }
        public string MachineName { get => machineName; set => machineName = value; }
    }
}