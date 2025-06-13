using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ET.Models;
using Microsoft.Maui.Storage;

namespace ET.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "employees.db3");
            _database = new SQLiteAsyncConnection(dbPath);
            InitializeDatabase().Wait();
        }

        private async Task InitializeDatabase()
        {
            await _database.CreateTableAsync<EmployeeRecord>();
            await _database.CreateTableAsync<DailyRate>();
        }

        public async Task<List<EmployeeRecord>> GetRecords(string employeeId)
        {
            return await _database.Table<EmployeeRecord>()
                .Where(r => r.EmployeeId == employeeId)
                .OrderBy(r => r.StartTime)
                .ToListAsync();
        }

        public async Task SaveRecord(EmployeeRecord record)
        {
            if (record.Id == 0)
                await _database.InsertAsync(record);
            else
                await _database.UpdateAsync(record);
        }

        public async Task DeleteRecord(EmployeeRecord record)
        {
            await _database.DeleteAsync(record);
        }
    }
}
