using GoodHamburger.DTO;
using GoodHamburger.Models;
using GoodHamburger.Repositories.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtraController : ControllerBase
    {
        private readonly IExtraRepository _extraRepository;
        public ExtraController(IExtraRepository extraRepository)
        {
            _extraRepository = extraRepository;
        }

        [HttpGet("SearchAllExtras")]
        public async Task<ActionResult<List<ExtraEndDTO>>> SearchAllExtras()
        {
            List<ExtraEndDTO> extras = await _extraRepository.SearchAllExtras();
            return Ok(extras);
        }

        [HttpPost("CreateExtra")]
        public async Task<ActionResult<ExtraModel>> CreateExtra(ExtraDTO extraDto)
        {
            var extraModel = new ExtraModel();
            extraModel.Name = extraDto.Name;
            extraModel.Price = extraDto.Price;
            ExtraModel extra = await _extraRepository.CreateExtra(extraModel);
            return Ok(extra);
        }
    }
}
