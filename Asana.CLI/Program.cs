using Asana.Library.Models;
using System;

namespace Asana
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var toDos = new List<ToDo>();
            var project = new List<Project>();
            int choiceInt;
            var itemCount = 0;
            var projectCount = 0;
            var toDoChoice = 0;
            do
            {
                Console.WriteLine("Choose a menu option:");
                Console.WriteLine("1. Create a ToDo");
                Console.WriteLine("2. List all ToDos");
                Console.WriteLine("3. List all outstanding ToDos");
                Console.WriteLine("4. Delete a ToDo");
                Console.WriteLine("5. Update a ToDo");
                Console.WriteLine("6. Create a Project");
                Console.WriteLine("7. List all Projects");
                Console.WriteLine("8. List all ToDos in a Project");
                Console.WriteLine("9. Delete a Project");
                Console.WriteLine("10. Update a Project");
                Console.WriteLine("11. Exit");

                var choice = Console.ReadLine() ?? "7";

                if (int.TryParse(choice, out choiceInt))
                {
                    switch (choiceInt)
                    {
                        case 1:
                            Console.Write("Name:");
                            var name = Console.ReadLine();
                            Console.Write("Description:");
                            var description = Console.ReadLine();

                            toDos.Add(new ToDo { Name = name,
                                Description = description,
                                IsCompleted = false,
                                Id = ++itemCount});
                            break;
                        case 2:
                            toDos.ForEach(Console.WriteLine);
                            break;
                        case 3:
                            toDos.Where(t => (t != null) && !(t?.IsCompleted ?? false))
                                .ToList()
                                .ForEach(Console.WriteLine);
                            break;
                        case 4:
                            
                            toDos.ForEach(Console.WriteLine);
                            Console.Write("ToDo to Delete: ");
                            toDoChoice = int.Parse(Console.ReadLine() ?? "0");

                            var reference = toDos.FirstOrDefault(t => t.Id == toDoChoice);
                            if (reference != null)
                            {
                                toDos.Remove(reference);
                            }
                            
                            break;
                        case 5:
                            
                            toDos.ForEach(Console.WriteLine);
                            Console.Write("ToDo to Update: ");
                            toDoChoice = int.Parse(Console.ReadLine() ?? "0");
                            var updateReference = toDos.FirstOrDefault(t => t.Id == toDoChoice);

                            if(updateReference != null)
                            {
                                Console.Write("Name:");
                                updateReference.Name = Console.ReadLine();
                                Console.Write("Description:");
                                updateReference.Description = Console.ReadLine();
                            }

                            break;
                        case 6:
                            Console.Write("Name:");
                            var prjName = Console.ReadLine();
                            Console.Write("Description:");
                            var prjDescription = Console.ReadLine();

                            project.Add(new Project
                            {
                                Name = prjName,
                                Description = prjDescription,
                                CompletePercent = 2,
                                Id = ++projectCount});
                            break;
                        case 7:
                            project.ForEach(Console.WriteLine);
                            break;
                        case 8:
                            project.ForEach(Console.WriteLine);
                            Console.Write("Project to List ToDos for: ");
                            var projectChoice = int.Parse(Console.ReadLine() ?? "0");
                            var projectReference = project.FirstOrDefault(p => p.Id == projectChoice);
                            if (projectReference != null && projectReference.ToDos != null)
                            {
                                projectReference.ToDos.ForEach(Console.WriteLine);
                            } else
                            {
                                Console.WriteLine("No ToDos found for this Project.");
                            }
                            break;
                        case 9:
                            project.ForEach(Console.WriteLine);
                            Console.Write("Project to Delete: ");
                            var projectDeleteChoice = int.Parse(Console.ReadLine() ?? "0");
                            var deleteReference = project.FirstOrDefault(p => p.Id == projectDeleteChoice);
                            if (deleteReference != null)
                            {
                                project.Remove(deleteReference);
                            }
                            break;
                        case 10:
                            project.ForEach(Console.WriteLine);
                            Console.Write("Project to Update: ");
                            var projectUpdateChoice = int.Parse(Console.ReadLine() ?? "0");
                            var updateProjectReference = project.FirstOrDefault(p => p.Id == projectUpdateChoice);
                            if (updateProjectReference != null)
                            {
                                Console.Write("Name:");
                                updateProjectReference.Name = Console.ReadLine();
                                Console.Write("Description:");
                                updateProjectReference.Description = Console.ReadLine();
                                //// Assuming you might want to update the CompletePercent as well
                                //Console.Write("Complete Percent:");
                                //updateProjectReference.CompletePercent = int.Parse(Console.ReadLine() ?? "0");
                            }
                            break;
                        case 11:
                            break;
                        default:
                            Console.WriteLine("ERROR: Unknown menu selection");
                            break;
                    }
                } else
                {
                    Console.WriteLine($"ERROR: {choice} is not a valid menu selection");
                }

            } while (choiceInt != 11);

        }
    }
}