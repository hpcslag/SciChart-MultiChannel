// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SplineChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateACustomChart.SplineLineSeries
{
    /// <summary>
    /// Interaction logic for CustomChartExampleView.xaml
    /// </summary>
    public partial class MainChart : UserControl
    {
        public MainChart()
        {
            InitializeComponent();

            //按下 Space 之後，回到波形 Overview
            this.KeyDown += (object sender, KeyEventArgs e) => {
                if(e.Key == Key.Space)
                {
                    this.sciChart.ZoomExtents();
                }
            };
        }

        private void ChartLoaded(object sender, RoutedEventArgs e)
        {
            /*chartTitle.Text = "波型圖標題";
            lineRenderSeries.IsDigitalLine = false; //是否為數位波形 (0,1)

            // Create a DataSeries of type X=double, Y=double
            var originalData = new XyDataSeries<double, double>() {SeriesName = "曲線圖名稱"};

            lineRenderSeries.DataSeries = originalData;



            // 1000: 精細度, 4: 頻率
            var data = DataManager.Instance.GetSinewave(1.0, 0.0, 1000,4);

            //增加資料的方法 1
            XYPoint xy = new XYPoint();
            xy.X = 11;
            xy.Y = 5;
            data.Add(xy);

            //增加資料的方法 2
            addData(data, 12,9);

            //Demo
            //DoubleSeries d = new DoubleSeries();
            //for(int i = 0; i < 100; i++)
            //{
            //    XYPoint xy = new XYPoint();
            //    xy.X = i;
            //    xy.Y = i % 2;
            //    d.Add(xy);
            //}



            //套用 data 波形，繪製在畫布上
            originalData.Append(data.XData, data.YData);*/

            chartTitle.Text = "波型圖標題";

            Task.Factory.StartNew(() =>
            {
                // Creates 8 dataseries with data on a background thread
                var dataSeries = new List<IDataSeries>();
                for (int i = 0; i < 6; i++)
                {
                    var ds = new XyDataSeries<double, double>() { SeriesName="CH "+i };
                    dataSeries.Add(ds);
                    var someData = DataManager.Instance.GetSinewave(i+1.0, 0.0, 1000, 4);

                    ds.Append(someData.XData, someData.YData);
                }

                // Creates 8 renderable series on the UI thread
                Dispatcher.BeginInvoke(new Action(() => CreateRenderableSeries(dataSeries)));
            });

            sciChart.ZoomExtents();
        }

        public static void addData(DoubleSeries data, double x, double y) {
            XYPoint xy = new XYPoint();
            xy.X = x;
            xy.Y = y;
            data.Add(xy);
        }

        private void CreateRenderableSeries(List<IDataSeries> result)
        {
            // Batch updates with one redraw
            using (sciChart.SuspendUpdates())
            {
                for (int i = 0; i < 6; i++)
                {
                    sciChart.RenderableSeries[i].DataSeries = result[i];
                }
            }
        }
    }
}
