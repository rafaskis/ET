using SQLite;
using System;

namespace ET.Models
{
    public class DailyRate
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string EmployeeId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal Rate { get; set; }
    }
}