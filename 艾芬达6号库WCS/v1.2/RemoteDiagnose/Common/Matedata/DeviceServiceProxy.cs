using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Matedata
{
    public abstract class DeviceServiceProxy<TChannel, TAssemblyAnyType> : DeviceService, IMatedataServiceProxy
    {
        protected ChannelFactory<TChannel> CreateChannelFactory()
        {
            ChannelFactory<TChannel> factory = new MatedataChannelFactory<TChannel, TAssemblyAnyType>(typeof(TChannel).Name);

            return factory;
        }

    }
}
