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
    public partial class LabHarvestPopup : Form
    {
        private int taskID;

        public LabHarvestPopup(int task_id)
        {
            InitializeComponent();
            this.taskID = task_id;
        }

        private void LabHarvestPopup_Load(object sender, EventArgs e)
        {
            // get current task with task ID passed from main screen
            Task currentTask = new TaskHandler().GetTaskWithID(this.taskID);
            FarmSection section = new FarmSectionHandler().FindFarmSectionWithID(currentTask.FieldID);
            Crop harvestedCrop = new CropHandler().GetCropWithID(section.CropID);

            // get list of containers
            List<Container> containers = new ContainerHandler().GetAllContainers(harvestedCrop.ContainerType);

            harvestContainer_listView.Items.Clear();

            if (containers != null)
            {
                // calculate expected required container for the harvest task
                decimal containerRequired = ((section.Dimension_x * section.Dimension_y) * harvestedCrop.CapacityUse) / containers[0].TotalCapacity;
                if (containerRequired < 1) containerRequired = 1;

                if((int)containerRequired <= containers.Count)
                {
                    containerInst_label.Text = "Expected container(s) used ( 0 / " + ((int)containerRequired).ToString() + " )";
                    containerInst_label.Tag = (int)containerRequired;

                    // iterate containers list
                    // fill in container details in list view
                    foreach (Container container in containers)
                    {
                        string[] row = new string[] { container.ContainerID, container.ContainerType, container.TotalCapacity.ToString() };
                        ListViewItem listViewItem = new ListViewItem(row);
                        listViewItem.Tag = container;

                        harvestContainer_listView.Items.Add(listViewItem);
                    }
                }
                else
                {
                    MessageBox.Show("There is no enough number of containers for storing the harvest\nPlease contact your manager");
                    this.DialogResult = DialogResult.Abort;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("There is no available containers for storing the harvest\nPlease contact your manager");
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private void harvestContainer_listView_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            int checkedCount = harvestContainer_listView.CheckedItems.Count;
            int containersCount = (int)containerInst_label.Tag;

            // if checked containers count does not exceed expected containers count
            if (checkedCount <= containersCount)
            {
                containerInst_label.Text = "Expected container(s) used ( " + checkedCount.ToString() + " / " + containersCount.ToString() + " )";
            }
            else
            {
                // display message for only accepting one extra to the expected containers count to be used
                DialogResult res = MessageBox.Show("Number of container used is exceeded number of expected containers used\nMaximum only 1 additional container is allowed to be used\nAre you sure to do this?", "Exceeded expected containers used", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if(res == DialogResult.Yes)
                {
                    // disable editing container
                    harvestContainer_listView.Enabled = false;
                    containerInst_label.Text = "Expected container(s) used ( " + checkedCount.ToString() + " / " + containersCount.ToString() + " )";
                }
                else if(res == DialogResult.No)
                {
                    e.Item.Checked = false;
                }
            }
        }

        private void submit_btn_Click(object sender, EventArgs e)
        {
            if(harvestContainer_listView.CheckedItems.Count > 0)
            {
                DialogResult res = DialogResult.Yes;

                // if checked containers are less than expected number of containers used
                if(harvestContainer_listView.CheckedItems.Count < (int)containerInst_label.Tag)
                {
                    res = MessageBox.Show("Number of container used less than number of expected containers used\nAre you sure to do this ? ", "Less than expected containers used", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }

                if(res == DialogResult.Yes)
                {
                    // get current task
                    Task currentTask = new TaskHandler().GetTaskWithID(this.taskID);

                    ContainerHandler containerHandler = new ContainerHandler();

                    // iterate checked containers
                    // update containers status for the task
                    foreach(ListViewItem checkedItem in harvestContainer_listView.CheckedItems)
                    {
                        Container checkedContainer = (Container)checkedItem.Tag;
                        containerHandler.UseContainer(currentTask.TaskID, checkedContainer, currentTask.CropID);
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Please check for used container(s)");
            }
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
