using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Text.Json;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Repositories;

namespace ToDoApp.Infrastructure.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly string _filePath;
        private List<ToDoItem> _tasks = new List<ToDoItem>();

        public ToDoRepository(IConfiguration config)
        {
            _filePath = config["FileSettings:FilePath"] ?? "tasks.json"; //Default fallback
            LoadData();
        }

        private void LoadData()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _tasks = JsonSerializer.Deserialize<List<ToDoItem>>(json) ?? new List<ToDoItem>();
            }
            else
            {
                File.WriteAllText(_filePath, "[]"); // Initialize with an empty JSON array
            }
        }

        private void SaveData()
        {
            var json = JsonSerializer.Serialize(_tasks);
            File.WriteAllText(_filePath, json);
        }

        public void Add(ToDoItem task)
        {
            task.Id = _tasks.Count + 1;
            _tasks.Add(task);

            SaveData();
        }

        public void UpdateStatus(int id, bool isCompleted)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);

            if (task != null) 
                task.IsCompleted = isCompleted;

            SaveData();
        }

        public void Update(int id, string? title, string? description, DateTime? dueDate)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);

            if (task != null)
            {
                task.Title = title ?? task.Title;
                task.Description = description ?? task.Description;
                task.DueDate = dueDate ?? task.DueDate;
            }
            SaveData();
        }

        public void Delete(int id)
        {
            _tasks.RemoveAll(t => t.Id == id);
            SaveData();
        }

        public List<ToDoItem> GetAll(bool? completed = null, bool overdueOnly = false, string sortBy = null)
        {
            var now = DateTime.UtcNow;

            // Filtering Logic
            var filteredTasks = _tasks
                .Where(t => completed == null || t.IsCompleted == completed)
                .Where(t => !overdueOnly || (t.DueDate.HasValue && t.DueDate.Value < now && !t.IsCompleted))
                .ToList();

            // Sorting Logic
            if (sortBy != null)
            {
                var sortedTasks = sortBy.ToLower() switch
                {
                    "title" => filteredTasks.OrderBy(t => t.Title).ToList(),
                    "duedate" => filteredTasks.OrderBy(t => t.DueDate).ToList(),
                    "createddate" => filteredTasks.OrderBy(t => t.CreatedAt).ToList(),
                    _ => filteredTasks.OrderBy(t => t.Id).ToList() // Default: Sort by ID
                };

                return sortedTasks;
            }
            else
            {
                return filteredTasks;
            }
        }

        public ToDoItem? GetById(int id)
        {
            return _tasks.FirstOrDefault(t => t.Id == id);
        }
    }
}
