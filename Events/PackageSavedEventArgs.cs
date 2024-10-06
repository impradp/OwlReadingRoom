using OwlReadingRoom.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.Events
{
    public class PackageSavedEventArgs : EventArgs
    {
        public CustomerPackageViewModel UpdatedCustomer { get; set; }

        public PackageSavedEventArgs(CustomerPackageViewModel updatedCustomer)
        {
            UpdatedCustomer = updatedCustomer;
        }
    }
}
