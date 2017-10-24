using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.Web.Models
{
    public class ClockInClockOut
    {
        public string Username { get; set; }
        public bool CanClockIn { get; set; }
        public string Notes { get; set; }
    }
}