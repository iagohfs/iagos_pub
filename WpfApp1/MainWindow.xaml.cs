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
        public List<Patron> patronList = new List<Patron>();
        Random random = new Random();

        public delegate string MessageMe();

        bool bouncerIsWorking;
        bool bartenderIsWorking;
        bool waitressIsWorking;
        bool firstBartenderIndex;
        protected bool bartenderHasServed;

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
        protected int bartenderTemp;
        int bouncerTemp;

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
                patronList.Add(new Patron());

                bouncerSpeed = runPubSpeed;
                bartenderSpeed = runPubSpeed;
                guestSpeed = (runPubSpeed * 2);

                pubIsOpen = true;
                cleanGlasses = 20;
                chairs = 15;
                dirtyGlasses = 0;
                nrOfGuestServed = 0;
                currentGuestIndex = 0;
                guestsSitting = 0;
                remaningGuests = 0;


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

            //bartenderHasServed = false;

            if (bartenderIsWorking && pubIsOpen && cleanGlasses > 0 && bartenderTemp > 0)
            {
                for (int i = 0; bartenderTemp > 0 && bartenderIsWorking && pubIsOpen && bartenderTemp > bartenderIndex; i--)
                {
                    //bartenderHasServed = false;
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

                groupOfPeople = random.Next(0, 6);
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
                        remaningGuests++;

                        if (Debug) { Log.Items.Insert(0, $"Bouncer forloop: {bouncerTemp}"); }

                        bouncerTemp--;
                        bartenderTemp = patronList[0].GuestList.Count;
                        //Log.Items.Add(bartenderTemp);
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

        public async void Guest()
        {
            await Task.Delay(1);
            //await Task.Delay(guestSpeed + runPubSpeed);            

            if (bartenderTemp > 0 && currentGuestIndex < bartenderTemp && bartenderHasServed)
            {
                await Task.Delay(guestSpeed + runPubSpeed);
                if (chairs > 0 && currentGuestIndex < bartenderTemp)
                {
                    Log.Items.Insert(0, patronList[0].GuestList[currentGuestIndex].Sit());
                    chairs--;
                    guestsSitting++;
                }
                else
                {
                    Log.Items.Insert(0, patronList[0].GuestList[currentGuestIndex].Stand());
                }

                if (currentGuestIndex < bartenderTemp)
                {
                    await Task.Run(async () => { while (!pubIsOpen) { await Task.Delay(1); } });
                    await Task.Delay(guestSpeed + runPubSpeed * 5);
                    await Task.Run(async () => { while (!pubIsOpen) { await Task.Delay(1); } });

                    Log.Items.Insert(0, patronList[0].GuestList[currentGuestIndex].Drink());
                    await Task.Delay(guestSpeed + runPubSpeed * 10);

                    await Task.Run(async () => { while (!pubIsOpen) { await Task.Delay(1); } });
                    Log.Items.Insert(0, patronList[0].GuestList[currentGuestIndex].Leave());
                    await Task.Delay(guestSpeed + runPubSpeed * 5);

                    await Task.Run(async () => { while (!pubIsOpen) { await Task.Delay(1); } });
                    currentGuestIndex++;
                    dirtyGlasses++;
                    chairs++;
                    guestsSitting--;
                    remaningGuests--;

                }

            }

            if (pubIsOpen) Guest();
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

        public void Start()
        {
            StartButtons();

            if (pubIsOpen)
            {
                Bouncer();
                Bartender();
                Waitress();
                Guest();
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

