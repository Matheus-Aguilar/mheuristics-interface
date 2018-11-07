using Microsoft.Win32;
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
    /// Interaction logic for LoadData.xaml
    /// </summary>
    public partial class HeuristicsView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public MainWindow mainWindow;
        public HeuristicsBase Heuristic;
        public bool isOpen = false;
        public List<string> parametrosHeuristica = new List<string>();

        public HeuristicsView()
        {
            InitializeComponent();
        }

        private void GA_Click(object sender, RoutedEventArgs e)
        {
            step1.Visibility = Visibility.Hidden;

            step2.Children.Add(new GeneticAlgorithmView { mainWindow = mainWindow, Heuristics = Heuristic });

            step2.Visibility = Visibility.Visible;

            isOpen = true;
        }

        private void SA_Click(object sender, RoutedEventArgs e)
        {
            step1.Visibility = Visibility.Hidden;

            step2.Children.Add(new SimulatedAnnealingView { mainWindow = mainWindow, Heuristics = Heuristic });

            step2.Visibility = Visibility.Visible;

            isOpen = true;
        }

        private void GRASP_Click(object sender, RoutedEventArgs e)
        {
            step1.Visibility = Visibility.Hidden;

            step2.Children.Add(new GRASPView { mainWindow = mainWindow, Heuristics = Heuristic });

            step2.Visibility = Visibility.Visible;

            isOpen = true;
        }

        private void VNS_Click(object sender, RoutedEventArgs e)
        {
            step1.Visibility = Visibility.Hidden;

            step2.Children.Add(new VNSView { mainWindow = mainWindow, Heuristics = Heuristic });

            step2.Visibility = Visibility.Visible;

            isOpen = true;
        }

        public void Reset()
        {
            step2.Children.Clear();

            step2.Visibility = Visibility.Hidden;

            step1.Visibility = Visibility.Visible;
        }
    }
}
