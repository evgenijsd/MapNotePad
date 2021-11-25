using MapNotePad.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MapNotePad.Services.Repository
{
    public class Repository : IRepository
    {
        private Lazy<SQLiteAsyncConnection> _database;

        public Repository()
        {
            _database = new Lazy<SQLiteAsyncConnection>(() =>
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "mapnotepad.db3");
                SQLiteAsyncConnection database = new SQLiteAsyncConnection(path);

                database.CreateTableAsync<Users>().Wait();
                database.CreateTableAsync<PinModel>().Wait();

                return database;
            });
        }

        #region -- Public helpers --
        public async Task<int> AddAsync<T>(T entity) where T : IEntity, new() =>
            await _database.Value.InsertAsync(entity);

        public async Task<List<T>> GetAllAsync<T>() where T : IEntity, new() =>
            await _database.Value.Table<T>().ToListAsync();

        public async Task<T> GetByIdAsync<T>(int id) where T : IEntity, new() =>
            await _database.Value.GetAsync<T>(id);

        public async Task<int> RemoveAsync<T>(T entity) where T : IEntity, new() =>
            await _database.Value.DeleteAsync(entity);

        public async Task<int> UpdateAsync<T>(T entity) where T : IEntity, new() =>
            await _database.Value.UpdateAsync(entity);

        public async Task<T> FindAsync<T>(Expression<Func<T, bool>> expression) where T : IEntity, new() =>
            await _database.Value.FindAsync<T>(expression);

        public async Task<List<T>> GetAsync<T>(Expression<Func<T, bool>> expression) where T : IEntity, new() =>
            await _database.Value.Table<T>().Where(expression).ToListAsync();
        #endregion

    }
}
