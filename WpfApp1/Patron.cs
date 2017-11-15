using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Patron : MainWindow
    {
        Random Random = new Random();
        public List<Guest> GuestList = new List<Guest>();
        List<string> Names = new List<string>()
        {
            "Gigi", "Aline","Ericka","Jeanine","Apryl",
            "Jarrod","Estelle","Magda","Coral","Ardelle",
            "Lena","Allen","Emmitt","Mafalda","Jannette",
            "Sal","Christen","Lane","Duane","Joseph",
            "Hien","Ruthe","Danyell","Britney","Romana",
            "Laraine","Branda","Kasha","Lecia","Hollie",
            "Lachelle","Svetlana","Joya","Karin","Jovan",
            "Kendal","Tanna","Jacalyn","Bellav","Joellev",
            "Domenica","Suzie","Errol","Lilly","Stacee",
            "Evalyn","Ardell","Gregoria","Shonta","Eldridge"
        };

        private int guestNr = 0;
        string currGuestName;

        /// <summary>
        /// Creates and add a single guest to the Patron GuestList and returns the info about the guest.
        /// </summary>
        public new object Bouncer()
        {
            currGuestName = Names[Random.Next(1, 31)];
            guestNr++;
            GuestList.Insert(0, new Guest(guestNr, currGuestName));

            return GuestList[0].GuestInfo();
        }

        /// <summary>
        /// Returns the size of the GuestList.
        /// </summary>
        public int GetListSize()
        {
            return GuestList.Count();
        }

        /// <summary>
        /// Returns a Guest object from a desired index of the GuestList.
        /// </summary>
        public object RtGuestFromList(int index)
        {
            return GuestList[index];
        }

        /// <summary>
        /// Tells which guest from the Guestlist has left.
        /// </summary>
        public void GuestLeft(int index)
        {
            Log.Items.Insert(0, $"{GuestList[index]} has left");
        }

    }
}
