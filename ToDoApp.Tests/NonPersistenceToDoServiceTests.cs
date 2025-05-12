using Moq;
using ToDoApp.Application.Services;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Repositories;

namespace ToDoApp.Tests
{
    public class NonPersistenceToDoServiceTests
    {
        private readonly Mock<IToDoRepository> _mockRepo;
        private readonly ToDoService _service;

        public NonPersistenceToDoServiceTests()
        {
            _mockRepo = new Mock<IToDoRepository>();
            _service = new ToDoService(_mockRepo.Object);
        }

        [Fact]
        public void AddTask_ShouldAddNewTask()
        {
            // Arrange
            var title = "Test Task";
            var description = "Test Description";
            var dueDate = DateTime.UtcNow.AddDays(1);

            // Act
            _service.AddTask(title, description, dueDate);

            // Assert
            _mockRepo.Verify(r => r.Add(It.IsAny<ToDoItem>()), Times.Once);
        }

        [Fact]
        public void CompleteTask_ShouldMarkTaskAsCompleted()
        {
            // Arrange
            var taskId = 1;

            // Act
            _service.CompleteTask(taskId);

            // Assert
            _mockRepo.Verify(r => r.UpdateStatus(taskId, true), Times.Once);
        }

        [Fact]
        public void IncompleteTask_ShouldMarkTaskAsCompleted()
        {
            // Arrange
            var taskId = 1;

            // Act
            _service.IncompleteTask(taskId);

            // Assert
            _mockRepo.Verify(r => r.UpdateStatus(taskId, false), Times.Once);
        }

        [Fact]
        public void UpdateTask_ShouldModifyTaskProperties()
        {
            // Arrange
            var taskId = 1;
            var newTitle = "Updated Task";
            var newDescription = "Updated Description";
            var newDueDate = DateTime.UtcNow.AddDays(3);

            _service.UpdateTask(taskId, newTitle, newDescription, newDueDate);

            // Assert
            _mockRepo.Verify(r => r.Update(taskId, newTitle, newDescription, newDueDate), Times.Once);
        }

        [Fact]
        public void DeleteTask_ShouldRemoveTask()
        {
            // Arrange
            var taskId = 1;

            // Act
            _service.DeleteTask(taskId);

            // Assert
            _mockRepo.Verify(r => r.Delete(taskId), Times.Once);
        }

        [Fact]
        public void GetTasks_ShouldReturnAllTasks()
        {
            // Arrange
            var tasks = new List<ToDoItem>
            {
                new ToDoItem { Id = 1, Title = "Task 1" },
                new ToDoItem { Id = 2, Title = "Task 2" }
            };

            _mockRepo.Setup(r => r.GetAll(null, false)).Returns(tasks);

            // Act
            var result = _service.GetTasks();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.Title == "Task 1");
            Assert.Contains(result, t => t.Title == "Task 2");
        }

        [Fact]
        public void GetTaskById_ShouldReturnCorrectTask()
        {
            // Arrange
            var taskId = 1;
            var expectedTask = new ToDoItem { Id = taskId, Title = "Test Task" };

            _mockRepo.Setup(r => r.GetById(taskId)).Returns(expectedTask);

            // Act
            var result = _service.GetTaskById(taskId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTask.Id, result.Id);
            Assert.Equal(expectedTask.Title, result.Title);
        }

        [Fact]
        public void GetTaskById_ShouldReturnNull_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = 99; // Non-existent task ID

            _mockRepo.Setup(r => r.GetById(taskId)).Returns((ToDoItem?)null);

            // Act
            var result = _service.GetTaskById(taskId);

            // Assert
            Assert.Null(result);
        }
    }   
}
