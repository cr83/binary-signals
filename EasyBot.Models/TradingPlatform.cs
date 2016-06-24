using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBot.Models
{
    public class TradingPlatform
    {
        public string Name { get; private set; }

        public Uri Uri { get; private set; }

        public TradingPlatform(string name, Uri uri)
        {
            this.Name = name;
            this.Uri = uri;
        }
    }
}
