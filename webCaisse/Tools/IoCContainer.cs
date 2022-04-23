using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;

namespace webCaisse.Tools
{
    public class IoCContainer
    {
        public static IUnityContainer Current { get; set; }
        public static T Resolve<T>()
        {
            return (T)Current.Resolve(typeof(T));
        }

    }
}