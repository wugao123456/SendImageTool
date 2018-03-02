namespace SendImagesTool
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.OpenFileButton = new System.Windows.Forms.Button();
            this.FilePathText = new System.Windows.Forms.TextBox();
            this.Status = new System.Windows.Forms.Label();
            this.SendStatusInfo = new System.Windows.Forms.Label();
            this.IPLabel = new System.Windows.Forms.Label();
            this.RemoteIPText = new System.Windows.Forms.TextBox();
            this.PortLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.RemotePortText = new System.Windows.Forms.NumericUpDown();
            this.RemoteAETitle = new System.Windows.Forms.TextBox();
            this.LocalIPLabel = new System.Windows.Forms.Label();
            this.LocalIPText = new System.Windows.Forms.TextBox();
            this.LocalAETitleLabel = new System.Windows.Forms.Label();
            this.LocalAEText = new System.Windows.Forms.TextBox();
            this.LocalPortlabel = new System.Windows.Forms.Label();
            this.LocalPortText = new System.Windows.Forms.NumericUpDown();
            this.Ping_Button = new System.Windows.Forms.Button();
            this.Sure_Button = new System.Windows.Forms.Button();
            this.Send_Button = new System.Windows.Forms.Button();
            this.Log_button = new System.Windows.Forms.Button();
            this.FilePathLabel = new System.Windows.Forms.Label();
            this.StudyTableDGV = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.RemotePortText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LocalPortText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.StudyTableDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenFileButton
            // 
            this.OpenFileButton.Location = new System.Drawing.Point(328, 38);
            this.OpenFileButton.Name = "OpenFileButton";
            this.OpenFileButton.Size = new System.Drawing.Size(75, 23);
            this.OpenFileButton.TabIndex = 0;
            this.OpenFileButton.Text = "打开文件";
            this.OpenFileButton.UseVisualStyleBackColor = true;
            this.OpenFileButton.Click += new System.EventHandler(this.OpenFileButton_Click);
            // 
            // FilePathText
            // 
            this.FilePathText.Location = new System.Drawing.Point(125, 40);
            this.FilePathText.Name = "FilePathText";
            this.FilePathText.Size = new System.Drawing.Size(146, 21);
            this.FilePathText.TabIndex = 1;
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Location = new System.Drawing.Point(96, 94);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(101, 12);
            this.Status.TabIndex = 2;
            this.Status.Text = "Send Status Info";
            // 
            // SendStatusInfo
            // 
            this.SendStatusInfo.AutoSize = true;
            this.SendStatusInfo.ForeColor = System.Drawing.Color.Red;
            this.SendStatusInfo.Location = new System.Drawing.Point(314, 94);
            this.SendStatusInfo.Name = "SendStatusInfo";
            this.SendStatusInfo.Size = new System.Drawing.Size(77, 12);
            this.SendStatusInfo.TabIndex = 3;
            this.SendStatusInfo.Text = "Waiting Send";
            // 
            // IPLabel
            // 
            this.IPLabel.AutoSize = true;
            this.IPLabel.Location = new System.Drawing.Point(54, 153);
            this.IPLabel.Name = "IPLabel";
            this.IPLabel.Size = new System.Drawing.Size(17, 12);
            this.IPLabel.TabIndex = 4;
            this.IPLabel.Text = "IP";
            // 
            // RemoteIPText
            // 
            this.RemoteIPText.Location = new System.Drawing.Point(162, 150);
            this.RemoteIPText.Name = "RemoteIPText";
            this.RemoteIPText.Size = new System.Drawing.Size(100, 21);
            this.RemoteIPText.TabIndex = 5;
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Location = new System.Drawing.Point(52, 204);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(29, 12);
            this.PortLabel.TabIndex = 7;
            this.PortLabel.Text = "Port";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(52, 256);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "AE Title";
            // 
            // RemotePortText
            // 
            this.RemotePortText.Location = new System.Drawing.Point(162, 204);
            this.RemotePortText.Maximum = new decimal(new int[] {
            -1981284352,
            -1966660860,
            0,
            0});
            this.RemotePortText.Name = "RemotePortText";
            this.RemotePortText.Size = new System.Drawing.Size(100, 21);
            this.RemotePortText.TabIndex = 9;
            // 
            // RemoteAETitle
            // 
            this.RemoteAETitle.Location = new System.Drawing.Point(162, 247);
            this.RemoteAETitle.Name = "RemoteAETitle";
            this.RemoteAETitle.Size = new System.Drawing.Size(100, 21);
            this.RemoteAETitle.TabIndex = 10;
            // 
            // LocalIPLabel
            // 
            this.LocalIPLabel.AutoSize = true;
            this.LocalIPLabel.Location = new System.Drawing.Point(344, 159);
            this.LocalIPLabel.Name = "LocalIPLabel";
            this.LocalIPLabel.Size = new System.Drawing.Size(47, 12);
            this.LocalIPLabel.TabIndex = 11;
            this.LocalIPLabel.Text = "LocalIP";
            // 
            // LocalIPText
            // 
            this.LocalIPText.Location = new System.Drawing.Point(423, 150);
            this.LocalIPText.Name = "LocalIPText";
            this.LocalIPText.Size = new System.Drawing.Size(100, 21);
            this.LocalIPText.TabIndex = 12;
            // 
            // LocalAETitleLabel
            // 
            this.LocalAETitleLabel.AutoSize = true;
            this.LocalAETitleLabel.Location = new System.Drawing.Point(320, 262);
            this.LocalAETitleLabel.Name = "LocalAETitleLabel";
            this.LocalAETitleLabel.Size = new System.Drawing.Size(83, 12);
            this.LocalAETitleLabel.TabIndex = 13;
            this.LocalAETitleLabel.Text = "LocalAE Title";
            // 
            // LocalAEText
            // 
            this.LocalAEText.Location = new System.Drawing.Point(423, 253);
            this.LocalAEText.Name = "LocalAEText";
            this.LocalAEText.Size = new System.Drawing.Size(100, 21);
            this.LocalAEText.TabIndex = 14;
            // 
            // LocalPortlabel
            // 
            this.LocalPortlabel.AutoSize = true;
            this.LocalPortlabel.Location = new System.Drawing.Point(344, 213);
            this.LocalPortlabel.Name = "LocalPortlabel";
            this.LocalPortlabel.Size = new System.Drawing.Size(59, 12);
            this.LocalPortlabel.TabIndex = 15;
            this.LocalPortlabel.Text = "LacalPort";
            // 
            // LocalPortText
            // 
            this.LocalPortText.Location = new System.Drawing.Point(423, 211);
            this.LocalPortText.Maximum = new decimal(new int[] {
            276447232,
            23283,
            0,
            0});
            this.LocalPortText.Name = "LocalPortText";
            this.LocalPortText.Size = new System.Drawing.Size(100, 21);
            this.LocalPortText.TabIndex = 16;
            // 
            // Ping_Button
            // 
            this.Ping_Button.Location = new System.Drawing.Point(30, 312);
            this.Ping_Button.Name = "Ping_Button";
            this.Ping_Button.Size = new System.Drawing.Size(75, 23);
            this.Ping_Button.TabIndex = 17;
            this.Ping_Button.Text = "Ping";
            this.Ping_Button.UseVisualStyleBackColor = true;
            this.Ping_Button.Click += new System.EventHandler(this.Ping_Button_Click);
            // 
            // Sure_Button
            // 
            this.Sure_Button.Location = new System.Drawing.Point(151, 312);
            this.Sure_Button.Name = "Sure_Button";
            this.Sure_Button.Size = new System.Drawing.Size(75, 23);
            this.Sure_Button.TabIndex = 18;
            this.Sure_Button.Text = "确认";
            this.Sure_Button.UseVisualStyleBackColor = true;
            this.Sure_Button.Click += new System.EventHandler(this.Sure_Button_Click);
            // 
            // Send_Button
            // 
            this.Send_Button.Location = new System.Drawing.Point(280, 312);
            this.Send_Button.Name = "Send_Button";
            this.Send_Button.Size = new System.Drawing.Size(75, 23);
            this.Send_Button.TabIndex = 19;
            this.Send_Button.Text = "发图";
            this.Send_Button.UseVisualStyleBackColor = true;
            this.Send_Button.Click += new System.EventHandler(this.Send_Button_Click);
            // 
            // Log_button
            // 
            this.Log_button.Location = new System.Drawing.Point(423, 312);
            this.Log_button.Name = "Log_button";
            this.Log_button.Size = new System.Drawing.Size(75, 23);
            this.Log_button.TabIndex = 20;
            this.Log_button.Text = "日志";
            this.Log_button.UseVisualStyleBackColor = true;
            this.Log_button.Click += new System.EventHandler(this.Log_button_Click);
            // 
            // FilePathLabel
            // 
            this.FilePathLabel.AutoSize = true;
            this.FilePathLabel.Location = new System.Drawing.Point(54, 43);
            this.FilePathLabel.Name = "FilePathLabel";
            this.FilePathLabel.Size = new System.Drawing.Size(53, 12);
            this.FilePathLabel.TabIndex = 21;
            this.FilePathLabel.Text = "FilePath";
            // 
            // StudyTableDGV
            // 
            this.StudyTableDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.StudyTableDGV.Location = new System.Drawing.Point(538, 38);
            this.StudyTableDGV.Name = "StudyTableDGV";
            this.StudyTableDGV.RowTemplate.Height = 23;
            this.StudyTableDGV.Size = new System.Drawing.Size(313, 309);
            this.StudyTableDGV.TabIndex = 22;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 362);
            this.Controls.Add(this.StudyTableDGV);
            this.Controls.Add(this.FilePathLabel);
            this.Controls.Add(this.Log_button);
            this.Controls.Add(this.Send_Button);
            this.Controls.Add(this.Sure_Button);
            this.Controls.Add(this.Ping_Button);
            this.Controls.Add(this.LocalPortText);
            this.Controls.Add(this.LocalPortlabel);
            this.Controls.Add(this.LocalAEText);
            this.Controls.Add(this.LocalAETitleLabel);
            this.Controls.Add(this.LocalIPText);
            this.Controls.Add(this.LocalIPLabel);
            this.Controls.Add(this.RemoteAETitle);
            this.Controls.Add(this.RemotePortText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.PortLabel);
            this.Controls.Add(this.RemoteIPText);
            this.Controls.Add(this.IPLabel);
            this.Controls.Add(this.SendStatusInfo);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.FilePathText);
            this.Controls.Add(this.OpenFileButton);
            this.Name = "Form1";
            this.Text = "推图工具";
            ((System.ComponentModel.ISupportInitialize)(this.RemotePortText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LocalPortText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.StudyTableDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OpenFileButton;
        private System.Windows.Forms.TextBox FilePathText;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.Label SendStatusInfo;
        private System.Windows.Forms.Label IPLabel;
        private System.Windows.Forms.TextBox RemoteIPText;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown RemotePortText;
        private System.Windows.Forms.TextBox RemoteAETitle;
        private System.Windows.Forms.Label LocalIPLabel;
        private System.Windows.Forms.TextBox LocalIPText;
        private System.Windows.Forms.Label LocalAETitleLabel;
        private System.Windows.Forms.TextBox LocalAEText;
        private System.Windows.Forms.Label LocalPortlabel;
        private System.Windows.Forms.NumericUpDown LocalPortText;
        private System.Windows.Forms.Button Ping_Button;
        private System.Windows.Forms.Button Sure_Button;
        private System.Windows.Forms.Button Send_Button;
        private System.Windows.Forms.Button Log_button;
        private System.Windows.Forms.Label FilePathLabel;
        private System.Windows.Forms.DataGridView StudyTableDGV;

    }
}

