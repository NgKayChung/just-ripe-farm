using System;
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
        }

        private void storage_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            timetable_panel.Visible = false;
            storageTop_panel.Visible = true;
            labourerTop_panel.Visible = false;
            machineTop_panel.Visible = false;
            shopWholesaleTop_panel.Visible = false;

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
        }

        private void shopWholesale_btn_Click(object sender, EventArgs e)
        {
            home_panel.Visible = false;
            timetable_panel.Visible = false;
            storageTop_panel.Visible = false;
            labourerTop_panel.Visible = false;
            machineTop_panel.Visible = false;
            shopWholesaleTop_panel.Visible = true;
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
                        if (assignTaskLab_listBox.SelectedItems.Count > 0)
                        {
                            FarmSection taskSelectedField = (FarmSection)taskField_comboBox.SelectedItem;
                            string taskType = taskType_txtBox.Text;
                            Crop taskSelectedCrop = (Crop)taskCrop_comboBox.SelectedItem;
                            CropMethod taskSelectdCropMethod = (CropMethod)taskMethod_comboBox.SelectedItem;
                            List<Labourer> taskSelectedLabourers = new List<Labourer>();

                            for (int i = 0; i < assignTaskLab_listBox.SelectedItems.Count; i++)
                            {
                                taskSelectedLabourers.Add((Labourer)assignTaskLab_listBox.SelectedItems[i]);
                            }

                            if (taskType.Equals("TREATMENT"))
                            {
                                if (taskEndDateTime.TimeOfDay.Subtract(taskStartDateTime.TimeOfDay).Days < 7)
                                {
                                    MessageBox.Show("Please select task time at least 7 days");
                                    return;
                                }
                            }

                            Task newTask = new Task(taskTitle, taskType, taskDescription, "PENDING", taskStartDateTime, taskEndDateTime, taskSelectedField.SectionID, taskSelectedCrop.CropID, taskSelectdCropMethod.MethodID, DateTime.Now, UserSession.Instance.UserID, taskSelectedLabourers);
                            string insertResult = new TaskHandler().AddNewTask(newTask);

                            if (insertResult.Equals("SUCCESS"))
                            {
                                MessageBox.Show("New task is successfully assigned");
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

        private void labourerAssignTask_btn_Click(object sender, EventArgs e)
        {
            labourer_panel.Visible = false;
            assignTask_panel.Visible = true;

            PopulateAssignTaskInputOptions();
        }

        private void shopProducts_btn_Click(object sender, EventArgs e)
        {
            shop_panel.Visible = false;
            shopProducts_panel.Visible = true;

            LoadProducts();
        }

        private void stockFertiliserFilter_btn_Click(object sender, EventArgs e)
        {
            var storage = (StockStorage)stockFertiliserFilter_btn.Tag;
            string type = "FERTILISER";

            LoadStocks(storage, type);
        }

        private void stockSeedFilter_btn_Click(object sender, EventArgs e)
        {
            var storage = (StockStorage)stockSeedFilter_btn.Tag;
            string type = "SEED";

            LoadStocks(storage, type);
        }

        private void stockPesticideFilter_btn_Click(object sender, EventArgs e)
        {
            var storage = (StockStorage)stockSeedFilter_btn.Tag;
            string type = "PESTICIDE";

            LoadStocks(storage, type);
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

                    int labourersNeeded = Convert.ToInt32(useTimeMinutes / taskTimeMinutes);
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
            taskCrop_comboBox.SelectedText = "";
            taskMethod_comboBox.Enabled = false;
            assignTaskSubmit_btn.Enabled = false;

            FarmSection farmSection = (FarmSection)taskField_comboBox.SelectedItem;

            switch(farmSection.Status)
            {
                case "NOT USED" :
                    taskType_txtBox.Text = "SOW";
                    taskCrop_comboBox.Enabled = true;
                    PopulateCropsList();
                    break;

                case "CULTIVATING":
                    taskType_txtBox.Text = "TREATMENT";
                    taskCrop_comboBox.Items.Add(new CropHandler().GetCropWithID(farmSection.CropID));
                    taskCrop_comboBox.SelectedIndex = 0;
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
                stockFertiliserFilter_btn.Tag = selectedItem;
                stockSeedFilter_btn.Tag = selectedItem;
                stockPesticideFilter_btn.Tag = selectedItem;

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
            taskMethod_comboBox.Enabled = true;
            PopulateCropMethodsList();
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

        private void LoadStocks(StockStorage storage)
        {
            StockHandler stockHandler = new StockHandler();
            List<Stock> stocks = stockHandler.FindStocksForStorage(storage.StorageID);

            PopulateStocksList(stocks);
        }

        private void LoadStocks(StockStorage storage, string stock_type)
        {
            StockHandler stockHandler = new StockHandler();
            List<Stock> stocks = stockHandler.FindStocksFilterType(storage.StorageID, stock_type);

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

        private void PopulateCropsList()
        {
            CropHandler cropHandler = new CropHandler();
            List<Crop> crops = cropHandler.GetAllCrops();

            if (crops != null)
            {
                foreach (Crop crop in crops)
                {
                    taskCrop_comboBox.Items.Add(crop);
                }
            }
        }

        private void PopulateCropMethodsList()
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
    }
}