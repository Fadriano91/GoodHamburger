using GoodHamburger.Models;

namespace GoodHamburger.Repositories.interfaces
{
    public interface ISandwichRepository
    {
        Task<List<SandwichModel>> SearchAllSandwiches();
        Task<SandwichModel> SearchSandwichById(int id);
        Task<SandwichModel> CreateSandwich(SandwichModel sandwich);

    }
}
