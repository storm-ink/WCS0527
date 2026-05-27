using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace WebUI
{
    //public class OpenSessionInViewFilterAttribute : ActionFilterAttribute, IExceptionFilter
    //{
    //    private bool _manuallyOpen = false;
    //    public OpenSessionInViewFilterAttribute():this(false)
    //    {
    //    }
    //    public OpenSessionInViewFilterAttribute(bool manuallyOpen)
    //    {
    //        _manuallyOpen = manuallyOpen;
    //    }
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        HttpContext.Current.Items["OpenSessionInViewFilterAttribute"] = this;
    //        if (!_manuallyOpen)
    //        {
    //            this.Open(filterContext.HttpContext);
    //        }
    //    }

    //    public void Open(HttpContextBase httpContext)
    //    {
    //        INHUnitOfWork unitOfWork = new NHUnitOfWork();
    //        INHUnitOfWork backupServerUnitOfWork = new NHBackupServerUnitOfWork();
    //        httpContext.Items["unitOfWork"] = unitOfWork;
    //        httpContext.Items["bakUnitOfWork"] = backupServerUnitOfWork;

    //    }

    //    public override void OnResultExecuted(ResultExecutedContext filterContext)
    //    {
    //        Debug.WriteLine("OnResultExecuted");
    //        INHUnitOfWork unitOfWork = (INHUnitOfWork)filterContext.HttpContext.Items["unitOfWork"];
    //        if (unitOfWork != null)
    //        {


    //            if (filterContext.Exception == null)
    //            {
    //                unitOfWork.Commit();
    //            }
    //            else
    //            {
    //                unitOfWork.Rollback();
    //            }
    //            unitOfWork.Dispose();
    //            filterContext.HttpContext.Items["unitOfWork"] = null;
    //        }

    //        unitOfWork = (INHUnitOfWork)filterContext.HttpContext.Items["bakUnitOfWork"];
    //        if (unitOfWork != null)
    //        {


    //            if (filterContext.Exception == null)
    //            {
    //                unitOfWork.Commit();
    //            }
    //            else
    //            {
    //                unitOfWork.Rollback();
    //            }
    //            unitOfWork.Dispose();
    //            filterContext.HttpContext.Items["bakUnitOfWork"] = null;
    //        }
    //    }


    //    public void OnException(ExceptionContext filterContext)
    //    {
    //        Debug.WriteLine("OnException");
    //        INHUnitOfWork unitOfWork = (INHUnitOfWork)filterContext.HttpContext.Items["unitOfWork"];
    //        if (unitOfWork != null)
    //        {
    //            unitOfWork.Rollback();
    //            unitOfWork.Dispose();
    //            filterContext.HttpContext.Items["unitOfWork"] = null;
    //        }

    //        unitOfWork = (INHUnitOfWork)filterContext.HttpContext.Items["bakUnitOfWork"];
    //        if (unitOfWork != null)
    //        {
    //            unitOfWork.Rollback();
    //            unitOfWork.Dispose();
    //            filterContext.HttpContext.Items["bakUnitOfWork"] = null;
    //        }

    //        LogManager.GetLogger("OpenSessionInView").ErrorException("错误", filterContext.Exception);
    //    }

    //    public static OpenSessionInViewFilterAttribute Current
    //    {
    //        get
    //        {
    //            return (OpenSessionInViewFilterAttribute)HttpContext.Current.Items["OpenSessionInViewFilterAttribute"];
    //        }
    //    }
    //}
}