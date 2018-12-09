using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class TaskStockHandler
    {
        public TaskStock GetFertiliserForTask(int task_id)
        {
            TaskStock task_stock = null;
            string sqlString = "SELECT `stocks`.`stock_id`, `stocks`.`name`, `stocks`.`brand`, `stocks`.`capacity_use`, `stocks`.`stock_type`, `storage_stock`.`quantity`, `task_stock`.`quantity_used` FROM `task_stock` " +
                    "INNER JOIN `stocks` ON `task_stock`.`stock_id` = `stocks`.`stock_id` " +
                    "INNER JOIN `storage_stock` ON `stocks`.`stock_id` = `storage_stock`.`stock_id` " +
                    "WHERE `task_stock`.`task_id` = " + task_id + " AND `stocks`.`stock_type` = 'FERTILISER';";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    string stockID = reader.GetString(0);
                    string stockName = reader.GetString(1);
                    string stockBrand = reader.GetString(2);
                    int capacityUse = reader.GetInt32(3);
                    string stockType = reader.GetString(4);
                    int stockQuantity = reader.GetInt32(5);
                    int usedQuantity = reader.GetInt32(6);

                    task_stock = new TaskStock(task_id, new Stock(stockID, stockName, stockBrand, capacityUse, stockType, stockQuantity), usedQuantity);
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return task_stock;
        }

        public List<TaskStock> GetStocksForTask(int task_id)
        {
            List<TaskStock> taskStocks = null;
            string sqlString = "SELECT `stocks`.`stock_id`, `stocks`.`name`, `stocks`.`brand`, `stocks`.`capacity_use`, `stocks`.`stock_type`, `storage_stock`.`quantity`, `task_stock`.`quantity_used` FROM `task_stock` " +
                    "INNER JOIN `stocks` ON `task_stock`.`stock_id` = `stocks`.`stock_id` " +
                    "INNER JOIN `storage_stock` ON `stocks`.`stock_id` = `storage_stock`.`stock_id` " +
                    "WHERE `task_stock`.`task_id` = " + task_id + ";";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                taskStocks = new List<TaskStock>();

                while (reader.Read())
                {
                    string stockID = reader.GetString(0);
                    string stockName = reader.GetString(1);
                    string stockBrand = reader.GetString(2);
                    int capacityUse = reader.GetInt32(3);
                    string stockType = reader.GetString(4);
                    int stockQuantity = reader.GetInt32(5);
                    int usedQuantity = reader.GetInt32(6);

                    taskStocks.Add(new TaskStock(task_id, new Stock(stockID, stockName, stockBrand, capacityUse, stockType, stockQuantity), usedQuantity));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return taskStocks;
        }
    }
}
