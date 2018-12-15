using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class CropMethodHandler
    {
        // get crop methods available to use for task on crop
        public List<CropMethod> GetAllCropMethodsOnCropAndTaskType(string cropID, string taskType)
        {
            List<CropMethod> cropMethods = null;
            string queryString = "SELECT `crop_method`.`method_id`, `crop_method`.`type`, `crop_method`.`method`, CONVERT(`crop_method`.`time_required`, DATETIME), `machinery`.`machinery_id`, `machinery`.`machine_name` FROM `crop_method` " +
                    "LEFT JOIN `machinery` ON `crop_method`.`machine_id` = `machinery`.`machinery_id` " +
                    "INNER JOIN `crops` ON `crop_method`.`crop_id` = `crops`.`crop_id` " +
                    "WHERE `crop_method`.`crop_id` = '" + cropID + "' " +
                        "AND `crop_method`.`type` = '" + taskType + "';";

            MySqlCommand sqlCommand = new MySqlCommand(queryString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                cropMethods = new List<CropMethod>();
                while (reader.Read())
                {
                    int method_id = reader.GetInt32(0);
                    string method_type = reader.GetString(1);
                    string method_name = reader.GetString(2);
                    DateTime time_required = reader.GetDateTime(3);
                    string machine_id = (reader.IsDBNull(4) ? "" : reader.GetString(4));
                    string machine_name = (reader.IsDBNull(5) ? "" : reader.GetString(5));

                    cropMethods.Add(new CropMethod(method_id, method_name, method_type, time_required, machine_id, machine_name));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return cropMethods;
        }

        // get crop method with method ID specified
        public CropMethod GetCropMethod(int method_id)
        {
            CropMethod cropMethod = null;
            string queryString = "SELECT `crop_method`.`method_id`, `crop_method`.`type`, `crop_method`.`method`, CONVERT(`crop_method`.`time_required`, DATETIME), `machinery`.`machinery_id`, `machinery`.`machine_name` FROM `crop_method` " +
                    "LEFT JOIN `machinery` ON `crop_method`.`machine_id` = `machinery`.`machinery_id` " +
                    "WHERE `crop_method`.`method_id` = " + method_id + ";";

            MySqlCommand sqlCommand = new MySqlCommand(queryString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                if (reader.Read())
                {
                    int cropMethod_id = reader.GetInt32(0);
                    string method_type = reader.GetString(1);
                    string method_name = reader.GetString(2);
                    DateTime time_required = reader.GetDateTime(3);
                    string machine_id = (reader.IsDBNull(4) ? "" : reader.GetString(4));
                    string machine_name = (reader.IsDBNull(5) ? "" : reader.GetString(5));

                    cropMethod = new CropMethod(cropMethod_id, method_name, method_type, time_required, machine_id, machine_name);
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return cropMethod;
        }
    }
}