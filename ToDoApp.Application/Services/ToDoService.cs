using System.Globalization;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Domain.Repositories;

namespace ToDoApp.Application.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _repository;

        public ToDoService(IToDoRepository repository)
        {
            _repository = repository;
        }

        public void AddTask(string title, string? description, DateTime? dueDate)
        {
            var task = new ToDoItem 
            { 
                Title = title, 
                Description = description, 
                DueDate = dueDate 
            };

            _repository.Add(task);
        }

        public void CompleteTask(int id)
        {
            _repository.UpdateStatus(id, true);
        }

        public void IncompleteTask(int id) 
        { 
            _repository.UpdateStatus(id, false); 
        }

        public void UpdateTask(int id, string? title, string? description, DateTime? dueDate)
        {
            _repository.Update(id, title, description, dueDate);
        }

        public void DeleteTask(int id)
        {
            _repository.Delete(id);
        }

        public List<ToDoItem> GetTasks()
        {
            return _repository.GetAll();
        }

        public ToDoItem? GetTaskById(int id) 
        {
            return _repository.GetById(id);
        }

        public List<ToDoItem> GetCompletedTasks()
        {
            return _repository.GetAll(completed: true);
        }

        public List<ToDoItem> GetIncompleteTasks()
        {
            return _repository.GetAll(completed: false);
        }

        public List<ToDoItem> GetOverdueTasks()
        {
            return _repository.GetAll(overdueOnly: true);
        }

        public List<ToDoItem> GetCompletedTasksSortedByTitle(bool ascending = true) {
            return _repository.GetAll(completed: true, sortBy: "title"); 
        }

        public List<ToDoItem> GetIncompleteTasksSortedByDueDate(bool ascending = true)
        {
            return _repository.GetAll(completed: false, sortBy: "title");
        }

        public List<ToDoItem> GetOverdueTasksSortedByCreatedDate(bool ascending = true)
        {
            return _repository.GetAll(overdueOnly: true, sortBy: "createddate");
        }

    }
}
