using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoForksRackLocations.asdf
{
    public interface IRackLocationBuilder
    {
        String Create(CreateRackLocationSettings settings);
    }
}
