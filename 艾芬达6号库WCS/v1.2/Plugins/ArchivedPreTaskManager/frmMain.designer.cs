namespace Wcs.App.Plugins.ArchivedPreTaskManager
{
    partial class frmMain
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.paging1 = new Wcs.App.Plugins.ArchivedPreTaskManager.Paging();
            this.dgvGrid = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBizType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrentLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colContainerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProperity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdditionalInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFinishedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRunningTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWaitTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxTaskTypes = new System.Windows.Forms.ComboBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cbxFromWms = new System.Windows.Forms.CheckBox();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tbxEndLocation = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxStartLocation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxContainerCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxTaskCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.paging1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(8, 914);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2410, 44);
            this.panel1.TabIndex = 4;
            // 
            // paging1
            // 
            this.paging1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.paging1.CurrentPage = 1;
            this.paging1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paging1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.paging1.Location = new System.Drawing.Point(0, 0);
            this.paging1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.paging1.Name = "paging1";
            this.paging1.PageCount = 1;
            this.paging1.PageSize = 25;
            this.paging1.QueryCount = 0;
            this.paging1.Size = new System.Drawing.Size(2410, 44);
            this.paging1.TabIndex = 0;
            this.paging1.TotalCount = 0;
            // 
            // dgvGrid
            // 
            this.dgvGrid.AllowUserToAddRows = false;
            this.dgvGrid.AllowUserToDeleteRows = false;
            this.dgvGrid.AllowUserToResizeRows = false;
            this.dgvGrid.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGrid.ColumnHeadersHeight = 24;
            this.dgvGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colTaskCode,
            this.colTaskType,
            this.colBizType,
            this.colStartLocation,
            this.colEndLocation,
            this.colCurrentLocation,
            this.colStatus,
            this.colContainerCode,
            this.colProperity,
            this.colDescription,
            this.colAdditionalInfo,
            this.colCreatedAt,
            this.colFinishedAt,
            this.colTotalTime,
            this.colRunningTime,
            this.colWaitTime});
            this.dgvGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGrid.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgvGrid.Location = new System.Drawing.Point(8, 134);
            this.dgvGrid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvGrid.Name = "dgvGrid";
            this.dgvGrid.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvGrid.RowHeadersWidth = 30;
            this.dgvGrid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(248)))));
            this.dgvGrid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvGrid.RowTemplate.Height = 23;
            this.dgvGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGrid.Size = new System.Drawing.Size(2410, 780);
            this.dgvGrid.TabIndex = 9;
            this.dgvGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGrid_CellDoubleClick);
            // 
            // colId
            // 
            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "编号";
            this.colId.MinimumWidth = 8;
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Width = 80;
            // 
            // colTaskCode
            // 
            this.colTaskCode.DataPropertyName = "TaskCode";
            this.colTaskCode.HeaderText = "任务号";
            this.colTaskCode.MinimumWidth = 8;
            this.colTaskCode.Name = "colTaskCode";
            this.colTaskCode.ReadOnly = true;
            this.colTaskCode.Width = 150;
            // 
            // colTaskType
            // 
            this.colTaskType.DataPropertyName = "TaskType";
            this.colTaskType.HeaderText = "任务类型";
            this.colTaskType.MinimumWidth = 8;
            this.colTaskType.Name = "colTaskType";
            this.colTaskType.ReadOnly = true;
            this.colTaskType.Width = 80;
            // 
            // colBizType
            // 
            this.colBizType.DataPropertyName = "BizType";
            this.colBizType.HeaderText = "业务类型";
            this.colBizType.MinimumWidth = 8;
            this.colBizType.Name = "colBizType";
            this.colBizType.ReadOnly = true;
            this.colBizType.Width = 80;
            // 
            // colStartLocation
            // 
            this.colStartLocation.DataPropertyName = "StartLocation";
            this.colStartLocation.HeaderText = "起始位置";
            this.colStartLocation.MinimumWidth = 8;
            this.colStartLocation.Name = "colStartLocation";
            this.colStartLocation.ReadOnly = true;
            this.colStartLocation.Width = 150;
            // 
            // colEndLocation
            // 
            this.colEndLocation.DataPropertyName = "EndLocation";
            this.colEndLocation.HeaderText = "结束位置";
            this.colEndLocation.MinimumWidth = 8;
            this.colEndLocation.Name = "colEndLocation";
            this.colEndLocation.ReadOnly = true;
            this.colEndLocation.Width = 150;
            // 
            // colCurrentLocation
            // 
            this.colCurrentLocation.DataPropertyName = "CurrentLocation";
            this.colCurrentLocation.HeaderText = "当前位置";
            this.colCurrentLocation.MinimumWidth = 8;
            this.colCurrentLocation.Name = "colCurrentLocation";
            this.colCurrentLocation.ReadOnly = true;
            this.colCurrentLocation.Width = 150;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "Status";
            this.colStatus.HeaderText = "状态";
            this.colStatus.MinimumWidth = 8;
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 150;
            // 
            // colContainerCode
            // 
            this.colContainerCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colContainerCode.DataPropertyName = "ContainerCode";
            this.colContainerCode.HeaderText = "容器编码";
            this.colContainerCode.MinimumWidth = 8;
            this.colContainerCode.Name = "colContainerCode";
            this.colContainerCode.ReadOnly = true;
            this.colContainerCode.Width = 150;
            // 
            // colProperity
            // 
            this.colProperity.DataPropertyName = "Priority";
            this.colProperity.HeaderText = "优先级";
            this.colProperity.MinimumWidth = 8;
            this.colProperity.Name = "colProperity";
            this.colProperity.ReadOnly = true;
            this.colProperity.Width = 70;
            // 
            // colDescription
            // 
            this.colDescription.DataPropertyName = "Description";
            this.colDescription.HeaderText = "描述";
            this.colDescription.MinimumWidth = 8;
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            this.colDescription.Width = 150;
            // 
            // colAdditionalInfo
            // 
            this.colAdditionalInfo.DataPropertyName = "TaskAdditionalInfo";
            this.colAdditionalInfo.HeaderText = "附加属性";
            this.colAdditionalInfo.MinimumWidth = 8;
            this.colAdditionalInfo.Name = "colAdditionalInfo";
            this.colAdditionalInfo.ReadOnly = true;
            this.colAdditionalInfo.Width = 150;
            // 
            // colCreatedAt
            // 
            this.colCreatedAt.DataPropertyName = "CreatedAt";
            dataGridViewCellStyle1.Format = "yyyy-MM-dd HH:mm:ss.fff";
            this.colCreatedAt.DefaultCellStyle = dataGridViewCellStyle1;
            this.colCreatedAt.HeaderText = "创建时间";
            this.colCreatedAt.MinimumWidth = 8;
            this.colCreatedAt.Name = "colCreatedAt";
            this.colCreatedAt.ReadOnly = true;
            this.colCreatedAt.Width = 150;
            // 
            // colFinishedAt
            // 
            this.colFinishedAt.DataPropertyName = "FinishedAt";
            this.colFinishedAt.HeaderText = "完成时间";
            this.colFinishedAt.MinimumWidth = 8;
            this.colFinishedAt.Name = "colFinishedAt";
            this.colFinishedAt.ReadOnly = true;
            this.colFinishedAt.Width = 150;
            // 
            // colTotalTime
            // 
            this.colTotalTime.HeaderText = "总计时长";
            this.colTotalTime.MinimumWidth = 8;
            this.colTotalTime.Name = "colTotalTime";
            this.colTotalTime.ReadOnly = true;
            this.colTotalTime.Width = 78;
            // 
            // colRunningTime
            // 
            this.colRunningTime.HeaderText = "执行时长";
            this.colRunningTime.MinimumWidth = 8;
            this.colRunningTime.Name = "colRunningTime";
            this.colRunningTime.ReadOnly = true;
            this.colRunningTime.Width = 78;
            // 
            // colWaitTime
            // 
            this.colWaitTime.HeaderText = "等待时长";
            this.colWaitTime.MinimumWidth = 8;
            this.colWaitTime.Name = "colWaitTime";
            this.colWaitTime.ReadOnly = true;
            this.colWaitTime.Width = 78;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxTaskTypes);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbxFromWms);
            this.groupBox1.Controls.Add(this.dtpEndDate);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.dtpStartDate);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.tbxEndLocation);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbxStartLocation);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbxContainerCode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbxTaskCode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(8, 8);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(2410, 126);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "任务查询";
            // 
            // cbxTaskTypes
            // 
            this.cbxTaskTypes.FormattingEnabled = true;
            this.cbxTaskTypes.Location = new System.Drawing.Point(1160, 33);
            this.cbxTaskTypes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxTaskTypes.Name = "cbxTaskTypes";
            this.cbxTaskTypes.Size = new System.Drawing.Size(154, 26);
            this.cbxTaskTypes.TabIndex = 32;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(1324, 78);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 34);
            this.btnExport.TabIndex = 31;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Increment = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDown1.Location = new System.Drawing.Point(770, 81);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(134, 28);
            this.numericUpDown1.TabIndex = 30;
            this.numericUpDown1.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(681, 87);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 18);
            this.label5.TabIndex = 29;
            this.label5.Text = "返回条数";
            // 
            // cbxFromWms
            // 
            this.cbxFromWms.AutoSize = true;
            this.cbxFromWms.Checked = true;
            this.cbxFromWms.CheckState = System.Windows.Forms.CheckState.Indeterminate;
            this.cbxFromWms.Location = new System.Drawing.Point(1160, 86);
            this.cbxFromWms.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbxFromWms.Name = "cbxFromWms";
            this.cbxFromWms.Size = new System.Drawing.Size(97, 22);
            this.cbxFromWms.TabIndex = 28;
            this.cbxFromWms.Text = "来自WMS";
            this.cbxFromWms.ThreeState = true;
            this.cbxFromWms.UseVisualStyleBackColor = true;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(430, 81);
            this.dtpEndDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.ShowCheckBox = true;
            this.dtpEndDate.Size = new System.Drawing.Size(224, 28);
            this.dtpEndDate.TabIndex = 27;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(342, 87);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(80, 18);
            this.label9.TabIndex = 26;
            this.label9.Text = "结束日期";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Location = new System.Drawing.Point(88, 81);
            this.dtpStartDate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.ShowCheckBox = true;
            this.dtpStartDate.Size = new System.Drawing.Size(224, 28);
            this.dtpStartDate.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 87);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 18);
            this.label8.TabIndex = 24;
            this.label8.Text = "开始日期";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(1324, 32);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 34);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tbxEndLocation
            // 
            this.tbxEndLocation.Location = new System.Drawing.Point(1017, 33);
            this.tbxEndLocation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxEndLocation.Name = "tbxEndLocation";
            this.tbxEndLocation.Size = new System.Drawing.Size(132, 28);
            this.tbxEndLocation.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(928, 39);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "终点位置";
            // 
            // tbxStartLocation
            // 
            this.tbxStartLocation.Location = new System.Drawing.Point(770, 33);
            this.tbxStartLocation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxStartLocation.Name = "tbxStartLocation";
            this.tbxStartLocation.Size = new System.Drawing.Size(132, 28);
            this.tbxStartLocation.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(681, 39);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "起始位置";
            // 
            // tbxContainerCode
            // 
            this.tbxContainerCode.Location = new System.Drawing.Point(430, 32);
            this.tbxContainerCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxContainerCode.Name = "tbxContainerCode";
            this.tbxContainerCode.Size = new System.Drawing.Size(224, 28);
            this.tbxContainerCode.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(342, 38);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "容器编码";
            // 
            // tbxTaskCode
            // 
            this.tbxTaskCode.Location = new System.Drawing.Point(88, 33);
            this.tbxTaskCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxTaskCode.Name = "tbxTaskCode";
            this.tbxTaskCode.Size = new System.Drawing.Size(224, 28);
            this.tbxTaskCode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "任务号";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2426, 966);
            this.Controls.Add(this.dgvGrid);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmMain";
            this.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.Text = "计划任务管理器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Paging paging1;
        private System.Windows.Forms.DataGridView dgvGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBizType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurrentLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContainerCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProperity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdditionalInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFinishedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRunningTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWaitTime;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbxTaskTypes;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbxFromWms;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox tbxEndLocation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxStartLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxContainerCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxTaskCode;
        private System.Windows.Forms.Label label1;
    }
}
