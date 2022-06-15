using Cwiczenie4_poprawa.Models.DTO;
using Cwiczenie4_poprawa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cwiczenie4_poprawa.Controllers
{
    [Route("api/warehouses2")]
    [ApiController]
    public class Warehouses2Controller : ControllerBase
    {
        private readonly IDbService _service;

        public Warehouses2Controller(IDbService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> PostProductInWarehouse(SomeSortOfWarehouse someSortOfWarehouse)
        {
            var result = await _service.AddProductByProcedureAsync(someSortOfWarehouse.IdProduct, someSortOfWarehouse.IdWarehouse, someSortOfWarehouse.Amount, someSortOfWarehouse.CreatedAt);
            if (result != -1)
                return Created("", result);
            return BadRequest("Bład wykonania zapytania");
        }
    }
}
