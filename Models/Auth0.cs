using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Models
{
    public class Auth0
    {
        public string Domain { get; set; }
        public string ClientId { get; set; }
        public string Audience { get; set; }
    }
}
