using System;

namespace BAL.ViewModel
{
    public class VehicleRegistrationCreateModel
    {
        public string VehicleRCNo { get; set; }
        public string OwnerName { get; set; }
        public string Model { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public string Status { get; set; } = "Not-Parked";
    }
}
