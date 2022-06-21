using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.VMs.Params
{
    public class LoginVM
    {
        public String Login { get; set; }
        public String Password { get; set; }
        public String Jeton { get; set; }
    }
}