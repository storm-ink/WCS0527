using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI
{
    public static class TypeHelper
    {
        //public static bool IsToManay(Type type)
        //{
        //    if (type.IsGenericType)
        //    {
        //        var def = type.GetGenericTypeDefinition();
        //        if (def == typeof(Iesi.Collections.Generic.ISet<>) || type == typeof(IList<>))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        //public static bool IsToOne(Object obj)
        //{
        //    var session = (NHibernate.ISession)HttpContext.Current.Items["nhsession"];
        //    return session.Contains(obj);
        //}

        //public static string GetEntityName(Object obj)
        //{
        //    var session = (NHibernate.ISession)HttpContext.Current.Items["nhsession"];
        //    return session.GetEntityName(obj);
        //}


    }


    [Serializable]
    public class UpdateModelException : Exception
    {
        public UpdateModelException() { }
        public UpdateModelException(string message) : base(message) { }
        public UpdateModelException(string message, Exception inner) : base(message, inner) { }
        protected UpdateModelException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}