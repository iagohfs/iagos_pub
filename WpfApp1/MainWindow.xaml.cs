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
        bool firstBartenderIndex;
        bool bartenderHasServed;

        bool pubIsOpen;
        bool groupDone;

        private bool Debug;

        private int runPubSpeed;
        private int bouncerSpeed;
        private int bartenderSpeed;
        private int guestSpeed;

        int cleanGlasses;
        int chairs;
        int nrOfGuestServed;
        int dirtyGlasses;

        int bartenderIndex;
        int bartenderTemp;
        int bouncerTemp;

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
                bartenderSpeed = runPubSpeed;
                guestSpeed = (runPubSpeed * 2);

                pubIsOpen = true;
                cleanGlasses = 20;
                chairs = 15;
                dirtyGlasses = 0;
                nrOfGuestServed = 0;

                bouncerIsWorking = true;

                bartenderIsWorking = true;
                firstBartenderIndex = true;
                bartenderHasServed = false;
                bartenderIndex = -1;

                groupDone = true;
                bouncerTemp = 0;

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
            runPubSpeed = (int)S_Speed.Value * 250;
        }

        public async void Bartender()
        {
            await Task.Run(async () => { while (!bartenderIsWorking) { await Task.Delay(1); } });
            await Task.Delay(bartenderSpeed + runPubSpeed);
            BarStatus();
            if (cleanGlasses <= 0)
            {
                Label_Bartender.Content = "Bartender (Waiting)";
            }
            else
            {
                Label_Bartender.Content = "Bartender (Working)";
            }

            bartenderHasServed = false;

            if (bartenderIsWorking && pubIsOpen && cleanGlasses > 0)
            {
                for (int i = 0; bartenderTemp > 0 && bartenderIsWorking && pubIsOpen && bartenderTemp > bartenderIndex; i--)
                {
                    bartenderHasServed = false;
                    i++;
                    await Task.Run(async () => { while (!bartenderIsWorking) { await Task.Delay(1); } });
                    await Task.Delay(bartenderSpeed + runPubSpeed);

                    if (bartenderTemp == 1 && firstBartenderIndex)
                    {
                        bartenderIndex++;

                        cleanGlasses--;
                        BarStatus();
                        await Task.Delay(bartenderSpeed + runPubSpeed);
                        L_Bartender.Items.Insert(0, "Serving " + patronList[0].GuestList[bartenderIndex].GuestInfo());

                        await Task.Delay(bartenderSpeed + runPubSpeed);
                        L_Bartender.Items.Insert(0, patronList[0].GuestList[bartenderIndex].Name + " served.");
                        nrOfGuestServed++;
                        BarStatus();
                        if (Debug) Log.Items.Insert(0, "Bart Index: " + bartenderIndex);

                        firstBartenderIndex = false;
                        bartenderHasServed = true;
                    }

                    if (bartenderTemp - 1 > bartenderIndex && cleanGlasses > 0)
                    {
                        bartenderIndex++;

                        cleanGlasses--;
                        BarStatus();
                        await Task.Delay(bartenderSpeed + runPubSpeed);
                        L_Bartender.Items.Insert(0, "Serving " + patronList[0].GuestList[bartenderIndex].GuestInfo());

                        await Task.Delay(bartenderSpeed + runPubSpeed);
                        L_Bartender.Items.Insert(0, patronList[0].GuestList[bartenderIndex].Name + " served.");
                        nrOfGuestServed++;
                        BarStatus();

                        if (Debug) Log.Items.Insert(0, "Bart Index: " + bartenderIndex);
                        bartenderHasServed = true;
                    }

                    if (cleanGlasses < 1)
                    {
                        Label_Bartender.Content = "Bartender (Waiting)";
                    }
                    else
                    {
                        Label_Bartender.Content = "Bartender (Working)";
                    }

                    if (bartenderTemp == bartenderIndex) { break; }
                    await Task.Delay(bartenderSpeed + runPubSpeed);
                    //Log.Items.Insert(0, bartenderTemp);
                }

            }

            if (bartenderIsWorking && pubIsOpen) Bartender();
        }

        public async void Bouncer()
        {
            await Task.Run(async () => { while (!bouncerIsWorking) { await Task.Delay(1); } });
            await Task.Delay(bouncerSpeed + runPubSpeed);

            if (bouncerTemp == 0 && groupDone && pubIsOpen)
            {
                await Task.Run(async () => { while (!bouncerIsWorking) { await Task.Delay(1); } });
                await Task.Delay(bouncerSpeed + runPubSpeed);

                groupOfPeople = random.Next(0, 15);
                bouncerTemp = groupOfPeople;

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
                    if (bouncerTemp <= 0)
                    {
                        bouncerTemp = 0;
                        groupDone = true;
                        break;
                    }

                    if (bouncerTemp > 0)
                    {
                        await Task.Run(async () => { while (!bouncerIsWorking) { await Task.Delay(1); } });
                        L_Guest.Items.Insert(0, patronList[0].Bouncer() + " has entered");
                        if (Debug) { Log.Items.Insert(0, $"Bouncer forloop: {bouncerTemp}"); }
                        bouncerTemp--;
                        bartenderTemp = patronList[0].GuestList.Count;
                    }

                    if (bouncerTemp <= 0)
                    {
                        bouncerTemp = 0;
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

            if (bouncerTemp <= 0 && pubIsOpen)
            {
                bouncerTemp = 0;
                groupDone = true;
            }

            if (Debug) { Log.Items.Insert(0, $"Bouncer final value: {bouncerTemp}"); }

            if (pubIsOpen && bouncerTemp == 0) Bouncer();
        }

        int guestIndex;
        public async void Guest()
        {
            await Task.Delay(1);

            if (bartenderHasServed || guestIndex < bartenderTemp - 1)
            {
                await Task.Delay(guestSpeed + runPubSpeed);

                if (chairs > 0)
                {
                    Log.Items.Insert(0, $"{patronList[0].GuestList[guestIndex].GuestInfo()} is sitting");
                    chairs--;
                    await Task.Delay(guestSpeed + runPubSpeed);
                    Log.Items.Insert(0, $"{patronList[0].GuestList[guestIndex].GuestInfo()} is drinking");
                    await Task.Delay(guestSpeed + runPubSpeed * 15);
                    Log.Items.Insert(0, $"{patronList[0].GuestList[guestIndex].GuestInfo()} has left");
                    guestIndex++;
                    chairs++;
                }
                else
                {
                    Log.Items.Insert(0, $"{patronList[0].GuestList[guestIndex].GuestInfo()} is standing");
                    await Task.Delay(guestSpeed + runPubSpeed);
                    Log.Items.Insert(0, $"{patronList[0].GuestList[guestIndex].GuestInfo()} is drinking");
                    await Task.Delay(guestSpeed + runPubSpeed * 15);
                    Log.Items.Insert(0, $"{patronList[0].GuestList[guestIndex].GuestInfo()} has left");
                    guestIndex++;
                }
            }

            if (pubIsOpen) Guest();
        }

        public void Start()
        {
            StartButtons();

            if (pubIsOpen)
            {
                Bouncer();
                Bartender();
                Guest();
            }
        }

        public void BarStatus()
        {
            Guest_Label.Content = $"Served: {nrOfGuestServed}\nSitting: {chairs}";
            BarStatus_Label.Content = $"Bar is Open!\nGlasses: {cleanGlasses}\nChairs: {chairs}";
            Dirty_Label.Content = $"Dirty Glass(es): {dirtyGlasses}";
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

    }
}

