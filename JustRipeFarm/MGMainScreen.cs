﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace JustRipeFarm
{
    public partial class MGMainScreen : Form
    {
        private const string EVAL_EMAIL = "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@[0-9a-zA-z]([-\\w]*[0-9a-zA-Z]\\.)+[a-zA-z]{2,9})$";

        public MGMainScreen()
        {
            UserSession.Instance.UserID = "MG18290";
            UserSession.Instance.UserFirstName = "John";
            UserSession.Instance.UserType = "MANAGER";
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            greeting_label.Text = "Welcome, " + UserSession.Instance.UserFirstName + "!";
            LoadHome();
        }

        private void home_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = true;
            timetable_panel.Visible = false;
            storageTop_panel.Visible = false;
            labourerTop_panel.Visible = false;
            machineTop_panel.Visible = false;
            shopWholesaleTop_panel.Visible = false;
            profile_panel.Visible = false;

            LoadHome();
        }

        private void timetable_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            timetable_panel.Visible = true;
            storageTop_panel.Visible = false;
            labourerTop_panel.Visible = false;
            machineTop_panel.Visible = false;
            shopWholesaleTop_panel.Visible = false;
            profile_panel.Visible = false;
        }

        private void storage_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            timetable_panel.Visible = false;
            storageTop_panel.Visible = true;
            labourerTop_panel.Visible = false;
            machineTop_panel.Visible = false;
            shopWholesaleTop_panel.Visible = false;
            profile_panel.Visible = false;

            LoadStockStorage();
        }

        private void labour_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            timetable_panel.Visible = false;
            storageTop_panel.Visible = false;
            labourerTop_panel.Visible = true;
            machineTop_panel.Visible = false;
            shopWholesaleTop_panel.Visible = false;
            profile_panel.Visible = false;

            LoadLabourers();
        }

        private void machine_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            timetable_panel.Visible = false;
            storageTop_panel.Visible = false;
            labourerTop_panel.Visible = false;
            machineTop_panel.Visible = true;
            shopWholesaleTop_panel.Visible = false;
            profile_panel.Visible = false;
        }

        private void shopWholesale_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            timetable_panel.Visible = false;
            storageTop_panel.Visible = false;
            labourerTop_panel.Visible = false;
            machineTop_panel.Visible = false;
            shopWholesaleTop_panel.Visible = true;
            profile_panel.Visible = false;
        }

        private void edit_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            timetable_panel.Visible = false;
            storageTop_panel.Visible = false;
            labourerTop_panel.Visible = false;
            machineTop_panel.Visible = false;
            shopWholesaleTop_panel.Visible = false;
            profile_panel.Visible = true;

            LoadUserProfileInfo();
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

        private void section_panel_click(object sender, EventArgs e, string title, string text, Panel section_panel)
        {
            section_toolTip.ToolTipTitle = title;
            section_toolTip.Show(text, section_panel);
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

            Random random = new Random();
            first_name = addLabfName_txtBox.Text;
            last_name = addLablName_txtBox.Text;
            email_address = addLabEmail_txtBox.Text;
            phone_number = addLabPhoneNum_txtBox.Text;

            if (first_name != "")
            {
                if (last_name != "")
                {
                    LabourerHandler labourerHandler = new LabourerHandler();
                    string currID = "";

                    if (Regex.IsMatch(email_address, EVAL_EMAIL))
                    {
                        currID = findNewLabIDNum(labourerHandler);
                        string uID = "LB" + currID;
                        string password = "JRF@" + currID;

                        Labourer labourer = new Labourer(uID, first_name, last_name, password, email_address, phone_number, u_type, DateTime.Today);
                        string result = labourerHandler.AddNewLabourer(labourer);

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
                    MessageBox.Show("Last name cannot be empty");
                }
            }
            else
            {
                MessageBox.Show("First name cannot be empty");
            }
        }

        private void assignTaskSubmit_btn_Click(object sender, EventArgs e)
        {
            DateTime taskStartDateTime = taskStartDate_datePicker.Value.Date + taskStartTime_datePicker.Value.TimeOfDay;
            DateTime taskEndDateTime = taskEndDate_datePicker.Value.Date + taskEndTime_datePicker.Value.TimeOfDay;
            string taskTitle = taskTitle_txtBox.Text.Trim();
            string taskDescription = taskDesc_txtBox.Text.Trim();

            if (taskStartDateTime.CompareTo(taskEndDateTime) < 0)
            {
                if (!taskTitle.Equals(""))
                {
                    if (!taskDescription.Equals(""))
                    {
                        if (assignTaskLab_listBox.SelectedItems.Count > 0 && assignTaskLab_listBox.SelectedItems.Count >= (int)assignTaskWarningMsg_label.Tag)
                        {
                            FarmSection taskSelectedField = (FarmSection)taskField_comboBox.SelectedItem;
                            string taskType = taskType_txtBox.Text;
                            Crop taskSelectedCrop = (Crop)taskCrop_comboBox.SelectedItem;
                            CropMethod taskSelectdCropMethod = (CropMethod)taskMethod_comboBox.SelectedItem;
                            List<Labourer> taskSelectedLabourers = new List<Labourer>();
                            List<TaskStock> taskSelectedStocks = new List<TaskStock>();

                            for (int i = 0; i < assignTaskLab_listBox.SelectedItems.Count; i++)
                            {
                                taskSelectedLabourers.Add((Labourer)assignTaskLab_listBox.SelectedItems[i]);
                            }

                            switch(taskType)
                            {
                                case "SOW":
                                    Stock seed, sow_fertiliser;

                                    if(taskSeed_comboBox.SelectedItem != null)
                                    {
                                        seed = ((Stock)taskSeed_comboBox.SelectedItem);

                                        if (taskSeed_numeric.Value > 0 || taskSeed_numeric.Value <= seed.Quantity)
                                        {
                                            if (taskFertiliser_comboBox.SelectedItem != null)
                                            {
                                                sow_fertiliser = ((Stock)taskFertiliser_comboBox.SelectedItem);

                                                if (taskFertiliser_numeric.Value > 0 || taskFertiliser_numeric.Value <= sow_fertiliser.Quantity)
                                                {
                                                    taskSelectedStocks.Add(new TaskStock(seed, (int)taskSeed_numeric.Value));
                                                    taskSelectedStocks.Add(new TaskStock(sow_fertiliser, (int)taskFertiliser_numeric.Value));
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Please input valid fertiliser quantity for sowing");
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Please select fertiliser for sowing");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please input valid seed quantity for sowing");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Please select seed for sowing");
                                        return;
                                    }
                                    break;
                                
                                case "TREATMENT":
                                    Stock treatment_fertiliser, pesticide = null;

                                    if (taskFertiliser_comboBox.SelectedItem != null)
                                    {
                                        treatment_fertiliser = ((Stock)taskFertiliser_comboBox.SelectedItem);

                                        if (taskFertiliser_numeric.Value > 0 || taskFertiliser_numeric.Value <= treatment_fertiliser.Quantity)
                                        {
                                            if (taskFertiliser_comboBox.SelectedItem != null)
                                            {
                                                pesticide = ((Stock)taskPesticide_comboBox.SelectedItem);

                                                if (taskPesticide_numeric.Value <= 0 || taskPesticide_numeric.Value > pesticide.Quantity)
                                                {
                                                    MessageBox.Show("Please input valid fertiliser quantity for sowing");
                                                    return;
                                                }
                                            }

                                            if (taskEndDateTime.TimeOfDay.Subtract(taskStartDateTime.TimeOfDay).Days >= 7)
                                            {
                                                taskSelectedStocks.Add(new TaskStock(treatment_fertiliser, (int)taskFertiliser_numeric.Value));
                                                
                                                if (pesticide != null) {
                                                    taskSelectedStocks.Add(new TaskStock(pesticide, (int)taskPesticide_numeric.Value));
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Please select task time at least 7 days");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Please input valid fertiliser quantity for treating crops");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Please select fertiliser for treating crops");
                                        return;
                                    }
                                    break;
                                
                                default:
                                    break;
                            }

                            Task newTask = new Task(taskTitle, taskType, taskDescription, "PENDING", taskStartDateTime, taskEndDateTime, taskSelectedField.SectionID, taskSelectedCrop.CropID, taskSelectdCropMethod.MethodID, DateTime.Now, UserSession.Instance.UserID);

                            string insertResult = new TaskHandler().AddNewTask(newTask, taskSelectedLabourers, taskSelectedStocks);
                            Console.WriteLine(insertResult);
                            if (insertResult.Equals("SUCCESS"))
                            {
                                MessageBox.Show("New task is successfully assigned");
                                LabourerBackTopPanel();
                            }
                            else
                            {
                                MessageBox.Show("Error occurred while assigning task, please try again later");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please select at least " + ((int)assignTaskWarningMsg_label.Tag).ToString() + " labourer(s)");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Task description should not be empty");
                    }
                }
                else
                {
                    MessageBox.Show("Task title should not be empty");
                }
            }
            else
            {
                MessageBox.Show("Task start time should be earlier than end time");
            }
        }

        private void ResetAssignTaskPage()
        {
            taskStartDate_datePicker.ResetText();
            taskStartTime_datePicker.ResetText();
            taskEndDate_datePicker.ResetText();
            taskEndTime_datePicker.ResetText();
            taskTitle_txtBox.Clear();
            taskDesc_txtBox.Clear();
            taskField_comboBox.Items.Clear();
            taskType_txtBox.Clear();
            taskCrop_comboBox.Items.Clear();
            taskCrop_comboBox.Enabled = false;
            taskMethod_comboBox.Items.Clear();
            taskMethod_comboBox.Enabled = false;
            taskSeed_comboBox.Items.Clear();
            taskSeed_comboBox.Enabled = false;
            taskSeed_numeric.ResetText();
            taskSeed_numeric.Enabled = false;
            seedStockQty_label.ResetText();
            taskFertiliser_comboBox.Items.Clear();
            taskFertiliser_comboBox.Enabled = false;
            taskFertiliser_numeric.ResetText();
            taskFertiliser_numeric.Enabled = false;
            fertiliserStockQty_label.ResetText();
            taskPesticide_comboBox.Items.Clear();
            taskPesticide_comboBox.Enabled = false;
            taskPesticide_numeric.ResetText();
            taskPesticide_numeric.Enabled = false;
            pesticideStockQty_label.ResetText();
            assignTaskLab_listBox.Items.Clear();
            assignTaskWarningMsg_label.ResetText();
        }

        private void labourerAssignTask_btn_Click(object sender, EventArgs e)
        {
            labourer_panel.Visible = false;
            assignTask_panel.Visible = true;

            PopulateAssignTaskInputOptions();
        }

        private void storageContainer_btn_Click(object sender, EventArgs e)
        {
            storage_panel.Visible = false;
            container_panel.Visible = true;

            LoadContainers();
        }

        private void wholesale_btn_Click(object sender, EventArgs e)
        {
            shopWholesale_panel.Visible = false;
            wholesale_panel.Visible = true;

            LoadWholesale();
        }

        private void shopProducts_btn_Click(object sender, EventArgs e)
        {
            shop_panel.Visible = false;
            shopProducts_panel.Visible = true;

            LoadProducts();
        }

        private void shop_btn_Click(object sender, EventArgs e)
        {
            shopWholesale_panel.Visible = false;
            shop_panel.Visible = true;
        }

        private void shopBuyers_btn_Click(object sender, EventArgs e)
        {
            shop_panel.Visible = false;
            buyers_panel.Visible = true;

            LoadBuyers();
        }

        private void shopSales_btn_Click(object sender, EventArgs e)
        {
            shop_panel.Visible = false;
            salesReport_panel.Visible = true;
        }

        private void searchSalesTrx_btn_Click(object sender, EventArgs e)
        {
            DateTime fromDate = salesFrom_datePicker.Value;
            DateTime toDate = salesTo_datePicker.Value;

            SaleHandler saleHandler = new SaleHandler();
            List<Sale> sales = saleHandler.GetSalesForDates(fromDate, toDate);

            sales_listView.Items.Clear();

            if (sales != null)
            {
                foreach (Sale sale in sales)
                {
                    List<Product> sale_products = sale.SaleProducts;
                    Product sale_product = sale_products.First();

                    string[] saleFirstRow = new string[] { sale.SaleID, sale.SaleDateTime.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("en-GB")), (sale.Buyer.FirstName + " " + sale.Buyer.LastName), sale_product.ProductName, sale_product.Quantity.ToString(), sale_product.Price.ToString("f2") };
                    sales_listView.Items.Add(new ListViewItem(saleFirstRow));

                    sale_products.RemoveAt(0);

                    while (sale_products.Count > 0)
                    {
                        sale_product = sale_products.First();

                        string[] productsRow = new string[] { "", "", "", sale_product.ProductName, sale_product.Quantity.ToString(), sale_product.Price.ToString("f2") };
                        sales_listView.Items.Add(new ListViewItem(productsRow));

                        sale_products.RemoveAt(0);
                    }

                    string[] totalAmountRow_last = new string[] { "", "", "", "", "", sale.TotalPrice.ToString("f2") };
                    ListViewItem listViewItem = new ListViewItem(totalAmountRow_last);
                    listViewItem.Font = new Font(listViewItem.Font, FontStyle.Bold);
                    sales_listView.Items.Add(listViewItem);

                    sales_listView.Items.Add(new ListViewItem(new string[] { }));
                }
            }
        }

        private void m_search_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string Type;
            Type = (string)comboBox1.SelectedItem;
            VehicleHandler vh = new VehicleHandler();

            List<Vehicle> lv = vh.getSelected(DbConnector.Instance.getConn(), Type);
            if(lv != null)
            {
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
        }

        private void timetableSearch_btn_Click(object sender, EventArgs e)
        {
            timetable_listView.Items.Clear();

            DateTime startDate = timetableStartDate_datePicker.Value.Date;
            DateTime endDate = timetableEndDate_datePicker.Value.Date;

            List<Task> tasks_list = new TaskHandler().GetAllPendingTasks(startDate, endDate);
            List<TaskStock> tasksStocks = new List<TaskStock>();
            List<List<Labourer>> tasksLabourers = new List<List<Labourer>>();
            List<FarmSection> tasksSection = new List<FarmSection>();
            List<Crop> tasksCrop = new List<Crop>();

            List<FarmSection> farmSections = new FarmSectionHandler().FindFarmSections();

            if (tasks_list != null)
            {
                foreach (Task task in tasks_list)
                {
                    if (task.TaskType == "")
                    {
                        tasksStocks.Add(new TaskStock());
                        tasksLabourers.Add(new List<Labourer>());
                    }
                    else
                    {
                        tasksStocks.Add(new TaskHandler().GetStocksForTask(task.TaskID));
                        tasksLabourers.Add(new TaskHandler().GetLabourersForTask(task.TaskID));
                    }
                    tasksSection.Add(new FarmSectionHandler().FindFarmSectionWithID(task.FieldID));
                    tasksCrop.Add(new CropHandler().GetCropWithID(task.CropID));
                }

                const int containerSize = 200;
                int totalFertilisersRequired = 0;

                // to iterate for date
                DateTime itDate = startDate;

                while (itDate <= endDate)
                {
                    // to indicate data added to the list
                    bool addedTask = false;

                    for (int i = 0; i < tasks_list.Count; i++)
                    {
                        if (itDate == tasks_list[i].StartDateTime.Date)
                        {
                            string labourers = "";
                            foreach (Labourer lab in tasksLabourers[i])
                            {
                                labourers += lab.Firstname + " " + lab.Lastname;
                                if (lab != tasksLabourers[i].Last()) labourers += ", ";
                            }

                            decimal containerRequired = 0;
                            if (tasks_list[i].TaskType == "HARVEST")
                            {
                                containerRequired = ((tasksSection[i].Dimension_x * tasksSection[i].Dimension_y) * tasksCrop[i].CapacityUse) / containerSize;
                                if (containerRequired < 1) containerRequired = 1;
                            }

                            ListViewItem item = new ListViewItem(new string[] { itDate.ToString("dd/MM/yyyy"), tasks_list[i].FieldID, tasksCrop[i].CropName, tasks_list[i].TaskType, (tasksStocks[i] != null ? tasksStocks[i].QuantityUse.ToString() : "-"), (containerRequired == 0 ? "-" : ((int)containerRequired).ToString() + " - " + tasksCrop[i].ContainerType), labourers });
                            timetable_listView.Items.Add(item);
                            totalFertilisersRequired += (tasksStocks[i] != null ? tasksStocks[i].QuantityUse : 0);
                            addedTask = true;
                        }
                    }

                    foreach (FarmSection section in farmSections)
                    {
                        if (itDate == section.ExpHarvestDate)
                        {
                            decimal containerRequired = 0;
                            Crop crop = new CropHandler().GetCropWithID(section.CropID);

                            containerRequired = ((section.Dimension_x * section.Dimension_y) * crop.CapacityUse) / containerSize;
                            if (containerRequired < 1) containerRequired = 1;

                            ListViewItem item = new ListViewItem(new string[] { itDate.ToString("dd/MM/yyyy"), section.SectionID, section.CropName, "READY", "-", ((int)containerRequired).ToString() + " - " + crop.ContainerType, "-" });
                            timetable_listView.Items.Add(item);
                            addedTask = true;
                        }
                    }

                    if (!addedTask)
                    {
                        ListViewItem item = new ListViewItem(new string[] { itDate.ToString("dd/MM/yyyy"), "", "", "", "", "", "" });
                        timetable_listView.Items.Add(item);
                    }

                    itDate = itDate.AddDays(1);
                }

                requiredFertiliser_label.Text = totalFertilisersRequired.ToString();
            }
            else
            {
                const int containerSize = 200;

                // to iterate for date
                DateTime itDate = startDate;

                while (itDate <= endDate)
                {
                    // to indicate data added to the list
                    bool addedTask = false;

                    foreach (FarmSection section in farmSections)
                    {
                        if (itDate == section.ExpHarvestDate)
                        {
                            decimal containerRequired = 0;
                            Crop crop = new CropHandler().GetCropWithID(section.CropID);

                            containerRequired = ((section.Dimension_x * section.Dimension_y) * crop.CapacityUse) / containerSize;
                            if (containerRequired < 1) containerRequired = 1;

                            ListViewItem item = new ListViewItem(new string[] { itDate.ToString("dd/MM/yyyy"), section.SectionID, section.CropName, "READY", "-", ((int)containerRequired).ToString() + " - " + crop.ContainerType, "-" });
                            timetable_listView.Items.Add(item);
                            addedTask = true;
                        }
                    }

                    if (!addedTask)
                    {
                        ListViewItem item = new ListViewItem(new string[] { itDate.ToString("dd/MM/yyyy"), "", "", "", "", "", "" });
                        timetable_listView.Items.Add(item);
                    }

                    itDate = itDate.AddDays(1);
                }
            }
        }

        private void wholesaleTransport_btn_Click(object sender, EventArgs e)
        {
            if (wholesaleContainer_listView.CheckedItems.Count > 0)
            {
                DateTime transportDateTime = transportDate_datePicker.Value.Date + transportTime_datePicker.Value.TimeOfDay;

                List<Container> selectedContainers = new List<Container>();
                foreach(ListViewItem listItem in wholesaleContainer_listView.CheckedItems)
                {
                    selectedContainers.Add((Container)listItem.Tag);
                }

                int numberOfContainers = selectedContainers.Count;

                // Obtain list of trucks for calculating number of required trucks for transport
                TruckHandler truckHandler = new TruckHandler();
                List<Truck> allTrucks = truckHandler.GetTrucks();

                // list to store required trucks for transport
                List<Truck> requiredTrucks = new List<Truck>();
                List<int> numOfRequiredTruck = new List<int>();

                foreach(Truck truck in allTrucks)
                {
                    if(numberOfContainers / truck.ContainerQuantity > 0)
                    {
                        int reqNumTruck = numberOfContainers / truck.ContainerQuantity;

                        if (truck.QuantityAvailable > 0)
                        {
                            if (truck.QuantityAvailable >= reqNumTruck)
                            {
                                requiredTrucks.Add(truck);
                                numOfRequiredTruck.Add(reqNumTruck);
                                numberOfContainers %= truck.ContainerQuantity;
                            }
                            else
                            {
                                requiredTrucks.Add(truck);
                                numOfRequiredTruck.Add(truck.QuantityAvailable);
                                numberOfContainers -= truck.QuantityAvailable * truck.ContainerQuantity;
                            }
                        }
                        else continue;
                    }
                }

                if(numberOfContainers <= 0)
                {
                    string promptMessage = "This wholesale transport requires:\n";
                    for(int i = 0;i < requiredTrucks.Count;i++)
                    {
                        promptMessage += requiredTrucks[i].mac_name + " -- " + numOfRequiredTruck[i].ToString() + "\n";
                    }
                    promptMessage += "Are you sure to do this ?";

                    DialogResult promptResult =  MessageBox.Show(promptMessage, "Wholesale - Containers Transport", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                    switch (promptResult)
                    {
                        case DialogResult.Yes:
                            // Update containers status
                            foreach (Container container in selectedContainers)
                            {
                                new ContainerHandler().ClearContainer(container);
                            }

                            // Update trucks quantity
                            for (int i = 0;i < requiredTrucks.Count;i++)
                            {
                                truckHandler.UpdateTruckQuantity(requiredTrucks[i], requiredTrucks[i].QuantityInUse + numOfRequiredTruck[i]);
                            }

                            DateTime arrivalDateTime = transportDateTime.Add(requiredTrucks[0].TimeRequired.TimeOfDay);
                            MessageBox.Show("Success !\nTransport will start on " + transportDateTime.ToString("dd/MMM/yyyy hh:mm:ss tt", CultureInfo.CreateSpecificCulture("en-GB")) + " and expected arrival at " + arrivalDateTime.ToString("dd/MMM/yyyy hh:mm:ss tt", CultureInfo.CreateSpecificCulture("en-GB")));
                            LoadWholesale();
                            break;

                        case DialogResult.No:
                            break;

                        case DialogResult.Cancel:
                            break;
                        
                        default:
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("There is no enough number of trucks to transport all the selected containers");
                }
            }
            else
            {
                MessageBox.Show("Please check at least one container to transport for wholesale");
            }
        }

        private void shopBack_btn_Click(object sender, EventArgs e)
        {
            ShopBackTopPanel();
        }

        private void assignTask_back_Click(object sender, EventArgs e)
        {
            LabourerBackTopPanel();
            LoadLabourers();
        }

        private void addLab_back_Click(object sender, EventArgs e)
        {
            LabourerBackTopPanel();
            LoadLabourers();
        }

        private void stock_back_Click(object sender, EventArgs e)
        {
            StorageBackTopPanel();
            LoadStockStorage();
        }

        private void storageContainer_back_Click(object sender, EventArgs e)
        {
            StorageBackTopPanel();
            LoadStockStorage();
        }

        private void wholesale_back_Click(object sender, EventArgs e)
        {
            ShopBackTopPanel();
        }

        private void shopProducts_back_Click(object sender, EventArgs e)
        {
            shopProducts_panel.Visible = false;
            shop_panel.Visible = true;
        }

        private void buyers_back_Click(object sender, EventArgs e)
        {
            shop_panel.Visible = true;
            buyers_panel.Visible = false;
        }

        private void salesReport_back_Click(object sender, EventArgs e)
        {
            shop_panel.Visible = true;
            salesReport_panel.Visible = false;
        }

        private void LabourerBackTopPanel()
        {
            labourer_panel.Visible = true;
            addLabourer_panel.Visible = false;
            assignTask_panel.Visible = false;

            ResetAssignTaskPage();
            LoadLabourers();

            addLabfName_txtBox.Clear();
            addLablName_txtBox.Clear();
            addLabEmail_txtBox.Clear();
            addLabPhoneNum_txtBox.Clear();
        }

        private void StorageBackTopPanel()
        {
            stock_panel.Visible = false;
            storage_panel.Visible = true;

            storage_listView.Items.Clear();
            stock_listView.Items.Clear();
        }

        private void ShopBackTopPanel()
        {
            shopWholesale_panel.Visible = true;
            shop_panel.Visible = false;
            shopProducts_panel.Visible = false;
            buyers_panel.Visible = false;
            salesReport_panel.Visible = false;
            wholesale_panel.Visible = false;
        }

        private void taskMethod_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime taskStartDateTime = taskStartDate_datePicker.Value.Date + taskStartTime_datePicker.Value.TimeOfDay;
            DateTime taskEndDateTime = taskEndDate_datePicker.Value.Date + taskEndTime_datePicker.Value.TimeOfDay;

            CropMethod selectedCropMethod = (CropMethod)taskMethod_comboBox.SelectedItem;

            double taskTimeMinutes, useTimeMinutes;

            assignTaskWarningMsg_label.Text = "";
            assignTaskWarningMsg_label.Tag = 1;

            if(taskType_txtBox.Text.Equals("HARVEST") || taskType_txtBox.Text.Equals("SOW"))
            {
                // if task time given is less than 1 day
                if((taskEndDateTime.Day - taskStartDateTime.Day) <= 0)
                {
                    // check task hours and minutes
                    taskTimeMinutes = taskEndDateTime.TimeOfDay.Subtract(taskStartDateTime.TimeOfDay).TotalMinutes;
                    useTimeMinutes = selectedCropMethod.TimeRequired.TimeOfDay.TotalMinutes;

                    int labourersNeeded = (int)(useTimeMinutes / taskTimeMinutes);
                    if (labourersNeeded <= 0) labourersNeeded = 1;
                    assignTaskWarningMsg_label.Text = "Minimum " + labourersNeeded.ToString() + " labourers are needed";
                    assignTaskWarningMsg_label.Tag = labourersNeeded;
                }
            }
            else if(taskType_txtBox.Text.Equals("TREATMENT"))
            {
                assignTaskWarningMsg_label.Text = "Minimum 7 days are required for this task";
            }

            assignTaskSubmit_btn.Enabled = true;
        }

        private void taskField_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            taskType_txtBox.Text = "";
            taskCrop_comboBox.Enabled = false;
            taskCrop_comboBox.Items.Clear();
            taskMethod_comboBox.Enabled = false;
            assignTaskSubmit_btn.Enabled = false;
            taskSeed_comboBox.Enabled = false;
            taskSeed_numeric.Enabled = false;
            seedStockQty_label.Text = "";
            taskFertiliser_comboBox.Enabled = false;
            taskFertiliser_numeric.Enabled = false;
            fertiliserStockQty_label.Text = "";
            taskPesticide_comboBox.Enabled = false;
            taskPesticide_numeric.Enabled = false;
            pesticideStockQty_label.Text = "";

            FarmSection farmSection = (FarmSection)taskField_comboBox.SelectedItem;

            switch(farmSection.Status)
            {
                case "NOT USED" :
                    taskType_txtBox.Text = "SOW";
                    taskCrop_comboBox.Enabled = true;
                    PopulateAssignTaskCrops();
                    break;

                case "CULTIVATING":
                    taskType_txtBox.Text = "TREATMENT";
                    taskCrop_comboBox.Items.Add(new CropHandler().GetCropWithID(farmSection.CropID));
                    taskCrop_comboBox.SelectedIndex = 0;
                    taskFertiliser_comboBox.Enabled = true;
                    taskFertiliser_numeric.Enabled = true;
                    PopulateAssignTaskFertilisers();
                    taskPesticide_comboBox.Enabled = true;
                    taskPesticide_numeric.Enabled = true;
                    PopulateAssignTaskPesticides();
                    break;

                case "HARVEST" :
                    taskType_txtBox.Text = "HARVEST";
                    taskCrop_comboBox.Items.Add(new CropHandler().GetCropWithID(farmSection.CropID));
                    taskCrop_comboBox.SelectedIndex = 0;
                    break;

                default :
                    MessageBox.Show("An error occurred");
                    break;
            }
        }

        private void storage_listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = (StockStorage)storage_listView.SelectedItems[0].Tag;
            if (selectedItem != null)
            {
                storage_panel.Visible = false;
                stock_panel.Visible = true;

                storageIDTitle_label.Text = "Storage " + selectedItem.StorageID;

                LoadStocks(selectedItem);
            }
        }

        private void products_listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (products_listView.SelectedItems.Count > 0)
            {
                var selectedProduct = (Product)products_listView.SelectedItems[0].Tag;
                if (selectedProduct != null)
                {
                    productCode_txtBox.Text = selectedProduct.ProductCode;
                    productName_txtBox.Text = selectedProduct.ProductName;
                    productQty_numUpDown.Value = selectedProduct.Quantity;
                    productPrice_txtBox.Text = selectedProduct.Price.ToString();
                    onSale_chkBox.Checked = selectedProduct.IsOnSale;
                }
            }
        }

        private void taskCrop_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (taskType_txtBox.Text == "SOW") {
                taskSeed_comboBox.Enabled = true;
                taskSeed_numeric.Enabled = true;
                PopulateAssignTaskSeeds();
            }

            if (taskType_txtBox.Text != "HARVEST")
            {
                taskFertiliser_comboBox.Enabled = true;
                taskFertiliser_numeric.Enabled = true;
                PopulateAssignTaskFertilisers();
            }

            taskMethod_comboBox.Enabled = true;
            PopulateAssignTaskCropMethods();
        }

        private void taskSeed_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            seedStockQty_label.Text = ((Stock)taskSeed_comboBox.SelectedItem).Quantity.ToString() + " in stock";
        }

        private void taskFertiliser_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            fertiliserStockQty_label.Text = ((Stock)taskFertiliser_comboBox.SelectedItem).Quantity.ToString() + " in stock";
        }

        private void taskPesticide_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            pesticideStockQty_label.Text = ((Stock)taskPesticide_comboBox.SelectedItem).Quantity.ToString() + " in stock";
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

        private void LoadWholesale()
        {
            ContainerHandler containerHandler = new ContainerHandler();
            List<Container> containers = containerHandler.GetWholesaleContainers();
            PopulateWholesaleContainers(containers);
        }

        private void PopulateWholesaleContainers(List<Container> wholesaleContainers)
        {
            wholesaleContainer_listView.Items.Clear();

            if(wholesaleContainers != null)
            {
                foreach(Container container in wholesaleContainers)
                {
                    string[] row = { container.ContainerID, container.ContainerType, container.Crop.CropName, container.ContainerStatus };
                    ListViewItem containerListItem = new ListViewItem(row);
                    containerListItem.Tag = container;

                    wholesaleContainer_listView.Items.Add(containerListItem);
                }
            }
        }

        private void LoadStocks(StockStorage storage)
        {
            StockHandler stockHandler = new StockHandler();
            List<Stock> stocks = stockHandler.FindStocksForStorage(storage.StorageID);

            PopulateStocksList(stocks);
        }

        private void PopulateStocksList(List<Stock> stocksList)
        {
            stock_listView.Items.Clear();

            if(stocksList != null)
            {
                foreach (Stock stock in stocksList)
                {
                    string[] row = new string[] { stock.ID, stock.Name, stock.Brand, stock.CapacityUse.ToString(), stock.Type, stock.Quantity.ToString() };
                    ListViewItem listViewItem = new ListViewItem(row);
                    listViewItem.Tag = stock;

                    stock_listView.Items.Add(listViewItem);
                }
            }
        }

        private void LoadStockStorage()
        {
            StockStorageHandler storageHandler = new StockStorageHandler();
            List<StockStorage> stockStorages = storageHandler.FindStockStorages();

            storage_listView.Items.Clear();

            foreach (StockStorage storage in stockStorages)
            {
                string[] row = new string[] { storage.StorageID, storage.UsedCapacity.ToString() + " / " + storage.TotalCapacity.ToString(), storage.Status };
                ListViewItem listViewItem = new ListViewItem(row);
                listViewItem.Tag = storage;

                storage_listView.Items.Add(listViewItem);
            }
        }

        private void LoadContainers()
        {
            ContainerHandler containerHandler = new ContainerHandler();
            List<Container> containers = containerHandler.GetAllContainers();

            container_listView.Items.Clear();

            foreach (Container container in containers)
            {
                string[] row = new string[] { container.ContainerID, container.ContainerType, (container.ContainerType == "REFRIGERATED" ? container.TemperatureSet.ToString() : "-"), (container.Crop.CropName != "" ? container.Crop.CropName : "-"), container.TotalCapacity.ToString(), container.ContainerStatus };
                ListViewItem listViewItem = new ListViewItem(row);
                listViewItem.Tag = container;

                container_listView.Items.Add(listViewItem);
            }
        }

        private string findNewLabIDNum(LabourerHandler labHandlr)
        {
            string newestID = labHandlr.GetNewestID();
            newestID = newestID.Substring(2, newestID.Length - 2);
            int newestYear = 2000 + int.Parse(newestID.Substring(0, 2));

            if (newestYear < DateTime.Today.Year)
            {
                return (DateTime.Today.Year - 2000).ToString() + "001";
            }
            else
            {
                return (int.Parse(newestID) + 1).ToString();
            }
        }

        private void LoadLabourers()
        {
            LabourerHandler labourerHandler = new LabourerHandler();
            List<Labourer> labourers = labourerHandler.FindAllLabourers();

            labourers_listView.Items.Clear();

            if (labourers != null)
            {
                foreach (Labourer labourer in labourers)
                {
                    string[] row = new string[] { labourer.UserID, (labourer.Firstname + " " + labourer.Lastname), labourer.EmailAddress, labourer.PhoneNumber, labourer.DateJoined.ToString("yyyy-MM-dd"), labourer.Status };

                    labourers_listView.Items.Add(new ListViewItem(row));
                }
            }
        }

        private void LoadProducts()
        {
            ProductHandler productHandler = new ProductHandler();
            List<Product> products = productHandler.FindAllProducts();

            products_listView.Items.Clear();

            if (products != null)
            {
                foreach (Product product in products)
                {
                    string[] row = new string[] { product.ProductCode, product.ProductName, product.Quantity.ToString(), product.Price.ToString(), (product.IsOnSale ? "Yes" : "No") };
                    ListViewItem listViewItem = new ListViewItem(row);
                    listViewItem.Tag = product;

                    products_listView.Items.Add(listViewItem);
                }
            }
        }

        private void LoadBuyers()
        {
            BuyerHandler buyerHandler = new BuyerHandler();
            List<Buyer> buyers = buyerHandler.GetBuyers();

            buyers_listView.Items.Clear();

            if (buyers != null)
            {
                foreach (Buyer buyer in buyers)
                {
                    string[] row = new string[] { (buyer.FirstName + " " + buyer.LastName), buyer.EmailAddress, buyer.PhoneNumber, buyer.VisitedTimes.ToString(), buyer.AvgVisitedTime, buyer.TotalSpent.ToString("f2"), buyer.CompanyName };
                    ListViewItem listViewItem = new ListViewItem(row);
                    listViewItem.Tag = buyer;

                    buyers_listView.Items.Add(listViewItem);
                }
            }
        }

        private void PopulateAssignTaskInputOptions()
        {
            FarmSectionHandler farmSectionHandler = new FarmSectionHandler();
            List<FarmSection> farmFields = farmSectionHandler.FindFarmSections();

            taskField_comboBox.Items.Clear();

            foreach (FarmSection field in farmFields)
            {
                taskField_comboBox.Items.Add(field);
            }

            LabourerHandler labourerHandler = new LabourerHandler();
            List<Labourer> labourers = labourerHandler.FindAllLabourers();

            assignTaskLab_listBox.Items.Clear();

            if (labourers != null)
            {
                foreach (Labourer labourer in labourers)
                {
                    if (labourer.Status.Equals("FREE")) assignTaskLab_listBox.Items.Add(labourer);
                }
            }
        }

        private void PopulateAssignTaskCrops()
        {
            CropHandler cropHandler = new CropHandler();
            List<Crop> crops = cropHandler.GetAllCropsForSow();

            taskCrop_comboBox.Items.Clear();

            if (crops != null)
            {
                foreach (Crop crop in crops)
                {
                    taskCrop_comboBox.Items.Add(crop);
                }
            }
        }

        private void PopulateAssignTaskCropMethods()
        {
            Crop selectedCrop = (Crop)taskCrop_comboBox.SelectedItem;
            string taskType = taskType_txtBox.Text;

            taskMethod_comboBox.Enabled = true;
            taskMethod_comboBox.Items.Clear();

            CropMethodHandler cropMethodHandler = new CropMethodHandler();
            List<CropMethod> cropMethods = cropMethodHandler.GetAllCropMethodsOnCropAndTaskType(selectedCrop.CropID, taskType);

            if (cropMethods != null)
            {
                foreach (CropMethod cropMethod in cropMethods)
                {
                    taskMethod_comboBox.Items.Add(cropMethod);
                }
            }
        }

        private void PopulateAssignTaskSeeds()
        {
            SeedHandler seedHandler = new SeedHandler();
            List<Seed> seeds = seedHandler.GetSeedsForCrop(((Crop)taskCrop_comboBox.SelectedItem).CropID);

            taskSeed_comboBox.Items.Clear();

            if (seeds != null)
            {
                foreach (Seed seed in seeds)
                {
                    taskSeed_comboBox.Items.Add(seed);
                }
            }
        }

        private void PopulateAssignTaskFertilisers()
        {
            FertiliserHandler fertiliserHandler = new FertiliserHandler();
            List<Fertiliser> fertilisers = fertiliserHandler.GetAllFertilisers();

            taskFertiliser_comboBox.Items.Clear();

            if (fertilisers != null)
            {
                foreach (Fertiliser fertiliser in fertilisers)
                {
                    taskFertiliser_comboBox.Items.Add(fertiliser);
                }
            }
        }

        private void PopulateAssignTaskPesticides()
        {
            PesticideHandler pesticideHandler = new PesticideHandler();
            List<Pesticide> pesticides = pesticideHandler.GetAllPesticides();

            taskPesticide_comboBox.Items.Clear();

            if (pesticides != null)
            {
                foreach (Pesticide pesticide in pesticides)
                {
                    taskPesticide_comboBox.Items.Add(pesticide);
                }
            }
        }

        private void labourerTop_panel_VisibleChanged(object sender, EventArgs e)
        {
            LabourerBackTopPanel();
        }

        private void storageTop_panel_VisibleChanged(object sender, EventArgs e)
        {
            StorageBackTopPanel();
        }

        private void shopWholesaleTop_panel_VisibleChanged(object sender, EventArgs e)
        {
            ShopBackTopPanel();
        }

        private void LoadUserProfileInfo()
        {
            profileFName_txtBox.Clear();
            profileLName_txtBox.Clear();
            profileEmail_txtBox.Clear();
            profilePhone_txtBox.Clear();
            profileOldPassword_txtBox.Clear();
            profileNewPassword_txtBox.Clear();
            profileConfirmPassword_txtBox.Clear();

            UserHandler userHandler = new UserHandler();
            User user = userHandler.GetUser(DbConnector.Instance.getConn(), UserSession.Instance.UserID);

            profileFName_txtBox.Text = user.Firstname;
            profileLName_txtBox.Text = user.Lastname;
            profileEmail_txtBox.Text = user.EmailAddress;
            profilePhone_txtBox.Text = user.PhoneNumber;
        }

        private void updateProfile_btn_Click(object sender, EventArgs e)
        {
            string firstName = profileFName_txtBox.Text.Trim();
            string lastName = profileLName_txtBox.Text.Trim();
            string emailAddress = profileEmail_txtBox.Text.Trim();
            string phoneNumber = profilePhone_txtBox.Text.Trim();

            if (firstName != "")
            {
                if (lastName != "")
                {
                    if (Regex.IsMatch(emailAddress, EVAL_EMAIL))
                    {
                        User u = new User();
                        u.UserID = UserSession.Instance.UserID;
                        u.Firstname = firstName;
                        u.Lastname = lastName;
                        u.EmailAddress = emailAddress;
                        u.PhoneNumber = phoneNumber;

                        UserHandler userHandler = new UserHandler();
                        userHandler.UpdateUserInfo(DbConnector.Instance.getConn(), u);

                        MessageBox.Show("Profile is successfully updated !");
                        LoadUserProfileInfo();
                    }
                    else
                    {
                        MessageBox.Show("Invalid email");
                    }
                }
                else
                    MessageBox.Show("Last name cannot be empty");
            }
            else
            {
                MessageBox.Show("First name cannot be empty");
            }
        }

        private void updatePassword_btn_Click(object sender, EventArgs e)
        {
            string oldPassword = profileOldPassword_txtBox.Text;
            string newPassword = profileNewPassword_txtBox.Text;
            string confirmPassword = profileConfirmPassword_txtBox.Text;

            UserHandler userHandler = new UserHandler();
            User user = userHandler.GetUser(DbConnector.Instance.getConn(), UserSession.Instance.UserID);

            if (oldPassword == user.Password)
            {
                if (newPassword == confirmPassword)
                {
                    userHandler.ChangePass(DbConnector.Instance.getConn(), user, newPassword);
                    MessageBox.Show("Password is successfully updated !");
                    LoadUserProfileInfo();
                }
                else
                {
                    MessageBox.Show("New password and confirm password are not match");
                }
            }
            else
            {
                MessageBox.Show("Invalid password");
            }
        }
    }
}