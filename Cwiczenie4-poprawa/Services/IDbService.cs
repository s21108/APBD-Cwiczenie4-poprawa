using Cwiczenie4_poprawa.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cwiczenie4_poprawa.Services
{
    public interface IDbService
    {
        bool CheckOrderIsCompleted(int id);
        bool CheckProductExist(int id);
        bool CheckWarehouseExist(int id);
        Order FindPurchaseOrder(int idProduct, int amount, DateTime createdAt);
        IEnumerable<ProductWarehouse> GetProductsWarehouses();
    }
}