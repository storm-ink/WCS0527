namespace Wcs.App.Plugins.TaskManager
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxKey = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxEndLocation = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxStartLocation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxContainerCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxTaskCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmsMainTask = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiSuspend = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiComplete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiArchive = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPriority = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.tsmiView = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAdditionalInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colManualPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWcsPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProperity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colContainerCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrentLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBizType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTaskCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvGrid = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.paging1 = new Wcs.App.Plugins.TaskManager.Paging();
            this.groupBox1.SuspendLayout();
            this.cmsMainTask.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbxKey);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.cmbStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbxEndLocation);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.tbxStartLocation);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tbxContainerCode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbxTaskCode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1607, 56);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "任务查询";
            // 
            // tbxKey
            // 
            this.tbxKey.Location = new System.Drawing.Point(795, 22);
            this.tbxKey.Name = "tbxKey";
            this.tbxKey.Size = new System.Drawing.Size(64, 21);
            this.tbxKey.TabIndex = 12;
            this.tbxKey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbxTaskCode_KeyUp);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(778, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "*";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(865, 21);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(61, 23);
            this.btnSearch.TabIndex = 10;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(673, 22);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(86, 20);
            this.cmbStatus.TabIndex = 9;
            this.cmbStatus.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbxTaskCode_KeyUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(637, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "状态";
            // 
            // tbxEndLocation
            // 
            this.tbxEndLocation.Location = new System.Drawing.Point(529, 22);
            this.tbxEndLocation.Name = "tbxEndLocation";
            this.tbxEndLocation.Size = new System.Drawing.Size(89, 21);
            this.tbxEndLocation.TabIndex = 7;
            this.tbxEndLocation.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbxTaskCode_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(470, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "终点位置";
            // 
            // tbxStartLocation
            // 
            this.tbxStartLocation.Location = new System.Drawing.Point(364, 22);
            this.tbxStartLocation.Name = "tbxStartLocation";
            this.tbxStartLocation.Size = new System.Drawing.Size(89, 21);
            this.tbxStartLocation.TabIndex = 5;
            this.tbxStartLocation.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbxTaskCode_KeyUp);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(305, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "起始位置";
            // 
            // tbxContainerCode
            // 
            this.tbxContainerCode.Location = new System.Drawing.Point(216, 22);
            this.tbxContainerCode.Name = "tbxContainerCode";
            this.tbxContainerCode.Size = new System.Drawing.Size(74, 21);
            this.tbxContainerCode.TabIndex = 3;
            this.tbxContainerCode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbxTaskCode_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(157, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "容器编码";
            // 
            // tbxTaskCode
            // 
            this.tbxTaskCode.Location = new System.Drawing.Point(59, 22);
            this.tbxTaskCode.Name = "tbxTaskCode";
            this.tbxTaskCode.Size = new System.Drawing.Size(82, 21);
            this.tbxTaskCode.TabIndex = 1;
            this.tbxTaskCode.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbxTaskCode_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "任务号";
            // 
            // cmsMainTask
            // 
            this.cmsMainTask.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSuspend,
            this.tsmiCancel,
            this.tsmiComplete,
            this.tsmiArchive,
            this.tsmiPriority,
            this.tsmiView,
            this.toolStripMenuItem1,
            this.tsmiRefresh});
            this.cmsMainTask.Name = "cmsMainTask";
            this.cmsMainTask.Size = new System.Drawing.Size(173, 180);
            this.cmsMainTask.Opening += new System.ComponentModel.CancelEventHandler(this.cmsMainTask_Opening);
            // 
            // tsmiSuspend
            // 
            this.tsmiSuspend.Image = global::Wcs.App.Plugins.TaskManager.Properties.Resources.暂停;
            this.tsmiSuspend.Name = "tsmiSuspend";
            this.tsmiSuspend.Size = new System.Drawing.Size(172, 22);
            this.tsmiSuspend.Text = "暂停";
            this.tsmiSuspend.Click += new System.EventHandler(this.tsmiSuspend_Click);
            // 
            // tsmiCancel
            // 
            this.tsmiCancel.Name = "tsmiCancel";
            this.tsmiCancel.Size = new System.Drawing.Size(172, 22);
            this.tsmiCancel.Text = "取消";
            this.tsmiCancel.Click += new System.EventHandler(this.tsmiCancel_Click);
            // 
            // tsmiComplete
            // 
            this.tsmiComplete.Image = global::Wcs.App.Plugins.TaskManager.Properties.Resources.同意;
            this.tsmiComplete.Name = "tsmiComplete";
            this.tsmiComplete.Size = new System.Drawing.Size(172, 22);
            this.tsmiComplete.Text = "强制完成";
            this.tsmiComplete.Click += new System.EventHandler(this.tsmiComplete_Click);
            // 
            // tsmiArchive
            // 
            this.tsmiArchive.Image = global::Wcs.App.Plugins.TaskManager.Properties.Resources.删除;
            this.tsmiArchive.Name = "tsmiArchive";
            this.tsmiArchive.Size = new System.Drawing.Size(172, 22);
            this.tsmiArchive.Text = "归档任务";
            this.tsmiArchive.Click += new System.EventHandler(this.tsmiArchive_Click);
            // 
            // tsmiPriority
            // 
            this.tsmiPriority.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9,
            this.toolStripMenuItem10,
            this.toolStripMenuItem11,
            this.toolStripMenuItem12,
            this.toolStripTextBox1});
            this.tsmiPriority.Name = "tsmiPriority";
            this.tsmiPriority.Size = new System.Drawing.Size(172, 22);
            this.tsmiPriority.Text = "优先级";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem3.Text = "1";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem_priority_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem4.Text = "2";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem_priority_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem5.Text = "3";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem_priority_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem6.Text = "4";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem_priority_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem7.Text = "5";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem_priority_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem8.Text = "6";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem_priority_Click);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem9.Text = "7";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripMenuItem_priority_Click);
            // 
            // toolStripMenuItem10
            // 
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem10.Text = "8";
            this.toolStripMenuItem10.Click += new System.EventHandler(this.toolStripMenuItem_priority_Click);
            // 
            // toolStripMenuItem11
            // 
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem11.Text = "9";
            this.toolStripMenuItem11.Click += new System.EventHandler(this.toolStripMenuItem_priority_Click);
            // 
            // toolStripMenuItem12
            // 
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new System.Drawing.Size(160, 22);
            this.toolStripMenuItem12.Text = "10";
            this.toolStripMenuItem12.Click += new System.EventHandler(this.toolStripMenuItem_priority_Click);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 23);
            this.toolStripTextBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.toolStripTextBox1_KeyUp);
            // 
            // tsmiView
            // 
            this.tsmiView.Image = global::Wcs.App.Plugins.TaskManager.Properties.Resources.详情;
            this.tsmiView.Name = "tsmiView";
            this.tsmiView.Size = new System.Drawing.Size(172, 22);
            this.tsmiView.Text = "查看详情";
            this.tsmiView.Click += new System.EventHandler(this.tsmiView_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(172, 22);
            this.toolStripMenuItem1.Text = "引发状态改变事件";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // tsmiRefresh
            // 
            this.tsmiRefresh.Name = "tsmiRefresh";
            this.tsmiRefresh.Size = new System.Drawing.Size(172, 22);
            this.tsmiRefresh.Text = "刷新";
            this.tsmiRefresh.Click += new System.EventHandler(this.tsmiRefresh_Click);
            // 
            // colCreatedAt
            // 
            this.colCreatedAt.DataPropertyName = "CreatedAt";
            dataGridViewCellStyle1.Format = "yyyy-MM-dd HH:mm:ss.fff";
            this.colCreatedAt.DefaultCellStyle = dataGridViewCellStyle1;
            this.colCreatedAt.HeaderText = "创建时间";
            this.colCreatedAt.Name = "colCreatedAt";
            this.colCreatedAt.ReadOnly = true;
            this.colCreatedAt.Width = 150;
            // 
            // colAdditionalInfo
            // 
            this.colAdditionalInfo.HeaderText = "附加属性";
            this.colAdditionalInfo.Name = "colAdditionalInfo";
            this.colAdditionalInfo.ReadOnly = true;
            // 
            // colDescription
            // 
            this.colDescription.DataPropertyName = "Description";
            this.colDescription.HeaderText = "描述";
            this.colDescription.Name = "colDescription";
            this.colDescription.ReadOnly = true;
            // 
            // colManualPriority
            // 
            this.colManualPriority.DataPropertyName = "ManualPriority";
            this.colManualPriority.HeaderText = "紧急执行";
            this.colManualPriority.Name = "colManualPriority";
            this.colManualPriority.ReadOnly = true;
            // 
            // colWcsPriority
            // 
            this.colWcsPriority.DataPropertyName = "WcsPriority";
            this.colWcsPriority.HeaderText = "WCS优先级";
            this.colWcsPriority.Name = "colWcsPriority";
            this.colWcsPriority.ReadOnly = true;
            // 
            // colProperity
            // 
            this.colProperity.DataPropertyName = "Priority";
            this.colProperity.HeaderText = "优先级";
            this.colProperity.Name = "colProperity";
            this.colProperity.ReadOnly = true;
            this.colProperity.Width = 70;
            // 
            // colContainerCode
            // 
            this.colContainerCode.DataPropertyName = "ContainerCodes";
            this.colContainerCode.HeaderText = "容器编码";
            this.colContainerCode.Name = "colContainerCode";
            this.colContainerCode.ReadOnly = true;
            this.colContainerCode.Width = 150;
            // 
            // colStatus
            // 
            this.colStatus.DataPropertyName = "Status";
            this.colStatus.HeaderText = "状态";
            this.colStatus.Name = "colStatus";
            this.colStatus.ReadOnly = true;
            // 
            // colCurrentLocation
            // 
            this.colCurrentLocation.DataPropertyName = "CurrentLocation";
            this.colCurrentLocation.HeaderText = "当前位置";
            this.colCurrentLocation.Name = "colCurrentLocation";
            this.colCurrentLocation.ReadOnly = true;
            // 
            // colEndLocation
            // 
            this.colEndLocation.DataPropertyName = "EndLocation";
            this.colEndLocation.HeaderText = "结束位置";
            this.colEndLocation.Name = "colEndLocation";
            this.colEndLocation.ReadOnly = true;
            // 
            // colStartLocation
            // 
            this.colStartLocation.DataPropertyName = "StartLocation";
            this.colStartLocation.HeaderText = "起始位置";
            this.colStartLocation.Name = "colStartLocation";
            this.colStartLocation.ReadOnly = true;
            // 
            // colBizType
            // 
            this.colBizType.DataPropertyName = "BizType";
            this.colBizType.HeaderText = "业务类型";
            this.colBizType.Name = "colBizType";
            this.colBizType.ReadOnly = true;
            this.colBizType.Width = 80;
            // 
            // colTaskType
            // 
            this.colTaskType.DataPropertyName = "TaskType";
            this.colTaskType.HeaderText = "任务类型";
            this.colTaskType.Name = "colTaskType";
            this.colTaskType.ReadOnly = true;
            this.colTaskType.Width = 80;
            // 
            // colTaskCode
            // 
            this.colTaskCode.DataPropertyName = "TaskCode";
            this.colTaskCode.HeaderText = "任务号";
            this.colTaskCode.Name = "colTaskCode";
            this.colTaskCode.ReadOnly = true;
            this.colTaskCode.Width = 120;
            // 
            // colId
            // 
            this.colId.DataPropertyName = "Id";
            this.colId.HeaderText = "编号";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            this.colId.Width = 80;
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
            this.colWcsPriority,
            this.colManualPriority,
            this.colDescription,
            this.colAdditionalInfo,
            this.colCreatedAt});
            this.dgvGrid.ContextMenuStrip = this.cmsMainTask;
            this.dgvGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGrid.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgvGrid.Location = new System.Drawing.Point(5, 61);
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
            this.dgvGrid.Size = new System.Drawing.Size(1607, 549);
            this.dgvGrid.TabIndex = 2;
            this.dgvGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvGrid_CellDoubleClick);
            this.dgvGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvGrid_CellFormatting);
            this.dgvGrid.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvList_RowPostPaint);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.paging1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(5, 610);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1607, 29);
            this.panel1.TabIndex = 4;
            // 
            // paging1
            // 
            this.paging1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.paging1.CurrentPage = 1;
            this.paging1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paging1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.paging1.Location = new System.Drawing.Point(0, 0);
            this.paging1.Name = "paging1";
            this.paging1.PageCount = 1;
            this.paging1.PageSize = 25;
            this.paging1.QueryCount = 0;
            this.paging1.Size = new System.Drawing.Size(1607, 29);
            this.paging1.TabIndex = 0;
            this.paging1.TotalCount = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1617, 644);
            this.Controls.Add(this.dgvGrid);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmMain";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "任务管理器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.cmsMainTask.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxEndLocation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxStartLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxContainerCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxTaskCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip cmsMainTask;
        private System.Windows.Forms.ToolStripMenuItem tsmiArchive;
        private System.Windows.Forms.ToolStripMenuItem tsmiPriority;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem10;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripMenuItem tsmiComplete;
        private System.Windows.Forms.ToolStripMenuItem tsmiRefresh;
        private System.Windows.Forms.ToolStripMenuItem tsmiSuspend;
        private System.Windows.Forms.ToolStripMenuItem tsmiCancel;
        private System.Windows.Forms.ToolStripMenuItem tsmiView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TextBox tbxKey;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCreatedAt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAdditionalInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colManualPriority;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWcsPriority;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProperity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContainerCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCurrentLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBizType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTaskCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridView dgvGrid;
        private System.Windows.Forms.Panel panel1;
        private Paging paging1;
    }
}
