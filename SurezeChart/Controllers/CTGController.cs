using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChartDirector;
using SurezeChart.Data;
using Microsoft.EntityFrameworkCore;
using SurezeChart.Models;
using Microsoft.AspNetCore.Http;

namespace SurezeChart.Controllers
{
    public class CTGController : Controller
    {
        private readonly ApplicationDbContext _context;
        private int width = 800;
        private Strip _strip;
        private DateTime startDate;
        private DateTime endDate;
        private double limit;
        private double totalData;
        private List<Strip> stripList = new List<Strip>();
        private Boolean wasInilized = false; 
        private DateTime lastUpdatedTime;
        private int lastCount;


        public CTGController(ApplicationDbContext context)
        {
            _context = context;
        }

       


        private void drawChart(RazorChartViewer viewer)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("StartDate")))
            {
                startDate = _context.Strips.AsNoTracking().OrderBy(o => o.CreateDate).Min(s => s.CreateDate);
            }
            else
            {
                startDate = DateTime.Parse(HttpContext.Session.GetString("StartDate"));
            }
            HttpContext.Session.SetString("StartDate", startDate.ToString());
            var timedStripData = _context.Strips.AsNoTracking().OrderBy(o => o.CreateDate).Where(w => w.CreateDate < startDate).DefaultIfEmpty().ToList();


         double[] dataSeriesA= timedStripData.Select(s => s.HRA).ToArray();
        double[] dataSeriesB= timedStripData.Select(s => s.HRB).ToArray();
        double[] dataSeriesC= timedStripData.Select(s => s.TOCO).ToArray();
        DateTime[] timeStamps= timedStripData.Select(s => s.CreateDate).ToArray(); ;

            //if (timedStripData.Count > 0)
            //{
            //    //timeStamps = timedStripData.Select(s => s.CreateDate).ToArray();
            //    dataSeriesA = timedStripData.Select(s => s.HRA).ToArray();
            //    dataSeriesB = timedStripData.Select(s => s.HRB).ToArray();

            //    dataSeriesC = timedStripData.Select(s => s.TOCO).ToArray();
            //}
            
            // Determine the visible x-axis range
            var viewPortStartDate = Chart.NTime(viewer.getValueAtViewPort("x", viewer.ViewPortLeft));
            var viewPortEndDate = Chart.NTime(viewer.getValueAtViewPort("x", viewer.ViewPortLeft + viewer.ViewPortWidth));
           
            //
            // Now we have obtained the data, we can plot the chart.
            //

            //================================================================================
            // Configure overall chart appearance.
            //================================================================================

            // Create an XYChart object of size 640 x 400 pixels
            XYChart c = new XYChart(width, 300);

            // Set the plotarea at (55, 55) with width 80 pixels less than chart width, and height 90
            // pixels less than chart height. Use a vertical gradient from light blue (f0f6ff) to sky
            // blue (a0c0ff) as background. Set border to transparent and grid lines to white (ffffff).
            c.setPlotArea(55, 10, c.getWidth() - 110, c.getHeight() - 40, 0xf0f6ff, -1, 0x87ceeb, 0x87ceeb, 0x87ceeb);
            c.setClipping();

            if (viewer.IsAttachmentRequest())
            {
                LegendBox b = c.addLegend(55, 28, false, "Arial Bold", 10);
                b.setBackground(Chart.Transparent, Chart.Transparent);
                b.setLineStyleKey();
            }

            // Set the x and y axis stems to transparent and the label font to 10pt Arial

            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);

            // Set the axes width to 2 pixels




            //================================================================================
            // Add data to chart
            //================================================================================

            //
            // In this example, we represent the data by lines. You may modify the code below to use
            // other layer types (areas, scatter plot, etc).
            //

            // Add a line layer for the lines, using a line width of 2 pixels
            LineLayer layer = c.addLineLayer2();
            //layer.setLineWidth(2);
            //c.setClipping();

            // In this demo, we do not have too many data points. In real code, the chart may contain a
            // lot of data points when fully zoomed out - much more than the number of horizontal pixels
            // in this plot area. So it is a good idea to use fast line mode.
            //layer.setFastLineMode();

            // Now we add the 3 data series to a line layer, using the color red (0xff3333), green
            // (0x008800) and blue (0x3333cc)
            layer.setXData(timeStamps);
            layer.addDataSet(dataSeriesA, 0x0000ff);
            layer.addDataSet(dataSeriesB, 0x800080);

            c.xAxis().setLabelFormat("{value|mm/dd/yyyy hh:nn:ss}");

            //int a = 0, s = -50;

            //c.addText(4, 10, "200", "", 8);
            //c.addText(4, 40, "180", "", 8);
            //c.addText(4, 70, "160", "", 8);
            //c.addText(4, 100, "140", "", 8);
            //c.addText(4, 130, "120", "", 8);
            //c.addText(4, 160, "100", "", 8);
            //c.addText(4, 190, "80", "", 8);
            //c.addText(4, 220 , "60", "", 8);

            //////Second       
            //c.addText(340 + s, 10 + a, "200", "", 8);
            //c.addText(340 + s, 40 + a, "180", "", 8);
            //c.addText(340 + s, 70 + a, "160", "", 8);
            //c.addText(340 + s, 100 + a, "140", "", 8);
            //c.addText(340 + s, 130 + a, "120", "", 8);
            //c.addText(340 + s, 160 + a, "100", "", 8);
            //c.addText(340 + s, 190 + a, "80", "", 8);
            //c.addText(340 + s, 220 + a, "60", "", 8);



            //////3rd           
            //c.addText(610 + s, 10 + a, "200", "", 8);
            //c.addText(610 + s, 40 + a, "180", "", 8);
            //c.addText(610 + s, 70 + a, "160", "", 8);
            //c.addText(610 + s, 100 + a, "140", "", 8);
            //c.addText(610 + s, 130 + a, "120", "", 8);
            //c.addText(610 + s, 160 + a, "100", "", 8);
            //c.addText(610 + s, 190 + a, "80", "", 8);
            //c.addText(610 + s, 220 + a, "60", "", 8);

            //================================================================================
            // Configure axis scale and labelling
            //================================================================================

            // Set the x-axis as a date/time axis with the scale according to the view port x range.
            // c.xAxis().setLabelOffset(80);

            viewer.syncDateAxisWithViewPort("x", c.xAxis());


            // For the automatic y-axis labels, set the minimum spacing to 30 pixels.
            //c.yAxis().setColors(Chart.Transparent, Chart.Transparent);
            c.yAxis().setTickDensity(11, 11);
            c.xAxis().setTickDensity(30, 10);


            //c.xAxis().setAutoScale();

            //////////////////////////////////////////

            //
            // Now we have obtained the data, we can plot the chart.
            //

            //================================================================================
            // Configure overall chart appearance.
            //================================================================================

            // Create an XYChart object of size 640 x 400 pixels
            XYChart d = new XYChart(width, 200);

            // Set the plotarea at (55, 55) with width 80 pixels less than chart width, and height 90
            // pixels less than chart height. Use a vertical gradient from light blue (f0f6ff) to sky
            // blue (a0c0ff) as background. Set border to transparent and grid lines to white (ffffff).
            d.setPlotArea(55, 10, c.getWidth() - 110, c.getHeight() - 170, 0xffffff, -1, 0x87ceeb, 0x87ceeb, 0x87ceeb);

            // As the data can lie outside the plotarea in a zoomed chart, we need to enable clipping.
            d.setClipping();

            if (viewer.IsAttachmentRequest())
            {
                LegendBox b = d.addLegend(55, 28, false, "Arial Bold", 10);
                b.setBackground(Chart.Transparent, Chart.Transparent);
                b.setLineStyleKey();
            }

            // Set the x and y axis stems to transparent and the label font to 10pt Arial
            //d.xAxis().setColors(Chart.Transparent);
            d.yAxis().setColors(Chart.Transparent);
            d.xAxis().setLabelStyle("Arial", 10);
            d.yAxis().setLabelStyle("Arial", 10);




            //d.addText(0, 18, "100", "", 10, 0x00008B);
            //d.addText(280, 18, "100", "", 10, 0x00008B);
            //d.addText(560, 18, "100", "", 10, 0x00008B);


            //d.addText(0,   210, "0", "", 10, 0x00008B);
            //d.addText(280, 210, "0", "", 10, 0x00008B);
            //d.addText(560, 210, "0", "", 10, 0x00008B);

            d.addText(650, 100, "Toco (%)", "", 10, 0x000000);

            //================================================================================
            // Add data to chart
            //================================================================================

            //
            // In this example, we represent the data by lines. You may modify the code below to use
            // other layer types (areas, scatter plot, etc).
            //

            // Add a line layer for the lines, using a line width of 2 pixels
            LineLayer layer2 = d.addLineLayer2();
            //layer2.setLineWidth(1);

            // In this demo, we do not have too many data points. In real code, the chart may contain a
            // lot of data points when fully zoomed out - much more than the number of horizontal pixels
            // in this plot area. So it is a good idea to use fast line mode.
            // layer2.setFastLineMode();

            // Now we add the 3 data series to a line layer, using the color red (0xff3333), green
            // (0x008800) and blue (0x3333cc)
            // layer2.setXData(timeStamps);

            layer2.addDataSet(dataSeriesC, 0x000000);
            layer2.setXData(timeStamps);



            //================================================================================
            // Configure axis scale and labelling
            //================================================================================

            // Set the x-axis as a date/time axis with the scale according to the view port x range.
            viewer.syncDateAxisWithViewPort("x", d.xAxis());

            // For the automatic y-axis labels, set the minimum spacing to 30 pixels.
            d.yAxis().setTickDensity(11, 11);
            d.xAxis().setTickDensity(30, 10);
            d.xAxis().setColors(Chart.Transparent, Chart.Transparent);

            //d.xAxis().setAutoScale();
            //d.yAxis().setAutoScale();

            //////////////////////////////////////////
            ///

            MultiChart multiChart = new MultiChart(width, 460);
            multiChart.addChart(0, 0, c);
            multiChart.addChart(0, c.getHeight(), d);


            if (viewer.IsAttachmentRequest())
            {
                // Output as PDF attachment
                viewer.Image = multiChart.makeWebImage(Chart.PDF);
            }
            else
            {
                // Output the chart
                viewer.Image = multiChart.makeWebImage(Chart.PNG);

                // Output Javascript chart model to the browser to suppport tracking cursor
                viewer.ChartModel = multiChart.getJsChartModel();
            }
        }

        public IActionResult Index()
        {
            RazorChartViewer viewer = ViewBag.Viewer = new RazorChartViewer(HttpContext, "chart1");


            // Draw chart using the most update data
            drawChart(viewer);

            // If is streaming request, output the chart only
            if (RazorChartViewer.IsStreamRequest(Request))
            {
                return File(viewer.StreamChart(), Response.ContentType);
            }

            ViewBag.startDate = DateTime.Now;
            return View();
           
        }
    }
}