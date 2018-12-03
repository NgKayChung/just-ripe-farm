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
        public string AddNewTask(Task task)
        {
            MySqlTransaction tr = null;

            try
            {
                tr = DbConnector.Instance.getConn().BeginTransaction();

                string sqlInsertQuery = "INSERT INTO `tasks`(`task_id`, `title`, `task_type`, `description`, `status`, `start_datetime`, `end_datetime`, `field_id`, `crop_id`, `method_use_id`, `assigned_datetime`, `assigned_by`) " +
                            "VALUES(NULL, '" + task.TaskTitle + "', '" + task.TaskType + "', '" + task.TaskDescription + "', '" + task.Status + "', '" + task.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + task.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + task.FieldID + "', '" + task.CropID + "', " + task.MethodID.ToString() + ", '" + task.AssignedDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + task.AssignedByID + "');";

                string labourerTaskInsertQuery = "INSERT INTO `labourer_task` (`labourer_id`, `task_id`) VALUES ";
                for (int i = 0; i < task.Labourers.Count; i++)
                {
                    labourerTaskInsertQuery += "('" + task.Labourers[i].UserID + "', LAST_INSERT_ID())";
                    if (i != task.Labourers.Count - 1) labourerTaskInsertQuery += ", ";
                }

                MySqlCommand sqlComm = DbConnector.Instance.getConn().CreateCommand();
                sqlComm.Connection = DbConnector.Instance.getConn();
                sqlComm.Transaction = tr;

                sqlComm.CommandText = sqlInsertQuery;
                sqlComm.ExecuteNonQuery();
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
    }
}