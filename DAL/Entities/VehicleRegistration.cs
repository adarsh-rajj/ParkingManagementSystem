using System;

namespace DAL.Entities
{
    public class VehicleRegistration
    {
        public int VehicleRcNoId { get; set; }  
        public string VehicleRCNo { get; set; }  
        public string OwnerName { get; set; }  
        public string Model { get; set; }  
        public DateTime DateOfRegistration { get; set; }  
        public string Status { get; set; } = "Not-Parked"; 
        public DateTime CreatedDate { get; set; }  
        public DateTime? ModifiedDate { get; set; }
        public string BlockNo { get; set; } = "None";
    }
}
