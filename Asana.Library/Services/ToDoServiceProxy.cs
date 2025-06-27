using Asana.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Services
{
    public class ToDoServiceProxy
    {
        private List<ToDo> _toDoList;
        public List<ToDo> ToDos
        {
            get
            {
                return _toDoList.Take(100).ToList();
            }

            private set
            {
                if (value != _toDoList)
                {
                    _toDoList = value;
                }
            }
        }

        private ToDoServiceProxy()
        {
            ToDos = new List<ToDo>
            {
                new ToDo{Id = 1, Name = "Task 1", Description = "My Task 1", IsCompleted=true}
            };
        }

        private static ToDoServiceProxy? instance;

        private int nextKey
        {
            get
            {
                if (ToDos.Any())
                {
                    return ToDos.Select(t => t.Id).Max() + 1;
                }
                return 1;
            }
        }

        public static ToDoServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new ToDoServiceProxy();
                }

                return instance;
            }
        }

        public ToDo? AddOrUpdate(ToDo? toDo)
        {
            if (toDo != null && toDo.Id == 0)
            {
                toDo.Id = nextKey;
                _toDoList.Add(toDo);
            }

            return toDo;
        }

        public void DisplayToDos(bool isShowCompleted = false)
        {
            if (isShowCompleted)
            {
                ToDos.ForEach(Console.WriteLine);
            }
            else
            {
                ToDos.Where(t => (t != null) && !(t.IsCompleted == true))
                     .ToList()
                     .ForEach(Console.WriteLine);
            }
        }

        public ToDo? GetById(int id)
        {
            return ToDos.FirstOrDefault(t => t.Id == id);
        }

        public void DeleteToDo(ToDo? toDo)
        {
            if (toDo == null)
            {
                return;
            }
            _toDoList.Remove(toDo);
        }

        public bool DeleteToDo(int id)
        {
            var todo = _toDoList.FirstOrDefault(t => t.Id == id);
            if (todo != null)
            {
                _toDoList.Remove(todo);
                var project = ProjectServiceProxy.Current.GetAllProjects().FirstOrDefault(p => p.Id == todo.ProjectId);
                project?.ToDos?.RemoveAll(t => t.Id == id);
                ProjectServiceProxy.Current.UpdateProjectCompletion(project);
                return true;
            }
            return false;
        }

    }
}