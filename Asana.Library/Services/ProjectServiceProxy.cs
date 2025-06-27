using Asana.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asana.Library.Services
{
    public class ProjectServiceProxy
    {
        private List<Project> _projects = new();
        private List<ToDo> _toDoList => ToDoServiceProxy.Current.ToDos;

        private static ProjectServiceProxy? instance;

        private int nextProjectKey => _projects.Any() ? _projects.Max(p => p.Id) + 1 : 1;


        public static ProjectServiceProxy Current
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProjectServiceProxy();
                }
                return instance;
            }
        }


        public Project AddProject(string name, string description)
        {
            var project = new Project
            {
                Id = nextProjectKey,
                Name = name,
                Description = description
            };
            _projects.Add(project);
            return project;
        }

        public bool DeleteProject(int id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project != null)
            {
                _projects.Remove(project);
                _toDoList.RemoveAll(t => t.ProjectId == id);
                return true;
            }
            return false;
        }

        public List<Project> GetAllProjects() => _projects;

        public bool UpdateProject(Project updatedProject)
        {
            var project = _projects.FirstOrDefault(p => p.Id == updatedProject.Id);
            if (project != null)
            {
                project.Name = updatedProject.Name;
                project.Description = updatedProject.Description;
                return true;
            }
            return false;
        }

        public List<ToDo> GetToDosByProject(int projectId)
        {
            return _toDoList.Where(t => t.ProjectId == projectId).ToList();
        }

        public void UpdateProjectCompletion(Project? project)
        {
            if (project == null || project.ToDos == null || project.ToDos.Count == 0)
            {
                if (project != null) project.CompletePercent = 0;
                return;
            }

            var todos = project.ToDos ?? new List<ToDo>();
            double completed = todos.Count(t => t.IsCompleted == true);
            project.CompletePercent = (int)((completed / (double)todos.Count) * 100);
        }
    }
}
