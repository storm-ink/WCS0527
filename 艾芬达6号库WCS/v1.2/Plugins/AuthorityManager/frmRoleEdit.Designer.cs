namespace Wcs.App.Plugins.AuthorityManager
{
    partial class frmRoleEdit
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("节点7");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("节点8");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("节点9");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("节点10");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("节点11");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("节点6", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("节点0", new System.Windows.Forms.TreeNode[] {
            treeNode6});
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("节点12");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("节点13");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("节点14");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("节点15");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("节点1", new System.Windows.Forms.TreeNode[] {
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("节点16");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("节点18");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("节点19");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("节点20");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("节点17", new System.Windows.Forms.TreeNode[] {
            treeNode14,
            treeNode15,
            treeNode16});
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("节点2", new System.Windows.Forms.TreeNode[] {
            treeNode13,
            treeNode17});
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("节点3");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("节点4");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("节点5");
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxName = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.tbxComment = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.tbxName);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.treeView1);
            this.panel1.Controls.Add(this.tbxComment);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(589, 577);
            this.panel1.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(101, 264);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(378, 27);
            this.label3.TabIndex = 10;
            this.label3.Text = "正在加载角色信息，请稍候...";
            // 
            // tbxName
            // 
            this.tbxName.Location = new System.Drawing.Point(71, 16);
            this.tbxName.Name = "tbxName";
            this.tbxName.Size = new System.Drawing.Size(149, 21);
            this.tbxName.TabIndex = 4;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(502, 14);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView1.CheckBoxes = true;
            this.treeView1.FullRowSelect = true;
            this.treeView1.ItemHeight = 22;
            this.treeView1.Location = new System.Drawing.Point(12, 47);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "节点7";
            treeNode1.Text = "节点7";
            treeNode2.Name = "节点8";
            treeNode2.Text = "节点8";
            treeNode3.Name = "节点9";
            treeNode3.Text = "节点9";
            treeNode4.Name = "节点10";
            treeNode4.Text = "节点10";
            treeNode5.Name = "节点11";
            treeNode5.Text = "节点11";
            treeNode6.Name = "节点6";
            treeNode6.Text = "节点6";
            treeNode7.Name = "节点0";
            treeNode7.Text = "节点0";
            treeNode8.Name = "节点12";
            treeNode8.Text = "节点12";
            treeNode9.Name = "节点13";
            treeNode9.Text = "节点13";
            treeNode10.Name = "节点14";
            treeNode10.Text = "节点14";
            treeNode11.Name = "节点15";
            treeNode11.Text = "节点15";
            treeNode12.Name = "节点1";
            treeNode12.Text = "节点1";
            treeNode13.Name = "节点16";
            treeNode13.Text = "节点16";
            treeNode14.Name = "节点18";
            treeNode14.Text = "节点18";
            treeNode15.Name = "节点19";
            treeNode15.Text = "节点19";
            treeNode16.Name = "节点20";
            treeNode16.Text = "节点20";
            treeNode17.Name = "节点17";
            treeNode17.Text = "节点17";
            treeNode18.Name = "节点2";
            treeNode18.Text = "节点2";
            treeNode19.Name = "节点3";
            treeNode19.Text = "节点3";
            treeNode20.Name = "节点4";
            treeNode20.Text = "节点4";
            treeNode21.Name = "节点5";
            treeNode21.Text = "节点5";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode12,
            treeNode18,
            treeNode19,
            treeNode20,
            treeNode21});
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(565, 516);
            this.treeView1.TabIndex = 9;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // tbxComment
            // 
            this.tbxComment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbxComment.Location = new System.Drawing.Point(296, 16);
            this.tbxComment.Name = "tbxComment";
            this.tbxComment.Size = new System.Drawing.Size(183, 21);
            this.tbxComment.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(261, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "备注";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "角色名称";
            // 
            // frmRoleEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 577);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRoleEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加/编辑角色";
            this.Load += new System.EventHandler(this.frmRoleEdit_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox tbxName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TextBox tbxComment;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;

    }
}