using AutoMapper;
using BAL.ViewModel;
using DAL.Entities;
using DAL.Repositories;
using DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;


namespace BAL.Services
{
    public class VehicleRegistrationService : IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        private bool disposed = false;

        public VehicleRegistrationService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public IEnumerable<VehicleRegistrationViewModel> GetAllVehicle()
        {
            IEnumerable<VehicleRegistration> vehicleList = _unitOfWork.VehicleRegistrationRepository.GetAllVehicle();
            return vehicleList.Select(Mapper.Map<VehicleRegistration, VehicleRegistrationViewModel>).ToList();
        }


        public IEnumerable<VehicleRegistrationViewModel> GetAllUnParkedVehicle()
        {
            IEnumerable<VehicleRegistration> vehicleList = _unitOfWork.VehicleRegistrationRepository.GetAllUnParkedVehicles();
            return vehicleList.Select(Mapper.Map<VehicleRegistration, VehicleRegistrationViewModel>).ToList();
        }

        public object GetRegisterVehicleData(DataTableAjaxPostModel model, string filterVehicle)
        {
            string searchValue = model.Search?.Value;

            if (filterVehicle != null)
            {
                searchValue = filterVehicle;
            }
            if (filterVehicle == "All")
            {
                searchValue = model.Search?.Value;
            }

            int sortColumnIndex = (model.Order != null && model.Order.Count > 0) ? model.Order[0].Column : 0;

            string sortColumnName = (model.Columns != null && model.Columns.Count > sortColumnIndex)
                ? model.Columns[sortColumnIndex]?.Name
                : "VehicleRcNoId";

            string sortDirection = (model.Order != null && model.Order.Count > 0)
                ? model.Order[0].Dir
                : "";

            IEnumerable<VehicleRegistration> registerVehicleData = _unitOfWork.VehicleRegistrationRepository.GetRegisterVehicle(searchValue, sortColumnName, sortDirection);

            int recordsTotal = registerVehicleData.Count();
            List<VehicleRegistration> data = registerVehicleData.Skip(model.Start).Take(model.Length).ToList();

            return new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal, data };
        }


        public object GetParkedVehicles(DataTableAjaxPostModel model, string blockNo)
        {
            string searchValue = model.Search?.Value;

            int sortColumnIndex = (model.Order != null && model.Order.Count > 0) ? model.Order[0].Column : 0;

            string sortColumnName = (model.Columns != null && model.Columns.Count > sortColumnIndex)
                ? model.Columns[sortColumnIndex]?.Name
                : "VehicleRcNoId";

            string sortDirection = (model.Order != null && model.Order.Count > 0)
                ? model.Order[0].Dir
                : "";
            IEnumerable<VehicleRegistration> parkVehicle = _unitOfWork.VehicleRegistrationRepository.GetParkedVehicles(blockNo, searchValue, sortColumnName, sortDirection);
            int recordsTotal = parkVehicle.Count();
            List<VehicleRegistration> data = parkVehicle.Skip(model.Start).Take(model.Length).ToList();

            return new { draw = model.Draw, recordsFiltered = recordsTotal, recordsTotal, data };
        }


        public void RegisterVehicle(VehicleRegistrationCreateModel vehicleRegistrationCreateModel)
        {
            VehicleRegistration vehicle = Mapper.Map<VehicleRegistration>(vehicleRegistrationCreateModel);
            _unitOfWork.VehicleRegistrationRepository.AddVehicleRegistration(vehicle);
            _unitOfWork.SaveChange();
        }

        public void DeleteRegisterVehicle(int vehicleId)
        {
            _unitOfWork.VehicleRegistrationRepository.DeleteVehicleRegistration(vehicleId);
            _unitOfWork.SaveChange();
        }

        public VehicleRegistrationViewModel GetVehicleById(int id)
        {
            VehicleRegistration vehicle = _unitOfWork.VehicleRegistrationRepository.GetById(id);
            return Mapper.Map<VehicleRegistrationViewModel>(vehicle);
        }

        public void UpdateVehicleRegistration(VehicleRegistrationViewModel model)
        {
            VehicleRegistration vehicle = Mapper.Map<VehicleRegistration>(model);
            _unitOfWork.VehicleRegistrationRepository.Update(vehicle);
            _unitOfWork.SaveChange();
        }

        public bool CheckUniqueRcNo(string vehicleRcNo)
        {
            return _unitOfWork.VehicleRegistrationRepository.UniqueVehicleRcNo(vehicleRcNo);
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
