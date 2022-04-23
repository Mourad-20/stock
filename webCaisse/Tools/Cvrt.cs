using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webCaisse.Tools
{
    public class Cvrt
    {
        public static T Convert<T>(Dictionary<String, Object> _dict, String _field)
        {
            if (_dict != null && _dict.ContainsKey(_field) && _dict[_field] != null)
            {
                return JsonConvert.DeserializeObject<T>(_dict[_field].ToString());
            }
            return default(T);
        }

        public static String dateToDateStr(DateTime? _date)
        {
            String _result = "";
            try
            {
                _result = (_date != null) ? ((DateTime)_date).ToString("dd/MM/yyyy") : "";
            }
            catch (Exception ex)
            {
                _result = "";
            }
            return _result;
        }

        public static DateTime? strToDateTime(String _str)
        {
            DateTime? _result = null;
            try
            {
                _result = DateTime.Parse(_str);
            }
            catch (Exception ex)
            {
                _result = null;
            }
            return _result;
        }

        public static Int64? strToInt64(String _str) {
            Int64? _result = null;
            try
            {
                _result = Int64.Parse(_str);
            }
            catch (Exception ex)
            {
                _result = null;
            }
            return _result;
        }

        public static String int64toStr(Int64? _nbr)
        {
            String _result = "";
            try
            {
                _result = (_nbr != null) ? _nbr.ToString() : "";
            }
            catch (Exception ex)
            {
                _result = "";
            }
            return _result;
        }

        public static String decimaltoStr(Decimal? _nbr)
        {
            String _result = "";
            try
            {
                _result = (_nbr != null) ? _nbr.ToString() : "";
            }
            catch (Exception ex)
            {
                _result = "";
            }
            return _result;
        }

        public static Decimal? strToDecimal(String _str)
        {
            Decimal? _result = null;
            try
            {
                _result = Decimal.Parse(_str);
            }
            catch (Exception ex)
            {
                _result = null;
            }
            return _result;
        }
        

    }
}