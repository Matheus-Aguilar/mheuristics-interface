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
    /// Interaction logic for Results.xaml
    /// </summary>
    public partial class Results : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string TempoExecucao { get; set; }
        public TimeSpan ValorTempoExecucao { get; set; }

        public Results()
        {
            InitializeComponent();

            DataContext = this;
        }
    }
}
