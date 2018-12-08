using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    class TruckHandler
    {
        public List<Truck> GetTrucks()
        {
            List<Truck> trucks = null;
            string sql = "SELECT `machinery`.`machinery_id`, `machinery`.`machine_name`, `machinery`.`manufacturer`, `machinery`.`model`, `machinery`.`description`, `machinery`.`machine_type`, `machinery`.`status`, `trucks`.`total_quantity`, `trucks`.`quantity_in_use`, `trucks`.`container_quantity`, CONVERT(`trucks`.`time_required`, DATETIME) FROM `trucks` " +
                    "INNER JOIN `machinery` ON `machinery`.`machinery_id` = `trucks`.`truck_id` " +
                    "ORDER BY `trucks`.`container_quantity` DESC;";

            MySqlCommand sqlComm = new MySqlCommand(sql, DbConnector.Instance.getConn());
            MySqlDataReader reader = sqlComm.ExecuteReader();

            if (reader.HasRows)
            {
                trucks = new List<Truck>();

                while (reader.Read())
                {
                    string machine_id = reader.GetString(0);
                    string machine_name = reader.GetString(1);
                    string manufacturer = reader.GetString(2);
                    string machine_model = reader.GetString(3);
                    string description = reader.GetString(4);
                    string machine_type = reader.GetString(5);
                    string machine_status = reader.GetString(6);
                    int total_quantity = reader.GetInt32(7);
                    int quantity_in_use = reader.GetInt32(8);
                    int container_quantity = reader.GetInt32(9);
                    DateTime time_required = reader.GetDateTime(10);

                    trucks.Add(new Truck(machine_id, machine_name, manufacturer, machine_model, description, machine_type, machine_status, total_quantity, quantity_in_use, container_quantity, time_required));
                }
            }

            if (!reader.IsClosed) reader.Close();
            sqlComm.Dispose();

            return trucks;
        }

        public int UpdateTruckQuantity(Truck truck, int newQuantity)
        {
            string sql = "UPDATE `trucks` SET `trucks`.`quantity_in_use` = '" + newQuantity + "' WHERE `trucks`.`truck_id` = '" + truck.mac_id + "';";

            MySqlCommand sqlComm = new MySqlCommand(sql, DbConnector.Instance.getConn());
            return sqlComm.ExecuteNonQuery();
        }
    }
}