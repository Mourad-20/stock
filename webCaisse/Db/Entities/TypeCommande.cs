using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Db.Entities
{
    public class TypeCommande
    {
        public Int64 Identifiant { get; set; }
        public String Code { get; set; }
        public String Libelle { get; set; }
     //   public Int64? EnActivite { get; set; }
      //  public Int64? Affichable { get; set; }
    }
}