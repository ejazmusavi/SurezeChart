using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SurezeChart.Data;
using SurezeChart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurezeChart.Hubs
{
    public class ChartHub : Hub
    {
        ApplicationDbContext _context;
        //static List<stripdatalog2> dummyData = null;

        public ChartHub(ApplicationDbContext context)
        {
            _context = context;
         
        }

        public static DateTime dt = DateTime.Now;


        static int index = 0;
        public void GetData()
        {
            var data = _context.StripData.AsNoTracking().Where(w => w.StripId == 58).OrderBy(o => o.CreatedDt).Skip(index).Take(1).Select(s => new ChartUpdateData { FHR1 = s.FHR1, FHR2 = s.FHR2, TOCO = s.TOCO1, Date = dt }).FirstOrDefault();

            if (index%10==0)
            {
                data.FHR1 = null;
            }
            if (data != null)
                index += 1;
            else index = 0;
            dt = dt.AddSeconds(1);
            Clients.All.SendAsync("updatechart", data);
        }


        public void InitialData()
        {
            var ctg = _context.StripData.AsNoTracking().Where(w => w.StripId == 58).
                Select(s => new ChartUpdateData { FHR1 = s.FHR1, FHR2 = s.FHR2, TOCO = s.TOCO1, Date = s.CreatedDt.Value }).
                OrderBy(o => o.Date).Take(1500).ToList();
            index = ctg.Count;
            for (int i = 0; i < ctg.Count(); i++)
            {
                if (i == 0)
                    continue;
                int j = i;
            LoopAgain:
                double seconds2 = (ctg[i].Date - (ctg[i - 1].Date)).TotalSeconds;
                if (seconds2 > 1)
                {
                    var data = new ChartUpdateData() { };
                    data.Date = ctg[i - 1].Date.AddSeconds(1);
                    data.FHR1 = null;
                    data.FHR2 = null;
                    data.TOCO = null;
                    data.Notes = null;
                    ctg.Insert(i, data);
                    goto LoopAgain;
                }
            }
            ctg = ctg.OrderBy(d => d.Date).ToList();
            Clients.Caller.SendAsync("initialChartData", ctg);
        }


        public class ChartUpdateData
        {
            public int Year { get; set; }
            public double? FHR1 { get; set; }
            public double? FHR2 { get; set; }
            public double? TOCO { get; set; }
            public double Singal { get; set; }
            public int StripId { get; set; }
            public DateTime Date { get; set; }
            public int RoomCode { get; set; }
            public string Notes { get; set; }
        }
    }
}
