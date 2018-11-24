using MySql.Data.MySqlClient;
using Panel;
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
    public partial class LoginButton : Form
    {
        public LoginButton()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {

            string username = LoginID.Text;
            string password = LoginPassword.Text;

            if ((username == "") && (password == ""))
            {
                MessageBox.Show("User name and password cannnot  be emprty");
            }
            else
            {




                DbConnector.Instance.connect();



                MySqlCommand cmd = new MySqlCommand("select username,password from login where username='" + username + "'and password='" + password + "'", DbConnector.Instance.getConn());



                MySqlDataReader rd = cmd.ExecuteReader();


                MySqlDataAdapter sda = new MySqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //sda.Fill(rd);

                if (rd.HasRows)
                {
                    UserClass.Instance.Loggerin = true;
                    UserClass.Instance.Username = username;

                    if (username.StartsWith("a"))
                    {
                        Testing_page ts = new Testing_page();
                        this.Hide();
                        ts.Show();
                    }
                    else
                    {
                        MessageBox.Show("Welcome adam");
                    }

                }
                else
                {
                    MessageBox.Show("Invalid username and password");
                }




            }
        }
    }
}
