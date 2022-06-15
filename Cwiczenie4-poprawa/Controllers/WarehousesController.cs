using Cwiczenie4_poprawa.Models;
using Cwiczenie4_poprawa.Models.DTO;
using Cwiczenie4_poprawa.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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
        public async Task<IActionResult> PostProductInWarehouse(SomeSortOfWarehouse productWarehouse)
        {
            //sprawdzenie produktu
            bool checkProdukt = await _service.CheckProductExistAsync(productWarehouse.IdProduct);
            bool checkWarehouse = await _service.CheckWarehouseExistAsync(productWarehouse.IdWarehouse);
            if (!checkProdukt)
                return NotFound($"Nie znaleziono produktu o id {productWarehouse.IdProduct}");
            if(!checkWarehouse)
                return NotFound($"Nie znaleziono magazynu o id {productWarehouse.IdWarehouse}");
            if (productWarehouse.Amount <= 0)
                return BadRequest($"Wartość Amount musi być większa od 0");

            //wyszukanie zamówienia spełniającego wymagania z naszym żądaniem
            Order order = await _service.FindPurchaseOrderAsync(productWarehouse.IdProduct, productWarehouse.Amount, productWarehouse.CreatedAt);
            if (order == null)
                return NotFound($"Nie znaleziono zamówienia, które spełnia warunki.");

            //sprawdzenie czy zamówienie nie zostało już zrealizowane
            bool checkOrderIsCompleted = await _service.CheckOrderIsCompletedAsync(order.IdOrder);
            if (checkOrderIsCompleted)
                return Ok("Zlecenie zostało już zrealizowane");

            //aktualizacja kolumny fulfilledAt
            await _service.UpdateOrderAsync(order.IdOrder, order);

            //wstawiamy rekord do tabeli Product_Warehouse
            await _service.InsertProductWarehouse(productWarehouse);

            ProductWarehouse pw = new ProductWarehouse { IdProductWarehouse = await _service.GetLastProductWarehouseId() };
  
            return Created("",pw.IdProductWarehouse);
        }
    }
}
