using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBot.Models
{
    public class InstrumentRequestModel
    {
        public string ConnectionId { get; set; }

        public string Symbol { get; set; }

        public string TimeFrame { get; set; }

        public decimal Range { get; set; }
    }

    public class InstrumentsListRequestModel
    {
        public string ConnectionId { get; set; }

        public List<InstrumentRequestModel> Instruments { get; set; }
    }
}