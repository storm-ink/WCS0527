namespace Wcs.App.Plugins.Reports
{
    partial class frmTaskReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxEndLocation = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxStartLocation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCount = new System.Windows.Forms.Button();
            this.cbxDirection = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dgvGrid = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colContainerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFinishedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalSeconds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblAgvSeconds = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblMaxSeconds = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblMinSeconds = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出为excel文档ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbxEndLocation);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbxStartLocation);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnCount);
            this.groupBox1.Controls.Add(this.cbxDirection);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtpEndTime);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dtpStartTime);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1019, 55);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "统计选项";
            // 
            // tbxEndLocation
            // 
            this.tbxEndLocation.Location = new System.Drawing.Point(801, 17);
            this.tbxEndLocation.Name = "tbxEndLocation";
            this.tbxEndLocation.Size = new System.Drawing.Size(87, 21);
            this.tbxEndLocation.TabIndex = 14;
            this.toolTip1.SetToolTip(this.tbxEndLocation, "请使用用户编码值，如：00-001-1201。可以使用通配符*，如：00-00-1**1");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(742, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "终点位置";
            // 
            // tbxStartLocation
            // 
            this.tbxStartLocation.Location = new System.Drawing.Point(632, 17);
            this.tbxStartLocation.Name = "tbxStartLocation";
            this.tbxStartLocation.Size = new System.Drawing.Size(87, 21);
            this.tbxStartLocation.TabIndex = 12;
            this.toolTip1.SetToolTip(this.tbxStartLocation, "请使用用户编码值，如：00-001-1201。可以使用通配符*，如：00-00-1**1");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(573, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "起始位置";
            // 
            // btnCount
            // 
            this.btnCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCount.Location = new System.Drawing.Point(963, 17);
            this.btnCount.Name = "btnCount";
            this.btnCount.Size = new System.Drawing.Size(50, 23);
            this.btnCount.TabIndex = 9;
            this.btnCount.Text = "统计";
            this.btnCount.UseVisualStyleBackColor = true;
            this.btnCount.Click += new System.EventHandler(this.btnCount_Click);
            // 
            // cbxDirection
            // 
            this.cbxDirection.AutoCompleteCustomSource.AddRange(new string[] {
            "",
            "入",
            "出"});
            this.cbxDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxDirection.FormattingEnabled = true;
            this.cbxDirection.Items.AddRange(new object[] {
            "",
            "入",
            "出"});
            this.cbxDirection.Location = new System.Drawing.Point(503, 17);
            this.cbxDirection.Name = "cbxDirection";
            this.cbxDirection.Size = new System.Drawing.Size(54, 20);
            this.cbxDirection.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(468, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "方向";
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndTime.Location = new System.Drawing.Point(295, 17);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(153, 21);
            this.dtpEndTime.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(236, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "结束时间";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartTime.Location = new System.Drawing.Point(75, 17);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(153, 21);
            this.dtpStartTime.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "开始时间";
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // dgvGrid
            // 
            this.dgvGrid.AllowUserToAddRows = false;
            this.dgvGrid.AllowUserToDeleteRows = false;
            this.dgvGrid.AllowUserToOrderColumns = true;
            this.dgvGrid.AllowUserToResizeColumns = false;
            this.dgvGrid.AllowUserToResizeRows = false;
            this.dgvGrid.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGrid.ColumnHeadersHeight = 24;
            this.dgvGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colTaskCode,
            this.colStartLocation,
            this.colEndLocation,
            this.colContainerCode,
            this.colCreatedAt,
            this.colFinishedAt,
            this.colTotalSeconds});
            this.dgvGrid.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGrid.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgvGrid.Location = new System.Drawing.Point(5, 60);
            this.dgvGrid.Name = "dgvGrid";
            this.dgvGrid.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvGrid.RowHeadersWidth = 30;
            this.dgvGrid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(248)))));
            this.dgvGrid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvGrid.RowTemplate.Height = 23;
            this.dgvGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGrid.Size = new System.Drawing.Size(1019, 463);
            this.dgvGrid.TabIndex = 3;
            this.dgvGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvGrid_CellFormatting);
            this.dgvGrid.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvList_RowPostPaint);
            // 
            // colId
            // 
            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "编号";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Width = 80;
            // 
            // colTaskCode
            // 
            this.colTaskCode.DataPropertyName = "TaskCode";
            this.colTaskCode.HeaderText = "任务号";
            this.colTaskCode.Name = "colTaskCode";
            this.colTaskCode.ReadOnly = true;
            this.colTaskCode.Width = 150;
            // 
            // colStartLocation
            // 
            this.colStartLocation.DataPropertyName = "StartLocationUserCode";
            this.colStartLocation.HeaderText = "起始位置";
            this.colStartLocation.Name = "colStartLocation";
            this.colStartLocation.ReadOnly = true;
            // 
            // colEndLocation
            // 
            this.colEndLocation.DataPropertyName = "EndLocationUserCode";
            this.colEndLocation.HeaderText = "结束位置";
            this.colEndLocation.Name = "colEndLocation";
            this.colEndLocation.ReadOnly = true;
            // 
            // colContainerCode
            // 
            this.colContainerCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colContainerCode.DataPropertyName = "ContainerCodes";
            this.colContainerCode.HeaderText = "容器编码";
            this.colContainerCode.Name = "colContainerCode";
            this.colContainerCode.ReadOnly = true;
            // 
            // colCreatedAt
            // 
            this.colCreatedAt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCreatedAt.DataPropertyName = "CreatedAt";
            dataGridViewCellStyle1.Format = "yyyy-MM-dd HH:mm:ss.fff";
            this.colCreatedAt.DefaultCellStyle = dataGridViewCellStyle1;
            this.colCreatedAt.HeaderText = "创建时间";
            this.colCreatedAt.Name = "colCreatedAt";
            this.colCreatedAt.ReadOnly = true;
            // 
            // colFinishedAt
            // 
            this.colFinishedAt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFinishedAt.DataPropertyName = "FinishedAt";
            dataGridViewCellStyle2.Format = "yyyy-MM-dd HH:mm:ss.fff";
            this.colFinishedAt.DefaultCellStyle = dataGridViewCellStyle2;
            this.colFinishedAt.HeaderText = "完成时间";
            this.colFinishedAt.Name = "colFinishedAt";
            this.colFinishedAt.ReadOnly = true;
            // 
            // colTotalSeconds
            // 
            this.colTotalSeconds.DataPropertyName = "TotalSeconds";
            this.colTotalSeconds.HeaderText = "耗时";
            this.colTotalSeconds.Name = "colTotalSeconds";
            this.colTotalSeconds.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblAgvSeconds);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.lblMaxSeconds);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.lblMinSeconds);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.lblRecordCount);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(5, 523);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1019, 40);
            this.panel1.TabIndex = 4;
            // 
            // lblAgvSeconds
            // 
            this.lblAgvSeconds.AutoSize = true;
            this.lblAgvSeconds.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblAgvSeconds.ForeColor = System.Drawing.Color.Green;
            this.lblAgvSeconds.Location = new System.Drawing.Point(569, 14);
            this.lblAgvSeconds.Name = "lblAgvSeconds";
            this.lblAgvSeconds.Size = new System.Drawing.Size(25, 12);
            this.lblAgvSeconds.TabIndex = 8;
            this.lblAgvSeconds.Text = "0秒";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(511, 14);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 7;
            this.label13.Text = "平均耗时：";
            // 
            // lblMaxSeconds
            // 
            this.lblMaxSeconds.AutoSize = true;
            this.lblMaxSeconds.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMaxSeconds.ForeColor = System.Drawing.Color.Green;
            this.lblMaxSeconds.Location = new System.Drawing.Point(347, 14);
            this.lblMaxSeconds.Name = "lblMaxSeconds";
            this.lblMaxSeconds.Size = new System.Drawing.Size(25, 12);
            this.lblMaxSeconds.TabIndex = 6;
            this.lblMaxSeconds.Text = "0秒";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(287, 14);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 5;
            this.label11.Text = "最长耗时：";
            // 
            // lblMinSeconds
            // 
            this.lblMinSeconds.AutoSize = true;
            this.lblMinSeconds.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMinSeconds.ForeColor = System.Drawing.Color.Green;
            this.lblMinSeconds.Location = new System.Drawing.Point(218, 14);
            this.lblMinSeconds.Name = "lblMinSeconds";
            this.lblMinSeconds.Size = new System.Drawing.Size(25, 12);
            this.lblMinSeconds.TabIndex = 4;
            this.lblMinSeconds.Text = "0秒";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(159, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 3;
            this.label9.Text = "最短耗时：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(75, 14);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "条任务";
            // 
            // lblRecordCount
            // 
            this.lblRecordCount.AutoSize = true;
            this.lblRecordCount.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblRecordCount.ForeColor = System.Drawing.Color.Green;
            this.lblRecordCount.Location = new System.Drawing.Point(31, 14);
            this.lblRecordCount.Name = "lblRecordCount";
            this.lblRecordCount.Size = new System.Drawing.Size(12, 12);
            this.lblRecordCount.TabIndex = 1;
            this.lblRecordCount.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "共";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出为excel文档ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(175, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // 导出为excel文档ToolStripMenuItem
            // 
            this.导出为excel文档ToolStripMenuItem.Name = "导出为excel文档ToolStripMenuItem";
            this.导出为excel文档ToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.导出为excel文档ToolStripMenuItem.Text = "导出为excel文档...";
            this.导出为excel文档ToolStripMenuItem.Click += new System.EventHandler(this.导出为excel文档ToolStripMenuItem_Click);
            // 
            // frmTaskReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 568);
            this.Controls.Add(this.dgvGrid);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmTaskReport";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "任务耗时报表";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCount;
        private System.Windows.Forms.ComboBox cbxDirection;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxEndLocation;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxStartLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridView dgvGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContainerCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFinishedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalSeconds;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblAgvSeconds;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblMaxSeconds;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblMinSeconds;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblRecordCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 导出为excel文档ToolStripMenuItem;
    }
}