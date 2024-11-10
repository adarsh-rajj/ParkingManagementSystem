using DAL.Context;
using DAL.Entities;
using DAL.Repositories.IRepositories;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace DAL.Repositories
{
    public class ParkingAllocationRepository : IParkingAllocationRepository
    {
        private readonly ParkingDbContext _context;

        public ParkingAllocationRepository(ParkingDbContext context)
        {
            _context = context;
        }

        public IQueryable<ParkingAllotment> GetParking(string searchValue, string sortColumnName, string sortDirection)
        {
            IQueryable<ParkingAllotment> parkingAllotmentData = _context.ParkingAllotments.AsQueryable();

            // Apply searching
            if (!string.IsNullOrEmpty(searchValue))
            {
                parkingAllotmentData = parkingAllotmentData.Where(x =>
                    x.VehicleRcNoId.ToString().Contains(searchValue.ToLower()) ||
                    x.BlockNo.ToLower().Contains(searchValue.ToLower()) ||
                    x.Description.ToLower().Contains(searchValue.ToLower()));
            }

            // Sorting
            parkingAllotmentData = parkingAllotmentData.OrderBy(sortColumnName + " " + sortDirection);

            return parkingAllotmentData;
        }

        public void AddParking(ParkingAllotment parkingAllotment)
        {
            if(parkingAllotment != null)
                _context.ParkingAllotments.Add(parkingAllotment);
        }

        public int GetVehicleRCNo(int allocationId)
        {
            ParkingAllotment parkingAllotment = _context.ParkingAllotments.First(x => x.AllocationId == allocationId);
            return parkingAllotment.VehicleRcNoId;
        }

        public string GetBlockNo(int allocationId)
        {
            ParkingAllotment parkingAllotment = _context.ParkingAllotments.First(x => x.AllocationId == allocationId);
            return parkingAllotment.BlockNo;
        }

        public ParkingAllotment GetById(int id)
        {
            return _context.ParkingAllotments.Find(id);
        }

        public void DeleteParkingAllocation(int allocationId)
        {
            ParkingAllotment allotment = GetById(allocationId);
            if (allotment != null)
            {
                _context.ParkingAllotments.Remove(allotment);
            }
        }

        public void Update(ParkingAllotment parking)
        {
            if (parking != null)
                _context.Entry(parking).State = EntityState.Modified;
        }
    }
}
