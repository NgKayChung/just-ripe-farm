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
        public string UserType { get => userType; set => userType = value; }
    }
}