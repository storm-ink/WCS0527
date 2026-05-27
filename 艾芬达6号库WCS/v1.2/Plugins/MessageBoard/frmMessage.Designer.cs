namespace Wcs.App.Plugins.MessageBoard
{
    partial class frmMessage
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
            Wcs.App.Plugins.MessageBoard.XMLViewerSettings xmlViewerSettings2 = new Wcs.App.Plugins.MessageBoard.XMLViewerSettings();
            this.button2 = new System.Windows.Forms.Button();
            this.xmlViewer1 = new Wcs.App.Plugins.MessageBoard.XMLViewer();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(541, 454);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 25);
            this.button2.TabIndex = 4;
            this.button2.Text = "返回(&ESC)";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // xmlViewer1
            // 
            this.xmlViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xmlViewer1.Location = new System.Drawing.Point(7, 8);
            this.xmlViewer1.Name = "xmlViewer1";
            this.xmlViewer1.ReadOnly = true;
            xmlViewerSettings2.AttributeKey = System.Drawing.Color.Red;
            xmlViewerSettings2.AttributeValue = System.Drawing.Color.Blue;
            xmlViewerSettings2.Element = System.Drawing.Color.DarkRed;
            xmlViewerSettings2.Tag = System.Drawing.Color.Blue;
            xmlViewerSettings2.Value = System.Drawing.Color.Black;
            this.xmlViewer1.Settings = xmlViewerSettings2;
            this.xmlViewer1.Size = new System.Drawing.Size(609, 439);
            this.xmlViewer1.TabIndex = 0;
            this.xmlViewer1.Text = "";
            // 
            // frmMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(622, 487);
            this.ControlBox = false;
            this.Controls.Add(this.button2);
            this.Controls.Add(this.xmlViewer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查看消息源格式";
            this.ResumeLayout(false);

        }

        #endregion

        private XMLViewer xmlViewer1;
        private System.Windows.Forms.Button button2;
    }
}