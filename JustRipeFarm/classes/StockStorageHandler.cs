using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class StockStorageHandler
    {
        // function to get all stock storages
        public List<StockStorage> FindStockStorages()
        {
            List<StockStorage> storages = null;
            string sqlString = "SELECT `storages`.`storage_id`, `storages`.`total_capacity`, SUM(`stocks`.`capacity_use` * `storage_stock`.`quantity`) AS 'used_capacity', `storages`.`status` " +
                "FROM `storages` " +
                "INNER JOIN `storage_stock` " +
                    "ON `storages`.`storage_id` = `storage_stock`.`storage_id` " +
                "INNER JOIN `stocks` " +
                    "ON `storage_stock`.`stock_id` = `stocks`.`stock_id` " +
                "GROUP BY `storages`.`storage_id`";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                storages = new List<StockStorage>();
                while (reader.Read())
                {
                    string storageID = reader.GetString(0);
                    int total_capacity = reader.GetInt32(1);
                    int used_capacity = reader.GetInt32(2);
                    string status = reader.GetString(3);

                    storages.Add(new StockStorage(storageID, total_capacity, used_capacity, status));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return storages;
        }
    }
}