namespace JustRipeFarm
{
    partial class LoginScreen
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
            this.loginID_txtBox = new System.Windows.Forms.TextBox();
            this.loginID_label = new System.Windows.Forms.Label();
            this.loginPassword_label = new System.Windows.Forms.Label();
            this.loginPassword_txtBox = new System.Windows.Forms.TextBox();
            this.login_btn = new System.Windows.Forms.Button();
            this.login_panel = new System.Windows.Forms.Panel();
            this.logo_pictureBox = new System.Windows.Forms.PictureBox();
            this.login_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logo_pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // loginID_txtBox
            // 
            this.loginID_txtBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginID_txtBox.Location = new System.Drawing.Point(123, 160);
            this.loginID_txtBox.Name = "loginID_txtBox";
            this.loginID_txtBox.Size = new System.Drawing.Size(179, 23);
            this.loginID_txtBox.TabIndex = 0;
            // 
            // loginID_label
            // 
            this.loginID_label.AutoSize = true;
            this.loginID_label.BackColor = System.Drawing.Color.Transparent;
            this.loginID_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginID_label.Location = new System.Drawing.Point(120, 144);
            this.loginID_label.Name = "loginID_label";
            this.loginID_label.Size = new System.Drawing.Size(51, 15);
            this.loginID_label.TabIndex = 1;
            this.loginID_label.Text = "Login ID";
            // 
            // loginPassword_label
            // 
            this.loginPassword_label.AutoSize = true;
            this.loginPassword_label.BackColor = System.Drawing.Color.Transparent;
            this.loginPassword_label.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginPassword_label.Location = new System.Drawing.Point(120, 199);
            this.loginPassword_label.Name = "loginPassword_label";
            this.loginPassword_label.Size = new System.Drawing.Size(57, 15);
            this.loginPassword_label.TabIndex = 2;
            this.loginPassword_label.Text = "Password";
            // 
            // loginPassword_txtBox
            // 
            this.loginPassword_txtBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginPassword_txtBox.Location = new System.Drawing.Point(123, 215);
            this.loginPassword_txtBox.Name = "loginPassword_txtBox";
            this.loginPassword_txtBox.PasswordChar = '*';
            this.loginPassword_txtBox.Size = new System.Drawing.Size(179, 23);
            this.loginPassword_txtBox.TabIndex = 3;
            // 
            // login_btn
            // 
            this.login_btn.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.login_btn.Location = new System.Drawing.Point(172, 276);
            this.login_btn.Name = "login_btn";
            this.login_btn.Size = new System.Drawing.Size(75, 23);
            this.login_btn.TabIndex = 4;
            this.login_btn.Text = "Login";
            this.login_btn.UseVisualStyleBackColor = true;
            this.login_btn.Click += new System.EventHandler(this.LoginBtn_Click);
            // 
            // login_panel
            // 
            this.login_panel.BackColor = System.Drawing.Color.White;
            this.login_panel.Controls.Add(this.logo_pictureBox);
            this.login_panel.Controls.Add(this.login_btn);
            this.login_panel.Controls.Add(this.loginID_txtBox);
            this.login_panel.Controls.Add(this.loginPassword_txtBox);
            this.login_panel.Controls.Add(this.loginID_label);
            this.login_panel.Controls.Add(this.loginPassword_label);
            this.login_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.login_panel.Location = new System.Drawing.Point(0, 0);
            this.login_panel.Name = "login_panel";
            this.login_panel.Size = new System.Drawing.Size(434, 321);
            this.login_panel.TabIndex = 5;
            // 
            // logo_pictureBox
            // 
            this.logo_pictureBox.Image = global::JustRipeFarm.Properties.Resources.just_ripe_farm_icon;
            this.logo_pictureBox.InitialImage = null;
            this.logo_pictureBox.Location = new System.Drawing.Point(163, 17);
            this.logo_pictureBox.Name = "logo_pictureBox";
            this.logo_pictureBox.Size = new System.Drawing.Size(100, 101);
            this.logo_pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.logo_pictureBox.TabIndex = 5;
            this.logo_pictureBox.TabStop = false;
            // 
            // LoginScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::JustRipeFarm.Properties.Resources.h8ujpY;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(434, 321);
            this.Controls.Add(this.login_panel);
            this.Name = "LoginScreen";
            this.Text = "JustRipe Farm - User Login";
            this.login_panel.ResumeLayout(false);
            this.login_panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logo_pictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox loginID_txtBox;
        private System.Windows.Forms.Label loginID_label;
        private System.Windows.Forms.Label loginPassword_label;
        private System.Windows.Forms.TextBox loginPassword_txtBox;
        private System.Windows.Forms.Button login_btn;
        private System.Windows.Forms.Panel login_panel;
        private System.Windows.Forms.PictureBox logo_pictureBox;
    }
}