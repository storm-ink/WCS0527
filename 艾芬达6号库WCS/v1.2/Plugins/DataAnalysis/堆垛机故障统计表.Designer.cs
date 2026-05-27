namespace Wcs.App.Plugins.DataAnalysis
{
    partial class 堆垛机故障统计表
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
            this.tbxDeviceName = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dgvGrid = new System.Windows.Forms.DataGridView();
            this.DeviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequestStateCommandReplyDataLog_ErrorCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._持续时间列 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.To = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dateTimeStart = new System.Windows.Forms.DateTimePicker();
            this.dateTimeEnd = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnExel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "设备名称：";
            // 
            // tbxDeviceName
            // 
            this.tbxDeviceName.AcceptsReturn = true;
            this.tbxDeviceName.Location = new System.Drawing.Point(83, 22);
            this.tbxDeviceName.Name = "tbxDeviceName";
            this.tbxDeviceName.Size = new System.Drawing.Size(100, 21);
            this.tbxDeviceName.TabIndex = 1;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(647, 22);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dgvGrid
            // 
            this.dgvGrid.AllowUserToAddRows = false;
            this.dgvGrid.AllowUserToDeleteRows = false;
            this.dgvGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGrid.BackgroundColor = System.Drawing.Color.White;
            this.dgvGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DeviceName,
            this.RequestStateCommandReplyDataLog_ErrorCode,
            this._持续时间列,
            this.From,
            this.To});
            this.dgvGrid.Location = new System.Drawing.Point(2, 71);
            this.dgvGrid.Name = "dgvGrid";
            this.dgvGrid.RowTemplate.Height = 23;
            this.dgvGrid.Size = new System.Drawing.Size(808, 493);
            this.dgvGrid.TabIndex = 3;
            this.dgvGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvGrid_CellFormatting);
            this.dgvGrid.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvList_RowPostPaint);
            // 
            // DeviceName
            // 
            this.DeviceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DeviceName.DataPropertyName = "DeviceName";
            this.DeviceName.HeaderText = "设备名称";
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.ReadOnly = true;
            // 
            // RequestStateCommandReplyDataLog_ErrorCode
            // 
            this.RequestStateCommandReplyDataLog_ErrorCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RequestStateCommandReplyDataLog_ErrorCode.DataPropertyName = "Code";
            this.RequestStateCommandReplyDataLog_ErrorCode.HeaderText = "报警码";
            this.RequestStateCommandReplyDataLog_ErrorCode.Name = "RequestStateCommandReplyDataLog_ErrorCode";
            this.RequestStateCommandReplyDataLog_ErrorCode.ReadOnly = true;
            // 
            // _持续时间列
            // 
            this._持续时间列.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._持续时间列.DataPropertyName = "TotalSeconds";
            this._持续时间列.HeaderText = "持续时间";
            this._持续时间列.Name = "_持续时间列";
            this._持续时间列.ReadOnly = true;
            // 
            // From
            // 
            this.From.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.From.DataPropertyName = "From";
            this.From.HeaderText = "发生时间";
            this.From.Name = "From";
            this.From.ReadOnly = true;
            // 
            // To
            // 
            this.To.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.To.DataPropertyName = "To";
            this.To.HeaderText = "结束时间";
            this.To.Name = "To";
            this.To.ReadOnly = true;
            // 
            // dateTimeStart
            // 
            this.dateTimeStart.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateTimeStart.Location = new System.Drawing.Point(268, 24);
            this.dateTimeStart.Name = "dateTimeStart";
            this.dateTimeStart.ShowCheckBox = true;
            this.dateTimeStart.Size = new System.Drawing.Size(133, 21);
            this.dateTimeStart.TabIndex = 4;
            // 
            // dateTimeEnd
            // 
            this.dateTimeEnd.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateTimeEnd.Location = new System.Drawing.Point(489, 22);
            this.dateTimeEnd.Name = "dateTimeEnd";
            this.dateTimeEnd.ShowCheckBox = true;
            this.dateTimeEnd.Size = new System.Drawing.Size(130, 21);
            this.dateTimeEnd.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(199, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "开始时间：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(418, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "结束时间：";
            // 
            // btnExel
            // 
            this.btnExel.Location = new System.Drawing.Point(735, 22);
            this.btnExel.Name = "btnExel";
            this.btnExel.Size = new System.Drawing.Size(75, 23);
            this.btnExel.TabIndex = 8;
            this.btnExel.Text = "导出";
            this.btnExel.UseVisualStyleBackColor = true;
            this.btnExel.Click += new System.EventHandler(this.btnExel_Click);
            // 
            // 堆垛机故障统计表
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 576);
            this.Controls.Add(this.btnExel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateTimeEnd);
            this.Controls.Add(this.dateTimeStart);
            this.Controls.Add(this.dgvGrid);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.tbxDeviceName);
            this.Controls.Add(this.label1);
            this.Name = "堆垛机故障统计表";
            this.Text = "堆垛机故障统计表";
            ((System.ComponentModel.ISupportInitialize)(this.dgvGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxDeviceName;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView dgvGrid;
        private System.Windows.Forms.DateTimePicker dateTimeStart;
        private System.Windows.Forms.DateTimePicker dateTimeEnd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequestStateCommandReplyDataLog_ErrorCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn _持续时间列;
        private System.Windows.Forms.DataGridViewTextBoxColumn From;
        private System.Windows.Forms.DataGridViewTextBoxColumn To;
        private System.Windows.Forms.Button btnExel;
    }
}