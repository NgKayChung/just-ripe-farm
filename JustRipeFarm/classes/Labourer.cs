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
        private string status;

        public Labourer() :base()
        {

        }

        public Labourer(string uID, string fName, string lName, string passwd, string emailAdd, string phoneNum, string uType, DateTime joinDate)
            :base(uID, fName, lName, passwd, emailAdd, phoneNum, uType)
        {
            date_joined = joinDate;
        }

        public Labourer(string uID, string fName, string lName, string emailAddress, string phoneNumber, DateTime joinDate, string stat) : base()
        {
            UserID = uID;
            Firstname = fName;
            Lastname = lName;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            date_joined = joinDate;
            Status = stat;
        }

        public Labourer(string uID, string fName, string lName, string emailAddress, string phoneNumber, DateTime joinDate) : base()
        {
            UserID = uID;
            Firstname = fName;
            Lastname = lName;
            EmailAddress = emailAddress;
            PhoneNumber = phoneNumber;
            date_joined = joinDate;
        }

        public override string ToString()
        {
            return (Firstname + " " + Lastname);
        }

        public DateTime DateJoined { get => date_joined; set => date_joined = value; }
        public string Status { get => status; set => status = value; }
    }
}
