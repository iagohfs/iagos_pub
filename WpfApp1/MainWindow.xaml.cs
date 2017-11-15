using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Patron> patronList = new List<Patron>();

        bool bouncerIsWorking;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void B_PP_Click(object sender, RoutedEventArgs e)
        {
            if (patronList.Count == 0)
            {
                patronList.Add(new Patron());
            }
            bouncerIsWorking = true;
            B_PP.IsEnabled = false;

            Dispatcher.Invoke(() => Bouncer());

            StartButtons();
        }

        private void B_Guest_Click(object sender, RoutedEventArgs e)
        {

        }

        private void B_Bouncer_Click(object sender, RoutedEventArgs e)
        {
            if (bouncerIsWorking)
            {
                bouncerIsWorking = false;
                B_Bouncer.Content = "Play";
            }
            else
            {
                bouncerIsWorking = true;
                B_Bouncer.Content = "Pause";
                Bouncer();
            }

        }

        public async void Bouncer()
        {
            for (int i = 0; bouncerIsWorking && patronList[0].GetListSize() < 20; i++)
            {
                while (!bouncerIsWorking) { }
                await Task.Delay(500);
                L_Guest.Items.Insert(0, patronList[0].Bouncer());
            }

        }

        public void StartButtons()
        {
            B_Bartender.IsEnabled = true;
            B_Bartender.Content = "Pause";

            B_Bouncer.IsEnabled = true;
            B_Bouncer.Content = "Pause";

            B_Guest.IsEnabled = true;
            B_Guest.Content = "Pause";

            B_Waitress.IsEnabled = true;
            B_Waitress.Content = "Pause";

            B_PP.Content = "Pause All";
        }
    }
}
