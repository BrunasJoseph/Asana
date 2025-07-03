using Asana.Library.Models;
using Asana.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Asana.Maui.ViewModels
{
    public class ProjectDetailViewModel
    {
        public ProjectDetailViewModel() {
            Model = new Project();

            DeleteCommand = new Command(DoDelete);
        }

        public ProjectDetailViewModel(int id)
        {
            Model = ProjectServiceProxy.Current.GetProjectById(id) ?? new Project();
            DeleteCommand = new Command(DoDelete);
        }

        public ProjectDetailViewModel(Project? model)
        {
            Model = model ?? new Project();
            DeleteCommand = new Command(DoDelete);
        }

        public void DoDelete() {
            ProjectServiceProxy.Current.DeleteProject(Model);
        }


        public Project? Model { get; set; }

        public ICommand? DeleteCommand { get; set; }

        public void AddOrUpdateProject()
        {
            ProjectServiceProxy.Current.AddOrUpdate(Model);
        }


    }
}
