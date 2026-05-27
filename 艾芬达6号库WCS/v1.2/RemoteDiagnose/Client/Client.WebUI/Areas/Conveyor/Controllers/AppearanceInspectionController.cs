using Spiral;
using Spiral.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NHibernate.Linq;

namespace Client.WebUI.Areas.Conveyor.Controllers
{
    public class AppearanceInspectionController : Controller
    {
        [WmsOperation("输送线.外形检测")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index_List(Client.Conveyor.AppearanceInspectionSpec spec, FormCollection form)
        {
            PageInfo pageInfo = Request.GetPageInfo();

            try
            {
                using (NhRepositoryContext context = NhRepositoryContextFactory.Instance.CreateRepositoryContext())
                {
                    using (NhUnitOfWork unitOfWork = context.BeginUnitOfWork())
                    {
                        int totalItemCount;
                        var factory = new NhRepositoryFactory(context);
                        var repository = factory.Create<Client.Conveyor.ConveyorRepository<Client.Conveyor.AppearanceInspection>>();
                        var list = repository.SelectSatisfying(spec, null, pageInfo.StartItemIndex, pageInfo.PageSize, out totalItemCount);
                        pageInfo.TotalItemCount = totalItemCount;
                        pageInfo.RoundPageIndex();

                        ViewData.Model = new PagedList.StaticPagedList<Client.Conveyor.AppearanceInspection>(list, pageInfo.PageIndex, pageInfo.PageSize, pageInfo.TotalItemCount);

                        unitOfWork.Commit();
                    }
                }

                ReturnInfo returnInfo = new ReturnInfo(Session, this.GetType().ToString());
                returnInfo.Create(pageInfo.PageIndex - 1, pageInfo.PageSize, spec);

                Response.Cache.SetCacheability(HttpCacheability.NoCache);

                return PartialView();


            }
            catch (Exception ex)
            {
                return Content("错误：" + ex.ToString());
            }
        }


        public ActionResult Report()
        {
            return View();
        }

        [OpenSessionInViewFilter]
        [HttpPost]
        public JsonResult CountByType(Int32? no, DateTime? startTime, DateTime? endTime)
        {
            NhRepositoryContext context = this.HttpContext.GetNhRepositoryContext();

            List<object> list = new List<object>();
            foreach (var typeName in Client.Conveyor.AppearanceInspectionRepository._allTypes)
            {
                var q = context.Session.Query<Client.Conveyor.AppearanceInspection>();

                q = q.Where(x => x.Back_Over || x.Front_Over || x.Left_Over || x.Right_Over || x.Too_High);

                if (no != null)
                {
                    q = q.Where(x => x.No == no);
                }
                if (startTime != null)
                {
                    q = q.Where(x => x.LongDate >= startTime);
                }

                if (endTime != null)
                {
                    q = q.Where(x => x.LongDate < endTime);
                }

                q = Client.Conveyor.AppearanceInspectionRepository.ApplyCountWhere(q, typeName);

                var count = q.Count();
                if (count == 0)
                {
                    continue;
                }

                list.Add(new
                {
                    name = typeName,
                    value = count,
                    color = App.Helper.GetRandomColor(),
                    totals = count
                });
            }

            return Json(list);
        }

        [OpenSessionInViewFilter]
        [HttpPost]
        public JsonResult CountByNoWithType(Int32? no,DateTime? startTime, DateTime? endTime,String type)
        {
            NhRepositoryContext context = this.HttpContext.GetNhRepositoryContext();

            List<object> list = new List<object>();
            var q = context.Session.Query<Client.Conveyor.AppearanceInspection>();
            q = q.Where(x => x.Back_Over || x.Front_Over || x.Left_Over || x.Right_Over || x.Too_High);
            if (no != null)
            {
                q = q.Where(x => x.No == no);
            }
            if (startTime != null)
            {
                q = q.Where(x => x.LongDate >= startTime);
            }

            if (endTime != null)
            {
                q = q.Where(x => x.LongDate < endTime);
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                q = Client.Conveyor.AppearanceInspectionRepository.ApplyCountWhere(q, type);
            }

            foreach (var item in q.GroupBy(x=>x.No).Select(x=>x.Key))
            {
                int result = 0;
                foreach (var typeName in Client.Conveyor.AppearanceInspectionRepository._allTypes)
                {
                    if (!string.IsNullOrWhiteSpace(type) && typeName != type)
                    {
                        continue;
                    }

                    var cq = context.Session.Query<Client.Conveyor.AppearanceInspection>();

                    cq = cq.Where(x => x.Back_Over || x.Front_Over || x.Left_Over || x.Right_Over || x.Too_High);

                    if (startTime != null)
                    {
                        cq = q.Where(x => x.LongDate >= startTime);
                    }

                    if (endTime != null)
                    {
                        cq = cq.Where(x => x.LongDate < endTime);
                    }

                    cq = cq.Where(x => x.No == item);

                    cq = Client.Conveyor.AppearanceInspectionRepository.ApplyCountWhere(cq, typeName);

                    var count = cq.Count();

                    result += count;
                }

                if (result == 0)
                {
                    continue;
                }

                list.Add(new
                {
                    name = item.ToString(),
                    value = result,
                    color = App.Helper.GetRandomColor(),
                    totals = result
                });
            }


            return Json(list);
        }
    }
}
