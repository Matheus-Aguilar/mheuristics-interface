using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
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
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : UserControl
    {
        public Details()
        {
            InitializeComponent();
        }

        public void SetData(int[] solucao)
        {
            FlorestaRegulada(solucao);
            VolumeCortado(solucao);
            Adjacencia(solucao);
            VPL(solucao);
        }

        private void VPL(int[] solucao)
        {
            var plotVPL = new OxyPlot.PlotModel();

            plotVPL.Axes.Add(new LinearAxis
            {
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                Title = "Ano",
                Minimum = 0,
                Maximum = Heuristics.HeuristicsBase.h + 1
            });

            plotVPL.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "Valor",
                Minimum = 0,
                MaximumPadding = 0.1
            });

            var serieVPL = new OxyPlot.Series.LineSeries { Title = "VPL", StrokeThickness = 2, CanTrackerInterpolatePoints = false };

            double[] valores = new double[Heuristics.HeuristicsBase.h];

            for (int i = 0; i < Heuristics.HeuristicsBase.n; i++)
                for (int k = 0; k < Heuristics.HeuristicsBase.h; k++)
                    if (Heuristics.HeuristicsBase.mCorte[i, solucao[i], k])
                        valores[k] += Heuristics.HeuristicsBase.mVPL[i, solucao[i]];

            for (var i = 0; i < Heuristics.HeuristicsBase.h; i++)
                serieVPL.Points.Add(new OxyPlot.DataPoint(i + 1, valores[i]));

            plotVPL.Series.Add(serieVPL);

            plot_interface_4.Model = plotVPL;
        }

        private void Adjacencia(int[] solucao)
        {
            var plotAdjacencia = new OxyPlot.PlotModel();

            plotAdjacencia.Axes.Add(new LinearAxis
            {
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                Title = "Ano",
                Minimum = 0,
                Maximum = Heuristics.HeuristicsBase.h + 1
            });

            plotAdjacencia.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "Area (ha)",
                Minimum = 0,
                MinimumPadding = 0.1,
                MaximumPadding = 0.1
            });

            double[] valores = new double[Heuristics.HeuristicsBase.h];

            /*
            var serieAdjacencia = new OxyPlot.Series.LineSeries { Title = "Quebra de adjacência", StrokeThickness = 2, Color = OxyPlot.OxyColor.FromRgb(0, 0, 255), CanTrackerInterpolatePoints = false };
            
            for (int i = 0; i < Heuristics.HeuristicsBase.n; i++)
                for (int k = 0; k < Heuristics.HeuristicsBase.h; k++)
                    if (Heuristics.HeuristicsBase.mCorte[i, solucao[i], k])
                        foreach (int vizinho in Heuristics.HeuristicsBase.talhoes[i].vizinhos)
                            if (Heuristics.HeuristicsBase.mCorte[vizinho, solucao[vizinho], k])
                            {
                                valores[k] += Heuristics.HeuristicsBase.talhoes[i].area;

                                break;
                            }

            for (var i = 0; i < Heuristics.HeuristicsBase.h; i++)
                serieAdjacencia.Points.Add(new OxyPlot.DataPoint(i + 1, valores[i]));
            
            plotAdjacencia.Series.Add(serieAdjacencia);
            */

            var serieIAC = new OxyPlot.Series.LineSeries { Title = "IAC por ano", StrokeThickness = 2, CanTrackerInterpolatePoints = false };

            double IAC, area2, distMaisProximo, areasCortadas;

            for (int i = 0; i < Heuristics.HeuristicsBase.h; i++)
            {
                IAC = areasCortadas = 0;

                for (int j = 0; j < Heuristics.HeuristicsBase.n; j++)
                    if (Heuristics.HeuristicsBase.mCorte[j, solucao[j], i])
                    {
                        areasCortadas += Heuristics.HeuristicsBase.talhoes[j].area * Heuristics.HeuristicsBase.talhoes[j].area;
                        area2 = Heuristics.HeuristicsBase.talhoes[j].area * Heuristics.HeuristicsBase.talhoes[j].area;

                        distMaisProximo = Double.PositiveInfinity;

                        for (int k = 0; k < Heuristics.HeuristicsBase.n; k++)
                            if (k != j && Heuristics.HeuristicsBase.mCorte[k, solucao[k], i])
                                distMaisProximo = Math.Min(distMaisProximo, Heuristics.HeuristicsBase.mDistancia[j, k]);

                        distMaisProximo /= 1000;

                        IAC += area2 / (distMaisProximo * distMaisProximo);
                    }

                valores[i] = IAC / areasCortadas;
            }

            for (var i = 0; i < Heuristics.HeuristicsBase.h; i++)
                serieIAC.Points.Add(new OxyPlot.DataPoint(i + 1, valores[i]));

            plotAdjacencia.Series.Add(serieIAC);

            plot_interface_3.Model = plotAdjacencia;
        }

        private void VolumeCortado(int[] solucao)
        {
            var plotVolumeCortado = new OxyPlot.PlotModel();

            plotVolumeCortado.Axes.Add(new LinearAxis
            {
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                Title = "Ano",
                Minimum = 0,
                Maximum = Heuristics.HeuristicsBase.h + 1
            });

            plotVolumeCortado.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "Volume (m³/ha)",
                MinimumPadding = 0.1,
                MaximumPadding = 0.1
            });

            var serieVolumeCortado = new OxyPlot.Series.LineSeries { Title = "Volume cortado", StrokeThickness = 2, CanTrackerInterpolatePoints = false };
            var serieMinVolumeCortado = new OxyPlot.Series.LineSeries { Selectable = false, Title = "Mínimo", Color = OxyPlot.OxyColor.FromRgb(255, 0, 0), CanTrackerInterpolatePoints = false };
            var serieMaxVolumeCortado = new OxyPlot.Series.LineSeries { Title = "Máximo", Color = OxyPlot.OxyColor.FromRgb(255, 0, 0), CanTrackerInterpolatePoints = false };

            for (var i = 0; i < Heuristics.HeuristicsBase.h; i++)
            {
                serieMinVolumeCortado.Points.Add(new OxyPlot.DataPoint(i + 1, Heuristics.HeuristicsBase.volMin));
                serieMaxVolumeCortado.Points.Add(new OxyPlot.DataPoint(i + 1, Heuristics.HeuristicsBase.volMax));
                serieVolumeCortado.Points.Add(new OxyPlot.DataPoint(i + 1, solucao.Select((p, idx) => Heuristics.HeuristicsBase.mVolume[idx, p, i]).Sum()));
            }

            plotVolumeCortado.Series.Add(serieVolumeCortado);
            plotVolumeCortado.Series.Add(serieMinVolumeCortado);
            plotVolumeCortado.Series.Add(serieMaxVolumeCortado);

            plot_interface_2.Model = plotVolumeCortado;
        }

        private void FlorestaRegulada(int[] solucao)
        {
            var plotFlorestaRegulada = new OxyPlot.PlotModel();

            plotFlorestaRegulada.Axes.Add(new LinearAxis
            {
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                Title = "Ano",
                Minimum = 0,
                Maximum = Heuristics.HeuristicsBase.r + 1
            });

            plotFlorestaRegulada.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "Area (ha)",
                MinimumPadding = 0.1,
                MaximumPadding = 0.1
            });

            var serieFlorestaRegulada = new OxyPlot.Series.LineSeries { Title = "Area de corte", StrokeThickness = 2, CanTrackerInterpolatePoints = false };
            var serieMinFlorestaRegulada = new OxyPlot.Series.LineSeries { Title = "Mínimo", Color = OxyPlot.OxyColor.FromRgb(255, 0, 0), CanTrackerInterpolatePoints = false };
            var serieMaxFlorestaRegulada = new OxyPlot.Series.LineSeries { Title = "Máximo", Color = OxyPlot.OxyColor.FromRgb(255, 0, 0), CanTrackerInterpolatePoints = false };

            for (var i = 0; i < Heuristics.HeuristicsBase.r; i++)
            {
                serieMinFlorestaRegulada.Points.Add(new OxyPlot.DataPoint(i + 1, Heuristics.HeuristicsBase.areaPorR_menosAlfaReg));
                serieMaxFlorestaRegulada.Points.Add(new OxyPlot.DataPoint(i + 1, Heuristics.HeuristicsBase.areaPorR_maisAlfaReg));
                serieFlorestaRegulada.Points.Add(new OxyPlot.DataPoint(i + 1, solucao.Select((p, idx) => Heuristics.HeuristicsBase.mRegArea[idx, p, i]).Sum()));
            }

            plotFlorestaRegulada.Series.Add(serieFlorestaRegulada);
            plotFlorestaRegulada.Series.Add(serieMinFlorestaRegulada);
            plotFlorestaRegulada.Series.Add(serieMaxFlorestaRegulada);

            plot_interface_1.Model = plotFlorestaRegulada;
        }
    }
}
