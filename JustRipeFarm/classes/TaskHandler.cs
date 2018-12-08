using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class TaskHandler
    {
        public string AddNewTask(Task task, List<Labourer> labourers, List<TaskStock> stocks)
        {
            MySqlTransaction tr = null;

            try
            {
                tr = DbConnector.Instance.getConn().BeginTransaction();

                string sqlInsertQuery = "INSERT INTO `tasks`(`task_id`, `title`, `task_type`, `description`, `status`, `start_datetime`, `end_datetime`, `field_id`, `crop_id`, `method_use_id`, `assigned_datetime`, `assigned_by`) " +
                            "VALUES(NULL, '" + task.TaskTitle + "', '" + task.TaskType + "', '" + task.TaskDescription + "', '" + task.Status + "', '" + task.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + task.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + task.FieldID + "', '" + task.CropID + "', " + task.MethodID.ToString() + ", '" + task.AssignedDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + task.AssignedByID + "');";

                string labourerTaskInsertQuery = "INSERT INTO `labourer_task` (`labourer_id`, `task_id`) VALUES ";
                for (int i = 0; i < labourers.Count; i++)
                {
                    labourerTaskInsertQuery += "('" + labourers[i].UserID + "', LAST_INSERT_ID())";
                    if (i != labourers.Count - 1) labourerTaskInsertQuery += ", ";
                }

                string stocksTaskInsertQuery = "";
                if (stocks.Count > 0)
                {
                    stocksTaskInsertQuery = "INSERT INTO `task_stock` (`task_id`, `stock_id`, `quantity_used`) VALUES ";
                    for (int i = 0; i < stocks.Count; i++)
                    {
                        stocksTaskInsertQuery += "(LAST_INSERT_ID(), '" + stocks[i].Stock.ID + "', " + stocks[i].QuantityUse + ")";
                        if (i != stocks.Count - 1) stocksTaskInsertQuery += ", ";
                    }
                }

                MySqlCommand sqlComm = DbConnector.Instance.getConn().CreateCommand();
                sqlComm.Connection = DbConnector.Instance.getConn();
                sqlComm.Transaction = tr;

                sqlComm.CommandText = sqlInsertQuery;
                sqlComm.ExecuteNonQuery();
                
                if (stocksTaskInsertQuery != "")
                {
                    sqlComm.CommandText = stocksTaskInsertQuery;
                    sqlComm.ExecuteNonQuery();
                }

                sqlComm.CommandText = labourerTaskInsertQuery;
                sqlComm.ExecuteNonQuery();

                tr.Commit();

                tr.Dispose();
                sqlComm.Dispose();

                return "SUCCESS";
            }
            catch(MySqlException ex)
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

        public List<Task> GetAllPendingTasks(DateTime startDate, DateTime endDate)
        {
            List<Task> tasks = null;
            string sqlString = "SELECT * FROM `tasks` WHERE `status` = 'PENDING' AND ((`start_datetime` BETWEEN '" + startDate.ToString("yyyy-MM-dd") + "' AND '" + endDate.ToString("yyyy-MM-dd") + "') OR (`end_datetime` BETWEEN '" + startDate.ToString("yyyy-MM-dd") + "' AND '" + endDate.ToString("yyyy-MM-dd") + "'));";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                tasks = new List<Task>();
                while (reader.Read())
                {
                    int task_id = reader.GetInt32(0);
                    string task_title = reader.GetString(1);
                    string task_type = reader.GetString(2);
                    string task_description = reader.GetString(3);
                    string status = reader.GetString(4);
                    DateTime startDateTime = reader.GetDateTime(5);
                    DateTime endDateTime = reader.GetDateTime(6);
                    string field_id = reader.GetString(7);
                    string crop_id = reader.GetString(8);
                    int method_id = reader.GetInt32(9);
                    DateTime assignDateTime = reader.GetDateTime(10);
                    string manager_id = reader.GetString(11);

                    tasks.Add(new Task(task_id, task_title, task_type, task_description, status, startDateTime, endDateTime, field_id, crop_id, method_id, assignDateTime, manager_id));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return tasks;
        }

        public List<Labourer> GetLabourersForTask(int task_id)
        {
            List<Labourer> labourers = null;
            string sqlString = "SELECT `users`.`user_id`, `users`.`first_name`, `users`.`last_name`, `users`.`email_address`, `users`.`phone_number`, `labourers`.`date_joined` FROM `labourer_task` " +
                    "INNER JOIN `labourers` ON `labourer_task`.`labourer_id` = `labourers`.`labourer_id` " +
                    "INNER JOIN `users` ON `labourers`.`labourer_id` = `users`.`user_id` " +
                    "WHERE `labourer_task`.`task_id` = " + task_id + ";";

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

                    labourers.Add(new Labourer(labourer_id, labourer_firstname, labourer_lastname, labourer_emailAddress, labourer_phoneNumber, labourer_joinedDate));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return labourers;
        }

        public TaskStock GetStocksForTask(int task_id)
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
    }
}