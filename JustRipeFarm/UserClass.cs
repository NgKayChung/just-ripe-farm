using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class UserClass
    {
        private bool loggin = false;
        private string username;
        private string usertype;
        private static UserClass instance;

        private UserClass()
        {

        }

        public static UserClass Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserClass();
                }
                return instance;
            }
        }       

        public bool Loggerin { get => loggin; set => loggin = value; }
        public string Username { get => username; set => username = value; }
        public string Usertype { get => usertype; set => usertype = value; }

    }
}
