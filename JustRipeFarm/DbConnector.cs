using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    public class DbConnector
    {
        MySqlConnection conn;
        
        private static DbConnector instance;

        private DbConnector()
        {
            //initialise the objects
        }

        public static DbConnector Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DbConnector();
                }
                return instance;
            }
        }

        public string connect()
        {
            string connStr = "server=localhost; user=root;database=justripe_farm_db;port=3306;password=";
            conn = new MySqlConnection(connStr);
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "Done";
        }

        public MySqlConnection getConn()
        {
            return conn;
        }
    }
}