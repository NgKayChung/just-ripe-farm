using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class WholesaleHandler
    {
        public List<Wholesale> GetAllWholesales(DateTime fromDate, DateTime toDate)
        {
            List<Wholesale> wholesales = null;

            string sqlWholesaleQuery = "SELECT `wholesale`.`wholesale_id`, `wholesale`.`start_date`, `wholesale`.`end_date`, `wholesale`.`assigned_date`, `wholesale`.`manager_id`, `users`.`first_name`, `users`.`last_name` FROM `wholesale` " +
                    "INNER JOIN `users` ON `wholesale`.`manager_id` = `users`.`user_id` " +
                    "WHERE DATE(`start_date`) >= '" + fromDate.ToString("yyyy-MM-dd") + "' AND DATE(`start_date`) <= '" + toDate.ToString("yyyy-MM-dd") + "'";

            MySqlCommand sqlCommand = new MySqlCommand(sqlWholesaleQuery, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.HasRows)
            {
                wholesales = new List<Wholesale>();
                while (reader.Read())
                {
                    string wholesaleID = reader.GetString(0);
                    DateTime startDateTime = reader.GetDateTime(1);
                    DateTime endDateTime = reader.GetDateTime(2);
                    DateTime assignDateTime = reader.GetDateTime(3);
                    string managerID = reader.GetString(4);
                    string managerFName = reader.GetString(5);
                    string managerLName = reader.GetString(6);

                    Wholesale wholesale = new Wholesale();
                    wholesale.WholesaleID = wholesaleID;
                    wholesale.StartDateTime = startDateTime;
                    wholesale.EndDateTime = endDateTime;
                    wholesale.AssignedDateTime = assignDateTime;
                    wholesale.Manager = new User(uID: managerID, fName: managerFName, lName: managerLName);

                    wholesales.Add(wholesale);
                }
            }

            if (!reader.IsClosed) reader.Close();

            if(wholesales != null)
            {
                foreach (Wholesale wholesale in wholesales)
                {
                    string sqlContainerQuery = "SELECT `wholesale_container`.`container_id`, `containers`.`container_type`, `wholesale_container`.`crop_id`, `crops`.`crop_name` FROM `wholesale_container` " +
                        "INNER JOIN `containers` ON `wholesale_container`.`container_id` = `containers`.`container_id` " +
                        "INNER JOIN `crops` ON `wholesale_container`.`crop_id` = `crops`.`crop_id` " +
                        "WHERE `wholesale_container`.`wholesale_id` = '" + wholesale.WholesaleID + "'; ";
                    sqlCommand.CommandText = sqlContainerQuery;
                    reader = sqlCommand.ExecuteReader();
                    List<Container> containers = new List<Container>();

                    while(reader.Read())
                    {
                        string containerID = reader.GetString(0);
                        string containerType = reader.GetString(1);
                        string cropID = reader.GetString(2);
                        string cropName = reader.GetString(3);

                        containers.Add(new Container(cont_id: containerID, cont_type: containerType, crop: new Crop(crop_id: cropID, crop_name: cropName)));
                    }
                    wholesale.Containers = containers;

                    if (!reader.IsClosed) reader.Close();

                    string sqlTruckQuery = "SELECT `wholesale_truck`.`truck_id`, `machinery`.`machine_name`, `trucks`.`container_quantity`, `wholesale_truck`.`quantity_use` FROM `wholesale_truck` " +
                            "INNER JOIN `trucks` ON `wholesale_truck`.`truck_id` = `trucks`.`truck_id` " +
                            "INNER JOIN `machinery` ON `wholesale_truck`.`truck_id` = `machinery`.`machinery_id`; ";
                    sqlCommand.CommandText = sqlTruckQuery;
                    reader = sqlCommand.ExecuteReader();
                    List<Truck> trucks = new List<Truck>();
                    List<int> truckQuantity = new List<int>();

                    while(reader.Read())
                    {
                        string truck_id = reader.GetString(0);
                        string truck_name = reader.GetString(1);
                        int container_quantity = reader.GetInt32(2);
                        int truck_quantity = reader.GetInt32(3);

                        trucks.Add(new Truck(machine_id: truck_id, machine_name: truck_name, container_quantity: container_quantity));
                        truckQuantity.Add(truck_quantity);
                    }
                    wholesale.Trucks = trucks;
                    wholesale.TrucksQuantity = truckQuantity;

                    if (!reader.IsClosed) reader.Close();
                }
            }

            sqlCommand.Dispose();

            return wholesales;
        }

        public string SendWholesale(Wholesale wholesale)
        {
            MySqlTransaction tr = null;

            try
            {
                tr = DbConnector.Instance.getConn().BeginTransaction();

                MySqlCommand sqlComm = DbConnector.Instance.getConn().CreateCommand();
                sqlComm.Connection = DbConnector.Instance.getConn();
                sqlComm.Transaction = tr;

                string sqlInsertWholesaleQuery = "INSERT INTO `wholesale`(`wholesale_id`, `start_date`, `end_date`, `assigned_date`, `manager_id`) VALUES ('" + wholesale.WholesaleID + "', '" + wholesale.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + wholesale.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + wholesale.AssignedDateTime.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + wholesale.Manager.UserID + "');";
                sqlComm.CommandText = sqlInsertWholesaleQuery;
                sqlComm.ExecuteNonQuery();

                foreach (Container container in wholesale.Containers)
                {
                    string sqlInsertContainerQuery = "INSERT INTO `wholesale_container`(`wholesale_id`, `container_id`, `crop_id`) VALUES ('" + wholesale.WholesaleID + "', '" + container.ContainerID + "', '" + container.Crop.CropID + "');";
                    sqlComm.CommandText = sqlInsertContainerQuery;
                    sqlComm.ExecuteNonQuery();
                }

                for (int i = 0; i < wholesale.Trucks.Count; i++)
                {
                    string sqlInsertTruckQuery = "INSERT INTO `wholesale_truck`(`wholesale_id`, `truck_id`, `quantity_use`) VALUES ('" + wholesale.WholesaleID + "', '" + wholesale.Trucks[i].mac_id + "', " + wholesale.TrucksQuantity[i] + ");";
                    sqlComm.CommandText = sqlInsertTruckQuery;
                    sqlComm.ExecuteNonQuery();
                }

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
    }
}