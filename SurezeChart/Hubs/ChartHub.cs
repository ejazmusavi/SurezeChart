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
    public class ChartTime
    {
        public int StripId { get; set; }
        public DateTime LastTime { get; set; }
    }
    public class ChartHub : Hub
    {
        ApplicationDbContext _context;
        public static List<ChartTime> LastTime = new List<ChartTime>();
        //static List<stripdatalog2> dummyData = null;

        public ChartHub(ApplicationDbContext context)
        {
            _context = context;
         
        }

        static int index = 0;
        public void GetData()
        {
            var lasttime = LastTime.Where(w => w.StripId == 58).Select(s=>s.LastTime).FirstOrDefault();
            var ctg = _context.StripData.AsNoTracking().
                Where(w => w.StripId == 58 & w.CreatedDt >= lasttime).
                OrderBy(o => o.CreatedDt).Take(2).
                Select(s => new ChartUpdateData { FHR1 = s.FHR1, FHR2 = s.FHR2, TOCO = s.TOCO1, Date = s.CreatedDt.Value }).ToList();

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

            var t = ctg.Where(w => w.Date == lasttime).FirstOrDefault();
            if(t!=null)
            {
                ctg.Remove(t);
            }
            var lasttime1 = LastTime.Where(w => w.StripId == 58).FirstOrDefault();
            if (lasttime1 != null)
                LastTime.Remove(lasttime1);

            ctg = ctg.OrderBy(d => d.Date).ToList();

            LastTime.Add(new ChartTime { StripId = 58, LastTime = ctg.Max(s => s.Date)});

            Clients.All.SendAsync("updatechart", ctg);
        }


        public void InitialData()
        {
            var ctg = _context.StripData.AsNoTracking().Where(w => w.StripId == 58).
                Select(s => new ChartUpdateData { FHR1 = s.FHR1, FHR2 = s.FHR2, TOCO = s.TOCO1, Date = s.CreatedDt.Value }).
                OrderBy(o => o.Date).Take(2000).ToList();
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
            var lasttime = LastTime.Where(w => w.StripId == 58).FirstOrDefault();
            if (lasttime != null)
                LastTime.Remove(lasttime);

            ctg = ctg.OrderBy(d => d.Date).ToList();

            LastTime.Add(new ChartTime { StripId = 58, LastTime = ctg.Max(s => s.Date) });
            Clients.Caller.SendAsync("initialChartData", ctg );
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
