using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastructure.DataAccess.Repositories
{
    public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
    {
        private readonly MyRecipeBookContext dbContext;
        public UserRepository(MyRecipeBookContext dbContext) => this.dbContext = dbContext;

        public async Task Add(User user) => await dbContext.AddAsync(user);

        public async Task<bool> ExistActiveUserWithEmail(string email) => await dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);
    

}
}
