using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WpfApp1
{/// <summary>
 /// Constructor for object Guest.
 /// </summary>
    public class Guest
    {
        public string Name { get; set; }
        public int Number { get; set; }

        private int guestNr = 0;
        private int index = 0;
        string currGuestName;

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

        /// <summary>
        /// Creates and add a single guest to the Patron public GuestList and returns the guest number and name.
        /// </summary>
        public object Bouncer()
        {
            currGuestName = Names[Random.Next(1, 31)];
            guestNr++;
            GuestList.Add(new Guest(guestNr, currGuestName));
            if (GuestList.Count > 1) { index++; }
            
            return GuestList[index].GuestInfo();
        }

        public Guest(int nr = 0, string name = "Default Name")
        {
            Number = nr;
            Name = name;
        }

        /// <summary>
        /// Returns guest number and name.
        /// </summary>
        public string GuestInfo() => $"{Number}. {Name}";

        public string Sit() => $"{Number}. {Name} is sitting";

        public string Stand() => $"{Number}. {Name} is standing";

        public string Drink() => $"{Number}. {Name} is drinking";

        public string Leave() => $"{Number}. {Name} has left";

    }
}