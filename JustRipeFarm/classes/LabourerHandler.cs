using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class LabourerHandler
    {

        public string Updateinfo(MySqlConnection conn, User u)
        {
            string sql = "Update users set first_name='" + u.Firstname + "', last_name='" + u.Lastname + "', email_address='" + u.EmailAddress + "',phone_number='" + u.PhoneNumber + "' where user_id='"+UserSession.Instance.UserID+"' and secret_password='"+UserSession.Instance.Password+"'";
            MySqlCommand sqlComm = new MySqlCommand(sql, DbConnector.Instance.getConn());
            return sqlComm.ExecuteNonQuery().ToString();
        }

        public string changePass(MySqlConnection conn, User u)
        {
            string sql = "Update users set secret_password='" + u.Password + "'where user_id='" + UserSession.Instance.UserID + "' and secret_password='" + UserSession.Instance.Password + "'";
            MySqlCommand sqlComm = new MySqlCommand(sql, DbConnector.Instance.getConn());
            return sqlComm.ExecuteNonQuery().ToString();
        }

        public string addNewLabourer(MySqlConnection conn, Labourer labourer)
        {
            string sql = "INSERT INTO users " +
                "(user_id, first_name, last_name, secret_password, email_address, phone_number, user_type) " + 
                "VALUES ('" + labourer.UserID + "', '" + labourer.Firstname + "', '" + labourer.Lastname + "', '" + labourer.Password + "', " + (labourer.EmailAddress != "" ? "'" + labourer.EmailAddress + "'" : "NULL") + ", " + (labourer.PhoneNumber != "" ? "'" + labourer.PhoneNumber + "'" : "NULL") + ", '" + labourer.UserType + "')";

            MySqlCommand sqlComm = new MySqlCommand(sql, DbConnector.Instance.getConn());

            if(sqlComm.ExecuteNonQuery() == 1)
            {
                sqlComm.Dispose();
                sqlComm = new MySqlCommand("INSERT INTO labourers (labourer_id, date_joined) VALUES('" + labourer.UserID + "', '" + labourer.DateJoined.ToString("yyyy-MM-dd") + "')", DbConnector.Instance.getConn());
                
                if(sqlComm.ExecuteNonQuery() == 1)
                {
                    sqlComm.Dispose();
                    return "SUCCESS";
                }
                else
                {
                    sqlComm.Dispose();
                    return "FAILED";
                }
            }
            else
            {
                sqlComm.Dispose();
                return "FAILED";
            }
        }
    }
}
