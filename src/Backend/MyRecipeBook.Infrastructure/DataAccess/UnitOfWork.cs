using MyRecipeBook.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastructure.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyRecipeBookContext dbContext;
        public UnitOfWork(MyRecipeBookContext dbContext) => this.dbContext = dbContext;

        public async Task Commit() => await dbContext.SaveChangesAsync();

    }
}
