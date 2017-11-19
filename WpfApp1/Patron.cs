using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{/// <summary>
 /// Patron aka bouncer class contains guest name list, public guestlist, bouncer who adds a guest to thelist and return a guest object.
 /// </summary>
    public class Patron : MainWindow
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
        private int index = 0;
        string currGuestName;

        /// <summary>
        /// Creates and add a single guest to the Patron public GuestList and returns the guest number and name.
        /// </summary>
        public new object Bouncer()
        {
            currGuestName = Names[Random.Next(1, 31)];
            guestNr++;
            GuestList.Add(new Guest(guestNr, currGuestName));
            if (GuestList.Count > 1) { index++; }

            return GuestList[index].GuestInfo();
        }

    }

}
