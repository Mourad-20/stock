using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using webCaisse.DMs;
using webCaisse.Reports.DMs;

namespace webCaisse.Taks.Contracts
{
    public interface IImpressionTask
    {
        void imprimerPreparation(Byte[] _content, String _printerName);
        void imprimerNote(Byte[] _content);
    }
}