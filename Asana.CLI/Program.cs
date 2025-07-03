using Asana.Library.Models;
using Asana.Library.Services;
using System;

namespace Asana
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var toDoSvc = ToDoServiceProxy.Current;
            var projectSvc = ProjectServiceProxy.Current;
            int choiceInt;
            do
            {
                Console.WriteLine("Choose a menu option:");
                Console.WriteLine("1. Create a Project");
                Console.WriteLine("2. List all Projects");
                Console.WriteLine("3. Delete a Project");
                Console.WriteLine("4. Update a Project");
                Console.WriteLine("5. Create a ToDo");
                Console.WriteLine("6. List all ToDos");
                Console.WriteLine("7. List all outstanding ToDos");
                Console.WriteLine("8. Delete a ToDo");
                Console.WriteLine("9. Update a ToDo");
                Console.WriteLine("10. List all ToDos in a Project");
                Console.WriteLine("11. Exit");

                var choice = Console.ReadLine() ?? "7";

                if (int.TryParse(choice, out choiceInt))
                {
                    switch (choiceInt)
                    {
                        case 1: // Create a Project
                            Console.Write("Project Name: ");
                            var pname = Console.ReadLine();
                            Console.Write("Project Description: ");
                            var pdesc = Console.ReadLine();

                            var project = new Project
                            {
                                Id = 0,
                                Name = pname,
                                Description = pdesc,
                                CompletePercent = 0
                            };
                            projectSvc.AddOrUpdate(project);
                            break;

                        case 2: // List all Projects
                            foreach (var p in projectSvc.GetAllProjects())
                            {
                                Console.WriteLine($"{p.Id}: {p.Name} - {p.CompletePercent}% complete");
                            }
                            break;

                        case 3: // Delete a Project
                            Console.Write("Project ID to delete: ");
                            int delId = int.Parse(Console.ReadLine());
                            projectSvc.DeleteProject(delId);
                            break;

                        case 4: // Update a Project
                            Console.Write("Project ID to update: ");
                            int updId = int.Parse(Console.ReadLine());
                            Console.Write("New Name: ");
                            var newName = Console.ReadLine();
                            Console.Write("New Description: ");
                            var newDesc = Console.ReadLine();
                            projectSvc.UpdateProject(new Project { Id = updId, Name = newName, Description = newDesc });
                            break;

                        case 5: // Create a ToDo
                            Console.Write("Name: ");
                            var name = Console.ReadLine();
                            Console.Write("Description: ");
                            var description = Console.ReadLine();
                            Console.Write("Priority (1=Low, 2=Medium, 3=High): ");
                            var priority = Console.ReadLine();
                            Console.Write("Project ID: ");
                            int pid = int.Parse(Console.ReadLine());

                            var todo = new ToDo
                            {
                                Id = 0,
                                Name = name,
                                Description = description,
                                Priority = 0,
                                IsCompleted = false,
                                ProjectId = pid
                            };

                            toDoSvc.AddOrUpdate(todo);
                            var proj = projectSvc.GetAllProjects().Find(p => p.Id == pid);
                            if (proj != null)
                            {
                                proj.ToDos ??= new List<ToDo>();
                                proj.ToDos.Add(todo);
                                projectSvc.UpdateProjectCompletion(proj);
                            }
                            break;

                        case 6: // List all ToDos 
                            toDoSvc.DisplayToDos(true);
                            break;

                        case 7: // List all outstanding ToDos
                            toDoSvc.DisplayToDos();
                            break;
                        case 8: // Delete a ToDo
                            toDoSvc.DisplayToDos(true);
                            Console.Write("ToDo to Delete: ");
                            var toDoChoice4 = int.Parse(Console.ReadLine() ?? "0");

                            var reference = toDoSvc.GetById(toDoChoice4);
                            toDoSvc.DeleteToDo(reference);
                            break;
                        case 9: // Update a ToDo
                            toDoSvc.DisplayToDos(true);
                            Console.Write("ToDo to Update: ");
                            var toDoChoice5 = int.Parse(Console.ReadLine() ?? "0");
                            var updateReference = toDoSvc.GetById(toDoChoice5);

                            if (updateReference != null)
                            {
                                Console.Write("Name: ");
                                updateReference.Name = Console.ReadLine();

                                Console.Write("Description: ");
                                updateReference.Description = Console.ReadLine();

                                Console.Write("Priority (1=Low, 2=Medium, 3=High): ");
                                var priorityInput = Console.ReadLine();
                                if (int.TryParse(priorityInput, out int parsedPriority))
                                {
                                    updateReference.Priority = parsedPriority;
                                }
                                else
                                {
                                    Console.WriteLine("Invalid input. Priority not updated.");
                                }

                                Console.Write("Is Completed (true/false): ");
                                var completedInput = Console.ReadLine();
                                updateReference.IsCompleted = bool.TryParse(completedInput, out bool completed)
                                    ? completed
                                    : false;
                            }

                            toDoSvc.AddOrUpdate(updateReference);
                            break;

                        case 10: // List all ToDos in a Project
                            Console.Write("Enter Project ID: ");
                            int pidView = int.Parse(Console.ReadLine());
                            var todos = projectSvc.GetToDosByProject(pidView);
                            foreach (var t in todos)
                            {
                                Console.WriteLine($"{t.Id}: {t.Name} | Done: {t.IsCompleted}");
                            }
                            break;
                        case 11: // Exit
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