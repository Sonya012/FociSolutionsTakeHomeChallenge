using FociSolutionsTakeHomeChallenge.Interfaces;
using FociSolutionsTakeHomeChallenge.Services;

try
{
    IConsole _console = new SystemConsole();
    ToDoListService _toDoListService = new ToDoListService(_console);
    bool _isRunning = true;

    while (_isRunning)
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(@"
                            To-Do List Application
                            ======================
                            1. Add New To-Do Item
                            2. Display The To-Do List
                            3. Update One To-Do Item
                            4. Delete One To-Do Item
                            5. Mark One To-Do Item as Completed
                            6. Sort To-Do Items By Due Date
                            7. Sort To-Do Items By Title
                            8. Filter To-Do Items By Completed
                            9. Filter To-Do Items By Not Completed
                            10. Exit The To-Do Application
                            ");

        Console.Write("Enter a selection: ");

        int action;
        string input = Console.ReadLine();

        if (!(int.TryParse(input, out action) && action >= 1 && action <= 10))
        {
            Console.WriteLine("Please enter a valid number between 1 and 10.");
        }

        switch (action)
        {
            case 1:
                string title;
                do
                {
                    Console.Write("Enter The To-Do Item Title: ");
                    title = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(title))
                    {
                        Console.WriteLine("The To-Do Item title cannot be empty. Please try again.");
                    }

                } while (string.IsNullOrWhiteSpace(title));


                string description;
                do
                {
                    Console.Write("Enter The To-Do Item Description: ");
                    description = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(description))
                    {
                        Console.WriteLine("The To-Do Item description cannot be empty. Please try again.");
                    }

                } while (string.IsNullOrWhiteSpace(description));


                DateTime dueDate;
                while (true)
                {
                    Console.Write("Enter The To-Do Item Due Date (YYYY-MM-DD): ");
                    if (DateTime.TryParse(Console.ReadLine(), out dueDate))
                    {
                        break;
                    }

                    Console.WriteLine("Invalid date format. Please enter the date in YYYY-MM-DD format.");
                }

                _toDoListService.AddToDoItem(title, description, dueDate);

                break;
            case 2:
                _toDoListService.DisplayToDoItems();

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                break;
            case 3:
                Guid updateId;
                while (true)
                {
                    Console.Write("Enter To-Do Item ID to be updated: ");
                    if (Guid.TryParse(Console.ReadLine(), out updateId))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid Item ID, it should be in GUID format.");
                }

                string updateTitle;
                do
                {
                    Console.Write("Enter The New To-Do Item Title: ");
                    updateTitle = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(updateTitle))
                    {
                        Console.WriteLine("The To-Do Item title cannot be empty. Please try again.");
                    }

                } while (string.IsNullOrWhiteSpace(updateTitle));

                string updateDescription;
                do
                {
                    Console.Write("Enter The New To-Do Item Description: ");
                    updateDescription = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(updateDescription))
                    {
                        Console.WriteLine("The To-Do Item description cannot be empty. Please try again.");
                    }

                } while (string.IsNullOrWhiteSpace(updateDescription));

                DateTime updateDueDate;
                while (true)
                {
                    Console.Write("Enter The New To-Do Item Due Date (YYYY-MM-DD): ");
                    if (DateTime.TryParse(Console.ReadLine(), out updateDueDate))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid date format. Please enter the date in YYYY-MM-DD format.");
                }

                _toDoListService.UpdateToDoItem(updateId, updateTitle, updateDescription, updateDueDate);
                break;
            case 4:
                Console.Write("Enter The To-Do Item ID to be deleted: ");
                Guid deleteId;
                if (!Guid.TryParse(Console.ReadLine(), out deleteId))
                {
                    Console.WriteLine("Invalid To-Do Item ID, it should be in GUID format.");
                    return;
                }

                _toDoListService.DeleteToDoItem(deleteId);
                break;
            case 5:
                Console.Write("Enter The To-Do Item ID to be marked as completed: ");
                Guid completedId;
                if (!Guid.TryParse(Console.ReadLine(), out completedId))
                {
                    Console.WriteLine("Invalid To-Do Item ID, it should be in GUID format.");
                    return;
                }

                _toDoListService.MarkToDoItemAsCompleted(completedId);
                break;
            case 6:
                _toDoListService.SortToDoItemsByDueDate();

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                break;
            case 7:
                _toDoListService.SortToDoItemsByTitle();

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                break;
            case 8:
                _toDoListService.FilterCompletedItems();

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                break;
            case 9:
                _toDoListService.FilterNotCompletedItems();

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                break;
            case 10:
                _isRunning = false;
                break;
            default:
                Console.WriteLine("Invalid option, please try again.");
                break;
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An unexpected error has occurred: {ex.Message}");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}