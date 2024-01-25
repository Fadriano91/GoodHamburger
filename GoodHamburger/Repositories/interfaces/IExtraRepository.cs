using GoodHamburger.DTO;
using GoodHamburger.Models;

namespace GoodHamburger.Repositories.interfaces
{
    public interface IExtraRepository
    {
        Task<List<ExtraEndDTO>> SearchAllExtras();
        Task<ExtraEndDTO> SearchExtraById(int id);
        Task<ExtraModel> CreateExtra(ExtraModel extra);
        Task<List<int>> SearchExtrasInOrderById(int id);
    }
}
