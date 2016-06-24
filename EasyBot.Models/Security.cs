using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyBot.Models.Enums;

namespace EasyBot.Models
{
    public class Security
    {
        public string Name { get; private set; }

        public string Symbol { get; private set; }

        public Decimal Range { get; private set; }

        public TimeFrame TimeFrame { get; private set; }

        public Security(string name, string symbol, Decimal range, TimeFrame timeFrame)
        {
            this.Name = name;
            this.Symbol = symbol;
            this.Range = range;
            this.TimeFrame = this.TimeFrame;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
