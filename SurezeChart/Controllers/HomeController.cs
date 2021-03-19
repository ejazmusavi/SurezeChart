using System;
using System.Linq;
using ChartDirector;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurezeChart.Data;
using SurezeChart.Models;
using static SurezeChart.Hubs.ChartHub;

namespace SurezeChart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult index2()
        {
            var ctg = _context.StripData.AsNoTracking().Where(w => w.StripId == 58).
                Select(s => new ChartUpdateData { FHR1 = s.FHR1, FHR2 = s.FHR2, TOCO = s.TOCO1, Date = s.CreatedDt.Value }).
                OrderBy(o => o.Date).Take(1500).ToList();
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
            return Json(new
            {
                dates = ctg.Select(s => s.Date).ToArray(),
                fhr1 = ctg.Select(s => s.FHR1).ToArray(),
                fhr2 = ctg.Select(s => s.FHR2).ToArray(),
                toco = ctg.Select(s => s.TOCO).ToArray()
            });
        }

    }
}