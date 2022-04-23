using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface ITypeUniteTask
    {
       // ICollection<TypeUniteDM> getGroupesOfUtilisateur(Int64 _idUtilisateur);
        ICollection<TypeUniteDM> getTypeUniteDMs();
        TypeUniteDM getTypeUniteDMByCode(String _code);
    }
}