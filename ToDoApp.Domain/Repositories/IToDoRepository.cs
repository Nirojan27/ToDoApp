﻿using ToDoApp.Domain.Entities;

namespace ToDoApp.Domain.Repositories
{
    public interface IToDoRepository
    {
        void Add(ToDoItem task);
        void Update(int id, string? title, string? description, DateTime? dueDate);
        void UpdateStatus(int id, bool isCompleted);
        void Delete(int id);
        List<ToDoItem> GetAll(bool? completed = null, bool overdueOnly = false, string sortBy = null);
        ToDoItem? GetById(int id);
    }

}
