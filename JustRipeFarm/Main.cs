using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JustRipeFarm
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void labour_button_Click(object sender, EventArgs e)
        {
            lbrpanel.Show();
        }

        private void add_lbr_Click(object sender, EventArgs e)
        {
            string fname, lname, lID, email, pnumber, pass;
            string type = "labourer";
            string evalid= "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@[0-9a-zA-z]([-\\w]*[0-9a-zA-Z]\\.)+[a-zA-z]{2,9})$";
fname = fname_box.Text;
            lname = lname_box.Text;
            lID = id_box.Text;
            email = email_box.Text;
            pnumber = pnumber_box.Text;
            pass = pass_box.Text;

            if (lID != "")
            {
                if(fname !="" )
                {
                    if(lname !="")
                    {
                        if(pass != "")
                        {
                            if(email !="")
                            {
                                if(Regex.IsMatch(email,evalid))
                                {
                                    DbConnector.Instance.connect();

                                   


                                    string sql = "INSERT INTO users(user_id,first_name,last_name,secret_password,email_address,phone_number,user_type)" + "VALUES ('" + lID + "','" + fname + "','" + lname + "','" + pass + "','" + email + "','" + pnumber + "','" + type + "')";
                                   

                                    MySqlCommand sqlComm = new MySqlCommand(sql, DbConnector.Instance.getConn());
                                    sqlComm.ExecuteNonQuery();
                                    MessageBox.Show("Added successfull");
                                    
                                }
                                else
                                {
                                    MessageBox.Show("Invalid Email address");
                                }

                            }
                            else
                            {
                                MessageBox.Show("Email cannot be empty");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Password cannot be empty");

                        }
                    }
                    else
                    {
                        MessageBox.Show("LastName cannot be empty");
                    }

                }
                else
                {
                    MessageBox.Show("FirstName cannot be empty");
                }
            }
            else
            {
                MessageBox.Show("ID cannot be empty");
            }
        }
    }
}
