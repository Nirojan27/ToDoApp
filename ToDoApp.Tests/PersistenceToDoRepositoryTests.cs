using ToDoApp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using ToDoApp.Infrastructure.Repositories;
using Xunit.Abstractions;

namespace ToDoApp.Tests
{
    public class PersistenceToDoRepositoryTests
    {
        private readonly string _testFilePath = "C:\\ToDoList\\ToDoTasks.json";
        private readonly ToDoRepository _repository;
        private readonly ITestOutputHelper _output;

        public PersistenceToDoRepositoryTests(ITestOutputHelper output)
        {
            var configData = new Dictionary<string, string?>
            {
                { "FileSettings:FilePath", _testFilePath }
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(configData)
                .Build();

            _repository = new ToDoRepository(config);
            _output = output;
        }

        [Fact]
        public void AddTask_ShouldWriteToFile()
        {
            // Arrange
            var task = new ToDoItem 
            { 
                Title = "Integration Test Task",
                Description = "This is a test task for integration testing.",
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            // Act
            _repository.Add(task);

            // Assert
            Assert.True(File.Exists(_testFilePath));

            var json = File.ReadAllText(_testFilePath);
            var tasks = JsonSerializer.Deserialize<List<ToDoItem>>(json);
            Assert.Contains(tasks, t => t.Title == "Integration Test Task");
        }

        [Fact]
        public void CompleteTask_ShouldMarkTaskAsCompletedInFile()
        {
            // Arrange
            var task = new ToDoItem { Id = 1, Title = "Task to Complete", Description = "Description", DueDate = DateTime.Now.AddDays(1), IsCompleted = false };
            _repository.Add(task);

            // Act
            _repository.UpdateStatus(task.Id, true);

            // Assert
            var json = File.ReadAllText(_testFilePath);
            var tasks = JsonSerializer.Deserialize<List<ToDoItem>>(json);
            var completedTask = tasks.FirstOrDefault(t => t.Id == task.Id);
            Assert.NotNull(completedTask);
            Assert.True(completedTask.IsCompleted);
        }

        [Fact]
        public void IncompleteTask_ShouldMarkTaskAsIncompleteInFile()
        {
            // Arrange
            var task = new ToDoItem { Id = 2, Title = "Task to Incomplete", Description = "Description", DueDate = DateTime.Now.AddDays(1), IsCompleted = true };
            _repository.Add(task);

            // Act
            _repository.UpdateStatus(task.Id, false);

            // Assert
            var json = File.ReadAllText(_testFilePath);
            var tasks = JsonSerializer.Deserialize<List<ToDoItem>>(json);
            var incompleteTask = tasks.FirstOrDefault(t => t.Id == task.Id);
            Assert.NotNull(incompleteTask);
            Assert.False(incompleteTask.IsCompleted);
        }

        [Fact]
        public void DeleteTask_ShouldRemoveTaskFromFile()
        {
            // Arrange
            var task = new ToDoItem { Id = 1, Title = "Task to Delete" };
            _repository.Add(task);

            // Act
            _repository.Delete(task.Id);

            // Assert
            var json = File.ReadAllText(_testFilePath);
            var tasks = JsonSerializer.Deserialize<List<ToDoItem>>(json);
            Assert.DoesNotContain(tasks, t => t.Id == task.Id);
        }

        [Fact]
        public void GetTasks_ShouldReturnAllTasksFromFile()
        {
            // Arrange
            var initialCount = _repository.GetAll().Count;
            var task1 = new ToDoItem { Title = "Get All Task #1", Description = "Testing GetTasks() #1", DueDate = DateTime.UtcNow.AddDays(1) };
            var task2 = new ToDoItem { Title = "Get All Task #2", Description = "Testing GetTasks() #1", DueDate = DateTime.UtcNow.AddDays(2) };
            _repository.Add(task1);
            _repository.Add(task2);

            // Act
            var tasks = _repository.GetAll();

            // Log output
            _output.WriteLine("Returned Tasks:");
            foreach (var t in tasks)
            {
                _output.WriteLine($"Task ID: {t.Id}, Title: {t.Title}, Description: {t.Description}, DueDate: {t.DueDate}");
            }

            // Assert
            Assert.Equal(initialCount + 2, tasks.Count);
        }

        [Fact]
        public void UpdateTask_ShouldModifyTaskProperties()
        {
            // Arrange
            var task = new ToDoItem { Id = 1, Title = "Old Title" };
            _repository.Add(task);

            // Act
            _repository.Update(task.Id, "New Title", null, null);

            // Assert
            var json = File.ReadAllText(_testFilePath);
            var tasks = JsonSerializer.Deserialize<List<ToDoItem>>(json);
            Assert.Contains(tasks, t => t.Title == "New Title");

            // Log output
            _output.WriteLine($"Returned Tasks: {string.Join(", ", tasks.Select(t => t.Title))}");
        }

        [Fact]
        public void GetTaskById_ShouldReturnCorrectTask()
        {
            // Arrange
            var task = new ToDoItem { Id = 1, Title = "Find Me" };
            _repository.Add(task);

            // Act
            var result = _repository.GetById(task.Id);

            // Log output
            _output.WriteLine($"Returned Tasks: {string.Join(", ", result.Title)}");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Find Me", result.Title);
        }
    }
}
