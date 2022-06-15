using Cwiczenie4_poprawa.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenie4_poprawa.Services
{
    public class DbService : IDbService
    {
        private readonly IConfiguration _configuration;

        public DbService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool CheckProductExist(int id)
        {
            Product product = new Product();
            using(SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                SqlCommand com = new SqlCommand($"select * from product where IdProduct = {id}",con);
                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    product = new Product { IdProduct = (int)dr["IdProduct"] };
                }
            }
            if (product.IdProduct != 0)
                return true;
            else return false;
        }
        public bool CheckWarehouseExist(int id)
        {
            Warehouse warehouse = new Warehouse();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                SqlCommand com = new SqlCommand($"select * from warehouse where IdWarehouse = {id}", con);
                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    warehouse = new Warehouse { IdWarehouse = (int)dr["IdWarehouse"] };
                }
            }
            if (warehouse.IdWarehouse != 0)
                return true;
            else return false;
        }

        public IEnumerable<Order> GetOrders()
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                SqlCommand com = new SqlCommand($"select * from order", con);
                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    orders.Add(new Order { IdOrder = (int)dr["IdOrder"], Amount = (int)dr["Amount"], IdProduct = (int)dr["IdProduct"] });
                }
            }
            return orders;
        }

        public IEnumerable<ProductWarehouse> GetProductsWarehouses()
        {
            List<ProductWarehouse> productWarehouses = new List<ProductWarehouse>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                SqlCommand com = new SqlCommand($"select * from product_warehouse", con);
                con.Open();
                SqlDataReader dr = com.ExecuteReader();
                while (dr.Read())
                {
                    productWarehouses.Add(new ProductWarehouse { IdProductWarehouse = (int)dr["IdProductWarehouse"], IdOrder = (int)dr["IdOrder"] });
                }
            }
            return productWarehouses;
        }

        public Order FindPurchaseOrder(int idProduct, int amount, DateTime createdAt)
        {
            var order = GetOrders().ToList().Where(x => x.IdProduct == idProduct && x.Amount == amount && x.CreatedAt < createdAt);
            return order.FirstOrDefault();
        }

        public bool CheckOrderIsCompleted(int id)
        {
            var order = GetProductsWarehouses().ToList().Where(x => x.IdOrder == id);
            if (order.Any())
                return true;
            else return false;
        }

        
    }
}
