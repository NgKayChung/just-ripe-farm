using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class UserSession
    {
        private bool loggedIn = false;
        private string userID;
        private string userFirstName;
        private string userLastName;
        private string password;
        private string email;
        private string phone_number;
        private string userType;
        private static UserSession instance;

        private UserSession()
        {

        }

        public static UserSession Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserSession();
                }
                return instance;
            }
        }       

        public bool LoggedIn { get => loggedIn; set => loggedIn = value; }
        public string UserID { get => userID; set => userID = value; }
        public string UserFirstName { get => userFirstName; set => userFirstName = value; }
        public string UserLastName { get => userLastName; set => userLastName = value; }
        public string Password { get => password; set => password = value; }
        public string Email { get => email; set => email = value; }
        public string PhoneNumber { get => phone_number; set => phone_number = value; }
        public string UserType { get => userType; set => userType = value; }
    }
}