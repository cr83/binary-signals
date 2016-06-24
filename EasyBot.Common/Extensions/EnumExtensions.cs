using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace EasyBot.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetJsArray(Type enumType)
        {
            string res = string.Empty;
            if (enumType.IsEnum)
            {
                var arr = Enum.GetNames(enumType);
                var serializer = new JavaScriptSerializer();
                return serializer.Serialize(arr);
            }
            return res;
        }
    }
}
