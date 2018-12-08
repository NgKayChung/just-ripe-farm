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
        public string GetNewestID()
        {
            string sqlString = "SELECT MAX(labourer_id) FROM `labourers`;";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                string labour_id = reader.GetString(0);
                sqlCommand.Dispose();
                return labour_id;
            }
            else
            {
                sqlCommand.Dispose();
                return null;
            }
        }

        public List<Labourer> FindAllLabourers()
        {
            List<Labourer> labourers = null;
            string sqlString = "SELECT `users`.`user_id`, `users`.`first_name`, `users`.`last_name`, `users`.`email_address`, `users`.`phone_number`, `labourers`.`date_joined`, IF(`tasks`.`task_id` IS NULL, 'FREE', IF(MAX(`tasks`.`end_datetime`) < NOW(), 'FREE', 'ASSIGNED')) AS `status` " +
                    "FROM `users` " +
                    "INNER JOIN `labourers` " +
                        "ON `users`.`user_id` = `labourers`.`labourer_id` " +
                    "LEFT JOIN `labourer_task` " +
                        "ON `users`.`user_id` = `labourer_task`.`labourer_id` " +
                    "LEFT JOIN `tasks` " +
                        "ON `labourer_task`.`task_id` = `tasks`.`task_id` " +
                    "WHERE `users`.`user_type` = 'LABOURER' " +
                    "GROUP BY `users`.`user_id`; ";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                labourers = new List<Labourer>();
                while (reader.Read())
                {
                    string labourer_id = reader.GetString(0);
                    string labourer_firstname = reader.GetString(1);
                    string labourer_lastname = reader.GetString(2);
                    string labourer_emailAddress = reader.GetString(3);
                    string labourer_phoneNumber = (reader.IsDBNull(4) ? "-" : reader.GetString(4));
                    DateTime labourer_joinedDate = reader.GetDateTime(5);
                    string labourer_status = reader.GetString(6);

                    labourers.Add(new Labourer(labourer_id, labourer_firstname, labourer_lastname, labourer_emailAddress, labourer_phoneNumber, labourer_joinedDate, labourer_status));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return labourers;
        }

        public string AddNewLabourer(Labourer labourer)
        {
            string sql = "INSERT INTO users " +
                "(user_id, first_name, last_name, secret_password, email_address, phone_number, user_type) " +
                "VALUES ('" + labourer.UserID + "', '" + labourer.Firstname + "', '" + labourer.Lastname + "', '" + labourer.Password + "', " + (labourer.EmailAddress != "" ? "'" + labourer.EmailAddress + "'" : "NULL") + ", " + (labourer.PhoneNumber != "" ? "'" + labourer.PhoneNumber + "'" : "NULL") + ", '" + labourer.UserType + "')";

            MySqlCommand sqlComm = new MySqlCommand(sql, DbConnector.Instance.getConn());

            if (sqlComm.ExecuteNonQuery() == 1)
            {
                sqlComm.Dispose();
                sqlComm = new MySqlCommand("INSERT INTO labourers (labourer_id, date_joined) VALUES('" + labourer.UserID + "', '" + labourer.DateJoined.ToString("yyyy-MM-dd") + "')", DbConnector.Instance.getConn());

                if (sqlComm.ExecuteNonQuery() == 1)
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