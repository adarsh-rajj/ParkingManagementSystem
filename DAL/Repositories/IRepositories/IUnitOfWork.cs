using System;

namespace DAL.Repositories.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IVehicleRegistrationRepository VehicleRegistrationRepository { get; }
        IParkingAllocationRepository ParkingAllocationRepository { get; }
        IBlockRepository BlockRepository { get; }
        void SaveChange();
    }
}
