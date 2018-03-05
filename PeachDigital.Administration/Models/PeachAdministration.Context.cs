﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class PeachAdministrationEntities : DbContext
    {
        public PeachAdministrationEntities()
            : base("name=PeachAdministrationEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ModulePermission> ModulePermissions { get; set; }
        public virtual DbSet<PermissionType> PermissionTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<Cinema> Cinemas { get; set; }
        public virtual DbSet<CinemaAddress> CinemaAddresses { get; set; }
        public virtual DbSet<BookingSystem> BookingSystems { get; set; }
        public virtual DbSet<FilmReleaseRule> FilmReleaseRules { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<HeadOffice> HeadOffices { get; set; }
        public virtual DbSet<MapPoint> MapPoints { get; set; }
        public virtual DbSet<Circuit> Circuits { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<ProviderConfiguration> ProviderConfigurations { get; set; }
        public virtual DbSet<ProviderValueMetaData> ProviderValueMetaDatas { get; set; }
        public virtual DbSet<ProviderValue> ProviderValues { get; set; }
    
        public virtual int UpdateCinema(Nullable<long> oldCinemaId, Nullable<long> newCinemaId, string name, string timezone, Nullable<bool> open, Nullable<bool> booking, Nullable<bool> @public, Nullable<bool> threeDSecure, Nullable<int> order, string managerName, string managerDesc, string external_Id, Nullable<long> bookingSystemID, Nullable<long> regionId, Nullable<long> circuit_Id, Nullable<int> dayStart, Nullable<bool> giftStore, string secondExternal_Id, Nullable<bool> isPaymentWithGiftCardEnabled, Nullable<bool> isLoyaltyEarnPointsEnabled, string ticketingUrl)
        {
            var oldCinemaIdParameter = oldCinemaId.HasValue ?
                new ObjectParameter("oldCinemaId", oldCinemaId) :
                new ObjectParameter("oldCinemaId", typeof(long));
    
            var newCinemaIdParameter = newCinemaId.HasValue ?
                new ObjectParameter("newCinemaId", newCinemaId) :
                new ObjectParameter("newCinemaId", typeof(long));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var timezoneParameter = timezone != null ?
                new ObjectParameter("Timezone", timezone) :
                new ObjectParameter("Timezone", typeof(string));
    
            var openParameter = open.HasValue ?
                new ObjectParameter("Open", open) :
                new ObjectParameter("Open", typeof(bool));
    
            var bookingParameter = booking.HasValue ?
                new ObjectParameter("Booking", booking) :
                new ObjectParameter("Booking", typeof(bool));
    
            var publicParameter = @public.HasValue ?
                new ObjectParameter("Public", @public) :
                new ObjectParameter("Public", typeof(bool));
    
            var threeDSecureParameter = threeDSecure.HasValue ?
                new ObjectParameter("ThreeDSecure", threeDSecure) :
                new ObjectParameter("ThreeDSecure", typeof(bool));
    
            var orderParameter = order.HasValue ?
                new ObjectParameter("Order", order) :
                new ObjectParameter("Order", typeof(int));
    
            var managerNameParameter = managerName != null ?
                new ObjectParameter("ManagerName", managerName) :
                new ObjectParameter("ManagerName", typeof(string));
    
            var managerDescParameter = managerDesc != null ?
                new ObjectParameter("ManagerDesc", managerDesc) :
                new ObjectParameter("ManagerDesc", typeof(string));
    
            var external_IdParameter = external_Id != null ?
                new ObjectParameter("External_Id", external_Id) :
                new ObjectParameter("External_Id", typeof(string));
    
            var bookingSystemIDParameter = bookingSystemID.HasValue ?
                new ObjectParameter("BookingSystemID", bookingSystemID) :
                new ObjectParameter("BookingSystemID", typeof(long));
    
            var regionIdParameter = regionId.HasValue ?
                new ObjectParameter("RegionId", regionId) :
                new ObjectParameter("RegionId", typeof(long));
    
            var circuit_IdParameter = circuit_Id.HasValue ?
                new ObjectParameter("Circuit_Id", circuit_Id) :
                new ObjectParameter("Circuit_Id", typeof(long));
    
            var dayStartParameter = dayStart.HasValue ?
                new ObjectParameter("DayStart", dayStart) :
                new ObjectParameter("DayStart", typeof(int));
    
            var giftStoreParameter = giftStore.HasValue ?
                new ObjectParameter("GiftStore", giftStore) :
                new ObjectParameter("GiftStore", typeof(bool));
    
            var secondExternal_IdParameter = secondExternal_Id != null ?
                new ObjectParameter("SecondExternal_Id", secondExternal_Id) :
                new ObjectParameter("SecondExternal_Id", typeof(string));
    
            var isPaymentWithGiftCardEnabledParameter = isPaymentWithGiftCardEnabled.HasValue ?
                new ObjectParameter("IsPaymentWithGiftCardEnabled", isPaymentWithGiftCardEnabled) :
                new ObjectParameter("IsPaymentWithGiftCardEnabled", typeof(bool));
    
            var isLoyaltyEarnPointsEnabledParameter = isLoyaltyEarnPointsEnabled.HasValue ?
                new ObjectParameter("IsLoyaltyEarnPointsEnabled", isLoyaltyEarnPointsEnabled) :
                new ObjectParameter("IsLoyaltyEarnPointsEnabled", typeof(bool));
    
            var ticketingUrlParameter = ticketingUrl != null ?
                new ObjectParameter("TicketingUrl", ticketingUrl) :
                new ObjectParameter("TicketingUrl", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateCinema", oldCinemaIdParameter, newCinemaIdParameter, nameParameter, timezoneParameter, openParameter, bookingParameter, publicParameter, threeDSecureParameter, orderParameter, managerNameParameter, managerDescParameter, external_IdParameter, bookingSystemIDParameter, regionIdParameter, circuit_IdParameter, dayStartParameter, giftStoreParameter, secondExternal_IdParameter, isPaymentWithGiftCardEnabledParameter, isLoyaltyEarnPointsEnabledParameter, ticketingUrlParameter);
        }
    
        public virtual int UpdateCircuit(Nullable<long> oldCircuitId, Nullable<long> newCircuitId, string name, string baseURI, string userName, string password, Nullable<bool> open, Nullable<bool> @public, Nullable<long> headOfficeID, Nullable<long> countryID, Nullable<long> contactID, Nullable<long> mapPointID, Nullable<bool> useLoyalty, Nullable<int> orderTimeoutSeconds)
        {
            var oldCircuitIdParameter = oldCircuitId.HasValue ?
                new ObjectParameter("oldCircuitId", oldCircuitId) :
                new ObjectParameter("oldCircuitId", typeof(long));
    
            var newCircuitIdParameter = newCircuitId.HasValue ?
                new ObjectParameter("newCircuitId", newCircuitId) :
                new ObjectParameter("newCircuitId", typeof(long));
    
            var nameParameter = name != null ?
                new ObjectParameter("Name", name) :
                new ObjectParameter("Name", typeof(string));
    
            var baseURIParameter = baseURI != null ?
                new ObjectParameter("BaseURI", baseURI) :
                new ObjectParameter("BaseURI", typeof(string));
    
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));
    
            var openParameter = open.HasValue ?
                new ObjectParameter("Open", open) :
                new ObjectParameter("Open", typeof(bool));
    
            var publicParameter = @public.HasValue ?
                new ObjectParameter("Public", @public) :
                new ObjectParameter("Public", typeof(bool));
    
            var headOfficeIDParameter = headOfficeID.HasValue ?
                new ObjectParameter("HeadOfficeID", headOfficeID) :
                new ObjectParameter("HeadOfficeID", typeof(long));
    
            var countryIDParameter = countryID.HasValue ?
                new ObjectParameter("CountryID", countryID) :
                new ObjectParameter("CountryID", typeof(long));
    
            var contactIDParameter = contactID.HasValue ?
                new ObjectParameter("ContactID", contactID) :
                new ObjectParameter("ContactID", typeof(long));
    
            var mapPointIDParameter = mapPointID.HasValue ?
                new ObjectParameter("MapPointID", mapPointID) :
                new ObjectParameter("MapPointID", typeof(long));
    
            var useLoyaltyParameter = useLoyalty.HasValue ?
                new ObjectParameter("UseLoyalty", useLoyalty) :
                new ObjectParameter("UseLoyalty", typeof(bool));
    
            var orderTimeoutSecondsParameter = orderTimeoutSeconds.HasValue ?
                new ObjectParameter("OrderTimeoutSeconds", orderTimeoutSeconds) :
                new ObjectParameter("OrderTimeoutSeconds", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("UpdateCircuit", oldCircuitIdParameter, newCircuitIdParameter, nameParameter, baseURIParameter, userNameParameter, passwordParameter, openParameter, publicParameter, headOfficeIDParameter, countryIDParameter, contactIDParameter, mapPointIDParameter, useLoyaltyParameter, orderTimeoutSecondsParameter);
        }
    }
}
