using Cwiczenie4_poprawa.Models;
using Cwiczenie4_poprawa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Cwiczenie4_poprawa.Controllers
{
    [Route("api/warehouses")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IDbService _service;

        public WarehousesController(IDbService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult PostProductInWarehouse(ProductWarehouse productWarehouse)
        {
            bool checkProdukt = _service.CheckProductExist(productWarehouse.IdProduct);
            bool checkWarehouse = _service.CheckWarehouseExist(productWarehouse.IdWarehouse);
            if (!checkProdukt)
                return NotFound($"Nie znaleziono produktu o id {productWarehouse.IdProduct}");
            if(!checkWarehouse)
                return NotFound($"Nie znaleziono magazynu o id {productWarehouse.IdWarehouse}");
            if (productWarehouse.Amount <= 0)
                return BadRequest($"Wartość Amount musi być większa od 0");

            Order order = _service.FindPurchaseOrder(productWarehouse.IdProduct, productWarehouse.Amount, productWarehouse.CreatedAt);
            if (order == null)
                return NotFound($"Nie znaleziono zamówienia, które spełnia warunki.");

            bool checkOrderIsCompleted = _service.CheckOrderIsCompleted(productWarehouse.IdOrder);
            if (checkOrderIsCompleted)
                return Ok("Zlecenie zostało już zrealizowane");


  
            return Created("",productWarehouse);
        }
    }
}
