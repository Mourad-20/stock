using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface IZoneTask
    {
        ICollection<ZoneDM> getZoneDMs([Optional] Int64? _enActivite);
    }
}