using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EasyBot.Models.Enums;

namespace EasyBot.Business
{
    public static class TimeFrameConverter
    {
        public static string ToMinutes(TimeFrame timeFrame)
        {
            switch (timeFrame)
            {
                case TimeFrame.M15:
                    return "15";
                case TimeFrame.M30:
                    return "30";
                case TimeFrame.H1:
                    return "60";
                case TimeFrame.M1:
                    return "1";
                case TimeFrame.M5:
                    return "5";
                default:
                    return "1";
            }
        }
    }
}
