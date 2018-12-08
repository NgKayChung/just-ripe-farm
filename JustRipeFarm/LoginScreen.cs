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
            login_panel.BackColor = Color.FromArgb(80, Color.White);
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            string user_id = loginID_txtBox.Text.Trim();
            string password = loginPassword_txtBox.Text.Trim();

            if ((user_id == "") || (password == ""))
            {
                MessageBox.Show("Login ID and password cannnot be empty");
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * from `users` where `user_id` = BINARY '" + user_id + "' and `secret_password` = BINARY '" + password + "'", DbConnector.Instance.getConn());
                MySqlDataReader rd = cmd.ExecuteReader();
                
                if(rd.HasRows)
                {
                    rd.Read();
                    UserSession.Instance.LoggedIn = true;
                    UserSession.Instance.UserID = user_id;
                    UserSession.Instance.UserFirstName = rd.GetString("first_name");
                    UserSession.Instance.UserType = rd.GetString("user_type");
                    rd.Close();

                    if(UserSession.Instance.UserType.Equals("MANAGER"))
                    {
                        MGMainScreen mainScreen = new MGMainScreen();
                        mainScreen.Show();
                        this.Close();
                    }
                    else if(UserSession.Instance.UserType.Equals("LABOURER"))
                    {
                        MGMainScreen mainScreen = new MGMainScreen();
                        mainScreen.Show();
                        this.Close();
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

                rd.Close();
            }
        }
    }
}