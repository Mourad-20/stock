using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.DMs
{
    public class MessageDM
    {
        public Int64 Identifiant { get; set; }
        public String Libelle { get; set; }
        public Int64? EnActivite { get; set; }
    }
}