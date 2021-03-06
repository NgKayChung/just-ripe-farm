﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class ContainerHandler
    {
        // function to get all containers for with type
        // specifying type is optional
        public List<Container> GetAllContainers(string cont_type = "")
        {
            List<Container> containers = null;
            string sqlString = "SELECT `containers`.`container_id`, `containers`.`container_type`, `containers`.`total_capacity`, `containers`.`status`, `containers`.`crop_id`, `crops`.`crop_name`, `crops`.`min_temperature`, `crops`.`max_temperature`, `crops`.`harvest_days`, `crops`.`capacity_use`, `crops`.`container_type` FROM `containers` " +
                    "LEFT JOIN `crops` ON `containers`.`crop_id` = `crops`.`crop_id`";
            int sqlLength = sqlString.Length;
            if (UserSession.Instance.UserType == "LABOURER") sqlString += " WHERE `containers`.`status` = 'AVAILABLE'";

            if(cont_type != "")
            {
                if (sqlString.Length != sqlLength) sqlString += " AND `containers`.`container_type` = '" + cont_type + "'";
                else sqlString += " WHERE `containers`.`container_type` = '" + cont_type + "'";
            }
            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                containers = new List<Container>();
                while (reader.Read())
                {
                    string container_id = reader.GetString(0);
                    string type = reader.GetString(1);
                    int capacity = reader.GetInt32(2);
                    string status = reader.GetString(3);

                    Crop crop = null;
                    if(reader.IsDBNull(4))
                    {
                        crop = new Crop("", "", 0, 0, 0, 0, "");
                    }
                    else
                    {
                        string crop_id = reader.GetString(4);
                        string crop_name = reader.GetString(5);
                        decimal min_temperature = (reader.IsDBNull(6) ? 0 : reader.GetDecimal(6));
                        decimal max_temperature = (reader.IsDBNull(7) ? 0 : reader.GetDecimal(7));
                        int harvest_days = reader.GetInt32(8);
                        int capacity_use = reader.GetInt32(9);
                        string container_type = reader.GetString(10);

                        crop = new Crop(crop_id, crop_name, min_temperature, max_temperature, harvest_days, capacity_use, container_type);
                    }
                    
                    containers.Add(new Container(container_id, type, capacity, status, crop));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return containers;
        }

        // function to get all containers used for harvesting task
        public List<Container> GetContainersForTask(int task_id)
        {
            List<Container> containers = null;
            string sqlString = "SELECT `containers`.`container_id`, `containers`.`container_type` FROM `task_container` " +
                    "INNER JOIN `containers` ON `task_container`.`container_id` = `containers`.`container_id` " +
                    "WHERE `task_container`.`task_id` = " + task_id.ToString() + ";";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                containers = new List<Container>();
                while (reader.Read())
                {
                    string container_id = reader.GetString(0);
                    string container_type = reader.GetString(1);
                    
                    containers.Add(new Container(container_id, container_type, 0, "", new Crop()));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return containers;
        }

        // function to get all containers for wholesale
        // containers which status is 'FULL'
        public List<Container> GetWholesaleContainers()
        {
            List<Container> containers = null;
            string sqlString = "SELECT `containers`.`container_id`, `containers`.`container_type`, `containers`.`total_capacity`, `containers`.`status`, `containers`.`crop_id`, `crops`.`crop_name`, `crops`.`min_temperature`, `crops`.`max_temperature`, `crops`.`harvest_days`, `crops`.`capacity_use`, `crops`.`container_type` FROM `containers` " +
                    "LEFT JOIN `crops` ON `containers`.`crop_id` = `crops`.`crop_id` " +
                    "WHERE `containers`.`status` = 'FULL';";

            MySqlCommand sqlCommand = new MySqlCommand(sqlString, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                containers = new List<Container>();
                while (reader.Read())
                {
                    string container_id = reader.GetString(0);
                    string type = reader.GetString(1);
                    int container_capacity = reader.GetInt32(2);
                    string status = reader.GetString(3);
                    string crop_id = reader.GetString(4);
                    string crop_name = reader.GetString(5);
                    decimal min_temperature = (reader.IsDBNull(6) ? 0 : reader.GetDecimal(6));
                    decimal max_temperature = (reader.IsDBNull(7) ? 0 : reader.GetDecimal(7));
                    int harvest_days = reader.GetInt32(8);
                    int capacity_use = reader.GetInt32(9);
                    string container_type = reader.GetString(10);
                    
                    containers.Add(new Container(container_id, type, container_capacity, status, new Crop(crop_id, crop_name, min_temperature, max_temperature, harvest_days, capacity_use, container_type)));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlCommand.Dispose();

            return containers;
        }

        // function to update containers for use in storing harvest
        public int UseContainer(int task_id, Container container, string crop_id)
        {
            string sql = "UPDATE `containers` SET `containers`.`status` = 'FULL', `containers`.`crop_id` = '" + crop_id + "' WHERE `containers`.`container_id` = '" + container.ContainerID + "';";
            MySqlCommand sqlComm = new MySqlCommand(sql, DbConnector.Instance.getConn());
            sqlComm.ExecuteNonQuery();

            sqlComm.CommandText = "INSERT INTO `task_container`() VALUES(" + task_id.ToString() + ", '" + container.ContainerID + "')";
            return sqlComm.ExecuteNonQuery();
        }
    }
}