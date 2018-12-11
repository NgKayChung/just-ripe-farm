using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class StockHandler
    {
        public List<Stock> FindStocksForStorage(string storage_id)
        {
            List<Stock> stocks = null;
            string sqlString = "SELECT `stocks`.`stock_id`, `stocks`.`name`, `stocks`.`brand`, `stocks`.`capacity_use`, `stocks`.`stock_type`, `storage_stock`.`quantity` " +
                "FROM `storages` " +
                "INNER JOIN `storage_stock` " +
	                "ON `storages`.`storage_id` = `storage_stock`.`storage_id` " +
                "INNER JOIN `stocks` " +
	                "ON `storage_stock`.`stock_id` = `stocks`.`stock_id` " +
                "WHERE `storage_stock`.`storage_id` = '" + storage_id + "';";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                stocks = new List<Stock>();
                while (reader.Read())
                {
                    string stockID = reader.GetString(0);
                    string stockName = reader.GetString(1);
                    string stockBrand = reader.GetString(2);
                    int capacityUse = reader.GetInt32(3);
                    string stockType = reader.GetString(4);
                    int stockQuantity = reader.GetInt32(5);

                    stocks.Add(new Stock(stockID, stockName, stockBrand, capacityUse, stockType, stockQuantity));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return stocks;
        }

        public List<Stock> FindStocksFilterType(string storage_id, string stock_type)
        {
            List<Stock> stocks = null;
            string sqlString = "SELECT `stocks`.`stock_id`, `stocks`.`name`, `stocks`.`brand`, `stocks`.`capacity_use`, `stocks`.`stock_type`, `storage_stock`.`quantity` " +
                "FROM `storages` " +
                "INNER JOIN `storage_stock` " +
                    "ON `storages`.`storage_id` = `storage_stock`.`storage_id` " +
                "INNER JOIN `stocks` " +
                    "ON `storage_stock`.`stock_id` = `stocks`.`stock_id` " +
                "WHERE `storage_stock`.`storage_id` = '" + storage_id + "' " +
                    "AND `stocks`.`stock_type` = '" + stock_type + "';";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                stocks = new List<Stock>();
                while (reader.Read())
                {
                    string stockID = reader.GetString(0);
                    string stockName = reader.GetString(1);
                    string stockBrand = reader.GetString(2);
                    int capacityUse = reader.GetInt32(3);
                    string stockType = reader.GetString(4);
                    int stockQuantity = reader.GetInt32(5);

                    stocks.Add(new Stock(stockID, stockName, stockBrand, capacityUse, stockType, stockQuantity));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return stocks;
        }

        public string UpdateStockData(Stock stock)
        {
            MySqlTransaction tr = null;

            try
            {
                tr = DbConnector.Instance.getConn().BeginTransaction();

                string sqlUpdateStockQuery = "UPDATE `stocks` SET `name` = '" + stock.Name + "', `brand` = '" + stock.Brand + "', `capacity_use` = " + stock.CapacityUse + " WHERE `stock_id` = '" + stock.ID + "'";
                string sqlUpdateStorageStockQuery = "UPDATE `storage_stock` SET `quantity` = " + stock.Quantity + " WHERE `stock_id` = '" + stock.ID + "'";

                MySqlCommand sqlComm = DbConnector.Instance.getConn().CreateCommand();
                sqlComm.Connection = DbConnector.Instance.getConn();
                sqlComm.Transaction = tr;


                sqlComm.CommandText = sqlUpdateStockQuery;
                sqlComm.ExecuteNonQuery();
                sqlComm.CommandText = sqlUpdateStorageStockQuery;
                sqlComm.ExecuteNonQuery();

                tr.Commit();

                tr.Dispose();
                sqlComm.Dispose();

                return "SUCCESS";
            }
            catch (MySqlException ex)
            {
                try
                {
                    tr.Rollback();
                }
                catch (MySqlException ex1)
                {
                    return ex1.ToString();
                }

                return ex.ToString();
            }
        }

        public int UpdateStockQuantity(Stock stock, int useQuantity)
        {
            string sql = "UPDATE `storage_stock` SET `quantity` = `quantity` - " + useQuantity.ToString() + " WHERE `stock_id` = '" + stock.ID + "'";
            MySqlCommand sqlComm = new MySqlCommand(sql, DbConnector.Instance.getConn());
            return sqlComm.ExecuteNonQuery();
        }
    }
}