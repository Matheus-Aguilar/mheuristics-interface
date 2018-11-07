using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Heuristics;

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
            Custos(solucao);
        }

        private void Custos(int[] solucao)
        {
            var plotCustos = new OxyPlot.PlotModel();

            var xAxis = new CategoryAxis
            {
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                Title = "Tipo de Custo",
                Minimum = -1,
                Maximum = 8
            };

            plotCustos.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "R$",
                Minimum = 0,
                MaximumPadding = 0.1
            });

            var serieCustos = new OxyPlot.Series.ColumnSeries { Title = "Custos Totais", StrokeThickness = 2, FillColor = OxyPlot.OxyColor.FromRgb(180, 180, 180) };
            serieCustos.LabelPlacement = OxyPlot.Series.LabelPlacement.Outside;
            serieCustos.LabelFormatString = "{0:.00}";


            double colheitaTotal = 0;
            double baldeioTotal = 0;
            double transporteTotal = 0;
            double silviculturaTotal = 0;
            double implantacaoTotal = 0;
            double anterioresTotal = 0;
            double VPLTotal = 0;
            double receitaTotal = 0;

            for(int i = 0; i < HeuristicsBase.n; i++)
            {
                colheitaTotal += HeuristicsBase.mColheita[i, solucao[i]] * HeuristicsBase.talhoes[i].area;
                baldeioTotal += HeuristicsBase.mBaldeio[i, solucao[i]] * HeuristicsBase.talhoes[i].area; ;
                transporteTotal += HeuristicsBase.mTransporte[i, solucao[i]] * HeuristicsBase.talhoes[i].area; ;
                silviculturaTotal += HeuristicsBase.mSilvicultura[i, solucao[i]] * HeuristicsBase.talhoes[i].area; ;
                implantacaoTotal += HeuristicsBase.mImplantacao[i, solucao[i]] * HeuristicsBase.talhoes[i].area; ;
                anterioresTotal += HeuristicsBase.mAnteriores[i, solucao[i]] * HeuristicsBase.talhoes[i].area;
                receitaTotal += HeuristicsBase.mReceita[i, solucao[i]] * HeuristicsBase.talhoes[i].area;
                VPLTotal += HeuristicsBase.mVPL[i, solucao[i]];
            }


            xAxis.ActualLabels.Add("Colheita\n(R$/10)");
            serieCustos.Items.Add(new OxyPlot.Series.ColumnItem(colheitaTotal/10));
            xAxis.ActualLabels.Add("Baldeio");
            serieCustos.Items.Add(new OxyPlot.Series.ColumnItem(baldeioTotal));
            xAxis.ActualLabels.Add("Transporte");
            serieCustos.Items.Add(new OxyPlot.Series.ColumnItem(transporteTotal));
            xAxis.ActualLabels.Add("Silvicultura");
            serieCustos.Items.Add(new OxyPlot.Series.ColumnItem(silviculturaTotal));
            xAxis.ActualLabels.Add("Implantação");
            serieCustos.Items.Add(new OxyPlot.Series.ColumnItem(implantacaoTotal));
            xAxis.ActualLabels.Add("   Custos\nAnteriores");
            serieCustos.Items.Add(new OxyPlot.Series.ColumnItem(anterioresTotal));
            xAxis.ActualLabels.Add("Receita Total\n   (R$/10)");
            serieCustos.Items.Add(new OxyPlot.Series.ColumnItem(receitaTotal/10));
            serieCustos.Items[6].Color = OxyPlot.OxyColor.FromRgb(30, 30, 30);
            xAxis.ActualLabels.Add("VPL");
            serieCustos.Items.Add(new OxyPlot.Series.ColumnItem(VPLTotal));
            serieCustos.Items[7].Color = OxyPlot.OxyColor.FromRgb(80, 80, 80);
            
            plotCustos.Axes.Add(xAxis);

            plotCustos.Series.Add(serieCustos);

            plot_interface_4.Model = plotCustos;
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
                Maximum = HeuristicsBase.h + 1
            });

            double[] valores = new double[HeuristicsBase.h];

            if (HeuristicsBase.areaAdjacencia)
            {
                plotAdjacencia.Axes.Add(new LinearAxis
                {
                    Key = "yAxis",
                    Position = AxisPosition.Left,
                    Title = "Area (ha)",
                    Minimum = 0,
                    MinimumPadding = 0.1,
                    MaximumPadding = 0.1
                });

                var serieAdjacencia = new OxyPlot.Series.LineSeries { Title = "Quebra de adjacência", StrokeThickness = 2, Color = OxyPlot.OxyColor.FromRgb(0, 0, 0), CanTrackerInterpolatePoints = false };

                for (int i = 0; i < HeuristicsBase.n; i++)
                    for (int k = 0; k < HeuristicsBase.h; k++)
                        if (HeuristicsBase.mCorte[i, solucao[i], k])
                            foreach (int vizinho in HeuristicsBase.talhoes[i].vizinhos)
                                if (HeuristicsBase.mCorte[vizinho, solucao[vizinho], k])
                                {
                                    valores[k] += HeuristicsBase.talhoes[i].area;

                                    break;
                                }

                for (var i = 0; i < HeuristicsBase.h; i++)
                    serieAdjacencia.Points.Add(new OxyPlot.DataPoint(i + 1, valores[i]));

                plotAdjacencia.Series.Add(serieAdjacencia);
            }
            else
            {
                plotAdjacencia.Axes.Add(new LinearAxis
                {
                    Key = "yAxis",
                    Position = AxisPosition.Left,
                    Title = "IAC",
                    Minimum = 0,
                    MinimumPadding = 0.1,
                    MaximumPadding = 0.1
                });

                var serieIAC = new OxyPlot.Series.LineSeries { Title = "IAC por ano", StrokeThickness = 2, CanTrackerInterpolatePoints = false, Color = OxyPlot.OxyColor.FromRgb(0, 0, 0) };

                double IAC, area2, distMaisProximo, areasCortadas;

                for (int i = 0; i < HeuristicsBase.h; i++)
                {
                    IAC = areasCortadas = 0;

                    for (int j = 0; j < HeuristicsBase.n; j++)
                        if (HeuristicsBase.mCorte[j, solucao[j], i])
                        {
                            areasCortadas += HeuristicsBase.talhoes[j].area * HeuristicsBase.talhoes[j].area;
                            area2 = HeuristicsBase.talhoes[j].area * HeuristicsBase.talhoes[j].area;

                            distMaisProximo = Double.PositiveInfinity;

                            for (int k = 0; k < HeuristicsBase.n; k++)
                                if (k != j && HeuristicsBase.mCorte[k, solucao[k], i])
                                    distMaisProximo = Math.Min(distMaisProximo, HeuristicsBase.mDistancia[j, k]);

                            distMaisProximo /= 1000;

                            IAC += area2 / (distMaisProximo * distMaisProximo);
                        }

                    valores[i] = IAC / areasCortadas;
                }

                for (var i = 0; i < HeuristicsBase.h; i++)
                    serieIAC.Points.Add(new OxyPlot.DataPoint(i + 1, valores[i]));

                plotAdjacencia.Series.Add(serieIAC);
            }

            plot_interface_3.Model = plotAdjacencia;
        }

        private void VolumeCortado(int[] solucao)
        {
            var plotVolumeCortado = new OxyPlot.PlotModel();

            var xAxis = new CategoryAxis
            {
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                Title = "Ano",
                Minimum = -1,
                Maximum = HeuristicsBase.h+1
            };

            plotVolumeCortado.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "Volume (m³)",
                Minimum = 0,
                MaximumPadding = 0.1
            });

            var serieVolumeCortado = new OxyPlot.Series.ColumnSeries { Title = "Volume cortado", StrokeThickness = 2, FillColor = OxyPlot.OxyColor.FromRgb(180, 180, 180) };
            var serieMinVolumeCortado = new OxyPlot.Series.LineSeries { Selectable = false, Title = "Mínimo", Color = OxyPlot.OxyColor.FromRgb(0, 0, 0), CanTrackerInterpolatePoints = false };
            var serieMaxVolumeCortado = new OxyPlot.Series.LineSeries { Title = "Máximo", Color = OxyPlot.OxyColor.FromRgb(0, 0, 0), CanTrackerInterpolatePoints = false };

            serieMinVolumeCortado.Points.Add(new OxyPlot.DataPoint(-1000, HeuristicsBase.volMin));
            serieMaxVolumeCortado.Points.Add(new OxyPlot.DataPoint(-1000, HeuristicsBase.volMax));
            serieMinVolumeCortado.Points.Add(new OxyPlot.DataPoint(1000, HeuristicsBase.volMin));
            serieMaxVolumeCortado.Points.Add(new OxyPlot.DataPoint(1000, HeuristicsBase.volMax));

            //Adicionando o volume médio ao gráfico
            xAxis.ActualLabels.Add("Md");
            serieVolumeCortado.Items.Add(new OxyPlot.Series.ColumnItem(HeuristicsBase.volumeMedio));
            serieVolumeCortado.Items[0].Color = OxyPlot.OxyColor.FromRgb(80, 80, 80);

            for (var i = 0; i < HeuristicsBase.h; i++)
            {
                //Adicionando o número do ano no eixo X do gráfico
                xAxis.ActualLabels.Add(i.ToString());
                serieVolumeCortado.Items.Add(new OxyPlot.Series.ColumnItem(solucao.Select((p, idx) => HeuristicsBase.mVolume[idx, p, i]).Sum()));
            }

            plotVolumeCortado.Axes.Add(xAxis);

            plotVolumeCortado.Series.Add(serieVolumeCortado);
            plotVolumeCortado.Series.Add(serieMinVolumeCortado);
            plotVolumeCortado.Series.Add(serieMaxVolumeCortado);

            plot_interface_2.Model = plotVolumeCortado;
        }

        private void FlorestaRegulada(int[] solucao)
        {
            var plotFlorestaRegulada = new OxyPlot.PlotModel();

            plotFlorestaRegulada.Axes.Add(new CategoryAxis
            {
                Key = "xAxis",
                Position = AxisPosition.Bottom,
                Title = "Ano",
                Minimum = -1,
                Maximum = HeuristicsBase.r + 1
            });

            plotFlorestaRegulada.Axes.Add(new LinearAxis
            {
                Key = "yAxis",
                Position = AxisPosition.Left,
                Title = "Area (ha)",
                Minimum = 0,
                MaximumPadding = 0.1
            });

            var serieFlorestaRegulada = new OxyPlot.Series.ColumnSeries { Title = "Area de corte", StrokeThickness = 2, FillColor = OxyPlot.OxyColor.FromRgb(180, 180, 180) };
            var serieMinFlorestaRegulada = new OxyPlot.Series.LineSeries { Title = "Mínimo", Color = OxyPlot.OxyColor.FromRgb(0, 0, 0), CanTrackerInterpolatePoints = false };
            var serieMaxFlorestaRegulada = new OxyPlot.Series.LineSeries { Title = "Máximo", Color = OxyPlot.OxyColor.FromRgb(0, 0, 0), CanTrackerInterpolatePoints = false };

            serieMinFlorestaRegulada.Points.Add(new OxyPlot.DataPoint(-1000, HeuristicsBase.areaPorR_menosAlfaReg));
            serieMaxFlorestaRegulada.Points.Add(new OxyPlot.DataPoint(-1000, HeuristicsBase.areaPorR_maisAlfaReg));
            serieMinFlorestaRegulada.Points.Add(new OxyPlot.DataPoint(1000, HeuristicsBase.areaPorR_menosAlfaReg));
            serieMaxFlorestaRegulada.Points.Add(new OxyPlot.DataPoint(1000, HeuristicsBase.areaPorR_maisAlfaReg));

            for (var i = 0; i < HeuristicsBase.r; i++)
                serieFlorestaRegulada.Items.Add(new OxyPlot.Series.ColumnItem(solucao.Select((p, idx) => HeuristicsBase.mRegArea[idx, p, i]).Sum()));

            plotFlorestaRegulada.Series.Add(serieFlorestaRegulada);
            plotFlorestaRegulada.Series.Add(serieMinFlorestaRegulada);
            plotFlorestaRegulada.Series.Add(serieMaxFlorestaRegulada);

            plot_interface_1.Model = plotFlorestaRegulada;
        }
    }
}
