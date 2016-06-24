using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBot.Models
{
    public class QuoteProviderStatusEventArgs : EventArgs
    {
        public string Status { get; private set; }

        public QuoteProviderStatusEventArgs(string status)
        {
            this.Status = status;
        }
    }
}
