using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class GroupeMapper
    {
        public static GroupeVM GroupeDMtoGroupeVM(GroupeDM _src)
        {
            GroupeVM _dest = null;
            if (_src != null)
            {
                _dest = new GroupeVM()
                {
                    Identifiant = _src.Identifiant,
                    Code = _src.Code,
                    Libelle = _src.Libelle,
                };
            }
            return _dest;
        }
    }
}