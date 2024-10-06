using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Models
{
    public class User
    {
        public String Name { get; set; }
        public String Email { get; set; }
        public String Role { get; set; }
        public String Picture { get; set; }
        public String GreetingInfo { get; set; }
    }
}
