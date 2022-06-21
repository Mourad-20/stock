using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface IDetailCommandeTask
    {
        ICollection<DetailCommandeDM> getDetailCommandeDMs([Optional]Int64? _idCommande, [Optional]Int64? _idCreePar);
        ICollection<DetailCommandeDM> getDetailCommandeDMsNonServi(Int64? _idCommande);
        ICollection<DetailCommandeDM> getDetailCommandesstockDMByIdArticle(Int64? _idArticle);
        
        void actualiserQuantiteServi(DetailCommandeDM _detailCommandeDM);
        void validateDetailCommandeDM(ICollection<DetailCommandeDM> _detailCommandeDMs);
        Int64? addDetailCommandeDM(DetailCommandeDM _detailCommandeDM);
        ICollection<DetailCommandeDM> getDetailCommandesDMByIdCommande(long? _idCommande);
        void removeDetailCommandeDM(Int64 _identifiant);
        void updateDetailCommandeDM(DetailCommandeDM _detailCommandeDM);
        ICollection<DetailCommandeDM> getDetailCommandesNonReglesDMByIdCommande(long? _idCommande);
    }
}