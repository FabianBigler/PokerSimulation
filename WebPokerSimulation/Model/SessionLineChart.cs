using Highsoft.Web.Mvc.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPokerSimulation.Model
{
    public class SessionLineChart
    {
        public XAxis XAxis { get; set; }
        public YAxis YAxis { get; set; }
        public List<Series> Series { get; set; }
    }
}