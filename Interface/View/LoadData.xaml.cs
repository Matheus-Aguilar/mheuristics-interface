using Heuristics;
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

namespace Interface.View
{
    /// <summary>
    /// Interaction logic for LoadData.xaml
    /// </summary>
    public partial class LoadData : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool DataLoaded { get; set; }

        public LoadData()
        {
            InitializeComponent();
        }

        private void LoadXLSX_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Arquivos de Excel|*.xlsx|Todos os arquivos|*.*"
            };

            dialog.FileOk += (_, __) =>
            {
                Cursor = Cursors.Wait;

                LoadClass.LoadXLSX(dialog.FileName);

                ((MainWindow)Application.Current.MainWindow).LoadedData();

                Cursor = Cursors.Arrow;
            };

            dialog.ShowDialog();
        }

        private void LoadJSON_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Arquivos JSON|*.json|Todos os arquivos|*.*"
            };

            dialog.FileOk += (_, __) =>
            {
                Cursor = Cursors.Wait;

                LoadClass.LoadJSON(dialog.FileName);

                ((MainWindow)Application.Current.MainWindow).LoadedData();

                Cursor = Cursors.Arrow;
            };

            dialog.ShowDialog();
        }
    }
}
