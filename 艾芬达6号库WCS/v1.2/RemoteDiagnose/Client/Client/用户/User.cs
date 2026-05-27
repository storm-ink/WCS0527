using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spiral;
using NLog;
using Spiral.Base;

namespace Client
{
    /// <summary>
    /// 用户。
    /// </summary>
    public class User : UserBase
    {
        //public ISet<Project> Projects { get; set; }
        public User():base()
        {
            //Projects = new HashSet<Project>();
        }
    }
}
