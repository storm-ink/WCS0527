using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;

namespace Client.Conveyor
{
    public class AppearanceInspectionRepository : Spiral.NhRepository<AppearanceInspection,Int32>
    {
        public static List<String> _allTypes = new List<string>();
        static AppearanceInspectionRepository()
        {
            _allTypes.Add("左超差");
            
            _allTypes.Add("右超差");
            
            _allTypes.Add("前超差");

            _allTypes.Add("后超差");
            
            _allTypes.Add("超高");
        }

        public static IQueryable<AppearanceInspection> ApplyCountWhere(IQueryable<AppearanceInspection> q,String type)
        {
            switch (type)
            {
                case "左超差":
                    q = q.Where(x => x.Left_Over == true);
                    break;
                case "右超差":
                    q = q.Where(x => x.Right_Over == true);
                    break;
                case "前超差":
                    q = q.Where(x => x.Front_Over == true);
                    break;
                case "后超差":
                    q = q.Where(x => x.Back_Over == true);
                    break;
                case "超高":
                    q = q.Where(x => x.Too_High == true);
                    break;
            }

            return q;
        }

        public AppearanceInspectionRepository(Spiral.NhRepositoryContext context)
            : base(context)
        {
           
        }
    }
}
