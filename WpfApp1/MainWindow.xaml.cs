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
        bool pubIsOpen;

        private int runPubSpeed;
        int bouncerSpeed;

        int groupOfPeople;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void B_PP_Click(object sender, RoutedEventArgs e)
        {
            if (B_PP.IsInitialized && patronList.Count > 0)
            {
                pubIsOpen = false;
                Reset();
            }

            if (patronList.Count == 0)
            {
                patronList.Add(new Patron());

                bouncerSpeed = runPubSpeed;

                pubIsOpen = true;
                running = true;

                bouncerIsWorking = true;
                groupDone = true;
                temp = 0;

                StartButtons();
                Start();
            }

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

            Start();
        }

        int temp;
        bool groupDone;

        public async void Bouncer()
        {
            await Task.Run(async () => { while (!bouncerIsWorking) { await Task.Delay(1); } });
            await Task.Delay(bouncerSpeed + runPubSpeed);

            if (temp == 0 && groupDone && pubIsOpen)
            {
                await Task.Run(async () => { while (!bouncerIsWorking) { await Task.Delay(1); } });
                await Task.Delay(bouncerSpeed + runPubSpeed);

                groupOfPeople = random.Next(0, 14);
                temp = groupOfPeople;

                await Task.Run(async () => { while (!bouncerIsWorking) { await Task.Delay(1); } });

                if (groupOfPeople > 0 && pubIsOpen)
                {
                    L_Bouncer.Items.Insert(0, $"Letting {groupOfPeople} guest(s) in");
                    groupDone = false;
                }

                await Task.Delay(bouncerSpeed + runPubSpeed);
            }

            await Task.Delay(bouncerSpeed + runPubSpeed);
            await Task.Run(async () => { while (!bouncerIsWorking) { await Task.Delay(1); } });

            if (!groupDone && pubIsOpen)
            {
                await Task.Run(async () => { while (!bouncerIsWorking) { await Task.Delay(1); } });

                for (int i = 0; i < groupOfPeople && pubIsOpen; i++)
                {
                    if (temp <= 0)
                    {
                        temp = 0;
                        groupDone = true;
                        break;
                    }

                    if (temp > 0)
                    {
                        await Task.Run(async () => { while (!bouncerIsWorking) { await Task.Delay(1); } });
                        L_Guest.Items.Insert(0, patronList[0].Bouncer() + " has entered");
                        if (Debug) { Log.Items.Insert(0, $"Bouncer forloop: {temp}"); }
                        temp--;
                    }

                    if (temp <= 0)
                    {
                        temp = 0;
                        groupDone = true;
                        break;
                    }

                    await Task.Delay(bouncerSpeed + runPubSpeed);
                }
            }
            else
            {
                if (pubIsOpen)
                {
                    L_Bouncer.Items.Insert(0, "Wating for guest");
                }

                await Task.Delay(bouncerSpeed + runPubSpeed);
            }

            if (temp <= 0 && pubIsOpen)
            {
                temp = 0;
                groupDone = true;
            }

            if (Debug) { Log.Items.Insert(0, $"Bouncer final value: {temp}"); }

            if (pubIsOpen && temp == 0) Bouncer();
        }

        int tempIndex;
        private bool Debug;
        private bool running;

        public async void Bartender()
        {
            tempIndex = patronList[0].GetListSize();
            Log.Items.Insert(0, "bartender method");
        }

        public async void Start()
        {
            if(pubIsOpen)Bouncer();
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

            B_PP.Content = "Stop Everything";
        }

        public void Reset()
        {
            B_Bartender.IsEnabled = false;
            B_Bartender.Content = "Pause";

            B_Bouncer.IsEnabled = false;
            B_Bouncer.Content = "Pause";

            B_Guest.IsEnabled = false;
            B_Guest.Content = "Pause";

            B_Waitress.IsEnabled = false;
            B_Waitress.Content = "Pause";

            B_PP.IsEnabled = false;

            Debugger.IsEnabled = false;
        }

        private void Debugger_Click(object sender, RoutedEventArgs e)
        {
            HelpL.Content = "";
            if (Debugger.Content.ToString() == "Debug Off")
            {
                Debugger.Content = "Debug On";
                Debug = true;
            }
            else
            {
                Debugger.Content = "Debug Off";
                Debug = false;
            }
        }

        private void S_Speed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            runPubSpeed = (int)S_Speed.Value * 200;
        }
    }
}

