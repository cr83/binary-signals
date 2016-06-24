using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace EasyBot.Common
{
    public static class AppGlobal
    {
        public static IUnityContainer Container { get; set; }
    }
}
