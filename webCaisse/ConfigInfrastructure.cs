using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace webCaisse
{
    public class ConfigInfrastructure
    {
        public static String BO_FILE_ROOT = ConfigurationManager.AppSettings["CaisseFileRoot"];
        public static String LOG_STANDARD = @"\Logs\Standard";
        public static Int32 EXPIRATION_TIME_TOKEN = Int32.Parse(ConfigurationManager.AppSettings["EXPIRATION_TIME_TOKEN"]);
        public static String CORS_ALLOW_ORIGIN = ConfigurationManager.AppSettings["CORS_ALLOW_ORIGIN"];
        public static Double? WIDTH_PAPER = Double.Parse(ConfigurationManager.AppSettings["WIDTH_PAPER"]);
        public static Boolean ACTIVATE_PRINT = Boolean.Parse(ConfigurationManager.AppSettings["ACTIVATE_PRINT"]);
        public static String IMPRIMANTE_CENTRALE = ConfigurationManager.AppSettings["IMPRIMANTE_CENTRALE"];

        
    }
}