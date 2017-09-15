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

        public void SetData(List<Tuple<double, double, double, double, double>> data)
        {
            var plotFO = new OxyPlot.PlotModel();
            var plotRR = new OxyPlot.PlotModel();
            var plotRA = new OxyPlot.PlotModel();
            var plotRV = new OxyPlot.PlotModel();

            var plotFlorestaRegulada = new OxyPlot.PlotModel();

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

            plotFlorestaRegulada.Axes.Add(new LinearAxis
            {
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                Title = "Ano"
            });

            plotFO.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "Função objetivo"
            });

            plotRR.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "Restrição de regulação de área"
            });

            plotRA.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "Restrição de adjacencia"
            });

            plotRV.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "Restrição de volume"
            });

            plotFlorestaRegulada.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Bottom,
                Title = "Area (ha)"
            });

            var serieFO = new OxyPlot.Series.LineSeries();
            var serieRR = new OxyPlot.Series.LineSeries();
            var serieRA = new OxyPlot.Series.LineSeries();
            var serieRV = new OxyPlot.Series.LineSeries();

            var serieFlorestaRegula = new OxyPlot.Series.LineSeries();

            int count = 1;

            foreach (var tupla in data)
            {
                serieFO.Points.Add(new OxyPlot.DataPoint(count, tupla.Item2));
                serieRR.Points.Add(new OxyPlot.DataPoint(count, tupla.Item5));
                serieRA.Points.Add(new OxyPlot.DataPoint(count, tupla.Item4));
                serieRV.Points.Add(new OxyPlot.DataPoint(count, tupla.Item3));

                count++;
            }

            /*var sol = ga.solucao;

            serieFlorestaRegula.Points.Add(new OxyPlot.DataPoint(1, sol.Select((p, idx) => Heuristics.HeuristicsBase.mRegArea[idx, p, 0]).Sum()));
            serieFlorestaRegula.Points.Add(new OxyPlot.DataPoint(2, sol.Select((p, idx) => Heuristics.HeuristicsBase.mRegArea[idx, p, 1]).Sum()));
            serieFlorestaRegula.Points.Add(new OxyPlot.DataPoint(3, sol.Select((p, idx) => Heuristics.HeuristicsBase.mRegArea[idx, p, 2]).Sum()));
            serieFlorestaRegula.Points.Add(new OxyPlot.DataPoint(4, sol.Select((p, idx) => Heuristics.HeuristicsBase.mRegArea[idx, p, 3]).Sum()));
            serieFlorestaRegula.Points.Add(new OxyPlot.DataPoint(5, sol.Select((p, idx) => Heuristics.HeuristicsBase.mRegArea[idx, p, 4]).Sum()));
            serieFlorestaRegula.Points.Add(new OxyPlot.DataPoint(6, sol.Select((p, idx) => Heuristics.HeuristicsBase.mRegArea[idx, p, 5]).Sum()));*/

            plotFO.Series.Add(serieFO);
            plotRR.Series.Add(serieRR);
            plotRA.Series.Add(serieRA);
            plotRV.Series.Add(serieRV);

            plot_interface_1.Model = plotFO;
            plot_interface_2.Model = plotRR;
            plot_interface_3.Model = plotRA;
            plot_interface_4.Model = plotRV;
        }
    }
}
