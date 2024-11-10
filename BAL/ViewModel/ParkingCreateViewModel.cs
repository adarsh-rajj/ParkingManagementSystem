using System;

namespace BAL.ViewModel
{
    public class ParkingCreateViewModel
    {
        public string BlockNo { get; set; }
        public int VehicleRcNoId { get; set; }
        public string Description { get; set; }
        public DateTime ParkingDateFrom { get; set; }
        public DateTime ParkingDateTo { get; set; }

    }
}
