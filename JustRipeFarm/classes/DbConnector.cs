using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    public class DbConnector
    {
        MySqlConnection conn = null;
        
        private static DbConnector instance;

        private DbConnector()
        {
            
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

        public MySqlConnection getConn()
        {
            if (conn == null)
            {
                string connStr = "server=localhost; user=root; database=justripe_farm_db; port=3306; password=";
                conn = new MySqlConnection(connStr);

                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            return conn;
        }
    }
}