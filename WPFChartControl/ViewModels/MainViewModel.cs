using System;
using System.CodeDom;
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
using WPFChartControl.Commons;
using WPFChartControl.Models;

namespace WPFChartControl.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
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
            SetPlotModel(subject,scoreFunc);
        }

        private void SetPlotModel(string subject, Func<TestScore,int> testScoreFunc)
        {
            IEnumerable<StudentWithScore> data = StudentWithScore.GetSeedDatas();

            // PlotModel 생성
            var plotManager = new OxyPlotManager($"{subject} 점수");

            // X축 생성
            plotManager.SetDataTimeAxisX("일자", "yyyy-MM-dd");

            // Y축 생성
            plotManager.SetAxisY("점수");

            // Legend 추가
            plotManager.SetLegend();

            // 데이터 추가
            var studentGroup = data.GroupBy(x => x.Student);

            // 학생별 색상 추가
            plotManager.SetOxyColors(studentGroup.Count());

            foreach (var studentData in studentGroup)
            {
                var dataPoint = studentData.Select(x =>
                    new DataPoint(DateTimeAxis.ToDouble(x.Score.Date), testScoreFunc(x.Score)));
                plotManager.AddLineSeriesDataPoints(studentData.Key.Name, dataPoint);
                plotManager.SetNextColor();
            }

            this.PlotModel = plotManager.PlotModel;

            OnPropertyChanged(nameof(PlotModel));
        }

        public PlotModel PlotModel { get; set; }
        public ICommand LoadOxyPlotCommand => new RelayCommand<string>(LoadOxyPlot);
    }
}
