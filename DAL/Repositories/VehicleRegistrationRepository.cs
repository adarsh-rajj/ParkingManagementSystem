using BOL.Constant;
using DAL.Context;
using DAL.Entities;
using DAL.Repositories.IRepositories;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace DAL.Repositories
{
    public class VehicleRegistrationRepository : IVehicleRegistrationRepository
    {
        private readonly ParkingDbContext _context;

        public VehicleRegistrationRepository(ParkingDbContext context)
        {
            _context = context;
        }

        public IEnumerable<VehicleRegistration> GetAllVehicle()
        {
            return _context.VehicleRegistrations;
        }

        public IEnumerable<VehicleRegistration> GetAllUnParkedVehicles()
        {
            return _context.VehicleRegistrations.Where(x => x.Status == VehicleRegistrationConstant.NotParked);
        }

        public IQueryable<VehicleRegistration> GetRegisterVehicle(string searchValue, string sortColumnName, string sortDirection)
        {
            IQueryable<VehicleRegistration> registerVehicleData = _context.VehicleRegistrations.AsQueryable();

            // Apply searching
            if (!string.IsNullOrEmpty(searchValue))
            {
                registerVehicleData = registerVehicleData.Where(x =>
                    x.VehicleRcNoId.ToString().Contains(searchValue.ToLower()) ||
                    x.VehicleRCNo.ToLower().Contains(searchValue.ToLower()) ||
                    x.OwnerName.ToLower().Contains(searchValue.ToLower()) ||
                    x.Status.ToLower().Contains(searchValue.ToLower()) ||
                    x.Model.ToLower().Contains(searchValue.ToLower()));  
            }

            // Sorting
            registerVehicleData = registerVehicleData.OrderBy(sortColumnName + " " + sortDirection);

            return registerVehicleData;
        }

        public void AddVehicleRegistration(VehicleRegistration vehicleRegistration)
        {
            if(vehicleRegistration != null)
                _context.VehicleRegistrations.Add(vehicleRegistration);
        }

        public VehicleRegistration GetById(int id)
        {
            return _context.VehicleRegistrations.Find(id);
        }

        public void Update(VehicleRegistration vehicle)
        {
            if(vehicle != null)
                _context.Set<VehicleRegistration>().AddOrUpdate(vehicle);
        }

        public void DeleteVehicleRegistration(int vehicleRegistrationId)
        {
            VehicleRegistration registration = GetById(vehicleRegistrationId);
            if (registration != null)
            {
                _context.VehicleRegistrations.Remove(registration);
            }
        }

        public IEnumerable<VehicleRegistration> GetParkedVehicles(string blockNo, string searchValue, string sortColumnName, string sortDirection)
        {
            IQueryable<VehicleRegistration> parkedVehiclesByBlockNo = _context.VehicleRegistrations
                                                                    .Where(v => v.Status == VehicleRegistrationConstant.Parked && v.BlockNo == blockNo)
                                                                    .ToList()
                                                                    .AsQueryable();

            // Apply searching
            if (!string.IsNullOrEmpty(searchValue))
            {
                parkedVehiclesByBlockNo = parkedVehiclesByBlockNo.Where(x =>
                    x.VehicleRcNoId.ToString().Contains(searchValue.ToLower()) ||
                    x.VehicleRCNo.ToLower().Contains(searchValue.ToLower()) ||
                    x.OwnerName.ToLower().Contains(searchValue.ToLower()) ||
                    x.Status.ToLower().Contains(searchValue.ToLower()) ||
                    x.Model.ToLower().Contains(searchValue.ToLower()));
            }

            // Sorting
            parkedVehiclesByBlockNo = parkedVehiclesByBlockNo.OrderBy(sortColumnName + " " + sortDirection);

            return parkedVehiclesByBlockNo;
            
        }


        public bool UniqueVehicleRcNo(string vehicleRcNo)
        {
            return _context.VehicleRegistrations.Any(x => x.VehicleRCNo == vehicleRcNo);
        }
    }
}
