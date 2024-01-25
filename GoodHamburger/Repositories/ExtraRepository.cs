using GoodHamburger.Data;
using GoodHamburger.DTO;
using GoodHamburger.Models;
using GoodHamburger.Repositories.interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Repositories
{
    public class ExtraRepository : IExtraRepository
    {
        private readonly GoodHamburgerDbContext _dbContext;
        public ExtraRepository(GoodHamburgerDbContext goodHamburgerDbContext)
        {
            _dbContext = goodHamburgerDbContext;
        }

        public async Task<List<ExtraEndDTO>> SearchAllExtras()
        {
            var extraEndDtoList = new List<ExtraEndDTO>();

            var extraModels = await _dbContext.Extras.ToListAsync();

            foreach (var extra in extraModels)
            {
                var extraEndDto = new ExtraEndDTO();
                extraEndDto.Id = extra.Id;
                extraEndDto.Name = extra.Name;
                extraEndDto.Price = extra.Price;

                extraEndDtoList.Add(extraEndDto);
            }

            return extraEndDtoList;
        }

        public async Task<ExtraModel> CreateExtra(ExtraModel extra)
        {
            await _dbContext.Extras.AddAsync(extra);
            await _dbContext.SaveChangesAsync();
            return extra;
        }

        public async Task<ExtraEndDTO> SearchExtraById(int id)
        {
            var extrabd = await _dbContext.Extras.FirstOrDefaultAsync(x => x.Id == id);

            if (extrabd == null)
            {
                throw new Exception($"Extra with Id {extrabd.Id} not found");
            }

            var extraDTO = new ExtraEndDTO
            {
                Id = extrabd.Id,
                Name = extrabd.Name,
                Price = extrabd.Price
            };

            return extraDTO;
        }

        public async Task<List<int>> SearchExtrasInOrderById(int id)
        {
            await Task.Delay(100);

             var extrasForOrder =  _dbContext.OrderExtras.Where(oe => oe.OrderId == id)
            .Select(oe => oe.ExtraId)
            .ToList();

            return extrasForOrder;
        }
    }
}
