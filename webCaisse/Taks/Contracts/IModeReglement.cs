using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface IModeReglementTask
    {
        ModeReglementDM getModeReglementDMByCode(String _code);
        ICollection<ModeReglementDM> getModeReglementDMs();

    }
}