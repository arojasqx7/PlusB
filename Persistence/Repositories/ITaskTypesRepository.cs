using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Persistence.Repositories
{
    public interface ITaskTypesRepository:IDisposable
    {
        IEnumerable<TaskType> GetTaskTypes();
        TaskType GetTaskTypeByID(int taskId);
        void InsertTaskType(TaskType taskType);
        void DeleteTaskType(int taskId);
        void UpdateTaskType(TaskType taskType);
        void Save();
    }
}
