using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.ViewModels
{
    public class CustomerDetailViewModel
    {
        public String FullName { get; set; }
        public String Gender { get; set; }
        public String Faculty { get; set; }
        public String DateOfBirth { get; set; }
        public String MobileNumber { get; set; }
        public String CurrentAddress { get; set; }
        public String PermanantAddress { get; set; }
        public DocumentViewModel Documents { get; set; }
    }
}
