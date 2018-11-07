using MahApps.Metro;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using OxyPlot.Axes;
using OxyPlot.Wpf;
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

        private void ExportResults_Click(object sender, RoutedEventArgs e)
        {
            string message;
            string caption;
            MessageBoxButton buttons;
            MessageBoxResult result;

            message = "Deseja exportar os dados para uma planilha do Excel?";
            caption = "Exportar Planilha";
            buttons = MessageBoxButton.YesNo;
            result = MessageBox.Show(message, caption, buttons);

            if (result == MessageBoxResult.Yes)
            {
                var dialog = new SaveFileDialog
                {
                    Filter = "Arquivos do Excel|*.xlsx|Todos os arquivos|*.*"
                };

                dialog.FileOk += (_, __) =>
                {
                    Cursor = Cursors.Wait;

                    ExportData.ExportResults(dialog.FileName, Heuristic.solucao, Results.ValorTempoExecucao, HeuristicsView.parametrosHeuristica);

                    Cursor = Cursors.Arrow;
                };

                dialog.ShowDialog();
            }

            message = "Deseja exportar as imagens dos gráficos para uma pasta?";
            caption = "Exportar Gráficos";
            buttons = MessageBoxButton.YesNo;
            result = MessageBox.Show(message, caption, buttons);

            if (result == MessageBoxResult.No)
                return;

            string folderName;
            string fileName;

            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            var folderResult = folderDialog.ShowDialog();

            if (folderResult != System.Windows.Forms.DialogResult.OK || string.IsNullOrWhiteSpace(folderDialog.SelectedPath))
                return;

            folderName = folderDialog.SelectedPath;

            var pngExporter1 = new PngExporter { Width = 900, Height = 600, Background = OxyPlot.OxyColor.FromRgb(255, 255, 255) };
            fileName = folderName + @"\area.png";
            pngExporter1.ExportToFile(Results.Details.plot_interface_1.Model, fileName);

            var pngExporter2 = new PngExporter { Width = 900, Height = 600, Background = OxyPlot.OxyColor.FromRgb(255, 255, 255) };
            fileName = folderName + @"\volume.png";
            pngExporter2.ExportToFile(Results.Details.plot_interface_2.Model, fileName);

            var pngExporter3 = new PngExporter { Width = 900, Height = 600, Background = OxyPlot.OxyColor.FromRgb(255, 255, 255) };
            fileName = folderName + @"\adjacencia.png";
            pngExporter3.ExportToFile(Results.Details.plot_interface_3.Model, fileName);

            var pngExporter4 = new PngExporter { Width = 900, Height = 600, Background = OxyPlot.OxyColor.FromRgb(255, 255, 255) };
            fileName = folderName + @"\custos.png";
            pngExporter4.ExportToFile(Results.Details.plot_interface_4.Model, fileName);

            var pngExporter5 = new PngExporter { Width = 900, Height = 600, Background = OxyPlot.OxyColor.FromRgb(255, 255, 255) };
            fileName = folderName + @"\funcaoobjetivo.png";
            pngExporter5.ExportToFile(Results.Overall.plot_interface_1.Model, fileName);

            var pngExporter6 = new PngExporter { Width = 900, Height = 600, Background = OxyPlot.OxyColor.FromRgb(255, 255, 255) };
            fileName = folderName + @"\restricaoarea.png";
            pngExporter6.ExportToFile(Results.Overall.plot_interface_2.Model, fileName);

            var pngExporter7 = new PngExporter { Width = 900, Height = 600, Background = OxyPlot.OxyColor.FromRgb(255, 255, 255) };
            fileName = folderName + @"\iac.png";
            pngExporter7.ExportToFile(Results.Overall.plot_interface_3.Model, fileName);

            var pngExporter8 = new PngExporter { Width = 900, Height = 600, Background = OxyPlot.OxyColor.FromRgb(255, 255, 255) };
            fileName = folderName + @"\restricaovolume.png";
            pngExporter8.ExportToFile(Results.Overall.plot_interface_4.Model, fileName);

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            HeuristicsView.parametrosHeuristica.Clear();

            if (step0.Visibility == Visibility.Visible || step2.Visibility == Visibility.Visible)
                return;

            if(step1.Visibility == Visibility.Visible && !HeuristicsView.isOpen)
            {
                HeuristicsView.Reset();
                step1.Visibility = Visibility.Hidden;
                step0.Visibility = Visibility.Visible;
                return;
            }

            HeuristicsView.isOpen = false;
            step3.Visibility = Visibility.Hidden;
            HeuristicsView.Reset();
            step1.Visibility = Visibility.Visible;
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            string fileName = Environment.CurrentDirectory + @"\..\..\help.pdf";
            Process.Start(fileName);
        }

        private void LoadData_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
