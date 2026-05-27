namespace Wcs.App.Plugins.ManualTask
{
    partial class frmAddTask
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
            this.components = new System.ComponentModel.Container();
            this.tbxTaskCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxContainerCodes = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxEndLocation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCacnel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cbxGiveTaskCode = new System.Windows.Forms.CheckBox();
            this.cbxSource = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxBizType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.radio_plan = new System.Windows.Forms.RadioButton();
            this.radio_running = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxDescription = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.cbxTaskType = new System.Windows.Forms.ComboBox();
            this.tbxStartLocation = new System.Windows.Forms.TextBox();
            this.tbxAdditionalInfo = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbx_unPreTaskFlag = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tbxTaskCode
            // 
            this.tbxTaskCode.Location = new System.Drawing.Point(150, 22);
            this.tbxTaskCode.Margin = new System.Windows.Forms.Padding(4);
            this.tbxTaskCode.Name = "tbxTaskCode";
            this.tbxTaskCode.Size = new System.Drawing.Size(248, 28);
            this.tbxTaskCode.TabIndex = 0;
            this.tbxTaskCode.Text = "<自动生成>";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 28);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 18);
            this.label4.TabIndex = 40;
            this.label4.Text = "任务号";
            // 
            // tbxContainerCodes
            // 
            this.tbxContainerCodes.Location = new System.Drawing.Point(150, 328);
            this.tbxContainerCodes.Margin = new System.Windows.Forms.Padding(4);
            this.tbxContainerCodes.Multiline = true;
            this.tbxContainerCodes.Name = "tbxContainerCodes";
            this.tbxContainerCodes.Size = new System.Drawing.Size(248, 64);
            this.tbxContainerCodes.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 330);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 18);
            this.label3.TabIndex = 39;
            this.label3.Text = "容器编码";
            // 
            // tbxEndLocation
            // 
            this.tbxEndLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tbxEndLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbxEndLocation.Location = new System.Drawing.Point(150, 244);
            this.tbxEndLocation.Margin = new System.Windows.Forms.Padding(4);
            this.tbxEndLocation.Name = "tbxEndLocation";
            this.tbxEndLocation.Size = new System.Drawing.Size(248, 28);
            this.tbxEndLocation.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 250);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 18);
            this.label2.TabIndex = 38;
            this.label2.Text = "结束位置";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 208);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 18);
            this.label1.TabIndex = 35;
            this.label1.Text = "起始位置";
            // 
            // btnCacnel
            // 
            this.btnCacnel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCacnel.Location = new System.Drawing.Point(304, 545);
            this.btnCacnel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCacnel.Name = "btnCacnel";
            this.btnCacnel.Size = new System.Drawing.Size(112, 34);
            this.btnCacnel.TabIndex = 12;
            this.btnCacnel.Text = "取消(&C)";
            this.btnCacnel.UseVisualStyleBackColor = true;
            this.btnCacnel.Click += new System.EventHandler(this.btnCacnel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(166, 545);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(112, 34);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "确定(&O)";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label6.Location = new System.Drawing.Point(147, 402);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 18);
            this.label6.TabIndex = 44;
            this.label6.Text = "说明：一行一个";
            // 
            // cbxGiveTaskCode
            // 
            this.cbxGiveTaskCode.AutoSize = true;
            this.cbxGiveTaskCode.Location = new System.Drawing.Point(424, 27);
            this.cbxGiveTaskCode.Margin = new System.Windows.Forms.Padding(4);
            this.cbxGiveTaskCode.Name = "cbxGiveTaskCode";
            this.cbxGiveTaskCode.Size = new System.Drawing.Size(70, 22);
            this.cbxGiveTaskCode.TabIndex = 1;
            this.cbxGiveTaskCode.Text = "指定";
            this.cbxGiveTaskCode.UseVisualStyleBackColor = true;
            this.cbxGiveTaskCode.CheckedChanged += new System.EventHandler(this.cbxGiveTaskCode_CheckedChanged);
            // 
            // cbxSource
            // 
            this.cbxSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSource.FormattingEnabled = true;
            this.cbxSource.Location = new System.Drawing.Point(150, 117);
            this.cbxSource.Margin = new System.Windows.Forms.Padding(4);
            this.cbxSource.Name = "cbxSource";
            this.cbxSource.Size = new System.Drawing.Size(248, 26);
            this.cbxSource.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(24, 123);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 18);
            this.label8.TabIndex = 52;
            this.label8.Text = "任务来源";
            // 
            // cbxBizType
            // 
            this.cbxBizType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBizType.FormattingEnabled = true;
            this.cbxBizType.Location = new System.Drawing.Point(150, 70);
            this.cbxBizType.Margin = new System.Windows.Forms.Padding(4);
            this.cbxBizType.Name = "cbxBizType";
            this.cbxBizType.Size = new System.Drawing.Size(248, 26);
            this.cbxBizType.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 76);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 18);
            this.label7.TabIndex = 50;
            this.label7.Text = "业务类型";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(424, 76);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(106, 22);
            this.checkBox1.TabIndex = 3;
            this.checkBox1.Text = "移动任务";
            this.toolTip1.SetToolTip(this.checkBox1, "勾选此选项表示该任务不允许堆垛机进行取放货操作，只移动位置。");
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // radio_plan
            // 
            this.radio_plan.AutoSize = true;
            this.radio_plan.Checked = true;
            this.radio_plan.Location = new System.Drawing.Point(150, 504);
            this.radio_plan.Name = "radio_plan";
            this.radio_plan.Size = new System.Drawing.Size(123, 22);
            this.radio_plan.TabIndex = 59;
            this.radio_plan.TabStop = true;
            this.radio_plan.Text = "计划任务池";
            this.toolTip1.SetToolTip(this.radio_plan, "正常模式");
            this.radio_plan.UseVisualStyleBackColor = true;
            // 
            // radio_running
            // 
            this.radio_running.AutoSize = true;
            this.radio_running.Location = new System.Drawing.Point(279, 504);
            this.radio_running.Name = "radio_running";
            this.radio_running.Size = new System.Drawing.Size(123, 22);
            this.radio_running.TabIndex = 60;
            this.radio_running.Text = "执行任务池";
            this.toolTip1.SetToolTip(this.radio_running, "非正常模式，任务不经历计划任务池的过滤直接下发到执行任务池");
            this.radio_running.UseVisualStyleBackColor = true;
            this.radio_running.CheckedChanged += new System.EventHandler(this.radio_running_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 165);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 18);
            this.label5.TabIndex = 55;
            this.label5.Text = "任务类型";
            // 
            // tbxDescription
            // 
            this.tbxDescription.Location = new System.Drawing.Point(150, 285);
            this.tbxDescription.Margin = new System.Windows.Forms.Padding(4);
            this.tbxDescription.Name = "tbxDescription";
            this.tbxDescription.Size = new System.Drawing.Size(248, 28);
            this.tbxDescription.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(24, 291);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 18);
            this.label9.TabIndex = 57;
            this.label9.Text = "备注信息";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(24, 426);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(80, 18);
            this.label10.TabIndex = 39;
            this.label10.Text = "附加属性";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label11.Location = new System.Drawing.Point(147, 460);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(161, 18);
            this.label11.TabIndex = 44;
            this.label11.Text = "格式：a1=v1,a2=v2";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(411, 256);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(116, 18);
            this.linkLabel1.TabIndex = 58;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "设置连续任务";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // cbxTaskType
            // 
            this.cbxTaskType.FormattingEnabled = true;
            this.cbxTaskType.Location = new System.Drawing.Point(150, 160);
            this.cbxTaskType.Margin = new System.Windows.Forms.Padding(4);
            this.cbxTaskType.Name = "cbxTaskType";
            this.cbxTaskType.Size = new System.Drawing.Size(248, 26);
            this.cbxTaskType.TabIndex = 5;
            // 
            // tbxStartLocation
            // 
            this.tbxStartLocation.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbxStartLocation.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbxStartLocation.Location = new System.Drawing.Point(150, 204);
            this.tbxStartLocation.Margin = new System.Windows.Forms.Padding(4);
            this.tbxStartLocation.Name = "tbxStartLocation";
            this.tbxStartLocation.Size = new System.Drawing.Size(248, 28);
            this.tbxStartLocation.TabIndex = 6;
            // 
            // tbxAdditionalInfo
            // 
            this.tbxAdditionalInfo.Location = new System.Drawing.Point(150, 426);
            this.tbxAdditionalInfo.Margin = new System.Windows.Forms.Padding(4);
            this.tbxAdditionalInfo.Name = "tbxAdditionalInfo";
            this.tbxAdditionalInfo.Size = new System.Drawing.Size(248, 28);
            this.tbxAdditionalInfo.TabIndex = 10;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(24, 508);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(62, 18);
            this.label12.TabIndex = 61;
            this.label12.Text = "任务池";
            // 
            // cbx_unPreTaskFlag
            // 
            this.cbx_unPreTaskFlag.AutoSize = true;
            this.cbx_unPreTaskFlag.Location = new System.Drawing.Point(424, 122);
            this.cbx_unPreTaskFlag.Name = "cbx_unPreTaskFlag";
            this.cbx_unPreTaskFlag.Size = new System.Drawing.Size(106, 22);
            this.cbx_unPreTaskFlag.TabIndex = 62;
            this.cbx_unPreTaskFlag.Text = "非预任务";
            this.cbx_unPreTaskFlag.UseVisualStyleBackColor = true;
            this.cbx_unPreTaskFlag.Visible = false;
            // 
            // frmAddTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 592);
            this.Controls.Add(this.cbx_unPreTaskFlag);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.radio_running);
            this.Controls.Add(this.radio_plan);
            this.Controls.Add(this.tbxAdditionalInfo);
            this.Controls.Add(this.tbxStartLocation);
            this.Controls.Add(this.cbxTaskType);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.tbxDescription);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cbxSource);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbxBizType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbxGiveTaskCode);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbxTaskCode);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbxContainerCodes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbxEndLocation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCacnel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddTask";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加手工任务";
            this.Load += new System.EventHandler(this.frmAddTask_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxTaskCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxContainerCodes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxEndLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCacnel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox cbxGiveTaskCode;
        private System.Windows.Forms.ComboBox cbxSource;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbxBizType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxDescription;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ComboBox cbxTaskType;
        private System.Windows.Forms.TextBox tbxStartLocation;
        private System.Windows.Forms.TextBox tbxAdditionalInfo;
        private System.Windows.Forms.RadioButton radio_plan;
        private System.Windows.Forms.RadioButton radio_running;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox cbx_unPreTaskFlag;
    }
}