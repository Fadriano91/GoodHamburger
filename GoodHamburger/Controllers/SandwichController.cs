using GoodHamburger.DTO;
using GoodHamburger.Models;
using GoodHamburger.Repositories;
using GoodHamburger.Repositories.interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SandwichController : ControllerBase
    {
        private readonly ISandwichRepository _sandwichRepository;
        private readonly IExtraRepository _extraRepository;

        public SandwichController(ISandwichRepository sandwichRepository, IExtraRepository extraRepository)
        {
            _sandwichRepository = sandwichRepository;
            _extraRepository = extraRepository;
        }

        [HttpGet("SearchAllSandwiches")]
        public async Task<ActionResult<List<SandwichModel>>> SearchAllSandwiches()
        {
            List<SandwichModel> sandwiches = await _sandwichRepository.SearchAllSandwiches();
            return Ok(sandwiches);
        }

        [HttpPost("CreateSandwich")]
        public async Task<ActionResult<SandwichModel>> CreateSandwich(SandwichDTO sandwidto)
        {
            SandwichModel sandwichModel = new SandwichModel();
            sandwichModel.Name = sandwidto.Name;
            sandwichModel.Price = sandwidto.Price;
            SandwichModel sandwich = await _sandwichRepository.CreateSandwich(sandwichModel);
            return Ok(sandwich);
        }

        [HttpGet("SearchAllSandwichesAndExtras")]
        public async Task<IActionResult> GetAllSandwichesWithExtras()
        {
            var sandwiches = await _sandwichRepository.SearchAllSandwiches();
            var extras = await _extraRepository.SearchAllExtras();

            return Ok(new { Sandwiches = sandwiches, Extras = extras });
        }
    }


}
