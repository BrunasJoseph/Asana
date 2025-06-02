namespace Asana.CLI.Models
{
    public class ToDo
    {
        private string? name;

        public string? Name
        {
            get
            {
                return name;
            }

            set
            {
                if(name != value)
                {
                    name = value;
                }
            }
        }
        private string? description;
        private bool? isDone;
        private int? priority;

        public ToDo()
        {
            
        }

    }
}