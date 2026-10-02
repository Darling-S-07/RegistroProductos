using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using RegistroProductos.Models;

namespace RegistroProductos.Data
{
    public class SQLiteDatabase
    {
        private SQLiteAsyncConnection _database;

        public SQLiteDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Productos>().Wait();
        }

        //create
        public async Task<int> AddProductosAsync(Productos producto) 
        {
            return await _database.InsertAsync(producto);
        }

        //Read
        public async Task<List<Productos>> GetProductosAsync()
        {
            return await _database.Table<Productos>().ToListAsync();
        }

        //Read by Id
        public async Task<Productos> GetProductosByIdAsync(int id)
        {
            return await _database.Table<Productos>().Where(p => p.IdProducto == id).FirstOrDefaultAsync();
        }

        //update
        public async Task<int> UpdateProductoAsync(Productos producto)
        {
            return await _database.UpdateAsync(producto);
        }

        //Delete
        public async Task<int> DeleteproductoAsync(Productos producto)
        {
            return await _database.DeleteAsync(producto);
        }
    }
}
