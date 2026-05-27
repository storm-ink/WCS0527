namespace Wcs.App.Plugins.LogTrace
{
    partial class frmLogTrace_TraceWindow
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbxLogNameMatch = new System.Windows.Forms.ComboBox();
            this.cbxStart = new System.Windows.Forms.CheckBox();
            this.nudMaxLines = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxOutputFormat = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxAutoScroll = new System.Windows.Forms.CheckBox();
            this.rbFatal = new System.Windows.Forms.RadioButton();
            this.rbError = new System.Windows.Forms.RadioButton();
            this.rbWarn = new System.Windows.Forms.RadioButton();
            this.rbInfo = new System.Windows.Forms.RadioButton();
            this.rbDebug = new System.Windows.Forms.RadioButton();
            this.rbTrace = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxLines)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.Location = new System.Drawing.Point(5, 87);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(780, 69);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbxLogNameMatch);
            this.groupBox1.Controls.Add(this.cbxStart);
            this.groupBox1.Controls.Add(this.nudMaxLines);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tbxOutputFormat);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbxAutoScroll);
            this.groupBox1.Controls.Add(this.rbFatal);
            this.groupBox1.Controls.Add(this.rbError);
            this.groupBox1.Controls.Add(this.rbWarn);
            this.groupBox1.Controls.Add(this.rbInfo);
            this.groupBox1.Controls.Add(this.rbDebug);
            this.groupBox1.Controls.Add(this.rbTrace);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(780, 76);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "日志跟踪选项";
            // 
            // tbxLogNameMatch
            // 
            this.tbxLogNameMatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxLogNameMatch.FormattingEnabled = true;
            this.tbxLogNameMatch.Items.AddRange(new object[] {
            "*"});
            this.tbxLogNameMatch.Location = new System.Drawing.Point(67, 45);
            this.tbxLogNameMatch.Name = "tbxLogNameMatch";
            this.tbxLogNameMatch.Size = new System.Drawing.Size(66, 20);
            this.tbxLogNameMatch.TabIndex = 15;
            this.tbxLogNameMatch.Text = "*";
            // 
            // cbxStart
            // 
            this.cbxStart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxStart.AutoSize = true;
            this.cbxStart.Location = new System.Drawing.Point(726, 47);
            this.cbxStart.Name = "cbxStart";
            this.cbxStart.Size = new System.Drawing.Size(48, 16);
            this.cbxStart.TabIndex = 14;
            this.cbxStart.Text = "启动";
            this.cbxStart.UseVisualStyleBackColor = true;
            this.cbxStart.CheckedChanged += new System.EventHandler(this.cbxStart_CheckedChanged);
            // 
            // nudMaxLines
            // 
            this.nudMaxLines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudMaxLines.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudMaxLines.Location = new System.Drawing.Point(198, 45);
            this.nudMaxLines.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudMaxLines.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudMaxLines.Name = "nudMaxLines";
            this.nudMaxLines.Size = new System.Drawing.Size(68, 21);
            this.nudMaxLines.TabIndex = 13;
            this.nudMaxLines.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(139, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "最大行数";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(272, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "日志等级";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "名称匹配";
            // 
            // tbxOutputFormat
            // 
            this.tbxOutputFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxOutputFormat.Location = new System.Drawing.Point(66, 18);
            this.tbxOutputFormat.Name = "tbxOutputFormat";
            this.tbxOutputFormat.Size = new System.Drawing.Size(708, 21);
            this.tbxOutputFormat.TabIndex = 8;
            this.tbxOutputFormat.Text = "${longdate} ${level} ${message} ${exception:format=Message, Type, ShortType, ToSt" +
    "ring, Method, StackTrace}";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "输出格式";
            // 
            // cbxAutoScroll
            // 
            this.cbxAutoScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxAutoScroll.AutoSize = true;
            this.cbxAutoScroll.Location = new System.Drawing.Point(672, 47);
            this.cbxAutoScroll.Name = "cbxAutoScroll";
            this.cbxAutoScroll.Size = new System.Drawing.Size(48, 16);
            this.cbxAutoScroll.TabIndex = 6;
            this.cbxAutoScroll.Text = "滚动";
            this.cbxAutoScroll.UseVisualStyleBackColor = true;
            // 
            // rbFatal
            // 
            this.rbFatal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbFatal.AutoSize = true;
            this.rbFatal.Location = new System.Drawing.Point(613, 47);
            this.rbFatal.Name = "rbFatal";
            this.rbFatal.Size = new System.Drawing.Size(53, 16);
            this.rbFatal.TabIndex = 5;
            this.rbFatal.TabStop = true;
            this.rbFatal.Text = "Fatal";
            this.rbFatal.UseVisualStyleBackColor = true;
            // 
            // rbError
            // 
            this.rbError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbError.AutoSize = true;
            this.rbError.Location = new System.Drawing.Point(554, 47);
            this.rbError.Name = "rbError";
            this.rbError.Size = new System.Drawing.Size(53, 16);
            this.rbError.TabIndex = 4;
            this.rbError.TabStop = true;
            this.rbError.Text = "Error";
            this.rbError.UseVisualStyleBackColor = true;
            // 
            // rbWarn
            // 
            this.rbWarn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbWarn.AutoSize = true;
            this.rbWarn.Location = new System.Drawing.Point(502, 47);
            this.rbWarn.Name = "rbWarn";
            this.rbWarn.Size = new System.Drawing.Size(47, 16);
            this.rbWarn.TabIndex = 3;
            this.rbWarn.TabStop = true;
            this.rbWarn.Text = "Warn";
            this.rbWarn.UseVisualStyleBackColor = true;
            // 
            // rbInfo
            // 
            this.rbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbInfo.AutoSize = true;
            this.rbInfo.Checked = true;
            this.rbInfo.Location = new System.Drawing.Point(449, 47);
            this.rbInfo.Name = "rbInfo";
            this.rbInfo.Size = new System.Drawing.Size(47, 16);
            this.rbInfo.TabIndex = 2;
            this.rbInfo.TabStop = true;
            this.rbInfo.Text = "Info";
            this.rbInfo.UseVisualStyleBackColor = true;
            // 
            // rbDebug
            // 
            this.rbDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbDebug.AutoSize = true;
            this.rbDebug.Location = new System.Drawing.Point(390, 47);
            this.rbDebug.Name = "rbDebug";
            this.rbDebug.Size = new System.Drawing.Size(53, 16);
            this.rbDebug.TabIndex = 1;
            this.rbDebug.TabStop = true;
            this.rbDebug.Text = "Debug";
            this.rbDebug.UseVisualStyleBackColor = true;
            // 
            // rbTrace
            // 
            this.rbTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbTrace.AutoSize = true;
            this.rbTrace.Location = new System.Drawing.Point(331, 47);
            this.rbTrace.Name = "rbTrace";
            this.rbTrace.Size = new System.Drawing.Size(53, 16);
            this.rbTrace.TabIndex = 0;
            this.rbTrace.TabStop = true;
            this.rbTrace.Text = "Trace";
            this.rbTrace.UseVisualStyleBackColor = true;
            // 
            // frmLogTrace_TraceWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 161);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.richTextBox1);
            this.MinimumSize = new System.Drawing.Size(800, 200);
            this.Name = "frmLogTrace_TraceWindow";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "日志跟踪";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmLogTrace_TraceWindow_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.SizeChanged += new System.EventHandler(this.frmLogTrace_TraceWindow_SizeChanged);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxLines)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbFatal;
        private System.Windows.Forms.RadioButton rbError;
        private System.Windows.Forms.RadioButton rbWarn;
        private System.Windows.Forms.RadioButton rbInfo;
        private System.Windows.Forms.RadioButton rbDebug;
        private System.Windows.Forms.RadioButton rbTrace;
        private System.Windows.Forms.CheckBox cbxAutoScroll;
        private System.Windows.Forms.TextBox tbxOutputFormat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudMaxLines;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox cbxStart;
        private System.Windows.Forms.ComboBox tbxLogNameMatch;
    }
}