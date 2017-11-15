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
        Random random = new Random();

        bool bouncerIsWorking;
        bool guestIsEntering;
        bool bartenderIsWorking;
        bool waitressIsWorking;

        int bouncerSpeed = 1000;

        int groupOfPeople;

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
            if (guestIsEntering)
            {
                guestIsEntering = false;
                Dispatcher.Invoke(() => { while (!guestIsEntering) { } });
                B_Guest.Content = "Play";

            }
            else
            {
                guestIsEntering = true;
                B_Guest.Content = "Pause";
            }
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
            }

            Bouncer();
        }

        int temp;
        public async void Bouncer()
        {
            if (temp == 0)
            {
                await Task.Run(async () => { for (int x = 0; !bouncerIsWorking; x++) { x--; await Task.Delay(1); } });
                groupOfPeople = random.Next(1, 14);
                temp = groupOfPeople;
                L_Bouncer.Items.Insert(0, $"Letting {groupOfPeople} guest(s) in");
            }
            
            await Task.Delay(bouncerSpeed);
            await Task.Run(async () => { for (int x = 0; !bouncerIsWorking; x++) { x--; await Task.Delay(1); } });

            Log.Items.Insert(0, $"temp 1: {temp}");
            Log.Items.Insert(0, $"GP size 1: {groupOfPeople}");

            await Task.Run(async () => { for (int x = 0; !bouncerIsWorking; x++) { x--; await Task.Delay(1); } });
            for (int i = 0; i < groupOfPeople && temp != 0; i++)
            {
                await Task.Run(async () => { for (int x = 0; !bouncerIsWorking; x++) { x--; await Task.Delay(1); } });
                if (temp != 0)
                {
                    await Task.Run(() => { for (int x = 0; !bouncerIsWorking; x++) { } });
                    L_Guest.Items.Insert(0, patronList[0].Bouncer());
                    temp--;
                    if (temp <= 0)
                    {
                        temp = 0;
                        break;
                    }
                }
                else
                {
                    await Task.Run(async () => { for (int x = 0; !bouncerIsWorking; x++) { x--; await Task.Delay(1); } });
                    i = groupOfPeople;
                }

                await Task.Delay(bouncerSpeed);
            }

            await Task.Run(async () => { for (int x = 0; !bouncerIsWorking; x++) { x--; await Task.Delay(1); } });
            Log.Items.Insert(0, $"temp 2: {temp}");
            Log.Items.Insert(0, $" ");

            await Task.Delay(bouncerSpeed);
            if (bouncerIsWorking && temp == 0)
            {
                await Task.Run(async () => { for (int x = 0; !bouncerIsWorking; x++) { x--; await Task.Delay(1); } });
                Bouncer();
            }
            await Task.Delay(bouncerSpeed);
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
