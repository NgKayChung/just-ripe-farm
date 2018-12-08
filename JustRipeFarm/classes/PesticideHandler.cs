using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class PesticideHandler
    {
        public List<Pesticide> GetAllPesticides()
        {
            List<Pesticide> pesticides = null;
            string sqlString = "SELECT `stocks`.`stock_id`, `stocks`.`name`, `stocks`.`brand`, `stocks`.`capacity_use`, `stocks`.`stock_type`, SUM(`storage_stock`.`quantity`), `pesticides`.`litre` " +
                    "FROM `stocks` " +
                    "INNER JOIN `pesticides` ON `stocks`.`stock_id` = `pesticides`.`pesticide_id` LEFT JOIN `storage_stock` ON `stocks`.`stock_id` = `storage_stock`.`stock_id` " +
                    "WHERE `stocks`.`stock_type` = 'PESTICIDE' " +
                    "GROUP BY `stocks`.`stock_id`;";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                pesticides = new List<Pesticide>();
                while (reader.Read())
                {
                    string stockID = reader.GetString(0);
                    string stockName = reader.GetString(1);
                    string stockBrand = reader.GetString(2);
                    int capacityUse = reader.GetInt32(3);
                    string stockType = reader.GetString(4);
                    int stockQuantity = reader.GetInt32(5);
                    decimal litre = reader.GetDecimal(6);

                    pesticides.Add(new Pesticide(stockID, stockName, stockBrand, capacityUse, stockType, stockQuantity, litre));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return pesticides;
        }
    }
}