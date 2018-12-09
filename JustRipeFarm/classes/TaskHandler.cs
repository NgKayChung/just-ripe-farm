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

        public List<Task> GetTasksForLabourer(string labourer_id)
        {
            List<Task> tasks = null;

            string sqlString = "SELECT `tasks`.`task_id`, `tasks`.`title`, `tasks`.`task_type`, `tasks`.`description`, `tasks`.`status`, `tasks`.`start_datetime`, `tasks`.`end_datetime`, `tasks`.`field_id`, `tasks`.`crop_id`, `tasks`.`method_use_id`, `tasks`.`assigned_datetime`, `tasks`.`assigned_by` FROM `labourer_task` " +
                "INNER JOIN `tasks` ON `labourer_task`.`task_id` = `tasks`.`task_id` " +
                "WHERE `labourer_task`.`labourer_id` = '" + labourer_id + "';";

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

        public int UpdateTaskStatus(Task task, string status)
        {
            string updateQuery = "UPDATE `tasks` SET `status` = '" + status + "' WHERE `task_id` = " + task.TaskID.ToString() + " AND `status` = '" + task.Status + "';";
            MySqlCommand sqlCommand = new MySqlCommand(updateQuery, DbConnector.Instance.getConn());
            return sqlCommand.ExecuteNonQuery();
        }
    }
}