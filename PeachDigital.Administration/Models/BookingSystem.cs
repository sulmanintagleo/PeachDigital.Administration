//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PeachDigital.Administration.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class BookingSystem
    {
        public long Id { get; set; }
        public string BookingURI { get; set; }
        public bool AllocatedSeating { get; set; }
        public bool LoyaltyEnabled { get; set; }
        public string Concessions { get; set; }
        public string MobileTickets { get; set; }
        public string SoldOutPercentage { get; set; }
        public long CinemaId { get; set; }
    }
}
