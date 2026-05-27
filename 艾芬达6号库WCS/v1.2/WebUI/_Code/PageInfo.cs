using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;


    /// <summary>
    /// 提供数据应如何分页的信息。页索引基于 1。
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        /// 基于 1 的页索引。
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小。
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 一共有多少项。
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// 起始项的索引，基于 0。
        /// </summary>
        public int StartItemIndex
        {
            get
            {
                return (PageIndex-1) * PageSize;
            }
        }
        /// <summary>
        /// 应在设置 TotalItemCount 之后调用，PageIndex 属性会调整为 1..max 之间的数值
        /// </summary>
        public void RoundPageIndex()
        {
            if (this.PageIndex < 1)
            {
                this.PageIndex = 1;
            }
            this.PageIndex = Math.Min(this.PageIndex, GetMaxPageIndex());
        }

        private int GetMaxPageIndex()
        {
            int i = (TotalItemCount % PageSize) == 0 ? TotalItemCount / PageSize : TotalItemCount / PageSize + 1;
            return Math.Max(1, i);
        }
    }


    /// <summary>
    /// 列表页分页的 helper。
    /// </summary>
    public static class PageHelper
    {
        public const string PageIndexParamName = "pageindex";
        public const string PageSizeParamName = "pagesize";
        private const string DefaultPageSizeAppSettingKey = "default-page-size";
        private const int DefaultPageSize = 10;

        private static int GetPageIndex(this HttpRequestBase request)
        {
            string str = request[PageIndexParamName];
            int result = 1;
            if (int.TryParse(str, out result) && result >= 1)
            {
                return result;
            }
            return 1;
        }

        private static int GetPageSize(this HttpRequestBase request)
        {
            string str = request[PageSizeParamName];
            int result = 0;
            if (int.TryParse(str, out result) && result > 0)
            {
                return result;
            }

            return GetDefaultPageSize();
        }

        public static PageInfo GetPageInfo(this HttpRequestBase request)
        {
            PageInfo pageInfo = new PageInfo();

            pageInfo.PageIndex = GetPageIndex(request);
            pageInfo.PageSize = GetPageSize(request);

            return pageInfo;

        }
        public static int GetDefaultPageSize()
        {
            string str = WebConfigurationManager.AppSettings[DefaultPageSizeAppSettingKey];
            int result = 0;
            if (int.TryParse(str, out result) && result > 0)
            {
                return result;
            }

            return DefaultPageSize;
        }

    }

    public class ReturnInfo
    {
        private string _url;
        HttpSessionStateBase _session;

        private string IsActiveKey
        {
            get
            {
                return string.Format("return-is-active:{0}", _url);
            }
        }

        private string PageIndexKey
        {
            get
            {
                return string.Format("return-page-index:{0}", _url);
            }
        }

        private string ConditionsKey
        {
            get
            {
                return string.Format("return-conditions:{0}", _url);
            }

        }

        public ReturnInfo(HttpSessionStateBase session, string url)
        {
            _session = session;
            _url = url;
        }

        public bool IsActive()
        {
            return _session[IsActiveKey] != null;
        }

        /// <summary>
        /// 返回此实例中保存的查询条件，不检查此实例是否处于活动状态，如果没有查询条件，则返回 null。
        /// </summary>
        /// <returns></returns>
        public object GetConditions()
        {
            return _session[ConditionsKey];

        }
        public int GetPageIndex()
        {
            // UI 上，分页索引基于 1
            object obj = _session[PageIndexKey];
            if (obj != null)
            {
                return (int)obj + 1;
            }
            return 1;
        }

        /// <summary>
        /// 从 session 中删除此实例
        /// </summary>
        public void Delete()
        {
            _session.Remove(IsActiveKey);
            _session.Remove(ConditionsKey);
            _session.Remove(PageIndexKey);
        }

        /// <summary>
        /// 将此实例创建到 session，但 IsActive 为 false
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="conditions"></param>
        public void Create(int pageIndex, object conditions)
        {
            _session.Remove(IsActiveKey);
            _session.Add(ConditionsKey, conditions);
            _session.Add(PageIndexKey, pageIndex);
        }

        /// <summary>
        /// 将此实例的 IsActive 设置为 true
        /// </summary>
        public void SetActive()
        {
            _session.Add(IsActiveKey, new object());
        }


    }

