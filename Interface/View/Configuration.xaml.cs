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
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double volumeMinimo { get; set; }
        public double volumeMaximo { get; set; }
        public double alfa { get; set; }
        public double beta { get; set; }
        public double gama { get; set; }
        public double areaMinima { get; set; }
        public double areaMaxima { get; set; }
        public bool isOpen { get; set; }

        public Configuration()
        {
            DataContext = this;

            volumeMinimo = Heuristics.HeuristicsBase.volMin;
            volumeMaximo = Heuristics.HeuristicsBase.volMax;
            alfa = Heuristics.HeuristicsBase.alfa;
            beta = Heuristics.HeuristicsBase.beta;
            gama = Heuristics.HeuristicsBase.gama;
            areaMinima = Heuristics.HeuristicsBase.areaPorR_menosAlfaReg;
            areaMaxima = Heuristics.HeuristicsBase.areaPorR_maisAlfaReg;
            
            isOpen = false;

            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Heuristics.HeuristicsBase.volMin = volumeMinimo;
            Heuristics.HeuristicsBase.volMax = volumeMaximo;
            Heuristics.HeuristicsBase.alfa = alfa;
            Heuristics.HeuristicsBase.beta = beta;
            Heuristics.HeuristicsBase.gama = gama;
            Heuristics.HeuristicsBase.areaPorR_menosAlfaReg = areaMinima;
            Heuristics.HeuristicsBase.areaPorR_maisAlfaReg = areaMaxima;

            isOpen = false;
        }
    }
}
