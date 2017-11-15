using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Patron : MainWindow
    {
        List<Guest> GuestList = new List<Guest>();
        private int guestNr = 0;

        public new object Bouncer()
        {
            guestNr++;
            GuestList.Insert(0, new Guest(guestNr, "Mike"));
            return GuestList[0].GuestInfo();
        }

        public int GetListSize()
        {
            return GuestList.Count();
        }

    }
}
