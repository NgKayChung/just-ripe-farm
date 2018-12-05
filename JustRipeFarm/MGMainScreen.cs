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
    public partial class MGMainScreen : Form
    {
        public MGMainScreen()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            greeting_label.Text = "Welcome, " + UserSession.Instance.UserFirstName + "!";
            //LoadHome();
        }

        private void LoadHome()
        {
            List<Panel> section_panels = new List<Panel>
            {
                section1_panel,
                section2_panel,
                section3_panel,
                section4_panel,
                section5_panel,
                section6_panel,
                section7_panel,
                section8_panel,
                section9_panel
            };

            FarmSectionHandler farmSectionHandler = new FarmSectionHandler();
            List<FarmSection> farmSections = farmSectionHandler.FindFarmSections();

            for (int i = 0; i < section_panels.Count; i++)
            {
                Panel currentPanel = section_panels.ElementAt(i);
                FarmSection section = farmSections.ElementAt(i);

                if (section.Status == "CULTIVATING")
                {
                    currentPanel.BackColor = Color.Gold;
                }
                else if (section.Status == "HARVEST")
                {
                    currentPanel.BackColor = Color.LimeGreen;
                }
                else if (section.Status == "NOT USED")
                {
                    currentPanel.BackColor = Color.LightGray;
                }

                string sowDateString = (section.SowDate.Year == 1900 ? "-" : section.SowDate.ToShortDateString());
                string harvestDateString = (section.ExpHarvestDate.Year == 1900 ? "-" : section.ExpHarvestDate.ToShortDateString());

                currentPanel.Click += delegate (object sender1, EventArgs e1)
                {
                    section_panel_click(sender1, e1, section.CropName, "Status: " + section.Status + "\nSowed date: " + sowDateString + "\nExpected harvest date: " + harvestDateString, currentPanel);
                };
            }
        }

        private void section_panel_click(object sender, EventArgs e, string title, string text, Panel section_panel)
        {
            section_toolTip.ToolTipTitle = title;
            section_toolTip.Show(text, section_panel);
        }

        private void home_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = true;
            timetable_panel.Visible = false;
            storageTop_panel.Visible = false;
            labourerTop_panel.Visible = false;
            machineTop_panel.Visible = false;
            shopTop_panel.Visible = false;

            LoadHome();
        }

        private void timetable_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            timetable_panel.Visible = true;
            storageTop_panel.Visible = false;
            labourerTop_panel.Visible = false;
            machineTop_panel.Visible = false;
            shopTop_panel.Visible = false;
        }

        private void storage_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            timetable_panel.Visible = false;
            storageTop_panel.Visible = true;
            labourerTop_panel.Visible = false;
            machineTop_panel.Visible = false;
            shopTop_panel.Visible = false;
        }

        private void labour_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            timetable_panel.Visible = false;
            storageTop_panel.Visible = false;
            labourerTop_panel.Visible = true;
            machineTop_panel.Visible = false;
            shopTop_panel.Visible = false;
        }

        private void machine_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            timetable_panel.Visible = false;
            storageTop_panel.Visible = false;
            labourerTop_panel.Visible = false;
            machineTop_panel.Visible = true;
            shopTop_panel.Visible = false;

            listView1.Items.Clear();
            VehicleHandler vh = new VehicleHandler();

            List<Vehicle> lv = vh.getVehicle(DbConnector.Instance.getConn());

            foreach (Vehicle v in lv)
            {
                ListViewItem lvi = new ListViewItem(v.mac_id);

                lvi.SubItems.Add(v.mac_name);
                lvi.SubItems.Add(v.mac_man);
                lvi.SubItems.Add(v.mac_model);
                lvi.SubItems.Add(v.mac_desc);
                lvi.SubItems.Add(v.mac_type);
                lvi.SubItems.Add(v.mac_status);

                listView1.Items.Add(lvi);



            }

        }

        private void shop_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            timetable_panel.Visible = false;
            storageTop_panel.Visible = false;
            labourerTop_panel.Visible = false;
            machineTop_panel.Visible = false;
            shopTop_panel.Visible = true;
        }

        private void logout_button_Click(object sender, EventArgs e)
        {
            UserSession.Instance.LoggedIn = false;
            UserSession.Instance.UserID = "";
            UserSession.Instance.UserType = "";

            LoginScreen loginScreen = new LoginScreen();
            this.Hide();
            loginScreen.Show();
        }

        private void exit_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addLabourer_btn_Click(object sender, EventArgs e)
        {
            labourer_panel.Visible = false;
            addLabourer_panel.Visible = true;
        }

        private void submitAddLab_btn_Click(object sender, EventArgs e)
        {
            string first_name, last_name, email_address, phone_number;
            string u_type = "LABOURER";
            string evalEmail = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@[0-9a-zA-z]([-\\w]*[0-9a-zA-Z]\\.)+[a-zA-z]{2,9})$";

            Random random = new Random();
            string uID = "LB" + random.Next(10).ToString() + random.Next(10).ToString() + random.Next(10).ToString() + random.Next(10).ToString() + random.Next(10).ToString();
            first_name = addLabfName_txtBox.Text;
            last_name = addLablName_txtBox.Text;
            email_address = addLabEmail_txtBox.Text;
            phone_number = addLabPhoneNum_txtBox.Text;

            if (first_name != "")
            {
                if (last_name != "")
                {
                    if (email_address != "")
                    {
                        if (Regex.IsMatch(email_address, evalEmail))
                        {
                            Labourer labourer = new Labourer(uID, first_name, last_name, "DefaultPass", email_address, phone_number, u_type, DateTime.Today);
                            LabourerHandler labourerHandler = new LabourerHandler();
                            string result = labourerHandler.addNewLabourer(DbConnector.Instance.getConn(), labourer);
                            if (result.Equals("SUCCESS"))
                            {
                                MessageBox.Show("Labourer is added successfully!");
                            }
                            else
                            {
                                MessageBox.Show("Failed to add labourer. Please try again later");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid email address");
                        }
                    }
                    else
                    {
                        Labourer labourer = new Labourer(uID, first_name, last_name, "DefaultPass", email_address, phone_number, u_type, DateTime.Today);
                        LabourerHandler labourerHandler = new LabourerHandler();
                        string result = labourerHandler.addNewLabourer(DbConnector.Instance.getConn(), labourer);
                        if (result.Equals("SUCCESS"))
                        {
                            MessageBox.Show("Labourer is added successfully!");
                        }
                        else
                        {
                            MessageBox.Show("Failed to add labourer. Please try again later");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Last name cannot be empty");
                }

            }
            else
            {
                MessageBox.Show("First name cannot be empty");
            }
        }

        private void addLab_back_Click(object sender, EventArgs e)
        {
            labourer_panel.Visible = true;
            addLabourer_panel.Visible = false;

            addLabfName_txtBox.Clear();
            addLablName_txtBox.Clear();
            addLabEmail_txtBox.Clear();
            addLabPhoneNum_txtBox.Clear();
        }

        private void m_search_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            string Type;
            Type = (string)comboBox1.SelectedItem;
            VehicleHandler vh = new VehicleHandler();
            

            List<Vehicle> lv = vh.getSelected(DbConnector.Instance.getConn(), Type);

            foreach (Vehicle v in lv)
            {
                ListViewItem lvi = new ListViewItem(v.mac_id);

                lvi.SubItems.Add(v.mac_name);
                lvi.SubItems.Add(v.mac_man);
                lvi.SubItems.Add(v.mac_model);
                lvi.SubItems.Add(v.mac_desc);
                lvi.SubItems.Add(v.mac_type);
                lvi.SubItems.Add(v.mac_status);

                listView1.Items.Add(lvi);

            }
            
        }

        private void edit_btn_Click(object sender, EventArgs e)
        {
            
            home_panel.Visible = false;
            timetable_panel.Visible = false;
            storageTop_panel.Visible = false;
            labourerTop_panel.Visible = false;
            machineTop_panel.Visible = false;
            shopTop_panel.Visible = false;
            panel1.Visible = true;

            

            labelID.Text = UserSession.Instance.UserID;
            txtFname.Text = UserSession.Instance.UserFirstName;
            txtLname.Text = UserSession.Instance.UserLastName;
            txtmail.Text = UserSession.Instance.Email;
            txtNumber.Text = UserSession.Instance.PhoneNumber;
        }

        private void udt_btn_Click(object sender, EventArgs e)
        {
            


            User u = new User();
            u.Firstname = txtFname.Text;
            u.Lastname = txtLname.Text;
            u.EmailAddress = txtmail.Text;
            u.PhoneNumber = txtNumber.Text;


            

            string evalEmail = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@[0-9a-zA-z]([-\\w]*[0-9a-zA-Z]\\.)+[a-zA-z]{2,9})$";
            if (txtFname.Text != "")
            {
                if (txtLname.Text != "")
                {
                    if (txtmail.Text != "")
                    {
                        if (Regex.IsMatch(txtmail.Text, evalEmail))
                        {
                            LabourerHandler lbh = new LabourerHandler();
                            lbh.Updateinfo(DbConnector.Instance.getConn(), u);
                            MessageBox.Show("Update Succesfull");
                        }
                        else
                            MessageBox.Show("Invalid email format");
                    }
                }
                else
                    MessageBox.Show("Last name cannot be empty");
            }
            else
                MessageBox.Show("First Name cannot be empty");
            

        }

        private void udt_pas_Click(object sender, EventArgs e)
        {
            string opass = old_pass.Text;
            string npass = New_pass.Text;
            string cpass = Con_pass.Text;

            User u = new User();
            u.Password = npass;
            

            if (opass == UserSession.Instance.Password)
            {
                if (npass == cpass)
                {
                    LabourerHandler lbh = new LabourerHandler();
                    lbh.changePass(DbConnector.Instance.getConn(), u);
                    MessageBox.Show("Password change successfull");
                }
                else
                {
                    MessageBox.Show("New password does not match confirm password");
                }
            }
            else
            {
                MessageBox.Show("Invalid password");
            }
        }
    }
}