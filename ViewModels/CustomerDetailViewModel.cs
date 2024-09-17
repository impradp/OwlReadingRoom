using OwlReadingRoom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlReadingRoom.ViewModels
{
    public class CustomerDetailViewModel
    {
        //Personal Details
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Faculty { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MobileNumber { get; set; }
        public string CurrentAddress { get; set; }
        public string PermanantAddress { get; set; }
        public string Disease { get; set; }
        public string Allergies { get; set; }

        public string Status { get; set; }
        //Booking details
        public BookingInfoViewModel BookingDetails { get; set; }
        //Transaction details
        public TransactionDetailViewModel TransactionDetails { get; set; }
        //Document details
        public DocumentViewModel Documents { get; set; }
        public bool HasDocuments => Documents is not null;
        public bool IsEmptyState => !HasDocuments;
    }
}
