namespace Wcs.App.Plugins.Reports
{
    partial class frmCraneWarningReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudMinValue = new System.Windows.Forms.NumericUpDown();
            this.cbxIgMinValue = new System.Windows.Forms.CheckBox();
            this.lblCreateIndex = new System.Windows.Forms.Label();
            this.btnCount = new System.Windows.Forms.Button();
            this.cbxFilterFirstAndLastErrors = new System.Windows.Forms.CheckBox();
            this.cbxCrane = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvGridCount = new System.Windows.Forms.DataGridView();
            this.colErrorCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrorDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrorCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colParcent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalSeonds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSecondsPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出为excel文档ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.另存为图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.导出为excel文档ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinValue)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGridCount)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.nudMinValue);
            this.groupBox1.Controls.Add(this.cbxIgMinValue);
            this.groupBox1.Controls.Add(this.lblCreateIndex);
            this.groupBox1.Controls.Add(this.btnCount);
            this.groupBox1.Controls.Add(this.cbxFilterFirstAndLastErrors);
            this.groupBox1.Controls.Add(this.cbxCrane);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtpEndTime);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dtpStartTime);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(988, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "统计选项";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(841, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "秒不统计";
            // 
            // nudMinValue
            // 
            this.nudMinValue.Location = new System.Drawing.Point(787, 16);
            this.nudMinValue.Name = "nudMinValue";
            this.nudMinValue.Size = new System.Drawing.Size(50, 21);
            this.nudMinValue.TabIndex = 12;
            // 
            // cbxIgMinValue
            // 
            this.cbxIgMinValue.AutoSize = true;
            this.cbxIgMinValue.Checked = true;
            this.cbxIgMinValue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxIgMinValue.Location = new System.Drawing.Point(743, 19);
            this.cbxIgMinValue.Name = "cbxIgMinValue";
            this.cbxIgMinValue.Size = new System.Drawing.Size(48, 16);
            this.cbxIgMinValue.TabIndex = 11;
            this.cbxIgMinValue.Text = "小于";
            this.cbxIgMinValue.UseVisualStyleBackColor = true;
            this.cbxIgMinValue.CheckedChanged += new System.EventHandler(this.cbxIgMinValue_CheckedChanged);
            // 
            // lblCreateIndex
            // 
            this.lblCreateIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCreateIndex.AutoSize = true;
            this.lblCreateIndex.Location = new System.Drawing.Point(904, 21);
            this.lblCreateIndex.Name = "lblCreateIndex";
            this.lblCreateIndex.Size = new System.Drawing.Size(11, 12);
            this.lblCreateIndex.TabIndex = 10;
            this.lblCreateIndex.Text = "?";
            this.toolTip1.SetToolTip(this.lblCreateIndex, "双击创建索引信息（只需要创建一次即可）");
            this.lblCreateIndex.DoubleClick += new System.EventHandler(this.lblCreateIndex_DoubleClick);
            // 
            // btnCount
            // 
            this.btnCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCount.Location = new System.Drawing.Point(921, 16);
            this.btnCount.Name = "btnCount";
            this.btnCount.Size = new System.Drawing.Size(50, 23);
            this.btnCount.TabIndex = 9;
            this.btnCount.Text = "统计";
            this.btnCount.UseVisualStyleBackColor = true;
            this.btnCount.Click += new System.EventHandler(this.btnCount_Click);
            // 
            // cbxFilterFirstAndLastErrors
            // 
            this.cbxFilterFirstAndLastErrors.AutoSize = true;
            this.cbxFilterFirstAndLastErrors.Checked = true;
            this.cbxFilterFirstAndLastErrors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbxFilterFirstAndLastErrors.Location = new System.Drawing.Point(626, 19);
            this.cbxFilterFirstAndLastErrors.Name = "cbxFilterFirstAndLastErrors";
            this.cbxFilterFirstAndLastErrors.Size = new System.Drawing.Size(108, 16);
            this.cbxFilterFirstAndLastErrors.TabIndex = 8;
            this.cbxFilterFirstAndLastErrors.Text = "跨天数据不统计";
            this.cbxFilterFirstAndLastErrors.UseVisualStyleBackColor = true;
            // 
            // cbxCrane
            // 
            this.cbxCrane.FormattingEnabled = true;
            this.cbxCrane.Location = new System.Drawing.Point(546, 17);
            this.cbxCrane.Name = "cbxCrane";
            this.cbxCrane.Size = new System.Drawing.Size(73, 20);
            this.cbxCrane.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(489, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "堆垛机";
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndTime.Location = new System.Drawing.Point(312, 17);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(153, 21);
            this.dtpEndTime.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(253, 21);
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
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(13, 74);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(988, 416);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(980, 390);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "合计";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvGridCount);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.chart1);
            this.splitContainer1.Size = new System.Drawing.Size(974, 384);
            this.splitContainer1.SplitterDistance = 640;
            this.splitContainer1.TabIndex = 0;
            // 
            // dgvGridCount
            // 
            this.dgvGridCount.AllowUserToAddRows = false;
            this.dgvGridCount.AllowUserToDeleteRows = false;
            this.dgvGridCount.AllowUserToOrderColumns = true;
            this.dgvGridCount.AllowUserToResizeColumns = false;
            this.dgvGridCount.AllowUserToResizeRows = false;
            this.dgvGridCount.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvGridCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvGridCount.ColumnHeadersHeight = 24;
            this.dgvGridCount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvGridCount.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colErrorCode,
            this.colErrorDescription,
            this.colErrorCount,
            this.colParcent,
            this.colTotalSeonds,
            this.colSecondsPercent});
            this.dgvGridCount.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvGridCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvGridCount.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgvGridCount.Location = new System.Drawing.Point(0, 0);
            this.dgvGridCount.Name = "dgvGridCount";
            this.dgvGridCount.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvGridCount.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvGridCount.RowHeadersWidth = 30;
            this.dgvGridCount.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(248)))));
            this.dgvGridCount.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvGridCount.RowTemplate.Height = 23;
            this.dgvGridCount.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGridCount.Size = new System.Drawing.Size(640, 384);
            this.dgvGridCount.TabIndex = 3;
            this.dgvGridCount.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvGridCount_CellFormatting);
            this.dgvGridCount.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvList_RowPostPaint);
            this.dgvGridCount.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvGridCount_RowsAdded);
            // 
            // colErrorCode
            // 
            this.colErrorCode.DataPropertyName = "Code";
            this.colErrorCode.HeaderText = "故障码";
            this.colErrorCode.Name = "colErrorCode";
            this.colErrorCode.ReadOnly = true;
            this.colErrorCode.Width = 60;
            // 
            // colErrorDescription
            // 
            this.colErrorDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colErrorDescription.DataPropertyName = "Description";
            this.colErrorDescription.HeaderText = "故障名称";
            this.colErrorDescription.Name = "colErrorDescription";
            this.colErrorDescription.ReadOnly = true;
            // 
            // colErrorCount
            // 
            this.colErrorCount.DataPropertyName = "Count";
            this.colErrorCount.HeaderText = "次数";
            this.colErrorCount.Name = "colErrorCount";
            this.colErrorCount.ReadOnly = true;
            this.colErrorCount.Width = 60;
            // 
            // colParcent
            // 
            this.colParcent.DataPropertyName = "Percent";
            this.colParcent.HeaderText = "%(次数)";
            this.colParcent.Name = "colParcent";
            this.colParcent.ReadOnly = true;
            // 
            // colTotalSeonds
            // 
            this.colTotalSeonds.DataPropertyName = "TotalSecnods";
            this.colTotalSeonds.HeaderText = "总时长";
            this.colTotalSeonds.Name = "colTotalSeonds";
            this.colTotalSeonds.ReadOnly = true;
            this.colTotalSeonds.Width = 120;
            // 
            // colSecondsPercent
            // 
            this.colSecondsPercent.DataPropertyName = "TotalSecondsPercent";
            this.colSecondsPercent.HeaderText = "%(时间)";
            this.colSecondsPercent.Name = "colSecondsPercent";
            this.colSecondsPercent.ReadOnly = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出为excel文档ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(175, 26);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // 导出为excel文档ToolStripMenuItem
            // 
            this.导出为excel文档ToolStripMenuItem.Name = "导出为excel文档ToolStripMenuItem";
            this.导出为excel文档ToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.导出为excel文档ToolStripMenuItem.Text = "导出为excel文档...";
            this.导出为excel文档ToolStripMenuItem.Click += new System.EventHandler(this.导出为excel文档ToolStripMenuItem_Click);
            // 
            // chart1
            // 
            chartArea1.Area3DStyle.Enable3D = true;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.ContextMenuStrip = this.contextMenuStrip2;
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(330, 384);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.另存为图片ToolStripMenuItem});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(137, 26);
            this.contextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip2_Opening);
            // 
            // 另存为图片ToolStripMenuItem
            // 
            this.另存为图片ToolStripMenuItem.Name = "另存为图片ToolStripMenuItem";
            this.另存为图片ToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.另存为图片ToolStripMenuItem.Text = "另存为图片";
            this.另存为图片ToolStripMenuItem.Click += new System.EventHandler(this.另存为图片ToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(980, 390);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "明细";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.ColumnHeadersHeight = 24;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Column1,
            this.Column2,
            this.Column3});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip3;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.RowHeadersWidth = 30;
            this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(248)))));
            this.dataGridView1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(974, 384);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvList_RowPostPaint);
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "DeviceName";
            this.Column4.HeaderText = "堆垛机名称";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Code";
            this.dataGridViewTextBoxColumn1.HeaderText = "故障码";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "Description";
            this.dataGridViewTextBoxColumn2.HeaderText = "故障名称";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "From";
            dataGridViewCellStyle2.Format = "yyyy-MM-dd HH:mm:ss.fff";
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.HeaderText = "开始时间";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 200;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "To";
            dataGridViewCellStyle3.Format = "yyyy-MM-dd HH:mm:ss.fff";
            this.Column2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column2.HeaderText = "结束时间";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 200;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "TotalSeconds";
            this.Column3.HeaderText = "持续时长";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 120;
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出为excel文档ToolStripMenuItem1});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(175, 26);
            this.contextMenuStrip3.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip3_Opening);
            // 
            // 导出为excel文档ToolStripMenuItem1
            // 
            this.导出为excel文档ToolStripMenuItem1.Name = "导出为excel文档ToolStripMenuItem1";
            this.导出为excel文档ToolStripMenuItem1.Size = new System.Drawing.Size(174, 22);
            this.导出为excel文档ToolStripMenuItem1.Text = "导出为excel文档...";
            this.导出为excel文档ToolStripMenuItem1.Click += new System.EventHandler(this.导出为excel文档ToolStripMenuItem1_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // frmCraneWarningReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 502);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmCraneWarningReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "堆垛机报警统计";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinValue)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGridCount)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxCrane;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbxFilterFirstAndLastErrors;
        private System.Windows.Forms.Button btnCount;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dgvGridCount;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label lblCreateIndex;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 导出为excel文档ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 另存为图片ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem 导出为excel文档ToolStripMenuItem1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrorCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrorDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrorCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParcent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalSeonds;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSecondsPercent;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudMinValue;
        private System.Windows.Forms.CheckBox cbxIgMinValue;
    }
}