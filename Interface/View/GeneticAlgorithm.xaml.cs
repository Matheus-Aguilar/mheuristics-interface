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

namespace Interface.View
{
    /// <summary>
    /// Interaction logic for GeneticAlgorithm.xaml
    /// </summary>
    public partial class GeneticAlgorithm : UserControl
    {
        public Heuristics.HeuristicsBase Heuristics;
        public MainWindow mainWindow;

        public GeneticAlgorithm()
        {
            InitializeComponent();
        }

        private void StartHeuristics(int populacaoInicial, double taxaCruzamento, double taxaMutacao, int numIteracoes)
        {
            BackgroundWorker bkw = new BackgroundWorker();

            mainWindow.StartHeuristic();

            mainWindow.Heuristic = new Heuristics.GeneticAlgorithm(populacaoInicial, taxaCruzamento, taxaMutacao, numIteracoes);

            bkw.DoWork += (sender, e) =>
            {
                mainWindow.Heuristic.Run();
            };

            //plotFlorestaRegulada.Series.Add(serieFlorestaRegula);

            bkw.RunWorkerCompleted += (sender, e) =>
            {
                mainWindow.EndHeuristic();
            };

            bkw.RunWorkerAsync();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow) Window.GetWindow(this);

            StartHeuristics(Convert.ToInt32(populacaoInicial.Text), Convert.ToDouble(taxaCruzamento.Text),
                Convert.ToDouble(taxaMutacao.Text), Convert.ToInt32(numIteracoes.Text));
        }
    }
}
