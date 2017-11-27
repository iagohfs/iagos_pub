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
        public delegate string MyDel(int i);

        bool bouncerIsWorking;
        bool bartenderIsWorking;
        bool waitressIsWorking;
        bool firstBartenderIndex;

        bool pubIsOpen;
        bool groupDone;

        bool couplesNight;
        bool busGroup;

        private bool Debug;

        protected int runPubSpeed;
        private int bouncerSpeed;
        private int bartenderSpeed;
        private int waitressSpeed;
        static int startGlassValue = 8;

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
                ClosePub();
            }

            if (patronList.Count == 0)
            {
                patronList.Add(new Guest());

                bouncerSpeed = runPubSpeed * 4;
                bartenderSpeed = runPubSpeed * 4;
                guestSpeed = 1000;

                chairs = 9;
                dirtyGlasses = 0;
                nrOfGuestServed = 0;
                currentGuestIndex = -1;
                guestsSitting = 0;
                remaningGuests = 0;
                groupOfPeople = 1;
                cleanGlasses = startGlassValue;

                pubIsOpen = true;
                groupDone = true;
                bouncerIsWorking = true;
                bartenderIsWorking = true;
                firstBartenderIndex = true;
                waitressIsWorking = true;
                bartenderIndex = -1;

                if (busGroup)
                {
                    bouncerSpeed = 2000;
                }

                if (couplesNight)
                {
                    groupOfPeople = 2;
                }

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
            waitressSpeed = (int)S_Waitress.Value * 1000;
        }

        public async void Bartender()
        {
            await Task.Run(async () => { while (!bartenderIsWorking) { await Task.Delay(1); } });
            BarStatus();

            if (cleanGlasses <= 0)
            {
                Label_Bartender.Content = "Bartender (Waiting)";
            }
            else
            {
                Label_Bartender.Content = "Bartender (Working)";
            }

            if (cleanGlasses > 0 && guestCount > 0)
            {
                for (int i = 0; guestCount > 0; i--)
                {
                    i++;

                    await Task.Run(async () => { while (!bartenderIsWorking) { await Task.Delay(1); } });

                    if (guestCount == 1 && firstBartenderIndex)
                    {
                        bartenderIndex++;
                        patronList[0].GuestList[bartenderIndex].Served = true;

                        await Task.Delay(bartenderSpeed);
                        L_Bartender.Items.Insert(0, "Serving " + patronList[0].GuestList[bartenderIndex].GuestInfo());

                        await Task.Delay(bartenderSpeed * 3);
                        cleanGlasses--;
                        BarStatus();

                        await Task.Delay(bartenderSpeed * 3);
                        L_Bartender.Items.Insert(0, patronList[0].GuestList[bartenderIndex].Name + " served.");
                        nrOfGuestServed++;
                        remaningGuests++;
                        BarStatus();

                        if (Debug) Log.Items.Insert(0, "Bart Index: " + bartenderIndex);
                        await Task.Delay(bartenderSpeed * 3);
                        firstBartenderIndex = false;
                    }

                    if (nrOfGuestServed < guestCount && cleanGlasses > 0)
                    {
                        bartenderIndex++;
                        await Task.Delay(bartenderSpeed);
                        L_Bartender.Items.Insert(0, "Serving " + patronList[0].GuestList[bartenderIndex].GuestInfo());

                        await Task.Delay(bartenderSpeed * 3);
                        cleanGlasses--;
                        BarStatus();

                        await Task.Delay(bartenderSpeed * 3);
                        L_Bartender.Items.Insert(0, patronList[0].GuestList[bartenderIndex].Name + " served.");
                        patronList[0].GuestList[bartenderIndex].Served = true;
                        nrOfGuestServed++;
                        remaningGuests++;
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

                    if (guestCount == bartenderIndex || !pubIsOpen) { break; }
                    //Log.Items.Insert(0, bartenderTemp);
                }

            }

            if (bartenderIsWorking && bouncerIsWorking || nrOfGuestServed < guestCount) Bartender();

            if (!pubIsOpen && nrOfGuestServed == guestCount)
            {
                Log.Items.Insert(0, "Bartender going home");
                bartenderIsWorking = false;
            }
        }

        public async void BusGroup()
        {
            await Task.Delay(bouncerSpeed * random.Next(3, 9));
            groupOfPeople = 15;
        }

        public async void Bouncer()
        {
            await Task.Delay(bouncerSpeed * random.Next(2, 10));
            await Task.Run(async () => { while (!bouncerIsWorking) { await Task.Delay(1); } });

            if (groupDone && pubIsOpen)
            {
                groupDone = false;
                await Task.Run(async () => { while (!bouncerIsWorking) { await Task.Delay(1); } });
                L_Bouncer.Items.Insert(0, $"Letting {groupOfPeople} guest(s) in");
            }

            if (!groupDone && pubIsOpen)
            {
                if (busGroup)
                {
                    BusGroup();
                    L_Bouncer.Items.Insert(0, $"Letting {groupOfPeople} guest(s) in");
                    busGroup = false;
                }

                bouncerGroupSize = groupOfPeople;
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
                    groupDone = true;
                }
                if (!busGroup) { groupOfPeople = 1; }
            }
            else
            {
                if (pubIsOpen)
                {
                    L_Bouncer.Items.Insert(0, "Wating for guest");
                }

                await Task.Delay(bouncerSpeed + runPubSpeed);
            }


            if (Debug) { Log.Items.Insert(0, $"Bouncer final value: {bouncerGroupSize}"); }

            if (pubIsOpen && bouncerGroupSize == 0) Bouncer();

        }

        public async void GuestAction()
        {
            await Task.Delay(1);

            if (chairs > 0)
            {
                if (nrOfGuestServed > currentGuestIndex + 1)
                {
                    currentGuestIndex++;

                    if (patronList[0].GuestList[currentGuestIndex].Served)
                    {
                        Guest guest = patronList[0].GuestList[currentGuestIndex];
                        MyDel my = new MyDel(guest.Do);
                        await Task.Delay(guestSpeed * 4);
                        Log.Items.Insert(0, my(1));
                        chairs--;
                        guestsSitting++;
                        await Task.Delay(guestSpeed * 5);
                        Log.Items.Insert(0, my(2));
                        await Task.Delay(guestSpeed * random.Next(9, 20));
                        Log.Items.Insert(0, my(3));
                        await Task.Delay(guestSpeed);
                        chairs++;
                        dirtyGlasses++;
                        guestsSitting--;
                        BarStatus();
                    }

                }

            }

        }

        public async void Waitress()
        {
            await Task.Run(async () => { while (!waitressIsWorking) { await Task.Delay(1); } });

            if (dirtyGlasses > 0)
            {
                for (int i = 0; i < dirtyGlasses; i++)
                {
                    L_Waitress.Items.Insert(0, $"Cleaning table");
                    dirtyGlasses--;
                    await Task.Run(async () => { while (!waitressIsWorking) { await Task.Delay(1); } });
                    await Task.Delay(waitressSpeed * 5);
                    L_Waitress.Items.Insert(0, $"Washing");

                    await Task.Run(async () => { while (!waitressIsWorking) { await Task.Delay(1); } });
                    await Task.Delay(waitressSpeed * 15);
                    L_Waitress.Items.Insert(0, $"Done!");

                    cleanGlasses++;

                    if (dirtyGlasses < 0)
                    {
                        dirtyGlasses = 0;
                    }

                    BarStatus();
                    await Task.Delay(waitressSpeed * 5);
                    await Task.Run(async () => { while (!waitressIsWorking) { await Task.Delay(1); } });
                }
            }

            if (cleanGlasses == startGlassValue && !pubIsOpen) { waitressIsWorking = false; Log.Items.Insert(0, "Waitress went home"); }

            if (waitressIsWorking || dirtyGlasses > 0) Waitress();

        }

        public async void GuestDo()
        {
            await Task.Delay(guestSpeed + runPubSpeed);
            GuestDel guest = new GuestDel(GuestAction);
            if (nrOfGuestServed <= guestCount) guest();
            GuestDo();
        }

        public void Start()
        {
            StartButtons();

            if (pubIsOpen)
            {
                Bouncer();
                Bartender();
                Waitress();
                GuestDo();
                Timer();
            }
        }

        public async void Timer()
        {
            await Task.Delay(2 * 5000);
            pubIsOpen = false;
            ClosePub();
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

        public async void ClosePub()
        {
            pubIsOpen = false;
            B_Bartender.IsEnabled = false;
            B_Bartender.Content = "Pause";

            B_Bouncer.IsEnabled = false;
            bouncerIsWorking = false;
            B_Bouncer.Content = "Pause";

            B_Waitress.IsEnabled = false;
            B_Waitress.Content = "Pause";

            B_PP.IsEnabled = false;

            Debugger.IsEnabled = false;

            await Task.Delay(runPubSpeed * 10);
            Log.Items.Insert(0, "Bouncer going home");

        }

        private void Couples_Night_Click(object sender, RoutedEventArgs e)
        {
            couplesNight = true;
            Bus_Group.IsEnabled = false;
            Couples_Night.IsEnabled = false;
            Stay_Longer.IsEnabled = false;

            Guest_Top_Label.Content = "Guest: Cuples Night";
        }

        private void Bus_Group_Click(object sender, RoutedEventArgs e)
        {
            busGroup = true;
            Couples_Night.IsEnabled = false;
            Bus_Group.IsEnabled = false;
            Stay_Longer.IsEnabled = false;

            Guest_Top_Label.Content = "Guest: Party Night";
        }

        private void Stay_Longer_Click(object sender, RoutedEventArgs e)
        {
            Couples_Night.IsEnabled = false;
            Bus_Group.IsEnabled = false;
            Stay_Longer.IsEnabled = false;
            guestSpeed = 2000;

            Guest_Top_Label.Content = "Guest: Chill Night";
        }
    }

}

