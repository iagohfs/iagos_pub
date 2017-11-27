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
    partial class MainWindow : Window
    {
        List<Guest> patronList = new List<Guest>();
        Random random = new Random();
        public delegate void GuestDel();

        bool bouncerIsWorking;
        bool bartenderIsWorking;
        bool waitressIsWorking;
        bool firstBartenderIndex;

        bool pubIsOpen;
        bool groupDone;

        private bool Debug;

        protected int runPubSpeed;
        private int bouncerSpeed;
        private int bartenderSpeed;
        private int waitressSpeed;

        int cleanGlasses;
        protected int dirtyGlasses;
        protected int chairs;

        int bartenderIndex;
        protected int guestCount;
        int bouncerGroupSize;

        int groupOfPeople;
        int currentGuestIndex;
        int guestsSitting;
        int nrOfGuestServed;
        protected int guestSpeed;
        private int remaningGuests;

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
                patronList.Add(new Guest());

                bouncerSpeed = runPubSpeed;
                bartenderSpeed = runPubSpeed;
                guestSpeed = 1000;

                pubIsOpen = true;
                cleanGlasses = 20;
                chairs = 15;
                dirtyGlasses = 0;
                nrOfGuestServed = 0;
                currentGuestIndex = -1;
                guestsSitting = 0;
                remaningGuests = 0;

                bouncerIsWorking = true;

                bartenderIsWorking = true;
                firstBartenderIndex = true;
                bartenderIndex = -1;

                groupDone = true;
                bouncerGroupSize = 0;

                Start();
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
                Bouncer();
            }
        }

        private void B_Bartender_Click(object sender, RoutedEventArgs e)
        {
            if (bartenderIsWorking)
            {
                bartenderIsWorking = false;
                B_Bartender.Content = "Play";
            }
            else
            {
                bartenderIsWorking = true;
                B_Bartender.Content = "Pause";
                Bartender();
            }
        }

        private void B_Waitress_Click(object sender, RoutedEventArgs e)
        {

            if (waitressIsWorking)
            {
                waitressIsWorking = false;
                B_Waitress.Content = "Play";
            }
            else
            {
                bartenderIsWorking = true;
                B_Waitress.Content = "Pause";
                Waitress();
            }

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

        private void S_Waitress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            waitressSpeed = (int)S_Waitress.Value * 200;
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

            if (bartenderIsWorking && pubIsOpen && cleanGlasses > 0 && guestCount > 0)
            {
                for (int i = 0; guestCount > 0 && bartenderIsWorking && pubIsOpen && guestCount > bartenderIndex; i--)
                {
                    i++;
                    await Task.Run(async () => { while (!bartenderIsWorking) { await Task.Delay(1); } });
                    await Task.Delay(bartenderSpeed + runPubSpeed);

                    if (guestCount == 1 && firstBartenderIndex)
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

                    }

                    if (guestCount - 1 > bartenderIndex && cleanGlasses > 0)
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
                    }

                    if (cleanGlasses < 1)
                    {
                        Label_Bartender.Content = "Bartender (Waiting)";
                    }
                    else
                    {
                        Label_Bartender.Content = "Bartender (Working)";
                    }

                    if (guestCount == bartenderIndex) { break; }
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

            if (bouncerGroupSize == 0 && groupDone && pubIsOpen)
            {
                await Task.Run(async () => { while (!bouncerIsWorking) { await Task.Delay(1); } });
                await Task.Delay(bouncerSpeed + runPubSpeed);

                groupOfPeople = random.Next(0, 6);
                bouncerGroupSize = groupOfPeople;

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
                    if (bouncerGroupSize <= 0)
                    {
                        bouncerGroupSize = 0;
                        groupDone = true;
                        break;
                    }

                    if (bouncerGroupSize > 0)
                    {
                        await Task.Run(async () => { while (!bouncerIsWorking) { await Task.Delay(1); } });
                        L_Guest.Items.Insert(0, patronList[0].Bouncer() + " has entered");
                        remaningGuests++;

                        if (Debug) { Log.Items.Insert(0, $"Bouncer forloop: {bouncerGroupSize}"); }

                        bouncerGroupSize--;
                        guestCount = patronList[0].GuestList.Count;
                        //Log.Items.Add(bartenderTemp);
                    }

                    if (bouncerGroupSize <= 0)
                    {
                        bouncerGroupSize = 0;
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

            if (bouncerGroupSize <= 0 && pubIsOpen)
            {
                bouncerGroupSize = 0;
                groupDone = true;
            }

            if (Debug) { Log.Items.Insert(0, $"Bouncer final value: {bouncerGroupSize}"); }

            if (pubIsOpen && bouncerGroupSize == 0) Bouncer();

        }

        public async void GuestAction()
        {
            await Task.Delay(1);

            if (currentGuestIndex < nrOfGuestServed - 1)

                if (nrOfGuestServed > 0 && currentGuestIndex < guestCount - 1)
                {
                    currentGuestIndex++;
                    Guest guest = patronList[0].GuestList[currentGuestIndex];
                    MyDel my = new MyDel(guest.Do);

                    if (chairs > 0 && currentGuestIndex < guestCount)
                    {
                        Log.Items.Insert(0, my(1));
                        chairs--;
                        await Task.Delay(guestSpeed * 5);
                        Log.Items.Insert(0, my(2));
                        await Task.Delay(guestSpeed * 10);
                        Log.Items.Insert(0, my(3));
                        await Task.Delay(guestSpeed);
                        chairs++;
                        dirtyGlasses++;

                    }
                    else
                    {
                        Log.Items.Insert(0, my(4));
                        await Task.Delay(guestSpeed * 5);
                        Log.Items.Insert(0, my(2));
                        await Task.Delay(guestSpeed * 10);
                        Log.Items.Insert(0, my(3));
                        await Task.Delay(guestSpeed);
                        dirtyGlasses++;

                    }
                }
        }

        public async void Waitress()
        {
            await Task.Run(async () => { while (!pubIsOpen) { await Task.Delay(1); } });
            await Task.Delay(waitressSpeed + runPubSpeed);

            if (dirtyGlasses > 0 && cleanGlasses <= 20)
            {
                for (int i = 0; i < dirtyGlasses; i++)
                {
                    L_Waitress.Items.Insert(0, $"Cleaning table");
                    await Task.Run(async () => { while (!pubIsOpen) { await Task.Delay(1); } });
                    await Task.Delay(waitressSpeed + runPubSpeed * 5);
                    L_Waitress.Items.Insert(0, $"Washing");

                    await Task.Run(async () => { while (!pubIsOpen) { await Task.Delay(1); } });
                    await Task.Delay(waitressSpeed + runPubSpeed * 5);
                    L_Waitress.Items.Insert(0, $"Done!");
                    dirtyGlasses--;
                    cleanGlasses++;

                    if (dirtyGlasses < 0)
                    {
                        dirtyGlasses = 0;
                    }

                    BarStatus();
                    await Task.Delay(waitressSpeed + runPubSpeed * 5);
                    await Task.Run(async () => { while (!pubIsOpen) { await Task.Delay(1); } });
                }
            }

            await Task.Run(async () => { while (!pubIsOpen) { await Task.Delay(1); } });
            if (pubIsOpen) Waitress();

        }

        public delegate string MyDel(int i);

        public async void TestThis()
        {
            await Task.Delay(guestSpeed + runPubSpeed);
            GuestDel guest = new GuestDel(GuestAction);
            guest();
            await Task.Delay(guestSpeed + runPubSpeed);
            TestThis();

        }

        public void Start()
        {
            StartButtons();

            if (pubIsOpen)
            {
                Bouncer();
                Bartender();
                Waitress();
                TestThis();
            }
        }

        public void BarStatus()
        {
            Guest_Label.Content = $"Served: {nrOfGuestServed}\nSitting: {guestsSitting}";
            BarStatus_Label.Content = $"Bar is Open!\nGlasses: {cleanGlasses}\nChairs: {chairs}";
            Dirty_Label.Content = $"Dirty Glass(es): {dirtyGlasses}";
        }

        public void StartButtons()
        {
            B_Bartender.IsEnabled = true;
            B_Bartender.Content = "Pause";

            B_Bouncer.IsEnabled = true;
            B_Bouncer.Content = "Pause";

            B_Waitress.IsEnabled = true;
            B_Waitress.Content = "Pause";

            B_PP.Content = "Stop Everything";
        }

        public async void Reset()
        {
            B_Bartender.IsEnabled = false;
            B_Bartender.Content = "Pause";

            B_Bouncer.IsEnabled = false;
            B_Bouncer.Content = "Pause";


            B_Waitress.IsEnabled = false;
            B_Waitress.Content = "Pause";

            B_PP.IsEnabled = false;

            Debugger.IsEnabled = false;

            await Task.Delay(runPubSpeed * 10);
            Log.Items.Insert(0, "Bouncer going home");

            await Task.Delay(runPubSpeed * 10);
            Log.Items.Insert(0, "Bartender going home");

            await Task.Delay(runPubSpeed * 10);
            Log.Items.Insert(0, "Waitress going home");

            await Task.Delay(runPubSpeed * 15);
            Log.Items.Insert(0, "Hostage Count: " + (patronList[0].GuestList.Count - remaningGuests));

        }

    }

}

