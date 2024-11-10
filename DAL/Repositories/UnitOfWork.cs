using DAL.Context;
using DAL.Repositories.IRepositories;
using System;

namespace DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ParkingDbContext _context = null;
        private bool disposed = false;

        private IVehicleRegistrationRepository _vehicleRegistrationRepository;
        private IParkingAllocationRepository _parkingAllocationRepository;
        private IBlockRepository _blockRepository;

        public UnitOfWork(ParkingDbContext context)
        {
            _context = context;
        }

        public UnitOfWork()
        {
            _context = new ParkingDbContext();
        }

        public IVehicleRegistrationRepository VehicleRegistrationRepository
        {
            get
            {
                return _vehicleRegistrationRepository ?? (_vehicleRegistrationRepository = new VehicleRegistrationRepository(_context));
            }
        }

        public IParkingAllocationRepository ParkingAllocationRepository
        {
            get
            {
                return _parkingAllocationRepository ?? (_parkingAllocationRepository = new ParkingAllocationRepository(_context));
            }
        }

        public IBlockRepository BlockRepository
        {
            get
            {
                return _blockRepository ?? (_blockRepository = new BlockRepository(_context));
            }
        }


        public void SaveChange()
        {
            _context.SaveChanges();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
