using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBot.Models
{
    public class InstrumentErrorEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public string Symbol { get; private set; }

        public InstrumentErrorEventArgs(string message, string symbol)
        {
            this.Message = message;
            this.Symbol = symbol;
        }
    }
}
