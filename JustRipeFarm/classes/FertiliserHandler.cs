using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class FertiliserHandler
    {
        public List<Fertiliser> GetAllFertilisers()
        {
            List<Fertiliser> fertilisers = null;
            string sqlString = "SELECT `stocks`.`stock_id`, `stocks`.`name`, `stocks`.`brand`, `stocks`.`capacity_use`, `stocks`.`stock_type`, SUM(`storage_stock`.`quantity`), `fertilisers`.`is_organic` " +
                    "FROM `stocks` " +
                    "INNER JOIN `fertilisers` ON `stocks`.`stock_id` = `fertilisers`.`fertiliser_id` LEFT JOIN `storage_stock` ON `stocks`.`stock_id` = `storage_stock`.`stock_id` " +
                    "WHERE `stocks`.`stock_type` = 'FERTILISER' " +
                    "GROUP BY `stocks`.`stock_id`; ";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                fertilisers = new List<Fertiliser>();
                while (reader.Read())
                {
                    string stockID = reader.GetString(0);
                    string stockName = reader.GetString(1);
                    string stockBrand = reader.GetString(2);
                    int capacityUse = reader.GetInt32(3);
                    string stockType = reader.GetString(4);
                    int stockQuantity = reader.GetInt32(5);
                    bool isOrganic = reader.GetBoolean(6);

                    fertilisers.Add(new Fertiliser(stockID, stockName, stockBrand, capacityUse, stockType, stockQuantity, isOrganic));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return fertilisers;
        }
    }
}