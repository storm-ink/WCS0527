using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wcs.App.Plugins.ArchivedPreTaskManager
{
    /// <summary>
    /// 声明委托
    /// </summary>
    /// <param name="e"></param>

    public delegate void EventPagingHandler(EventArgs e);

    public partial class Paging : UserControl

    {
        public Paging()
        {
            InitializeComponent();
            this.lblCurrentPage.LostFocus += LblCurrentPage_LostFocus;
            comboPageSize.LostFocus += ComboPageSize_LostFocus;
        }

        public event EventPagingHandler EventPaging;

        #region 公开属性
        private int _pageSize = 25;
        /// <summary>
        /// 每页显示记录数(默认25)
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value > 0)
                {
                    _pageSize = value;
                    this.comboPageSize.Text = _pageSize.ToString();
                }
                else if (value == -1)
                {
                    _pageSize = -1;
                }
                else
                {
                    _pageSize = 25;
                    this.comboPageSize.Text = _pageSize.ToString();
                }
            }
        }

        private int _currentPage = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                if (value > 0)
                {
                    _currentPage = value;
                }
                else
                {
                    _currentPage = 1;
                }
                this.lblCurrentPage.Text = _currentPage + "";
            }
        }
        private int _totalCount = 0;
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount
        {
            get
            {
                return _totalCount;
            }
            set
            {
                if (value >= 0)
                    _totalCount = value;
                else
                    _totalCount = 0;
                this.lblTotalCount.Text = _totalCount.ToString();
            }
        }
        private int _queryCount = 0;
        /// <summary>
        /// 查询记录数
        /// </summary>
        public int QueryCount
        {
            get
            {
                return _queryCount;
            }
            set
            {
                if (value >= 0)
                    _queryCount = value;
                else
                    _queryCount = 0;
                this.lblQueryCount.Text = _queryCount.ToString();
                CalculatePageCount();
            }
        }
        private int _pageCount = 0;
        /// <summary>
        /// 页数
        /// </summary>
        public int PageCount
        {
            get
            {
                return _pageCount;
            }
            set
            {
                if (value >= 0)
                {
                    _pageCount = value;
                }
                else
                {
                    _pageCount = 1;
                }
                this.lblPageCount.Text = _pageCount.ToString();
                if (this.lblCurrentPage != null && this.lblCurrentPage.Text != null && int.TryParse(this.lblCurrentPage.Text, out int currentPage) && currentPage > _pageCount)
                    this.lblCurrentPage.Text = "1";
            }
        }
        #endregion
        /// <summary>
        /// 计算页数
        /// </summary>
        private void CalculatePageCount()
        {
            if (this.QueryCount > 0)
            {
                this.PageCount = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(this.QueryCount) / Convert.ToDouble(this.PageSize)));
            }
            else
            {
                this.PageCount = 1;
            }
        }

        /// <summary>
        /// 数据绑定
        /// </summary>
        public void Bind()
        {
            if (this.EventPaging != null)//当事件不为空时，进行数据绑定
            {
                this.EventPaging(new EventArgs());
            }
            if (this.CurrentPage > this.PageCount)
            {
                this.CurrentPage = this.PageCount;
            }
            this.lblPageCount.Text = this.PageCount + "";
            this.lblQueryCount.Text = this.QueryCount + "";
            if (this.CurrentPage == 1)
            {
                this.labFirst.Enabled = false;
                this.labPrev.Enabled = false;
            }
            else
            {
                this.labFirst.Enabled = true;
                this.labPrev.Enabled = true;

            }
            if (this.CurrentPage == this.PageCount)
            {
                this.labNext.Enabled = false;
                this.labLast.Enabled = false;
            }
            else
            {
                this.labNext.Enabled = true;
                this.labLast.Enabled = true;
            }
            if (this.QueryCount == 0)
            {
                this.labFirst.Enabled = false;
                this.labPrev.Enabled = false;
                this.labNext.Enabled = false;
                this.labLast.Enabled = false;
            }
        }

        /// <summary>
        ///  改变每页条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboPageSize.Text.Trim() == "全部")
            {
                this.PageSize = -1;
            }
            else
            {
                this.PageSize = Convert.ToInt32(comboPageSize.Text);
            }
            this.Bind();
        }

        private void labFirst_Click(object sender, EventArgs e)
        {
            this.CurrentPage = 1;
            this.Bind();
        }

        private void labPrev_Click(object sender, EventArgs e)
        {
            this.CurrentPage -= 1;
            this.Bind();
        }

        private void labNext_Click(object sender, EventArgs e)
        {
            this.CurrentPage += 1;
            this.Bind();
        }

        private void labLast_Click(object sender, EventArgs e)
        {
            this.CurrentPage = this.PageCount;
            this.Bind();
        }

        private void lblCurrentPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var str = lblCurrentPage.Text;
                if (int.TryParse(str, out int currentPage) && this.CurrentPage != currentPage && this.PageCount >= currentPage)
                {
                    this.CurrentPage = currentPage;
                    this.Bind();
                }
                else
                    lblCurrentPage.Text = this.CurrentPage + "";
            }
        }
        private void LblCurrentPage_LostFocus(object sender, EventArgs e)
        {
            var str = lblCurrentPage.Text;
            if (int.TryParse(str, out int currentPage) && this.CurrentPage != currentPage)
            {
                this.CurrentPage = currentPage;
                this.Bind();
            }
            else
                lblCurrentPage.Text = this.CurrentPage + "";
        }

        private void comboPageSize_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var str = comboPageSize.Text;
            if (str == "全部")
            {
                if (this.PageSize == -1)
                    return;
                else
                {
                    this.PageSize = -1;
                    this.Bind();
                }
            }
            else
            {
                if (int.TryParse(str, out int pageSize))
                {
                    if (this.PageSize == pageSize)
                        return;
                    else
                    {
                        this.PageSize = pageSize;
                        this.Bind();
                    }
                }
            }
        }

        private void ComboPageSize_LostFocus(object sender, EventArgs e)
        {
            var str = comboPageSize.Text;
            if (str == "全部")
            {
                if (this.PageSize == -1)
                    return;
                else
                {
                    this.PageSize = -1;
                    this.Bind();
                }
            }
            else
            {
                if (int.TryParse(str, out int pageSize))
                {
                    if (this.PageSize == pageSize)
                        return;
                    else
                    {
                        this.PageSize = pageSize;
                        this.Bind();
                    }
                }
                else
                    this.comboPageSize.Text = this.PageSize + "";
            }
        }

        private void comboPageSize_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var str = comboPageSize.Text;
                if (str == "全部")
                {
                    if (this.PageSize == -1)
                        return;
                    else
                    {
                        this.PageSize = -1;
                        this.Bind();
                    }
                }
                else
                {
                    if (int.TryParse(str, out int pageSize))
                    {
                        if (this.PageSize == pageSize)
                            return;
                        else
                        {
                            this.PageSize = pageSize;
                            this.Bind();
                        }
                    }
                    else
                        this.comboPageSize.Text = this.PageSize + "";
                }
            }
        }
    }
}