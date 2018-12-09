using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class User
    {
        private string user_id;
        private string firstname;
        private string lastname;
        private string password;
        private string email_address;
        private string phone_number;
        private string user_type;

        public User()
        {

        }

        public User(string uID, string fName, string lName, string passwd, string emailAdd, string phoneNum, string uType)
        {
            user_id = uID;
            firstname = fName;
            lastname = lName;
            password = passwd;
            email_address = emailAdd;
            phone_number = phoneNum;
            user_type = uType;
        }

        public string UserID { get => user_id; set => user_id = value; }
        public string Firstname { get => firstname; set => firstname = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public string Fullname
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }
        public string Password { get => password; set => password = value; }
        public string EmailAddress { get => email_address; set => email_address = value; }
        public string PhoneNumber { get => phone_number; set => phone_number = value; }
        public string UserType { get => user_type; set => user_type = value; }
    }
}
