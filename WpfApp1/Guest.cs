using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Guest
    {
        public string Name { get; set; }
        public int Number { get; set; }

        public Guest(int nr = 0, string name = "Default Name")
        {
            Number = nr;
            Name = name;
        }

        public string GuestInfo()
        {            
            return $"{Number}. {Name}";
        }        
    }
}
