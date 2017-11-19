using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{/// <summary>
 /// Constructor for object Guest.
 /// </summary>
    class Guest : Patron
    {
        public string Name { get; set; }
        public int Number { get; set; }

        public Guest(int nr = 0, string name = "Default Name")
        {
            Number = nr;
            Name = name;
        }

        /// <summary>
        /// Returns guest number and name.
        /// </summary>
        public string GuestInfo()
        {
            return $"{Number}. {Name}";
        }
    }
}
