using Cwiczenie4_poprawa.Models;
using Cwiczenie4_poprawa.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cwiczenie4_poprawa.Services
{
    public interface IDbService
    {
        Task<int> AddProductByProcedureAsync(int idProduct, int idWarehouse, int amount, DateTime createdAt);
        Task<bool> CheckOrderIsCompletedAsync(int id);
        Task<bool> CheckProductExistAsync(int id);
        Task<bool> CheckWarehouseExistAsync(int id);
        Task<Order> FindPurchaseOrderAsync(int idProduct, int amount, DateTime createdAt);
        Task<int> GetLastProductWarehouseId();
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductWarehouse>> GetProductsWarehousesAsync();
        Task InsertProductWarehouse(SomeSortOfWarehouse newProductWarehouse);
        Task UpdateOrderAsync(int id, Order order);
    }
}