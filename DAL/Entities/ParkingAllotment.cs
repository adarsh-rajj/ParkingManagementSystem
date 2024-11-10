using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class ParkingAllotment
    {
        public int AllocationId { get; set; } 
        public string BlockNo { get; set; }  
        public int VehicleRcNoId { get; set; }  
        public string Description { get; set; }  
        public DateTime ParkingDateFrom { get; set; }  
        public DateTime ParkingDateTo { get; set; }  
        public DateTime CreatedDate { get; set; }  
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey("VehicleRcNoId")]
        public virtual VehicleRegistration Vehicle { get; set; }
    }
}
