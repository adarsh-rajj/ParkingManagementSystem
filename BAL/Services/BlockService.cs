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
    public class BlockService : IDisposable
    {
        private readonly IUnitOfWork _unitOfWork;
        private bool disposed = false;

        public BlockService()
        {
            _unitOfWork = new UnitOfWork();
        }

        public IEnumerable<BlockViewModel> GetAllBlock()
        {
            IEnumerable<ParkingBlock> blockList = _unitOfWork.BlockRepository.GetAllBlocks();
            return blockList.Select(Mapper.Map<ParkingBlock, BlockViewModel>).ToList();
        }

        public void UpdateBlock(BlockViewModel model)
        {
            ParkingBlock block = Mapper.Map<ParkingBlock>(model);
            _unitOfWork.BlockRepository.Update(block);
            _unitOfWork.SaveChange();
        }
        
        public BlockViewModel GetBlockDetailsByBlockNo(string blockNo)
        { 
            ParkingBlock block = _unitOfWork.BlockRepository.GetBlockDetailByBlockNo(blockNo);
            if(block != null) 
                return Mapper.Map<ParkingBlock, BlockViewModel>(block) ;
            return null;
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
