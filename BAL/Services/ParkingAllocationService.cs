using AutoMapper;
using BAL.ViewModel;
using BOL.Constant;
using DAL.Entities;
using DAL.Repositories;
using DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BAL.Services
{
    public class ParkingAllocationService : IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly BlockService _blockService;
        private readonly VehicleRegistrationService _vehicleService;
        private bool disposed = false;

        public ParkingAllocationService()
        {
            _unitOfWork = new UnitOfWork();
            _blockService = new BlockService();
            _vehicleService = new VehicleRegistrationService();
        }

        public object GetParkingAllocationData(DataTableAjaxPostModel model, string blockNo)
        {
            string searchValue = model.Search?.Value;

            if (blockNo != null)
            {
                searchValue = blockNo;
            }
            if (blockNo == "All")
            {
                searchValue = model.Search?.Value;
            }

            int sortColumnIndex = (model.Order != null && model.Order.Count > 0) ? model.Order[0].Column : 0;

            string sortColumnName = (model.Columns != null && model.Columns.Count > sortColumnIndex)
                ? model.Columns[sortColumnIndex]?.Name
                : "BlockNo";

            string sortDirection = (model.Order != null && model.Order.Count > 0)
                ? model.Order[0].Dir
                : "";

            IQueryable<ParkingAllotment> parkingAllocationData = _unitOfWork.ParkingAllocationRepository.GetParking(searchValue, sortColumnName, sortDirection);

            int recordsTotal = parkingAllocationData.Count();
            List<ParkingAllotment> data = parkingAllocationData.Skip(model.Start).Take(model.Length).ToList();

            return new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal, data };
        }

        public void AddParking(ParkingCreateViewModel parkingCreateViewModel)
        {
            ParkingAllotment parking = Mapper.Map<ParkingAllotment>(parkingCreateViewModel);
            BlockViewModel block = _blockService.GetBlockDetailsByBlockNo(parkingCreateViewModel.BlockNo);

            if (block.Capacity != 0 && block != null)
            {
                _unitOfWork.ParkingAllocationRepository.AddParking(parking);
                _unitOfWork.SaveChange();

                // Update vehicle registration status and blockNo
                VehicleRegistrationViewModel vehicle = _vehicleService.GetVehicleById(parkingCreateViewModel.VehicleRcNoId);
                if (vehicle != null)
                {
                    vehicle.BlockNo = parkingCreateViewModel.BlockNo;
                    vehicle.Status = VehicleRegistrationConstant.Parked;
                    _vehicleService.UpdateVehicleRegistration(vehicle);
                }

                // Update block capacity
                block.Capacity -= 1;
                _blockService.UpdateBlock(block);
                 
            }
        }

        public ParkingAllocationViewModel GetAllocationById(int id)
        {
            ParkingAllotment allocation = _unitOfWork.ParkingAllocationRepository.GetById(id);
            return Mapper.Map<ParkingAllocationViewModel>(allocation);
        }

        public void UpdateParkingAllocation(ParkingAllocationViewModel model)
        {
            ParkingAllotment vehicle = Mapper.Map<ParkingAllotment>(model);
            _unitOfWork.ParkingAllocationRepository.Update(vehicle);
            _unitOfWork.SaveChange();

            // Update vehicle registration blockNo
            VehicleRegistrationViewModel vehicles = _vehicleService.GetVehicleById(model.VehicleRcNoId);
            if (vehicle != null)
            {
                vehicle.BlockNo = model.BlockNo;
                _vehicleService.UpdateVehicleRegistration(vehicles);
            }

            // Update block capacity
            BlockViewModel block = _blockService.GetBlockDetailsByBlockNo(model.BlockNo);
            BlockViewModel previousBlock = _blockService.GetBlockDetailsByBlockNo(model.PreviousBlock);

            if (block.BlockId != previousBlock.BlockId)
            {
                block.Capacity -= 1;
                previousBlock.Capacity += 1;
                _blockService.UpdateBlock(block);
                _blockService.UpdateBlock(previousBlock);
            }
        }

        public int GetVehicleId(int allocationId)
        {
            return _unitOfWork.ParkingAllocationRepository.GetVehicleRCNo(allocationId);
        }

        public string GetBlockNoById(int allocationId)
        {
            return _unitOfWork.ParkingAllocationRepository.GetBlockNo(allocationId);
        }

        public void DeleteParkingAllocation(int allocationId)
        {
            // Update vehicle registration status
            int VehicleRcNoId = GetVehicleId(allocationId);
            VehicleRegistrationViewModel vehicle = _vehicleService.GetVehicleById(VehicleRcNoId);
            if (vehicle != null)
            {
                vehicle.BlockNo = VehicleRegistrationConstant.None;
                vehicle.Status = VehicleRegistrationConstant.NotParked;
                _vehicleService.UpdateVehicleRegistration(vehicle);
            }

            // Update block capacity
            string blockNo = GetBlockNoById(allocationId);
            BlockViewModel block = _blockService.GetBlockDetailsByBlockNo(blockNo);
            if (block != null)
            {
                block.Capacity += 1;
                _blockService.UpdateBlock(block);
            }
            _unitOfWork.ParkingAllocationRepository.DeleteParkingAllocation(allocationId);
            _unitOfWork.SaveChange();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _unitOfWork?.Dispose();  
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
