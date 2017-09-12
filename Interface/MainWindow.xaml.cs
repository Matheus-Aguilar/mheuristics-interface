using Microsoft.Win32;
using OxyPlot.Axes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Arquivos de Excel|*.xlsx|Todos os arquivos|*.*"
            };

            dialog.FileOk += (sender, args) =>
            {
                Cursor = Cursors.Wait;

                Heuristics.LoadClass.LoadXLSX(dialog.FileName);

                Cursor = Cursors.Arrow;
            };

            dialog.ShowDialog();
            
            InitializeComponent();
        }

        private void StartHeuristics(int populacaoInicial, double taxaCruzamento, double taxaMutacao, int numIteracoes)
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

            var serieFO = new OxyPlot.Series.LineSeries();
            var serieRR = new OxyPlot.Series.LineSeries();
            var serieRA = new OxyPlot.Series.LineSeries();
            var serieRV = new OxyPlot.Series.LineSeries();

            BackgroundWorker bkw = new BackgroundWorker();

            bkw.DoWork += (sender, e) =>
            {
                var ga = new Heuristics.GeneticAlgorithm(populacaoInicial, taxaCruzamento, taxaMutacao, numIteracoes);

                ga.Run();

                int count = 1;
                
                foreach(var tupla in ga.Iteracoes)
                {
                    serieFO.Points.Add(new OxyPlot.DataPoint(count, tupla.Item2));
                    serieRR.Points.Add(new OxyPlot.DataPoint(count, tupla.Item5));
                    serieRA.Points.Add(new OxyPlot.DataPoint(count, tupla.Item4));
                    serieRV.Points.Add(new OxyPlot.DataPoint(count, tupla.Item3));

                    count++;
                }
            };

            plotFO.Series.Add(serieFO);
            plotRR.Series.Add(serieRR);
            plotRA.Series.Add(serieRA);
            plotRV.Series.Add(serieRV);

            bkw.RunWorkerCompleted += (sender, e) =>
            {
                step2.Visibility = Visibility.Hidden;
                step3.Visibility = Visibility.Visible;

                plot_interface_1.Model = plotFO;
                plot_interface_2.Model = plotRR;
                plot_interface_3.Model = plotRA;
                plot_interface_4.Model = plotRV;
            };

            
            step1.Visibility = Visibility.Hidden;
            step2.Visibility = Visibility.Visible;
            bkw.RunWorkerAsync();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StartHeuristics(Convert.ToInt32(populacaoInicial.Text), Convert.ToDouble(taxaCruzamento.Text), 
                Convert.ToDouble(taxaMutacao.Text), Convert.ToInt32(numIteracoes.Text));
        }
    }
}
