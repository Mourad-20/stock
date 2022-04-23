using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using webCaisse.Reports.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface IRapportTask
    {
        Byte[] genererTicketPreparation(ICollection<RptDetailCommande> _rptDetailCommandes, String _numeroCommande, String _dateGeneration
            , String _nomServeur, String _libelleLocalite, String _libelleZone);
        Byte[] genererTicketNote(ICollection<RptDetailCommande> _rptDetailCommandes, String _numeroCommande, String _dateGeneration
            , String _nomServeur, String _libelleLocalite,String _total);
    }
}