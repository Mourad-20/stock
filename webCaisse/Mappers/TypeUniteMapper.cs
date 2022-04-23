using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class TypeUniteMapper
    {
        public static TypeUniteVM TypeUniteDMtoTypeUniteVM(TypeUniteDM _src)
        {
            TypeUniteVM _dest = null;
            if (_src != null)
            {
                _dest = new TypeUniteVM()
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