using MahApps.Metro;
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
using Heuristics;

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private event EventHandler HeuristicStarted;
        private event EventHandler HeuristicEnded;
        private event EventHandler DataLoaded;
        public HeuristicsBase Heuristic;
        
        public MainWindow()
        {
            HeuristicStarted += MainWindow_HeuristicStarted;

            HeuristicEnded += MainWindow_HeuristicEnded;

            DataLoaded += MainWindow_DataLoaded;
            
            InitializeComponent();

            HeuristicsView.mainWindow = this;
            HeuristicsView.Heuristic = Heuristic;
        }

        private void MainWindow_DataLoaded(object sender, EventArgs e)
        {
            step0.Visibility = Visibility.Hidden;

            Configuration.Reload();

            step1.Visibility = Visibility.Visible;
        }

        public void LoadedData()
        {
            DataLoaded?.Invoke(this, null);
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
            Results.Details.SetData(Heuristic.solucao);

            WindowState = WindowState.Maximized;

            step2.Visibility = Visibility.Hidden;
            step3.Visibility = Visibility.Visible;
        }

        private void OpenConfigurations_Click(object sender, RoutedEventArgs e)
        {
            Configuration.isOpen = true;
        }

        private void ExportJSON_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Arquivos JSON|*.json|Todos os arquivos|*.*"
            };

            dialog.FileOk += (_, __) =>
            {
                Cursor = Cursors.Wait;

                ExportData.Export(dialog.FileName);

                Cursor = Cursors.Arrow;
            };

            dialog.ShowDialog();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            step3.Visibility = Visibility.Hidden;
            HeuristicsView.Reset();
            step1.Visibility = Visibility.Visible;
        }
    }
}
