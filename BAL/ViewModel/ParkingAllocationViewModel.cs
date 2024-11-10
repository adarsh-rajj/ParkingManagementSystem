using System;
using System.Collections.Generic;

namespace BAL.ViewModel
{
    public class ParkingAllocationViewModel
    {
        public string AllocationId { get; set; }
        public string BlockNo { get; set; }
        public int VehicleRcNoId { get; set; }
        public string Description { get; set; }
        public DateTime ParkingDateFrom { get; set; }
        public DateTime ParkingDateTo { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string PreviousBlock { get; set; }

        public IEnumerable<BlockViewModel> Blocks { get; set; }
    }
}
