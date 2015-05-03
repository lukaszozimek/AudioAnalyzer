using AudioAnalyzer.Client.ViewModel;
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
using System.Windows.Shapes;



namespace AudioAnalyzer.Client.View
{
    /// <summary>
    /// Interaction logic for SpectrumView.xaml
    /// </summary>
    public partial class SpectrumView : Window
    {
        ControlPanelViewModel viewModel;
        public SpectrumView()
        {
            InitializeComponent();

            viewModel = new ControlPanelViewModel(this.waveForm, this.analyzer);
            this.controlPanel.DataContext = viewModel;           
        }
    }
}
