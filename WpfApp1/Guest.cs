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
        public new string Name { get; set; }
        public int Number { get; set; }

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