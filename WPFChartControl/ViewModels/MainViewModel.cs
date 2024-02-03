using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using WpfBase;
using WPFChartControl.Models;

namespace WPFChartControl.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            SetPlotModel();
        }

        Func<TestScore, int> GetScoreFunc(string subject)
        {
            switch (subject)
            {
                case "국어":
                    return x => x.KorScore;

                case "영어":
                    return x => x.EngScore;

                case "수학":
                    return x => x.MathScore;
            }

            return default;
        }

        private void LoadOxyPlot(string subject)
        {
            Func<TestScore, int> scoreFunc = GetScoreFunc(subject);
            //SetPlotModel(subject, scoreFunc);
        }

        private void SetPlotModel(/*string subject, Func<TestScroe, int> testScoreFunc*/)
        {
            IEnumerable<StudentWithScore> data = StudentWithScore.GetSeedDatas();

            // PlotModel 생성
            PlotModel = new PlotModel() {Title = "국어 점수"};

            // X축 생성
            PlotModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "일자",
                StringFormat ="yyyy-MM-dd",
                MajorGridlineStyle = LineStyle.Solid,
            });

            // Y축 생성
            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "점수",
                Minimum = 0,
                Maximum = 100,
            });

            // Legend 추가
            var legend = new Legend
            {
                LegendPlacement = LegendPlacement.Outside,
                LegendPosition = LegendPosition.RightTop,
                LegendOrientation = LegendOrientation.Vertical,
            };

            PlotModel.Legends.Add(legend);

            // 데이터 추가
            var studentGroup = data.GroupBy(x => x.Student);
            var oxyColors = OxyPalettes.HueDistinct(studentGroup.Count()).Colors;
            int oxyColorIndex = 0;

            foreach (var studentData in studentGroup)
            {
                var color = oxyColors[oxyColorIndex];
                var lineSeries = new LineSeries
                {
                    Title = studentData.Key.Name,
                    Color = color,
                    MarkerStroke = color,
                    StrokeThickness = 2,
                    MarkerType = MarkerType.Circle,
                    MarkerSize = 4
                };
                oxyColorIndex++;

                foreach (var std in studentData)
                {
                    lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(std.Score.Date), std.Score.KorScore));
                }

                PlotModel.Series.Add(lineSeries);
            }

            this.PlotModel = PlotModel;
        }

        public PlotModel PlotModel { get; set; }
        public ICommand LoadOxyPlotCommand => new RelayCommand<string>(LoadOxyPlot);
    }
}
