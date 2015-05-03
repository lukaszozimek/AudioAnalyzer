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
using AudioAnalyzer.Client.ViewModel;

namespace AudioAnalyzer.Client
{
    public partial class MainWindow : Window
    {
        ControlPanelViewModel viewModel;
       // LiveViewModel _model;
       //this.DataContext = _model;
        public MainWindow()

        {
            InitializeComponent();



            viewModel = new ControlPanelViewModel(this.waveForm, this.analyzer);
            this.controlPanel.DataContext = viewModel;
        }

        protected override void OnClosed(EventArgs e)
        {
            LiveViewModel.StopSubscribig();
        }
    }
}
