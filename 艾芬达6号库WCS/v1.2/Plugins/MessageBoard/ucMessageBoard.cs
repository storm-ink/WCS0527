using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wcs.App.Plugins.MessageBoard
{
    public partial class ucMessageBoard : UserControl
    {
        BindingSource bs;
        MessageBindingListView blv;
        public ucMessageBoard()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                cp.ExStyle |= 0x02000000;

                return cp;
            }

        }

        private void ucMessageBoard_Load(object sender, EventArgs e)
        {
            blv = new MessageBindingListView();

            blv.FilterHandler = filterHandler;
            blv.Size = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<Int32>("MessageBoard/显示条数", 10);

            foreach (ToolStripMenuItem item in toolStripMenuItem1.DropDownItems)
            {
                item.Checked = blv.Size.ToString() == item.Text;
            }

            bs = new BindingSource();
            bs.DataSource = blv;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = bs;

            消息ToolStripMenuItem.Checked = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("MessageBoard/消息", true);
            警告ToolStripMenuItem.Checked = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("MessageBoard/警告", true);
            错误ToolStripMenuItem.Checked = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("MessageBoard/错误", true);
            紧急ToolStripMenuItem.Checked = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("MessageBoard/紧急", true);
            tsmiAutoScroll.Checked = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("MessageBoard/自动滚动", true);

            for (int i = 0; i <= 10; i++)
            {
                var item = (ToolStripMenuItem)tsmiAutoMg.DropDownItems.Add(i.ToString() + "秒");
                if (i == 0)
                {
                    item.Text = "禁用";
                }
                item.Tag = i;
                item.Click += Item_Click;
                item.Checked = (i == Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<Int32>("MessageBoard/自动合并", 3));
            }

            setFilter();

            Wcs.Framework.MessageBoard.AbstractMessageBoard.Instance.Added += Instance_RemovedOrAdded;
            Wcs.Framework.MessageBoard.AbstractMessageBoard.Instance.Removed += Instance_RemovedOrAdded;
        }

        private void Item_Click(object sender, EventArgs e)
        {
            var mi = (ToolStripMenuItem)sender;
            var pm = (ToolStripMenuItem)mi.OwnerItem;
            foreach (ToolStripMenuItem item in pm.DropDownItems)
            {
                item.Checked = (mi == item);
            }

            var v = Convert.ToInt32(mi.Tag);

            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<Int32>("MessageBoard/自动合并", v);

        }

        void Instance_RemovedOrAdded(Wcs.Framework.MessageBoard.AbstractMessageBoard board, Wcs.Framework.MessageBoard.AbstractMessage message)
        {
            if (this.Visible == false || this.Parent.Visible == false)
            {
                return;
            }
            this.BeginInvoke(new MethodInvoker(() =>
            {
                lock (this)
                //Instance_RemovedOrAdded(board, message);
                {
                    var list = bs.DataSource as BindingList<Wcs.Framework.MessageBoard.AbstractMessage>;
                    var autoScroll = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("MessageBoard/自动滚动", true);
                    var index = list.ToList().FindIndex(x => x.Id == message.Id);
                    if (index < 0)
                    {
                        //自动合并
                        var mergeOffset = Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<Int32>("MessageBoard/自动合并", 3);
                        if (mergeOffset != 0)
                        {
                            var mergeItemIndex = list
                                .FindLastIndex(x => x.Title == message.Title
                              && x.Source == message.Source
                              && x.Level == message.Level
                              && message.OccurringTime.Subtract(x.OccurringTime).TotalSeconds <= mergeOffset);

                            if (mergeItemIndex < 0)
                            {
                                bs.Insert(0, message);

                                if (autoScroll)
                                {
                                    dataGridView1.FirstDisplayedScrollingRowIndex = 0;
                                }
                            }
                            else
                            {
                                bs.RemoveAt(mergeItemIndex);
                                bs.Insert(0, message);
                                //bs[mergeItemIndex] = message;
                                //bs.ResetItem(mergeItemIndex);
                                if (autoScroll)
                                {
                                    dataGridView1.FirstDisplayedScrollingRowIndex = 0;
                                    //dataGridView1.FirstDisplayedScrollingRowIndex = mergeItemIndex;
                                }
                            }
                        }
                        else
                        {
                            bs.Insert(0, message);
                            if (autoScroll)
                            {
                                dataGridView1.FirstDisplayedScrollingRowIndex = 0;
                            }
                        }
                    }
                    else
                    {
                        bs[index] = message;
                        bs.ResetItem(index);
                        if (autoScroll)
                        {
                            dataGridView1.FirstDisplayedScrollingRowIndex = index;
                        }
                    }
                }
            }));
        }


        void filterHandler(object Sender, ref bool Include)
        {
            var item = (Wcs.Framework.MessageBoard.AbstractMessage)Sender;
            if (!Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("MessageBoard/消息"))
            {
                if (item.Level == Wcs.Framework.MessageBoard.MessageLevel.Info)
                {
                    Include = false;
                    return;
                }
            }
            if (!Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("MessageBoard/警告"))
            {
                if (item.Level == Wcs.Framework.MessageBoard.MessageLevel.Warning)
                {
                    Include = false;
                    return;
                }
            }
            if (!Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("MessageBoard/错误"))
            {
                if (item.Level == Wcs.Framework.MessageBoard.MessageLevel.Error)
                {
                    Include = false;
                    return;
                }
            }
            if (!Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.GetSetting<bool>("MessageBoard/紧急"))
            {
                if (item.Level == Wcs.Framework.MessageBoard.MessageLevel.Emergency)
                {
                    Include = false;
                    return;
                }
            }

            Include = true;

        }

        private void 隐藏窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Parent.Visible = false;
        }

        private void 消息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var it = (System.Windows.Forms.ToolStripMenuItem)sender;

            var v = !it.Checked;
            it.Checked = v;
            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<bool>(string.Format("MessageBoard/{0}", it.Text), v);

            setFilter();

            #region 保持菜单项显示状态
            var ts = (ToolStripMenuItem)sender;
            List<ToolStripMenuItem> stack = new List<ToolStripMenuItem>();
            stack.Add(ts);
            while (ts.OwnerItem != null && ts.OwnerItem is ToolStripMenuItem)
            {
                ts = (ToolStripMenuItem)ts.OwnerItem;
                stack.Add(ts);
            }
            stack.Reverse();

            foreach (ToolStripMenuItem item in stack)
            {
                item.ShowDropDown();
            }
            #endregion
        }

        private void setFilter()
        {
            //var ts = (ToolStripMenuItem)消息ToolStripMenuItem.OwnerItem;
            //string filter = "";
            //foreach (ToolStripMenuItem item in ts.DropDownItems)
            //{
            //    if (item.Checked)
            //    {
            //        var v = (Wcs.Framework.MessageBoard.MessageLevel)Enum.Parse(typeof(Wcs.Framework.MessageBoard.MessageLevel), Convert.ToString(item.Tag));

            //        filter +=string.Format( "or Level={0} ",(int)v);
            //    }
            //}

            //if (!String.IsNullOrWhiteSpace(filter))
            //{
            //    filter = filter.Substring(2);
            //}

            //bs.Filter = filter;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }

                if (e.ColumnIndex == 0)
                {
                    if (dataGridView1.Rows.Count > e.RowIndex)
                    {
                        var d = (Wcs.Framework.MessageBoard.AbstractMessage)dataGridView1.Rows[e.RowIndex].DataBoundItem;
                        e.Value = imageList1.Images[d.Level.ToString()];
                    }
                }
            }
            catch
            {
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            var ts = (ToolStripMenuItem)sender;
            foreach (ToolStripMenuItem item in ts.DropDownItems)
            {
                if (item != ts)
                {
                    item.Checked = false;
                }
                else
                {
                    item.Checked = true;
                }
            }

            var rowsMax = Convert.ToInt32(ts.Text);
            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<Int32>("MessageBoard/显示条数", rowsMax);
            blv.Size = rowsMax;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex < 0)
            {
                return;
            }

            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            var msg = (Wcs.Framework.MessageBoard.AbstractMessage)dataGridView1.CurrentRow.DataBoundItem;
            using (frmMessage frm = new frmMessage(msg))
            {
                frm.ShowDialog();
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            blv.Clear();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, new SolidBrush(Color.Black), e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + 5);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("添加行号时发生错误，错误信息：" + ex.Message, "操作失败");
            }
        }

        private void tsmiAutoScroll_Click(object sender, EventArgs e)
        {
            var it = (System.Windows.Forms.ToolStripMenuItem)sender;

            var v = !it.Checked;
            it.Checked = v;
            Wcs.Framework.Cfg.WcsConfiguration.Instance.SettingCollection.SetSetting<bool>("MessageBoard/自动滚动", v);
        }
    }
}
