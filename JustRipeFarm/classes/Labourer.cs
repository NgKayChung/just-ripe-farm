using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class Labourer : User
    {
        private DateTime date_joined;

        public Labourer() :base()
        {

        }

        public Labourer(string uID, string fName, string lName, string passwd, string emailAdd, string phoneNum, string uType, DateTime joinDate)
            :base(uID, fName, lName, passwd, emailAdd, phoneNum, uType)
        {
            date_joined = joinDate;
        }

        public DateTime DateJoined { get => date_joined; set => date_joined = value; }
    }
}
