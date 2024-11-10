using DAL.Entities;
using System.Linq;

namespace DAL.Repositories.IRepositories
{
    public interface IParkingAllocationRepository
    {
        IQueryable<ParkingAllotment> GetParking(string searchValue, string sortColumnName, string sortDirection);
        void AddParking(ParkingAllotment parkingAllotment);
        ParkingAllotment GetById(int id);
        void DeleteParkingAllocation(int allocationId);
        int GetVehicleRCNo(int allocationId);
        string GetBlockNo(int allocationId);
        void Update(ParkingAllotment parking);
    }
}
