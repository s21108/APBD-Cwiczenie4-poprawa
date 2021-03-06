using Cwiczenie4_poprawa.Models;
using Cwiczenie4_poprawa.Models.DTO;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

        public async Task <bool> CheckProductExistAsync(int id)
        {
            Product product = new Product();
            using(SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                SqlCommand com = new SqlCommand($"select * from product where IdProduct = {id}",con);
                await con.OpenAsync();
                DbTransaction tran = await con.BeginTransactionAsync();
                com.Transaction = (SqlTransaction)tran;
                SqlDataReader dr = await com.ExecuteReaderAsync();
                try
                {
                    while (dr.Read())
                    {
                        product = new Product { IdProduct = (int)dr["IdProduct"] };
                    }
                    await dr.CloseAsync();
                    await tran.CommitAsync();
                }
                catch(SqlException)
                {
                    await tran.RollbackAsync();
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }
            }

            if (product.IdProduct != 0)
                return true;
            else return false;
        }
        public async Task<bool> CheckWarehouseExistAsync(int id)
        {
            Warehouse warehouse = new Warehouse();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                SqlCommand com = new SqlCommand($"select * from warehouse where IdWarehouse = {id}", con);
                await con.OpenAsync();
                DbTransaction tran = await con.BeginTransactionAsync();
                com.Transaction = (SqlTransaction)tran;
                SqlDataReader dr = await com.ExecuteReaderAsync();
                try
                {
                    while (await dr.ReadAsync())
                    {
                        warehouse = new Warehouse { IdWarehouse = (int)dr["IdWarehouse"] };
                    }
                    await dr.CloseAsync();
                    await tran.CommitAsync();
                    
                }
                catch (SqlException)
                {
                    await tran.RollbackAsync();
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }

            }
            if (warehouse.IdWarehouse != 0)
                return true;
            else return false;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            List<Order> orders = new List<Order>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                SqlCommand com = new SqlCommand($"select * from [order]", con);
                await con.OpenAsync();
                DbTransaction tran = await con.BeginTransactionAsync();
                com.Transaction = (SqlTransaction)tran;
                SqlDataReader dr = await com.ExecuteReaderAsync();
                try
                {
                  
                    while (await dr.ReadAsync())
                    {
                        orders.Add(new Order { IdOrder = (int)dr["IdOrder"], Amount = (int)dr["Amount"], IdProduct = (int)dr["IdProduct"], CreatedAt = (DateTime)dr["CreatedAt"], FulfilledAt = (DateTime)dr["FulfilledAt"] });
                    }
                    await dr.CloseAsync();
                    await tran.CommitAsync();
                }
                catch(SqlException)
                {
                    await tran.RollbackAsync();
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }
            }
            return orders;
        }

        public async Task<IEnumerable<ProductWarehouse>> GetProductsWarehousesAsync()
        {
            List<ProductWarehouse> productWarehouses = new List<ProductWarehouse>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                SqlCommand com = new SqlCommand($"select * from product_warehouse", con);
                await con.OpenAsync();
                DbTransaction tran = await con.BeginTransactionAsync();
                com.Transaction = (SqlTransaction)tran;
                SqlDataReader dr = await com.ExecuteReaderAsync();
                try
                {
                    while (await dr.ReadAsync())
                    {
                        productWarehouses.Add(new ProductWarehouse { IdProductWarehouse = (int)dr["IdProductWarehouse"], IdOrder = (int)dr["IdOrder"] });
                    }
                    await dr.CloseAsync();
                    await tran.CommitAsync();
                    
                }
                catch (SqlException)
                {
                    await tran.RollbackAsync();
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }
            }
            return productWarehouses;
        }

        public async Task<Order> FindPurchaseOrderAsync(int idProduct, int amount, DateTime createdAt)
        {
            var orders = await GetOrdersAsync();
            var order = orders.ToList().Where(x => x.IdProduct == idProduct && x.Amount == amount && x.CreatedAt < createdAt);
            return order.FirstOrDefault();
        }

        public async Task<bool> CheckOrderIsCompletedAsync(int id)
        {
            var orders = await GetProductsWarehousesAsync();
            var order = orders.ToList().Where(x => x.IdOrder == id);
            if (order.Any())
                return true;
            else return false;
        }

        public async Task UpdateOrderAsync(int id, Order order)
        {
            using(SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var com = new SqlCommand($"update [order] set FulfilledAt = @param1 where IdOrder = @param2", con);
                com.Parameters.AddWithValue("@param1",order.FulfilledAt);
                com.Parameters.AddWithValue("@param2", id);
                await con.OpenAsync();
                DbTransaction tran = await con.BeginTransactionAsync();
                com.Transaction = (SqlTransaction)tran;

                try
                {
                    await com.ExecuteNonQueryAsync();
                    await tran.CommitAsync();
                }
                catch (SqlException)
                {
                    await tran.RollbackAsync();
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            List<Product> products = new List<Product>();
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                SqlCommand com = new SqlCommand($"select * from product", con);
                await con.OpenAsync();
                DbTransaction tran = await con.BeginTransactionAsync();
                com.Transaction = (SqlTransaction)tran;
                SqlDataReader dr = await com.ExecuteReaderAsync();
                try
                {
                    while (await dr.ReadAsync())
                    {
                        products.Add(new Product { IdProduct = (int)dr["IdProduct"], Name = dr["Name"].ToString(), Price = (decimal)dr["Price"] });
                    }
                    await dr.CloseAsync();
                    await tran.CommitAsync();
                    
                }
                catch (SqlException)
                {
                    await tran.RollbackAsync();
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }
            }
            return products.Where(x => x.IdProduct == id).FirstOrDefault();

        }

        public async Task InsertProductWarehouse(SomeSortOfWarehouse newProductWarehouse)
        {
            Order order = await FindPurchaseOrderAsync(newProductWarehouse.IdProduct,newProductWarehouse.Amount,newProductWarehouse.CreatedAt);
            Product product = await GetProductByIdAsync(newProductWarehouse.IdProduct);
            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Default")))
            {
                var com = new SqlCommand($"insert into product_warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)" +
                    $"values (@param1, @param2, @param3, @param4, @param5, @param6)",con);
                com.Parameters.AddWithValue("@param1", newProductWarehouse.IdWarehouse);
                com.Parameters.AddWithValue("@param2", newProductWarehouse.IdProduct);
                com.Parameters.AddWithValue("@param3", order.IdOrder);
                com.Parameters.AddWithValue("@param4", newProductWarehouse.Amount);
                com.Parameters.AddWithValue("@param5", product.Price * newProductWarehouse.Amount);
                com.Parameters.AddWithValue("@param6", DateTime.Now);
                await con.OpenAsync();
                DbTransaction tran = await con.BeginTransactionAsync();
                com.Transaction = (SqlTransaction)tran;

                try
                {
                    await com.ExecuteNonQueryAsync();
                    await tran.CommitAsync();
                }
                catch (SqlException)
                {
                    await tran.RollbackAsync();
                }
                catch (Exception)
                {
                    await tran.RollbackAsync();
                }
            }
        }

        public async Task<int> GetLastProductWarehouseId()
        {
            var productWarehouses = await GetProductsWarehousesAsync();
            var lastInt = productWarehouses.LastOrDefault().IdProductWarehouse;
            return lastInt;
        }

        public async Task<int> AddProductByProcedureAsync(int idProduct, int idWarehouse, int amount, DateTime createdAt)
        {
            int primaryKey = -1;
            using var con = new SqlConnection(_configuration.GetConnectionString("Default"));
            await con.OpenAsync(); 
            SqlCommand command = con.CreateCommand();
            DbTransaction tran = await con.BeginTransactionAsync();
            try
            {
                command.Transaction = (SqlTransaction)tran;
                command.Connection = con;
                command.CommandText = "AddProductToWarehouse";
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@IdProduct", idProduct);
                command.Parameters.AddWithValue("@IdWarehouse", idWarehouse);
                command.Parameters.AddWithValue("@Amount", amount);
                command.Parameters.AddWithValue("@CreatedAt", createdAt);
                primaryKey = int.Parse((await command.ExecuteScalarAsync()).ToString());
                await tran.CommitAsync();
            }
            catch (SqlException)
            {
                await tran.RollbackAsync();
            }
            catch (Exception)
            {
                await tran.RollbackAsync();
            }

            return primaryKey;
        }
    }
}
