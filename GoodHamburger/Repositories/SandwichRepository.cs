using GoodHamburger.Data;
using GoodHamburger.Models;
using GoodHamburger.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Repositories
{
    public class SandwichRepository : ISandwichRepository
    {
        private readonly GoodHamburgerDbContext _dbContext;
        public SandwichRepository(GoodHamburgerDbContext goodHamburgerDbContext) 
        {
            _dbContext = goodHamburgerDbContext;
        }

        public async Task<List<SandwichModel>> SearchAllSandwiches()
        {
            return await _dbContext.Sandwiches.ToListAsync();
        }

        public async Task<SandwichModel> CreateSandwich(SandwichModel sandwich)
        {
            await _dbContext.Sandwiches.AddAsync(sandwich);
            await _dbContext.SaveChangesAsync();

            return sandwich;
        }

        public async Task<SandwichModel> SearchSandwichById(int id)
        {
            return await _dbContext.Sandwiches.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
