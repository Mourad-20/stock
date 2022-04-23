using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using System.Runtime.InteropServices;


namespace webCaisse.Taks.Contracts
{
    public interface ISeanceTask
    {
        Int64? ouvrirSeance(SeanceDM _seanceDM);
        void fermerSeance(SeanceDM _seanceDM);
        SeanceDM getSeanceActive(Int64 _idUtilisateur);
        SeanceDM getSeanceCaisse(Int64 _idCaisse);
        SeanceDM getSeancebyId(Int64 _idSeance);
        ICollection<SeanceDM> getSeanceDMs([Optional]DateTime? _ddebut, [Optional]DateTime? _dfin);

        ICollection<SeanceDM> getSeancesActives();
    }
}