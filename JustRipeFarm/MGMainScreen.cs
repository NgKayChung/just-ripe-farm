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
    public partial class MGMainScreen : Form
    {
        public MGMainScreen()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            LoadHome();
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

            for(int i = 0;i < section_panels.Count;i++)
            {
                Panel currentPanel = section_panels.ElementAt(i);
                FarmSection section = farmSections.ElementAt(i);
                    
                if(section.Status == "CULTIVATING")
                {
                    currentPanel.BackColor = Color.Gold;
                }
                else if(section.Status == "HARVEST")
                {
                    currentPanel.BackColor = Color.LimeGreen;
                }
                else if(section.Status == "NOT USED")
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

            LoadHome();
        }

        private void timetable_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            timetable_panel.Visible = true;
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
    }
}