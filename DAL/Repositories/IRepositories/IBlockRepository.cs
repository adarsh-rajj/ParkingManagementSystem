using DAL.Entities;
using System;
using System.Collections.Generic;

namespace DAL.Repositories.IRepositories
{
    public interface IBlockRepository : IDisposable
    {
        IEnumerable<ParkingBlock> GetAllBlocks();
        ParkingBlock GetBlockDetailByBlockNo(string blockNo);
        void Update(ParkingBlock block);
    }
}
