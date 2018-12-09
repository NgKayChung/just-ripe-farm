using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
   public class VehicleHandler
    {
        public List<Vehicle> getVehicle(MySqlConnection conn)
        {
            List<Vehicle> vl = null;
            string sql = "SELECT * FROM `machinery`";

            MySqlCommand sqlComm = new MySqlCommand(sql, conn);
            MySqlDataReader reader = sqlComm.ExecuteReader();

            if (reader.HasRows)
            {
                vl = new List<Vehicle>();

                while (reader.Read())
                {
                    string macid = reader.GetString("machinery_id");
                    string macname = reader.GetString("machine_name");
                    string macman = reader.GetString("manufacturer");
                    string mac_mod = reader.GetString("model");
                    string macdesc = reader.GetString("description");
                    string mactype = reader.GetString("machine_type");
                    string mac_stat = reader.GetString("status");

                    Vehicle v = new Vehicle(macid, macname, macman, mac_mod, macdesc, mactype, mac_stat);

                    vl.Add(v);
                }
            }

            sqlComm.Dispose();
            reader.Close();

            return vl;
        }

        public List<Vehicle> getSelected(MySqlConnection conn, string type)
        {
            List<Vehicle> vl = null;
            string sql = "SELECT * FROM `machinery` WHERE `machine_type` ='"+type+"'";

            MySqlCommand sqlComm = new MySqlCommand(sql, conn);
            MySqlDataReader reader = sqlComm.ExecuteReader();

            if (reader.HasRows)
            {
                vl = new List<Vehicle>();

                while (reader.Read())
                {
                    string macid = reader.GetString("machinery_id");
                    string macname = reader.GetString("machine_name");
                    string macman = reader.GetString("manufacturer");
                    string mac_mod = reader.GetString("model");
                    string macdesc = reader.GetString("description");
                    string mactype = reader.GetString("machine_type");
                    string mac_stat = reader.GetString("status");

                    Vehicle v = new Vehicle(macid, macname, macman, mac_mod, macdesc, mactype, mac_stat);

                    vl.Add(v);
                }
            }

            sqlComm.Dispose();
            reader.Close();

            return vl;
        }
    }
}