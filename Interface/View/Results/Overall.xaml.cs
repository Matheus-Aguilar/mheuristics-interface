using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Heuristics;

namespace Interface.View
{
    /// <summary>
    /// Interaction logic for Overall.xaml
    /// </summary>
    public partial class Overall : UserControl
    {
        public Overall()
        {
            InitializeComponent();
        }

        public void SetData(List<Tuple<double, double, double, double, double, double>> data)
        {
            var plotFO = new OxyPlot.PlotModel();
            var plotRR = new OxyPlot.PlotModel();
            var plotRA = new OxyPlot.PlotModel();
            var plotRV = new OxyPlot.PlotModel();

            plotFO.Axes.Add(new LinearAxis
            {
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                Title = "Iterações"
            });

            plotRR.Axes.Add(new LinearAxis
            {
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                Title = "Iterações"
            });

            plotRA.Axes.Add(new LinearAxis
            {
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                Title = "Iterações"
            });

            plotRV.Axes.Add(new LinearAxis
            {
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                Title = "Iterações"
            });

            plotFO.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "Função objetivo (R$)"
            });

            plotRR.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "Restrição de regulação de área (ha)"
            });

            plotRA.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "IAC"
            });

            plotRV.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "Restrição de volume (m³)"
            });
            var serieFO = new OxyPlot.Series.LineSeries { CanTrackerInterpolatePoints = false, Color = OxyPlot.OxyColor.FromRgb(0, 0, 0) };
            var serieRR = new OxyPlot.Series.LineSeries { CanTrackerInterpolatePoints = false, Color = OxyPlot.OxyColor.FromRgb(0, 0, 0) };
            var serieRA = new OxyPlot.Series.LineSeries { CanTrackerInterpolatePoints = false, Color = OxyPlot.OxyColor.FromRgb(0, 0, 0) };
            var serieRV = new OxyPlot.Series.LineSeries { CanTrackerInterpolatePoints = false, Color = OxyPlot.OxyColor.FromRgb(0, 0, 0) };

            int count = 1;

            foreach (var tupla in data)
            {
                serieFO.Points.Add(new OxyPlot.DataPoint(count, tupla.Item2));
                serieRR.Points.Add(new OxyPlot.DataPoint(count, tupla.Item5));
                serieRA.Points.Add(new OxyPlot.DataPoint(count, tupla.Item4));
                serieRV.Points.Add(new OxyPlot.DataPoint(count, tupla.Item3 - tupla.Item6));

                count++;
            }
            
            plotFO.Series.Add(serieFO);
            plotRR.Series.Add(serieRR);
            plotRA.Series.Add(serieRA);
            plotRV.Series.Add(serieRV);

            plot_interface_1.Model = plotFO;
            plot_interface_2.Model = plotRR;
            plot_interface_3.Model = plotRA;
            plot_interface_4.Model = plotRV;

            HeuristicsBase.fObjetivoFinal = data[data.Count - 1].Item2;
            HeuristicsBase.rAdjFinal = data[data.Count - 1].Item5;
            HeuristicsBase.rAreaFinal = data[data.Count - 1].Item4;
            HeuristicsBase.rVolumeFinal = data[data.Count - 1].Item3 - data[data.Count - 1].Item6;
        }
    }
}
