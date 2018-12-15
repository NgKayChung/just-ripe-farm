using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class SeedHandler
    {
        // function to get all seeds as a list
        public List<Seed> GetSeedsForCrop(string crop_id)
        {
            List<Seed> seeds = null;
            string sqlString = "SELECT `stocks`.`stock_id`, `stocks`.`name`, `stocks`.`brand`, `stocks`.`capacity_use`, `stocks`.`stock_type`, SUM(`storage_stock`.`quantity`) " +
                    "FROM `stocks` " +
                    "INNER JOIN `seeds` ON `stocks`.`stock_id` = `seeds`.`seed_id` " +
                    "LEFT JOIN `storage_stock` ON `stocks`.`stock_id` = `storage_stock`.`stock_id` " +
                    "WHERE `seeds`.`crop_id` = '" + crop_id + "' " +
                    "GROUP BY `stocks`.`stock_id`;";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                seeds = new List<Seed>();
                while (reader.Read())
                {
                    string stockID = reader.GetString(0);
                    string stockName = reader.GetString(1);
                    string stockBrand = reader.GetString(2);
                    int capacityUse = reader.GetInt32(3);
                    string stockType = reader.GetString(4);
                    int stockQuantity = reader.GetInt32(5);

                    seeds.Add(new Seed(stockID, stockName, stockBrand, capacityUse, stockType, stockQuantity));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return seeds;
        }
    }
}