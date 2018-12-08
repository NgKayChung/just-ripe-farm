using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class UserHandler
    {
        public User GetUser(MySqlConnection conn, string ID)
        {
            User user = null;
            string sql = "SELECT `user_id`, `first_name`, `last_name`, `secret_password`, `email_address`, `phone_number`, `user_type` FROM `users` where `user_id`='" + ID+"'";

            MySqlCommand sqlComm = new MySqlCommand(sql, conn);
            MySqlDataReader reader = sqlComm.ExecuteReader();

            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    string userid = ID;
                    string firstname = reader.GetString(1);
                    string lastname = reader.GetString(2);
                    string pass = reader.GetString(3);
                    string email = reader.GetString(4);
                    string phone = (reader.IsDBNull(5) ? "" : reader.GetString(5));
                    string type = reader.GetString(6);

                    user = new User(userid, firstname, lastname, pass, email, phone, type);
                }
            }

            sqlComm.Dispose();
            reader.Close();

            return user;
        }

        public int UpdateUserInfo(MySqlConnection conn, User user)
        {
            string sql = "UPDATE `users` SET `first_name`='" + user.Firstname + "', `last_name`='" + user.Lastname + "', `email_address`='" + user.EmailAddress + "', `phone_number`='" + user.PhoneNumber + "' WHERE `user_id`='" + user.UserID + "'";
            MySqlCommand sqlComm = new MySqlCommand(sql, DbConnector.Instance.getConn());
            return sqlComm.ExecuteNonQuery();
        }

        public int ChangePass(MySqlConnection conn, User user, string newPassword)
        {
            string sql = "UPDATE `users` SET `secret_password`='" + newPassword + "' WHERE `user_id`='" + user.UserID + "' AND `secret_password` = BINARY '" + user.Password + "';";
            MySqlCommand sqlComm = new MySqlCommand(sql, DbConnector.Instance.getConn());
            return sqlComm.ExecuteNonQuery();
        }
    }
}
