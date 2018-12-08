using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class UserHandler
    {

        public List<User> getUser(MySqlConnection conn, string ID)
        {
            string sql = "SELECT * FROM users where user_id='"+UserSession.Instance.UserID+"'";

            MySqlCommand sqlComm = new MySqlCommand(sql, conn);
            MySqlDataReader reader = sqlComm.ExecuteReader();

            if (reader.HasRows)
            {
                List<User> ul = new List<User>();

                while (reader.Read())
                {
                    string userid = UserSession.Instance.UserID;
                    string firstname = reader.GetString("first_name");
                    string lasname = reader.GetString("last_name");
                    string pass = reader.GetString("secret_password");
                    string mail = reader.GetString("email_address");
                    string phone = reader.GetString("phone_number");
                    string type = reader.GetString("user_type");

                    User u = new User(userid, firstname, lasname, pass, mail, phone, type);

                    ul.Add(u);

                }
                reader.Close();
                return ul;




            }
            else
            {
                return null;
            }
        }

        public string Updateinfo(MySqlConnection conn, User u)
        {
            string sql = "update users set first_name='" + u.Firstname + "', last_name='" + u.Lastname + "', email_address='" + u.EmailAddress + "',phone_number='" + u.PhoneNumber + "' where user_id='" + UserSession.Instance.UserID + "'";
            MySqlCommand sqlComm = new MySqlCommand(sql, DbConnector.Instance.getConn());
            return sqlComm.ExecuteNonQuery().ToString();
        }

        public string changePass(MySqlConnection conn, User u)
        {
            string sql = "Update users set secret_password='" + u.Password + "'where user_id='" + UserSession.Instance.UserID + "' and secret_password='" + UserSession.Instance.Password + "'";
            MySqlCommand sqlComm = new MySqlCommand(sql, DbConnector.Instance.getConn());
            return sqlComm.ExecuteNonQuery().ToString();
        }
    }
}
