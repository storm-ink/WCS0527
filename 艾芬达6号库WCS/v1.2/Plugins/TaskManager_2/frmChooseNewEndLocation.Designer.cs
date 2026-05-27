namespace Wcs.App.Plugins.TaskManager
{
    partial class frmChooseNewEndLocation
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
            this.label1 = new System.Windows.Forms.Label();
            this.lblTaskNo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblContainerCodes = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxEndLocation = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTaskType = new System.Windows.Forms.Label();
            this.lblAdditationInfo = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "任务号：";
            // 
            // lblTaskNo
            // 
            this.lblTaskNo.AutoSize = true;
            this.lblTaskNo.Location = new System.Drawing.Point(81, 19);
            this.lblTaskNo.Name = "lblTaskNo";
            this.lblTaskNo.Size = new System.Drawing.Size(11, 12);
            this.lblTaskNo.TabIndex = 1;
            this.lblTaskNo.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "条码号：";
            // 
            // lblContainerCodes
            // 
            this.lblContainerCodes.AutoSize = true;
            this.lblContainerCodes.Location = new System.Drawing.Point(81, 65);
            this.lblContainerCodes.Name = "lblContainerCodes";
            this.lblContainerCodes.Size = new System.Drawing.Size(11, 12);
            this.lblContainerCodes.TabIndex = 1;
            this.lblContainerCodes.Text = "-";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "新终点：";
            // 
            // cbxEndLocation
            // 
            this.cbxEndLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxEndLocation.FormattingEnabled = true;
            this.cbxEndLocation.Location = new System.Drawing.Point(81, 128);
            this.cbxEndLocation.Name = "cbxEndLocation";
            this.cbxEndLocation.Size = new System.Drawing.Size(159, 20);
            this.cbxEndLocation.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(81, 165);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(71, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "确定(&OK)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(171, 165);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(71, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "取消(&ESC)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "类型：";
            // 
            // lblTaskType
            // 
            this.lblTaskType.AutoSize = true;
            this.lblTaskType.Location = new System.Drawing.Point(81, 41);
            this.lblTaskType.Name = "lblTaskType";
            this.lblTaskType.Size = new System.Drawing.Size(11, 12);
            this.lblTaskType.TabIndex = 1;
            this.lblTaskType.Text = "-";
            // 
            // lblAdditationInfo
            // 
            this.lblAdditationInfo.Location = new System.Drawing.Point(81, 87);
            this.lblAdditationInfo.Name = "lblAdditationInfo";
            this.lblAdditationInfo.Size = new System.Drawing.Size(161, 35);
            this.lblAdditationInfo.TabIndex = 1;
            this.lblAdditationInfo.Text = "-";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "附加属性：";
            // 
            // frmChooseNewEndLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 200);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cbxEndLocation);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblAdditationInfo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblContainerCodes);
            this.Controls.Add(this.lblTaskType);
            this.Controls.Add(this.lblTaskNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmChooseNewEndLocation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "请选择新的任务终点";
            this.Load += new System.EventHandler(this.frmChooseNewEndLocation_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTaskNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblContainerCodes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbxEndLocation;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblTaskType;
        private System.Windows.Forms.Label lblAdditationInfo;
        private System.Windows.Forms.Label label6;
    }
}