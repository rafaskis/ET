using SQLite;
using System;
using System.Collections.Generic;

namespace ET.Models
{
    public class EmployeeRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string EmployeeId { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string TaskDescription { get; set; } = string.Empty;
        public string PaymentStatus { get; set; } = "No";
        public double TotalHours { get; set; }
        public decimal TotalPayment { get; set; }

        [Ignore]
        public List<DailyRate> DailyRates { get; set; } = new();
    }
}