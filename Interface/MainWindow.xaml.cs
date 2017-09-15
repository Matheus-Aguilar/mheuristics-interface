using MahApps.Metro.Controls;
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
    public partial class MainWindow : MetroWindow
    {
        public event EventHandler HeuristicStarted;
        public event EventHandler HeuristicEnded;
        public Heuristics.HeuristicsBase Heuristic;
        
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

            HeuristicStarted += MainWindow_HeuristicStarted;

            HeuristicEnded += MainWindow_HeuristicEnded;
            
            InitializeComponent();

            GeneticAlgorithm.mainWindow = this;
            GeneticAlgorithm.Heuristics = Heuristic;
        }

        public void StartHeuristic()
        {
            HeuristicStarted?.Invoke(this, null);
        }

        public void EndHeuristic()
        {
            HeuristicEnded?.Invoke(this, null);
        }
        
        private void MainWindow_HeuristicStarted(object sender, EventArgs e)
        {
            step1.Visibility = Visibility.Hidden;
            step2.Visibility = Visibility.Visible;
        }

        private void MainWindow_HeuristicEnded(object sender, EventArgs e)
        {
            Results.Overall.SetData(Heuristic.Iteracoes);

            step2.Visibility = Visibility.Hidden;
            step3.Visibility = Visibility.Visible;
        }
    }
}
