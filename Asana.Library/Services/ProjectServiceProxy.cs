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
        private List<Project> _projects;

        public List<Project> Projects
        {
            get
            {
                return _projects.Take(100).ToList();
            }
            private set
            {
                if (value != _projects)
                {
                    _projects = value;
                }
            }
        }
        private List<ToDo> _toDoList => ToDoServiceProxy.Current.ToDos;

        private static ProjectServiceProxy? instance;


        private int nextProjectKey
        {
            get
            {
                if (_projects.Any())
                {
                    return _projects.Select(p => p.Id).Max() + 1;
                }
                return 1;
            }
        }

        private ProjectServiceProxy()
        {
            Projects = new List<Project>
            {
                new Project { Id = 1, Name = "Sample Project", Description = "This is a sample project." },
                new Project { Id = 2, Name = "Another Project", Description = "This is another sample project." }
            };
        }


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

        public Project? AddOrUpdate(Project? proj)
        {
            if (proj != null && proj.Id == 0)
            {
                proj.Id = nextProjectKey;
                _projects.Add(proj);
            }

            return proj;
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

        public void DeleteProject(Project? project)
        {
            if (project == null)
            {
                return;
            }
            _projects.Remove(project);
        }

        public Project? GetProjectById(int id)
        {
            return _projects.FirstOrDefault(p => p.Id == id);
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
