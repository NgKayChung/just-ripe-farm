using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JustRipeFarm
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {

            string user_id = LoginID.Text;
            string password = LoginPassword.Text;

            if ((user_id == "") || (password == ""))
            {
                MessageBox.Show("Login ID and password cannnot be empty");
            }
            else
            {
                DbConnector.Instance.connect();
                
                MySqlCommand cmd = new MySqlCommand("select user_type from users where user_id='" + user_id + "' and secret_password='" + password + "'", DbConnector.Instance.getConn());
                
                MySqlDataReader rd = cmd.ExecuteReader();
                
                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);

                if (rd.HasRows)
                {
                    rd.Read();
                    UserClass.Instance.Loggerin = true;
                    UserClass.Instance.Username = user_id;
                    UserClass.Instance.Usertype = rd.GetString("user_type");

                    if (UserClass.Instance.Usertype.Equals("MANAGER"))
                    {
                        Main ts = new Main();
                        this.Hide();
                        ts.Show();
                    }
                    else
                    {
                        MessageBox.Show("Welcome LABOURER");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid login ID and password");
                }
            }
        }
    }
}