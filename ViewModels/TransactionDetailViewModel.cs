﻿namespace OwlReadingRoom.ViewModels
{
    public class TransactionDetailViewModel
    {
        public int? TransactionId { get; set; }
        public double PackageAmount { get; set; }
        public double LockerAmount { get; set; }
        public double ParkingAmount { get; set; }
        public double TotalAmount { get; set; }
        public double PaidAmount { get; set; }
        public double DueAmount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public string InvoiceNumber
        {
            get { return $"{DateTimeOffset.Now.ToUnixTimeMilliseconds()}"; }
        }

        public int LockerQty
        {
            get { return this.LockerAmount > 0 ? 1 : 0; }
        }
        public int ParkingQty
        {
            get { return this.LockerAmount > 0 ? 1 : 0; }
        }
    }
}
