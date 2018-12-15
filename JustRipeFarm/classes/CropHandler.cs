using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class CropHandler
    {
        public List<Crop> GetAllCropsForSow()
        {
            List<Crop> crops = null;
            string sqlString = "SELECT `crops`.`crop_id`, `crops`.`crop_name`, `crops`.`min_temperature`, `crops`.`max_temperature`, `crops`.`harvest_days`, `crops`.`capacity_use`, `crops`.`container_type` FROM `crops` ";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                crops = new List<Crop>();
                while (reader.Read())
                {
                    string crop_id = reader.GetString(0);
                    string crop_name = reader.GetString(1);
                    decimal min_temperature = (reader.IsDBNull(2) ? 0 : reader.GetDecimal(2));
                    decimal max_temperature = (reader.IsDBNull(3) ? 0 : reader.GetDecimal(3));
                    int harvest_days = reader.GetInt32(4);
                    int capacity_use = reader.GetInt32(5);
                    string container_type = reader.GetString(6);

                    crops.Add(new Crop(crop_id, crop_name, min_temperature, max_temperature, harvest_days, capacity_use, container_type));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return crops;
        }

        public bool IsSuitableSow(Crop crop, DateTime sowDate)
        {
            string sqlString = "SELECT SUM(IF(`month` = UPPER(DATE_FORMAT('" + sowDate.ToString("yyyy-MM-dd") + "', '%M')), 1, 0)) FROM `crop_sow_time` WHERE `crop_id` = '" + crop.CropID + "'";
            MySqlCommand sqlComm = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlComm.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                int isSuitable = reader.GetInt32(0);

                if (!reader.IsClosed) reader.Close();

                return isSuitable > 0;
            }

            if (!reader.IsClosed) reader.Close();

            return false;
        }

        public Crop GetCropWithID(string cropID)
        {
            Crop crop = null;
            string sqlString = "SELECT `crop_id`, `crop_name`, `min_temperature`, `max_temperature`, `harvest_days`, `capacity_use`, `container_type` FROM `crops` WHERE `crop_id` = '" + cropID + "'";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                
                string crop_id = reader.GetString(0);
                string crop_name = reader.GetString(1);
                decimal min_temperature = (reader.IsDBNull(2) ? 0 : reader.GetDecimal(2));
                decimal max_temperature = (reader.IsDBNull(3) ? 0 : reader.GetDecimal(3));
                int harvest_days = reader.GetInt32(4);
                int capacity_use = reader.GetInt32(5);
                string container_type = reader.GetString(6);

                crop = new Crop(crop_id, crop_name, min_temperature, max_temperature, harvest_days, capacity_use, container_type);
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return crop;
        }
    }
}