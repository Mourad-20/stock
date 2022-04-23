using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using webCaisse.VMs.Models;

namespace webCaisse.Mappers
{
    public class AffectationMessageMapper
    {
        public static AffectationMessageVM AffectationMessageDMtoAffectationMessageVM(AffectationMessageDM _src)
        {
            AffectationMessageVM _dest = null;
            if (_src != null)
            {
                _dest = new AffectationMessageVM()
                {
                    Identifiant = _src.Identifiant,
                    IdDetailCommande = _src.IdDetailCommande,
                    IdMessage = _src.IdMessage,
                    LibelleMessage = _src.LibelleMessage,
                };
            }
            return _dest;
        }
        public static AffectationMessageDM AffectationMessageVMtoAffectationMessageDM(AffectationMessageVM _src)
        {
            AffectationMessageDM _dest = null;
            if (_src != null)
            {
                _dest = new AffectationMessageDM()
                {
                    Identifiant = _src.Identifiant,
                    IdDetailCommande = _src.IdDetailCommande,
                    IdMessage = _src.IdMessage,
                };
            }
            return _dest;
        }


        
    }
}