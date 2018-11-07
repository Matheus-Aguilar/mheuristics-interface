using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for GeneticAlgorithm.xaml
    /// </summary>
    public partial class SimulatedAnnealingView : UserControl, INotifyPropertyChanged
    {
        public HeuristicsBase Heuristics;
        public MainWindow mainWindow;

        public double t { get; set; }
        public double tf { get; set; }
        public double taxaResf { get; set; }
        public int contIteracao { get; set; }
        public bool opt1 { get { return opt == 1; } set { opt = 1; } }
        public bool opt2 { get { return opt == 2; } set { opt = 2; } }

        private int opt;

        public event PropertyChangedEventHandler PropertyChanged;

        public SimulatedAnnealingView()
        {
            DataContext = this;

            t = 100000;
            tf = 1000;
            taxaResf = 0.95;
            contIteracao = 100;
            opt = 2;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.HeuristicsView.parametrosHeuristica.Add("Têmpera Simulada"); //Nome
            mainWindow.HeuristicsView.parametrosHeuristica.Add("Temparatura Inicial");
            mainWindow.HeuristicsView.parametrosHeuristica.Add(t.ToString());
            mainWindow.HeuristicsView.parametrosHeuristica.Add("Temperatura Final");
            mainWindow.HeuristicsView.parametrosHeuristica.Add(tf.ToString());
            mainWindow.HeuristicsView.parametrosHeuristica.Add("Taxa de Resfriamento");
            mainWindow.HeuristicsView.parametrosHeuristica.Add(taxaResf.ToString());
            mainWindow.HeuristicsView.parametrosHeuristica.Add("Número de Iterações");
            mainWindow.HeuristicsView.parametrosHeuristica.Add(contIteracao.ToString());
            mainWindow.HeuristicsView.parametrosHeuristica.Add("Tipo");
            mainWindow.HeuristicsView.parametrosHeuristica.Add(opt == 1 ? "1-Opt" : "2-Opt");

            BackgroundWorker bkw = new BackgroundWorker();

            mainWindow.StartHeuristic();

            mainWindow.Heuristic = new SimulatedAnnealing(t, tf, taxaResf, contIteracao, opt);
            
            var watch = System.Diagnostics.Stopwatch.StartNew();

            bkw.DoWork += (_, __) =>
            {
                mainWindow.Heuristic.Run();
            };

            bkw.RunWorkerCompleted += (_, __) =>
            {
                watch.Stop();

                mainWindow.Results.ValorTempoExecucao = watch.Elapsed;
                mainWindow.Results.TempoExecucao = "Tempo de execução: " + watch.Elapsed.ToString(@"hh\:mm\:ss");

                mainWindow.EndHeuristic();
            };

            bkw.RunWorkerAsync();
        }
    }
}
