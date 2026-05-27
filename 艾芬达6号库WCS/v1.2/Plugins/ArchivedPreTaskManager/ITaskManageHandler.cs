using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wcs.Framework;

namespace Wcs.App.Plugins.ArchivedPreTaskManager
{
    public interface ITaskManageHandler
    {
        void BeforeCancel(Task task);
        void AfterCancel(Task task);

        void BeforeResume(Task task);
        void AfterResume(Task task);
    }
}
