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
    public partial class LoginScreen : Form
    {
        public LoginScreen()
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
                MySqlCommand cmd = new MySqlCommand("SELECT user_type from users where user_id = '" + user_id + "' and secret_password = '" + password + "'", DbConnector.Instance.getConn());
                
                MySqlDataReader rd = cmd.ExecuteReader();
                
                if(rd.HasRows)
                {
                    rd.Read();
                    UserSession.Instance.LoggedIn = true;
                    UserSession.Instance.UserID = user_id;
                    UserSession.Instance.UserType = rd.GetString("user_type");

                    if(UserSession.Instance.UserType.Equals("MANAGER"))
                    {
                        MGMainScreen ts = new MGMainScreen();
                        this.Hide();
                        ts.Show();
                    }
                    else if(UserSession.Instance.UserType.Equals("LABOURER"))
                    {
                        MGMainScreen ts = new MGMainScreen();
                        this.Hide();
                        ts.Show();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect login");
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