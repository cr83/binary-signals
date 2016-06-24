using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasyBot.Models;

namespace EasyBot.Business
{
    public static class Config
    {
        private static readonly List<TradingPlatform> DefaultPlatforms = new List<TradingPlatform>()
        {
          new TradingPlatform("Select Platform", (Uri) null),
          new TradingPlatform("24option", new Uri("http://option.go2jump.org/SHJJ4")),
          new TradingPlatform("TradeRush", new Uri("http://tracking.traderush.com/20053?campaignId=ob")),
          new TradingPlatform("CitiTrader", new Uri("http://www.cititrader.com/?campaign=25&P=ob")),
          new TradingPlatform("XP Markets", new Uri("http://aff.xpmarkets.com/l.aspx?A=31&SubAffiliateID=ob")),
          new TradingPlatform("Option Bit", new Uri("http://aff.optionbit.com/l.aspx?A=611&SubAffiliateID=ob"))
        };

        private static readonly List<Instrument> Instruments = new List<Instrument>()
        {
          new Instrument("AUD/USD", "AUD/USD", new Decimal(1, 0, 0, false, (byte) 4)),
          new Instrument("EUR/GBP", "EUR/GBP", new Decimal(1, 0, 0, false, (byte) 4)),
          new Instrument("EUR/JPY", "EUR/JPY", new Decimal(1, 0, 0, false, (byte) 2)),
          new Instrument("EUR/USD", "EUR/USD", new Decimal(1, 0, 0, false, (byte) 4)),
          new Instrument("GBP/CHF", "GBP/CHF", new Decimal(1, 0, 0, false, (byte) 4)),
          new Instrument("GBP/JPY", "GBP/JPY", new Decimal(1, 0, 0, false, (byte) 2)),
          new Instrument("GBP/USD", "GBP/USD", new Decimal(1, 0, 0, false, (byte) 4)),
          new Instrument("NZD/USD", "NZD/USD", new Decimal(1, 0, 0, false, (byte) 4)),
          new Instrument("USD/CAD", "USD/CAD", new Decimal(1, 0, 0, false, (byte) 4)),
          new Instrument("USD/CHF", "USD/CHF", new Decimal(1, 0, 0, false, (byte) 4)),
          new Instrument("USD/JPY", "USD/JPY", new Decimal(1, 0, 0, false, (byte) 2)),
          new Instrument("USD/RUB", "USD/RUB", new Decimal(1, 0, 0, false, (byte) 4)),
          new Instrument("USD/SGD", "USD/SGD", new Decimal(1, 0, 0, false, (byte) 4)),
          new Instrument("USD/TRY", "USD/TRY", new Decimal(1, 0, 0, false, (byte) 4)),
          new Instrument("USD/ZAR", "USD/ZAR", new Decimal(1, 0, 0, false, (byte) 4))
        };

        public static string[] GetUniqueSymbolsList()
        {
            return Enumerable.ToArray<string>(Enumerable.Select<Instrument, string>((IEnumerable<Instrument>)Config.Instruments, (Func<Instrument, string>)(i => i.Symbol)));
        }

        public static List<Instrument> GetInstruments(QuoteProvider quoteProvider)
        {
            List<Instrument> instruments = new List<Instrument>();
            Config.Instruments.ForEach((i) => instruments.Add(new Instrument(i.Name, i.Symbol, i.PipCost)));

            foreach (Instrument instrument in instruments)
            {
                instrument.Range = 10; //Settings.Default.InstrumentRange;
                instrument.TimeFrame = Models.Enums.TimeFrame.M1;// Instrument.ParseTimeFrame(Settings.Default.InstrumentTimeFrame);
                instrument.SetQuoteProvider(quoteProvider);
            }
            return instruments;
        }
    }
}
