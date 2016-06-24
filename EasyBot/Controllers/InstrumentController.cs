using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EasyBot.Attributes;
using EasyBot.Business;
using EasyBot.Streaming;
using EasyBot.Models.Enums;
using EasyBot.Models;

namespace EasyBot.Controllers
{
    public partial class InstrumentController : Controller
    {
        [AjaxOnly]
        public virtual JsonResult GetInstrument(InstrumentRequestModel model)
        {
            if (String.IsNullOrEmpty(model.Symbol))
                throw new ArgumentNullException("symbol");
            var instrument = Streamer.GetOrCreate(model.ConnectionId).GetInstruments().First(i => i.Symbol == model.Symbol);
            return new JsonResult
            {
                Data = new { instrument.Symbol, TimeFrame = Enum.GetName(typeof(TimeFrame), instrument.TimeFrame), instrument.Range },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [AjaxOnly]
        public virtual JsonResult GetIntruments(InstrumentRequestModel model)
        {
            var instruments = Streamer.GetOrCreate(model.ConnectionId)
                .GetInstruments()
                .Select(i => new { i.Symbol, TimeFrame = Enum.GetName(typeof(TimeFrame), i.TimeFrame), i.Range, i.Available });
            return new JsonResult
            {
                Data = instruments,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [AjaxOnly]
        public virtual JsonResult SetInstrumentsSettings(InstrumentsListRequestModel model)
        {
            if (model.Instruments != null)
            {
                foreach (var instrument in model.Instruments)
                {
                    Streamer.GetOrCreate(model.ConnectionId).UpdateInstrumentSettings(instrument.Symbol, instrument.TimeFrame, instrument.Range);
                }
            }

            return new JsonResult
            {
                Data = "Ok"
            };
        }

        [AjaxOnly]
        public virtual EmptyResult SubscribeOnHistory(InstrumentRequestModel model)
        {
            Streamer.GetOrCreate(model.ConnectionId).SubscribeOnHistory(model.Symbol, model.TimeFrame);
            return new EmptyResult();
        }
    }
}