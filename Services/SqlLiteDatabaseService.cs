using QanooniRishta.Models;
using SQLite;

namespace QanooniRishta.Services
{
    public class SqlLiteDatabaseService
    {
        private readonly SQLiteAsyncConnection _db;

        public SqlLiteDatabaseService()
        {
            var appDataDir = FileSystem.AppDataDirectory;

            //var oldDbPath = Path.Combine(appDataDir, "qanooniRishta.db");

            //if (File.Exists(oldDbPath))
            //{
            //    File.Delete(oldDbPath);
            //}

            var newDbPath = Path.Combine(appDataDir, "qanooniRishta.db");

            _db = new SQLiteAsyncConnection(newDbPath);
        }


        public async Task InitAsync<T>() where T : new()
        {
            await _db.CreateTableAsync<T>();
        }

        // Get all records of T
        public Task<List<T>> GetAllAsync<T>() where T : new()
        {
            return _db.Table<T>().ToListAsync();
        }

        // Insert a record of T
        public Task<int> AddAsync<T>(T item)
        {
            return _db.InsertAsync(item);
        }

        // Delete a record of T
        public Task<int> DeleteAsync<T>(T item)
        {
            return _db.DeleteAsync(item);
        }

        // Update a record of T
        public Task<int> UpdateAsync<T>(T item)
        {
            return _db.UpdateAsync(item);
        }

        // Get by ID (assumes primary key named "Id")
        public Task<T> GetByIdAsync<T>(object id) where T : new()
        {
            return _db.FindAsync<T>(id);
        }
    }
}