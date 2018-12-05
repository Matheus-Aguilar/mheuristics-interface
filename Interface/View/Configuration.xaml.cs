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
        public double alfaRegArea { get; set; }
        public double alfaRegVol { get; set; }
        public double betaRegVol { get; set; }
        public bool isOpen { get; set; }
        public int minimizar { get; set; }
        public int restricaoAdj { get; set; }
        public int greenUp { get; set; }

        public Configuration()
        {
            DataContext = this;
            
            isOpen = false;

            InitializeComponent();
        }

        public void Reload()
        {
            volumeMinimo = HeuristicsBase.volMin;
            volumeMaximo = HeuristicsBase.volMax;
            alfa = HeuristicsBase.alfa;
            beta = HeuristicsBase.beta;
            gama = HeuristicsBase.gama;
            alfaRegArea = HeuristicsBase.alfaRegArea;
            alfaRegVol = HeuristicsBase.alfaRegVol;
            betaRegVol = HeuristicsBase.betaRegVol;
            greenUp = HeuristicsBase.greenUp;

            restricaoAdj = HeuristicsBase.areaAdjacencia ? 1 : 0;
            minimizar = HeuristicsBase.minimizar ? 1 : 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            HeuristicsBase.volMin = volumeMinimo;
            HeuristicsBase.volMax = volumeMaximo;
            HeuristicsBase.alfa = alfa;
            HeuristicsBase.beta = beta;
            HeuristicsBase.gama = gama;
            HeuristicsBase.alfaRegArea = alfaRegArea;
            HeuristicsBase.alfaRegVol = alfaRegVol;
            HeuristicsBase.betaRegVol = betaRegVol;
            HeuristicsBase.areaPorR_menosAlfaReg = HeuristicsBase.areaPorR * (1 - alfaRegArea);
            HeuristicsBase.areaPorR_maisAlfaReg = HeuristicsBase.areaPorR * (1 + alfaRegArea);
            HeuristicsBase.areaAdjacencia = restricaoAdj == 1;
            HeuristicsBase.minimizar = minimizar == 1;
            HeuristicsBase.greenUp = greenUp;

            isOpen = false;
        }
    }
}
