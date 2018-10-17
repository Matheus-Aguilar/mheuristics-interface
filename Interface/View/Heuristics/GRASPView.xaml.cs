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
using System.ComponentModel;

namespace Interface.View
{
    /// <summary>
    /// Interaction logic for GRASP.xaml
    /// </summary>
    public partial class GRASPView : UserControl, INotifyPropertyChanged
    {
        public HeuristicsBase Heuristics;
        public MainWindow mainWindow;

        public double alfaGrasp { get; set; }
        public int numIteracoesLocal { get; set; }
        public int numIteracoesGuloso { get; set; }
        public bool opt1 { get { return opt == 1; } set { opt = 1; } }
        public bool opt2 { get { return opt == 2; } set { opt = 2; } }
        public bool cardinalidade { get { return tipo == 1; } set { tipo = 1; } }
        public bool valor { get { return tipo == 2; } set { tipo = 2; } }

        private int opt;
        private int tipo;

        public event PropertyChangedEventHandler PropertyChanged;

        public GRASPView()
        {
            DataContext = this;

            alfaGrasp = 0.05;
            numIteracoesLocal = 100;
            numIteracoesGuloso = 10;
            opt = 1;
            tipo = 2;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BackgroundWorker bkw = new BackgroundWorker();

            mainWindow.StartHeuristic();

            mainWindow.Heuristic = new GRASP(alfaGrasp, numIteracoesLocal, numIteracoesGuloso - 1, opt, tipo);

            var watch = System.Diagnostics.Stopwatch.StartNew();

            bkw.DoWork += (_, __) =>
            {
                mainWindow.Heuristic.Run();
            };

            bkw.RunWorkerCompleted += (_, __) =>
            {
                watch.Stop();

                mainWindow.Results.ValorTempoExecucao = watch.Elapsed.ToString("c");
                mainWindow.Results.TempoExecucao = "Tempo de execução: " + mainWindow.Results.ValorTempoExecucao;

                mainWindow.EndHeuristic();
            };

            bkw.RunWorkerAsync();
        }
    }
}
