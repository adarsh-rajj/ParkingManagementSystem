using DAL.Context;
using DAL.Entities;
using DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.Remoting.Contexts;

namespace DAL.Repositories
{
    public class BlockRepository : IBlockRepository, IDisposable
    {
        private readonly ParkingDbContext _context;

        public BlockRepository(ParkingDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ParkingBlock> GetAllBlocks()
        {
            return _context.ParkingBlocks;
        }
        
        public ParkingBlock GetBlockDetailByBlockNo(string blockNo)
        {
            return _context.ParkingBlocks.FirstOrDefault(x => x.BlockNo == blockNo);
        }

        public void Update(ParkingBlock block)
        {
            if (block != null) 
                _context.Set<ParkingBlock>().AddOrUpdate(block);
        }

        private bool disposed = false;

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

