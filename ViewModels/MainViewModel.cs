using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ET.Models;
using ET.Services;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Maui.Storage;
using System.Windows.Input;
using System;

namespace ET.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly DatabaseService _dbService;

        [ObservableProperty]
        private string employeeId = string.Empty;

        [ObservableProperty]
        private string taskDescription = string.Empty;

        [ObservableProperty]
        private ObservableCollection<EmployeeRecord> records = new();

        public MainViewModel(DatabaseService dbService)
        {
            _dbService = dbService;
            SaveCommand = new AsyncRelayCommand(SaveRecordAsync);
            ExportCommand = new AsyncRelayCommand(ExportDataAsync);
            ImportCommand = new AsyncRelayCommand(ImportDataAsync);
            LoadRecordsCommand = new AsyncRelayCommand(LoadRecordsAsync);
        }

        public ICommand SaveCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand LoadRecordsCommand { get; }

        private async Task SaveRecordAsync()
        {
            if (string.IsNullOrWhiteSpace(EmployeeId) || EmployeeId.Length != 4)
            {
                await Shell.Current.DisplayAlert("Error", "Employee ID must be 4 digits.", "OK");
                return;
            }
            var record = new EmployeeRecord
            {
                EmployeeId = EmployeeId,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                TaskDescription = TaskDescription,
                PaymentStatus = "No"
            };
            await _dbService.SaveRecord(record);
            TaskDescription = string.Empty;
            await LoadRecordsAsync();
        }

        private async Task LoadRecordsAsync()
        {
            if (string.IsNullOrWhiteSpace(EmployeeId) || EmployeeId.Length != 4)
                return;

            var list = await _dbService.GetRecords(EmployeeId);
            Records = new ObservableCollection<EmployeeRecord>(list);
        }

        private async Task ExportDataAsync()
        {
            var list = await _dbService.GetRecords(EmployeeId);
            var json = JsonConvert.SerializeObject(list, Formatting.Indented);
            var filePath = Path.Combine(FileSystem.AppDataDirectory, $"{EmployeeId}_backup.json");
            await File.WriteAllTextAsync(filePath, json);
            await Shell.Current.DisplayAlert("Export", $"Data exported to {filePath}", "OK");
        }

        private async Task ImportDataAsync()
        {
            var result = await FilePicker.Default.PickAsync();
            if (result == null || string.IsNullOrEmpty(result.FullPath)) return;

            var json = await File.ReadAllTextAsync(result.FullPath);
            var importedRecords = JsonConvert.DeserializeObject<List<EmployeeRecord>>(json);
            if (importedRecords == null) return;

            foreach (var record in importedRecords)
                await _dbService.SaveRecord(record);

            await LoadRecordsAsync();
            await Shell.Current.DisplayAlert("Import", "Data imported successfully.", "OK");
        }
    }
}
