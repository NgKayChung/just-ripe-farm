namespace JustRipeFarm
{
    partial class LabHarvestPopup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.submit_btn = new System.Windows.Forms.Button();
            this.harvestContainer_listView = new System.Windows.Forms.ListView();
            this.containerID_header = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.containerType_header = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.containerCapacity_header = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cancel_btn = new System.Windows.Forms.Button();
            this.containerInst_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // submit_btn
            // 
            this.submit_btn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.submit_btn.Location = new System.Drawing.Point(202, 212);
            this.submit_btn.Name = "submit_btn";
            this.submit_btn.Size = new System.Drawing.Size(75, 23);
            this.submit_btn.TabIndex = 0;
            this.submit_btn.Text = "Submit";
            this.submit_btn.UseVisualStyleBackColor = true;
            this.submit_btn.Click += new System.EventHandler(this.submit_btn_Click);
            // 
            // harvestContainer_listView
            // 
            this.harvestContainer_listView.CheckBoxes = true;
            this.harvestContainer_listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.containerID_header,
            this.containerType_header,
            this.containerCapacity_header});
            this.harvestContainer_listView.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.harvestContainer_listView.FullRowSelect = true;
            this.harvestContainer_listView.GridLines = true;
            this.harvestContainer_listView.Location = new System.Drawing.Point(-2, -2);
            this.harvestContainer_listView.Name = "harvestContainer_listView";
            this.harvestContainer_listView.Size = new System.Drawing.Size(374, 208);
            this.harvestContainer_listView.TabIndex = 1;
            this.harvestContainer_listView.UseCompatibleStateImageBehavior = false;
            this.harvestContainer_listView.View = System.Windows.Forms.View.Details;
            this.harvestContainer_listView.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.harvestContainer_listView_ItemChecked);
            // 
            // containerID_header
            // 
            this.containerID_header.Text = "Container ID";
            this.containerID_header.Width = 105;
            // 
            // containerType_header
            // 
            this.containerType_header.Text = "Container Type";
            this.containerType_header.Width = 135;
            // 
            // containerCapacity_header
            // 
            this.containerCapacity_header.Text = "Total Capacity";
            this.containerCapacity_header.Width = 130;
            // 
            // cancel_btn
            // 
            this.cancel_btn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cancel_btn.Location = new System.Drawing.Point(288, 212);
            this.cancel_btn.Name = "cancel_btn";
            this.cancel_btn.Size = new System.Drawing.Size(75, 23);
            this.cancel_btn.TabIndex = 0;
            this.cancel_btn.Text = "Cancel";
            this.cancel_btn.UseVisualStyleBackColor = true;
            this.cancel_btn.Click += new System.EventHandler(this.cancel_btn_Click);
            // 
            // containerInst_label
            // 
            this.containerInst_label.AutoSize = true;
            this.containerInst_label.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.containerInst_label.Location = new System.Drawing.Point(4, 216);
            this.containerInst_label.Name = "containerInst_label";
            this.containerInst_label.Size = new System.Drawing.Size(0, 15);
            this.containerInst_label.TabIndex = 2;
            // 
            // LabHarvestPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(365, 242);
            this.ControlBox = false;
            this.Controls.Add(this.containerInst_label);
            this.Controls.Add(this.harvestContainer_listView);
            this.Controls.Add(this.cancel_btn);
            this.Controls.Add(this.submit_btn);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LabHarvestPopup";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Choose container(s) used for storing the harvest";
            this.Load += new System.EventHandler(this.LabHarvestPopup_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button submit_btn;
        private System.Windows.Forms.ListView harvestContainer_listView;
        private System.Windows.Forms.ColumnHeader containerID_header;
        private System.Windows.Forms.ColumnHeader containerType_header;
        private System.Windows.Forms.ColumnHeader containerCapacity_header;
        private System.Windows.Forms.Button cancel_btn;
        private System.Windows.Forms.Label containerInst_label;
    }
}