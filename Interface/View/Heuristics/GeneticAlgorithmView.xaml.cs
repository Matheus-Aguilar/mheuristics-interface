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
    public partial class GeneticAlgorithmView : UserControl, INotifyPropertyChanged
    {
        public HeuristicsBase Heuristics;
        public MainWindow mainWindow;
        
        public int populacaoInicial { get; set; }
        public double taxaCruzamento { get; set; }
        public double taxaMutacao { get; set; }
        public int numIteracoes { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public GeneticAlgorithmView()
        {
            DataContext = this;

            populacaoInicial = 20;
            taxaCruzamento = 0.5;
            taxaMutacao = 0.05;
            numIteracoes = 400;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bkw = new BackgroundWorker();

            mainWindow.StartHeuristic();

            mainWindow.Heuristic = new GeneticAlgorithm(this.populacaoInicial, taxaCruzamento, taxaMutacao, numIteracoes);
            
            var watch = System.Diagnostics.Stopwatch.StartNew();

            bkw.DoWork += (_, __) =>
            {
                mainWindow.Heuristic.Run();
            };

            bkw.RunWorkerCompleted += (_, __) =>
            {
                watch.Stop();

                mainWindow.Results.TempoExecucao = "Tempo de execução: " + watch.Elapsed.ToString("c");

                mainWindow.EndHeuristic();
            };

            bkw.RunWorkerAsync();
        }
    }
}
