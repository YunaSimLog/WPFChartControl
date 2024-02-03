using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;

namespace WPFChartControl.Commons
{
    public class OxyPlotManager
    {
        private readonly PlotModel _plotModel = new PlotModel();
        private IList<OxyColor>? _oxyColors;
        private int _oxyColorIndex;

        public OxyPlotManager(string title)
        {
            _plotModel = new PlotModel{Title = title};
        }

        public PlotModel PlotModel => _plotModel;

        /// <summary>
        /// X축 생성
        /// </summary>
        /// <param name="title"></param>
        /// <param name="stringFormat"></param>
        public void SetDataTimeAxisX(string title, string stringFormat)
        {
            PlotModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = title,
                IntervalType = DateTimeIntervalType.Months,
                StringFormat = stringFormat,
                MajorGridlineStyle = LineStyle.Solid,
            });
        }

        /// <summary>
        /// Y축 생성
        /// </summary>
        /// <param name="title"></param>
        public void SetAxisY(string title)
        {
            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = title,
            });
        }

        /// <summary>
        /// Legend 설정
        /// </summary>
        public void SetLegend()
        {
            // Legend 추가
            var legend = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.RightTop,
                LegendOrientation = LegendOrientation.Vertical,
            };

            PlotModel.Legends.Add(legend);
        }

        public void SetOxyColors(int count)
        {
            _oxyColors = OxyPalettes.HueDistinct(count).Colors;
        }

        public void SetNextColor()
        {
            _oxyColorIndex = _oxyColors?.Count == _oxyColorIndex ? 0 : ++_oxyColorIndex;
        }

        public void AddLineSeriesDataPoints(string title, IEnumerable<DataPoint> dataPoints)
        {
            var color = _oxyColors == null ? OxyColors.Blue : _oxyColors[_oxyColorIndex];
            var lineSeries = new LineSeries
            {
                Title = title,
                Color = color,
                MarkerStroke = color,
                StrokeThickness = 2,
                MarkerType = MarkerType.Circle,
                MarkerSize = 4
            };

            lineSeries.Points.AddRange(dataPoints);

            PlotModel.Series.Add(lineSeries);
        }
    }
}
