using System;
using System.Collections.Generic;

namespace WhiskyApp.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private WhiskyDbContext context;

        public UnitOfWork(WhiskyDbContext context,
            IDestilleryRepository destilleryRepository,
            IWhiskyRepository whiskyRepository
        )
        {
            this.context = context;

            this.WhiskyRepository = whiskyRepository;
            this.DestilleryRepository = destilleryRepository;
        }

        public IWhiskyRepository WhiskyRepository { get; private set; }
        public IDestilleryRepository DestilleryRepository { get; private set; }

        public void Save()
        {
            context.SaveChanges();
        }

        public void SaveAsync()
        {
            context.SaveChangesAsync();
        }
    }
}