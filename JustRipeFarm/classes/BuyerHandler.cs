using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class BuyerHandler
    {
        public List<Buyer> GetBuyers()
        {
            List<Buyer> buyers = null;
            string sqlString = "SELECT `buyers`.`first_name`, `buyers`.`last_name`, `buyers`.`email_address`, `buyers`.`phone_number`, `buyers`.`company_name`, COUNT(`sales`.`sale_id`) AS `visit_count`, TIME_FORMAT(TIME(FROM_UNIXTIME(AVG(UNIX_TIMESTAMP(TIME(`sales`.`sale_datetime`))))), '%H:%i:%s') AS `average_visit_time`, SUM(`sales`.`total_price`) AS `total_sales` " +
                    "FROM `buyers` " +
                    "INNER JOIN `sales` " +
	                    "ON `buyers`.`buyer_id` = `sales`.`buyer_id` " +
                    "GROUP BY `buyers`.`buyer_id`";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                buyers = new List<Buyer>();
                while (reader.Read())
                {
                    string firstName = reader.GetString(0);
                    string lastName = reader.GetString(1);
                    string emailAddress = (reader.IsDBNull(2) ? "-" : reader.GetString(2));
                    string phoneNumber = (reader.IsDBNull(3) ? "-" : reader.GetString(3));
                    string companyName = (reader.IsDBNull(4) ? "-" : reader.GetString(4));
                    int visitedCount = reader.GetInt32(5);
                    string avgVisitTime = reader.GetString(6);
                    decimal totalSpent = reader.GetDecimal(7);

                    buyers.Add(new Buyer(firstName, lastName, emailAddress, phoneNumber, visitedCount, avgVisitTime, totalSpent, companyName));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return buyers;
        }
    }
}