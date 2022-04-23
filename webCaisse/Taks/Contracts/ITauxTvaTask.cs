using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.Db.Entities;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface ITauxTvaTask
    {
        // ICollection<TypeUniteDM> getGroupesOfUtilisateur(Int64 _idUtilisateur);
        TauxTva getTypeUniteByTaux(double _taux);
    }
}