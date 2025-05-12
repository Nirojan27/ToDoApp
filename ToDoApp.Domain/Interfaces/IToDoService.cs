using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Domain.Interfaces
{
    public interface IToDoService
    {
        void AddTask(string title, string? description, DateTime? dueDate);
        void CompleteTask(int id);
        void IncompleteTask(int id);
        void UpdateTask(int id, string? title, string? description, DateTime? dueDate);
        void DeleteTask(int id);
        List<ToDoItem> GetTasks();
        ToDoItem? GetTaskById(int id);
    }
}
