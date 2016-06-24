using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasyBot.Models.Enums;

namespace EasyBot.Models
{
    public class SignalEventArgs : EventArgs
    {
        public Signal Signal { get; private set; }

        public SignalEventArgs(SignalType type, string symbol, TimeFrame timeFrame, Decimal range, DateTime dateTime)
        {
            this.Signal = new Signal(type, symbol, timeFrame, range, dateTime);
        }
    }
}
