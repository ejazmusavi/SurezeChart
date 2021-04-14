using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SurezeChart.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurezeChart.Hubs
{
    public class ChartTime
    {
        public string conId { get; set; }
        public int StripId { get; set; }
        public DateTime LastTime { get; set; }
    }
    public class ChartHub : Hub
    {
        ApplicationDbContext _context;
        public static List<ChartTime> LastTime = new List<ChartTime>();
        //static List<stripdatalog2> dummyData = null;
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        } 

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
        public ChartHub(ApplicationDbContext context)
        {
            _context = context;

        }
        static int index = 0;
        public void GetData(int stripid, string conId)
        {
            //Clients.All.SendAsync("updatechartError", time.ToString(), "helo");
            try
            {
                var lasttime = LastTime.Where(w => w.StripId == stripid && w.conId==conId).
                    Select(s => s.LastTime).FirstOrDefault();
                Clients.All.SendAsync("updatechartError", "time list", LastTime);
                var ctg = _context.StripData.AsNoTracking().
                    Where(w => w.StripId == stripid & w.CreatedDt >= lasttime).
                    OrderBy(o => o.CreatedDt).Take(2).
                    Select(s => new ChartUpdateData { FHR1 = s.FHR1, FHR2 = s.FHR2, TOCO = s.TOCO1, Date = s.CreatedDt.Value }).ToList();

                for (int i = 0; i < ctg.Count(); i++)
                {
                    if (i == 0)
                        continue;

                    LoopAgain:
                    double seconds2 = (ctg[i].Date - ctg[i-1].Date).TotalSeconds;
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
                    else
                    {
                        var fhr1Diff = ctg[i].FHR1 - ctg[i - 1].FHR1;

                        if (fhr1Diff > 15 || fhr1Diff < -15 || ctg[i].FHR1 == 0)
                        {
                            ctg[i].FHR1 = null;
                        }

                        var fhr2Diff = ctg[i].FHR2 - ctg[i - 1].FHR2;

                        if (fhr2Diff > 15 || fhr2Diff < -15 || ctg[i].FHR2 == 0)
                        {
                            ctg[i].FHR2 = null;
                        }
                    }
                }

                var t = ctg.Where(w => w.Date == lasttime).FirstOrDefault();
                if (t != null)
                {
                    ctg.Remove(t);
                }
                var lasttime1 = LastTime.Where(w => w.StripId == stripid && w.conId==conId).FirstOrDefault();
                if (lasttime1 != null)
                    LastTime.Remove(lasttime1);

                ctg = ctg.OrderBy(d => d.Date).ToList();
                if (ctg.Count > 0)
                    LastTime.Add(new ChartTime {conId=conId, StripId = stripid, LastTime = ctg.Max(s => s.Date) });

                Clients.All.SendAsync("updatechart", ctg);
            }
            catch(Exception ee) {
                Clients.All.SendAsync("updatechartError", stripid, ee.Message);

            }
        }

        public void InitialData(int stripid, string conId)
        {
            var ctg = _context.StripData.AsNoTracking().Where(w => w.StripId == stripid).
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
                else
                {
                    var fhr1Diff = ctg[i].FHR1 - ctg[i - 1].FHR1;

                    if (fhr1Diff > 15 || fhr1Diff < -15 || ctg[i].FHR1 == 0)
                    {
                        ctg[i].FHR1 = null;
                    }

                    var fhr2Diff = ctg[i].FHR2 - ctg[i - 1].FHR2;

                    if (fhr2Diff > 15 || fhr2Diff < -15 || ctg[i].FHR2 == 0)
                    {
                        ctg[i].FHR2 = null;
                    }
                }
            }
            var lasttime = LastTime.Where(w => w.StripId == stripid && w.conId==conId).FirstOrDefault();
            if (lasttime != null)
                LastTime.Remove(lasttime);

            ctg = ctg.OrderBy(d => d.Date).ToList();
            if (ctg.Count > 0)
                LastTime.Add(new ChartTime { conId = conId, StripId = stripid, LastTime = ctg.Max(s => s.Date) });
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
