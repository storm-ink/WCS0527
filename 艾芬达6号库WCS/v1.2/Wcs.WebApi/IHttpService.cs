using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wcs.WebApi
{
    public interface IHttpService
    {
        IHttpService Initialization(int port);
        Task StartHttpServer();
        Task CloseHttpServer();
        void Dispose();
    }
}
