using Project.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Web.DAL
{
    public interface ITimeClockDAL
    {
        List<TimeCard> GetTimeCardHistory(string username);
        void ClockIn(TimeCard newTime);
        void ClockOut(TimeCard tc);
        bool CanClockIn(string username);
    }
}
