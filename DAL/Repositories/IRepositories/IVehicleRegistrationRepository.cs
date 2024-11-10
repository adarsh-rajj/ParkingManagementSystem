using DAL.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.IRepositories
{
    public interface IVehicleRegistrationRepository
    {
        IEnumerable<VehicleRegistration> GetAllVehicle();
        IEnumerable<VehicleRegistration> GetAllUnParkedVehicles();
        IQueryable<VehicleRegistration> GetRegisterVehicle(string searchValue, string sortColumnName, string sortDirection);
        IEnumerable<VehicleRegistration> GetParkedVehicles(string blockNo, string searchValue, string sortColumnName, string sortDirection);
        void AddVehicleRegistration(VehicleRegistration vehicleRegistration);
        VehicleRegistration GetById(int id);
        void Update(VehicleRegistration vehicle);
        void DeleteVehicleRegistration(int VehicleRegistrationId);
        bool UniqueVehicleRcNo(string vehicleRcNo);
    }
}
