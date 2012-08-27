using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glimpse.VersionCheck
{
    public static class ObjectExtension
    {
        public static string GetTypeIfNotNull(this object value)
        {
            if (value != null)
                return value.GetType().FullName;
            return "";
        }
    }
}
