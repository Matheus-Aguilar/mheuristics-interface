﻿using System;
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
    public partial class VNSView : UserControl, INotifyPropertyChanged
    {
        public HeuristicsBase Heuristics;
        public MainWindow mainWindow;

        public int contIteracao { get; set; }

        private int opt;

        public event PropertyChangedEventHandler PropertyChanged;

        public VNSView()
        {
            DataContext = this;

            contIteracao = 100;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.HeuristicsView.parametrosHeuristica.Add("VNS"); //Nome
            mainWindow.HeuristicsView.parametrosHeuristica.Add("Número de Iterações");
            mainWindow.HeuristicsView.parametrosHeuristica.Add(contIteracao.ToString());

            BackgroundWorker bkw = new BackgroundWorker();

            mainWindow.StartHeuristic();

            mainWindow.Heuristic = new VNS(contIteracao);

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
