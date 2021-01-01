using System;
using System.Linq;
using ChartDirector;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurezeChart.Data;
using SurezeChart.Models;

namespace NetMvcCharts.Controllers
{
    public class HomeController : Controller
    {
        //
        // Initialize the WebChartViewer when the page is first loaded
        //
        private readonly ApplicationDbContext _context;
        private int width = 800;
        

        public HomeController(ApplicationDbContext context)
        {
            _context = context;

        }
        


        private void initViewer(RazorChartViewer viewer)
        {
            var stripList = _context.Strips.AsNoTracking();

            var startDate = stripList.Select(s => s.CreateDate).Min();
            var endDate = stripList.Select(s => s.CreateDate).Max();
          
            viewer.setFullRange("x", startDate, endDate);

            // Initialize the view port to show the last 366 days (out of 1826 days)
            viewer.ViewPortWidth = 0.03;

            //viewer.ViewPortLeft = 1 - viewer.ViewPortWidth;

            // Set the maximum zoom to 10 days (out of 1826 days)
            
            viewer.ViewPortLeft = 1 - 0.1;
            
        }

        //
        // Create a random table for demo purpose.
        //
       

        //
        // Draw the chart
        //
        private void drawChart(RazorChartViewer viewer)
        {
            // Determine the visible x-axis range
            DateTime viewPortStartDate = Chart.NTime(viewer.getValueAtViewPort("x", viewer.ViewPortLeft));
            DateTime viewPortEndDate = Chart.NTime(viewer.getValueAtViewPort("x", viewer.ViewPortLeft + viewer.ViewPortWidth));

            var dataList = _context.Strips.OrderBy(o => o.CreateDate).AsNoTracking().ToList();
            var stripsCount = _context.Strips.Count();
            // Strips Data
            DateTime[] timeStamps = new DateTime[stripsCount];
            double[] dataSeriesA= new double[stripsCount];
            double[] dataSeriesB = new double[stripsCount];
            double[] dataSeriesC = new double[stripsCount]; 

            if (stripsCount > 0)
            {
                int i = 0;
                foreach (var item in dataList)
                {
                    
                        dataSeriesA[i] = item.HRA;
                        dataSeriesB[i] = item.HRB;
                        timeStamps[i] = item.CreateDate;
                        dataSeriesC[i] = item.TOCO;

                        i++;
                    
                }
            }

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
            c.setPlotArea(55, 10, c.getWidth()-110, c.getHeight()-40, 0xf0f6ff, -1, 0x87ceeb, 0x87ceeb, 0x87ceeb);
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
            c.yAxis().setTickDensity(11,11);           
            c.xAxis().setTickDensity(30,10);

         
            //c.xAxis().setAutoScale();

            //////////////////////////////////////////

            //
            // Now we have obtained the data, we can plot the chart.
            //

            //================================================================================
            // Configure overall chart appearance.
            //================================================================================

            // Create an XYChart object of size 640 x 400 pixels
            XYChart d = new XYChart(width,200);

            // Set the plotarea at (55, 55) with width 80 pixels less than chart width, and height 90
            // pixels less than chart height. Use a vertical gradient from light blue (f0f6ff) to sky
            // blue (a0c0ff) as background. Set border to transparent and grid lines to white (ffffff).
            d.setPlotArea(55, 10, c.getWidth() - 110, c.getHeight()-170, 0xffffff , -1, 0x87ceeb, 0x87ceeb, 0x87ceeb);

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

        private void drawFullChart(RazorViewPortControl vp, RazorChartViewer viewer)
        {
            // We need to draw a small thumbnail chart for the full data range. The simplest method is to
            // simply get the full data to draw the chart. If the full data are very large (eg. millions
            // of points), for such a small thumbnail chart, it is often acceptable to just retreive a
            // small sample of the data.
            //
            // In this example, there are only around 5500 points for the 3 data series. This amount is
            // not large to ChartDirector, so we simply pass all the data to ChartDirector.
          //  RanTable r = getRandomTable();


            

            ////Strips Data

            var dataList = _context.Strips.OrderBy(o => o.CreateDate).AsNoTracking().ToList();
            var stripsCount = _context.Strips.Count();
            // Strips Data
            DateTime[] timeStamps = new DateTime[stripsCount];
            double[] dataSeriesA = new double[stripsCount];
            double[] dataSeriesB = new double[stripsCount];
            double[] dataSeriesC = new double[stripsCount];

            if (stripsCount > 0)
            {
                int i = 0;
                foreach (var item in dataList)
                {
                                           
                        dataSeriesA[i] = item.HRA;
                        dataSeriesB[i] = item.HRB;
                        timeStamps[i] = item.CreateDate;
                        dataSeriesC[i] = item.TOCO;

                        i++;
                        
                }
            }


            // Create an XYChart object of size 640 x 60 pixels
            XYChart c = new XYChart(width, 40);

            // Set the plotarea with the same horizontal position as that in the main chart for
            // alignment. The vertical position is set to equal to the chart height.
            c.setPlotArea(55, 0, c.getWidth()-110, c.getHeight(), 0xc0d8ff, -1, 0x888888,
                Chart.Transparent, 0xffffff);

            // Set the x axis stem to transparent and the label font to 10pt Arial
            c.xAxis().setColors(Chart.Transparent);
            c.xAxis().setLabelStyle("Arial", 10);

            // Put the x-axis labels inside the plot area by setting a negative label gap. Use
            // setLabelAlignment to put the label at the right side of the tick.
            c.xAxis().setLabelGap(-1);
            c.xAxis().setLabelAlignment(1);

            // Set the y axis stem and labels to transparent (that is, hide the labels)
            c.yAxis().setColors(Chart.Transparent, Chart.Transparent);

            c.setClipping();

            // Add a line layer for the lines with fast line mode enabled
            LineLayer layer = c.addLineLayer2();
            //layer.setFastLineMode();

            // Now we add the 3 data series to a line layer, using the color red (0xff3333), green
            // (0x008800) and blue (0x3333cc)
        
            layer.addDataSet(dataSeriesA, 0x0000ff);
            layer.addDataSet(dataSeriesB, 0x800080);

            // The x axis scales should reflect the full range of the view port
           // c.xAxis().setDateScale(viewer.getValueAtViewPort("x", 0), viewer.getValueAtViewPort("x", 1));

            // For the automatic x-axis labels, set the minimum spacing to 75 pixels.
            c.xAxis().setTickDensity(100);

            // For the auto-scaled y-axis, as we hide the labels, we can disable axis rounding. This can
            // make the axis scale fit the data tighter.
            c.yAxis().setRounding(false, false);


            ////////////////////////////////////////////////

            XYChart d = new XYChart(width, 50);

            // Set the plotarea with the same horizontal position as that in the main chart for
            // alignment. The vertical position is set to equal to the chart height.
            d.setPlotArea(55, 0, d.getWidth()-110, d.getHeight(), 0xc0d8ff, -1, 0x888888,
                Chart.Transparent, 0xffffff);

            // Set the x axis stem to transparent and the label font to 10pt Arial
           // d.xAxis().setColors(Chart.Transparent);
            d.xAxis().setLabelStyle("Arial", 10);
            d.xAxis().setDateScale(viewer.getValueAtViewPort("x", 0), viewer.getValueAtViewPort("x", 1));
            // Put the x-axis labels inside the plot area by setting a negative label gap. Use
            // setLabelAlignment to put the label at the right side of the tick.
            d.xAxis().setLabelGap(-17);
            d.xAxis().setLabelAlignment(2);

            d.setClipping();
            // Set the y axis stem and labels to transparent (that is, hide the labels)
            d.yAxis().setColors(Chart.Transparent, Chart.Transparent);

            // Add a line layer for the lines with fast line mode enabled
            LineLayer layer2 = d.addLineLayer2();
            layer2.setFastLineMode();
            layer2.setXData(timeStamps);
            // Now we add the 3 data series to a line layer, using the color red (0xff3333), green
            // (0x008800) and blue (0x3333cc)
            layer2.setXData(timeStamps);
            layer2.addDataSet(dataSeriesC, 0x000000);

            // The x axis scales should reflect the full range of the view port
            d.xAxis().setDateScale(viewer.getValueAtViewPort("x", 0), viewer.getValueAtViewPort("x", 1));

            // For the automatic x-axis labels, set the minimum spacing to 75 pixels.
            d.xAxis().setTickDensity(150);            
            d.xAxis().setColors(Chart.Transparent);

            // For the auto-scaled y-axis, as we hide the labels, we can disable axis rounding. This can
            // make the axis scale fit the data tighter.
            d.yAxis().setRounding(false, false);


            //////////////////////////////////////////////


            MultiChart multiChart = new MultiChart(width, 100);
            multiChart.addChart(0, 0, c);
            multiChart.addChart(0, c.getHeight(), d);
            vp.Image = multiChart.makeWebImage(Chart.PNG);


           
        }

        public ActionResult Index()
        {
            RazorChartViewer viewer = ViewBag.Viewer = new RazorChartViewer(HttpContext, "chart1");
            RazorViewPortControl viewPortCtrl = ViewBag.ViewPortControl = new RazorViewPortControl(HttpContext, "chart2");

            //
            // This script handles both the full page request, as well as the subsequent partial updates
            // (AJAX chart updates). We need to determine the type of request first before we processing
            // it.
            //
            if (RazorChartViewer.IsPartialUpdateRequest(Request))
            {
                // Is a partial update request.
                drawChart(viewer);

                if (viewer.IsAttachmentRequest())
                {
                    return File(viewer.StreamChart(), Response.ContentType, "demochart.pdf");
                }
                else
                {
                    return Content(viewer.PartialUpdateChart());
                }
            }

            //
            // If the code reaches here, it is a full page request.
            //

            // Initialize the WebChartViewer and draw the chart.
            initViewer(viewer);
            drawChart(viewer);

            // Draw a thumbnail chart representing the full range in the WebViewPortControl
            drawFullChart(viewPortCtrl, viewer);
            return View();
        }
    }
}