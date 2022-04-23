using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface ICommandeTask
    {
        ICollection<CommandeDM> getCommandeDMs([Optional]String _code, [Optional]String _numero, [Optional] Int64? _idEtatCommande, [Optional] Int64? _idCreePar,
            [Optional] Int64? _idSeance, [Optional] Int64? _idServeur, [Optional]DateTime? _ddebut, [Optional]DateTime? _dfin, [Optional]List<Int64?> _caisseids);

        CommandeDM getCommandeDMById(Int64? _identifiant);
        
       Int64? allimentationstock(CommandeDM _commandeDM);
        Int64? etablirCommande(CommandeDM _commandeDM);
        String genererNumeroCommande();
        Int64? validerCommande(CommandeDM _commandeDM);
        Int64? modifierCommande(CommandeDM _commandeDM);
        ICollection<CommandeDM> getCommandeDMsNonRegle(Int64? _idSeance);
        Boolean isCommandeReglee(Int64 _idCommande);
        void changerEtatCommande(Int64 _idCommande, String _codeEtat);
        void envoyerTicketPreparation(Int64 _idCommande);
        void envoyerTicketNote(Int64 _idCommande);

        
    }
}