using FociSolutionsTakeHomeChallenge.Interfaces;
using FociSolutionsTakeHomeChallenge.Models;

namespace FociSolutionsTakeHomeChallenge.Services
{
    public class ToDoListService
    {
        private readonly List<ToDoItem> _todoItems = new();
        private readonly IConsole _console;

        /// <summary>
        /// Creates an instance of ToDoListService
        /// </summary>
        /// <param name="console">Console for injection</param>
        public ToDoListService(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
        }

        /// <summary>
        /// Add an To-Do Item to the main To-Do Items list.
        /// </summary>
        /// <param name="title">Title of the To-Do Item</param>
        /// <param name="description">Description of the To-Do Item</param>
        /// <param name="dueDate">The due date of the To-Do Item</param>
        public void AddToDoItem(string title, string description, DateTime dueDate)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("The To-Do Item title cannot be null or empty.", nameof(title));

            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("The To-Do Item description cannot be null or empty.", nameof(description));

            var newToDoItem= new ToDoItem
            {
                ItemId =  Guid.NewGuid(),
                Title = title,
                Description = description,
                DueDate = dueDate,
                Completed = false
            };

            _todoItems.Add(newToDoItem);
            _console.WriteLine("The To-Do Item has been added successfully!");
        }

        /// <summary>
        /// Updates an existing To-Do Item. If not found will return a message.
        /// </summary>
        /// <param name="todoItemId">The To-Do Item Id to be updated</param>
        /// <param name="newTitle">The new title</param>
        /// <param name="newDescription">The new description</param>
        /// <param name="newDueDate">the new due date</param>
        public void UpdateToDoItem(Guid todoItemId, string newTitle, string newDescription, DateTime newDueDate)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("The To-Do Item title cannot be null or empty.", nameof(newTitle));

            if (string.IsNullOrWhiteSpace(newDescription))
                throw new ArgumentException("The To-do Item description cannot be null or empty.", nameof(newDescription));

            var toDoItem = _todoItems.FirstOrDefault(t => t.ItemId == todoItemId);

            if (toDoItem != null)
            {
                toDoItem.Title = newTitle;
                toDoItem.Description = newDescription;
                toDoItem.DueDate = newDueDate;
                _console.WriteLine("The To-Do Item has been updated successfully!");
            }
            else
            {
                _console.WriteLine("The To-Do Item was not found.");
            }
        }

        /// <summary>
        /// Deletes an existing To-Do Item. If not found will return a message.
        /// </summary>
        /// <param name="itemId">The To-Do Item Id</param>
        public void DeleteToDoItem(Guid itemId)
        {
            var todoItem = _todoItems.FirstOrDefault(t => t.ItemId == itemId);

            if (todoItem != null)
            {
                _todoItems.Remove(todoItem);
                _console.WriteLine("The To-Do item has been deleted successfully!");
            }
            else
            {
                _console.WriteLine("The To-Do item was not found.");
            }
        }

        /// <summary>
        /// Marks an existing To-Do Item as completed. If not found will return a message.
        /// </summary>
        /// <param name="itemId">The To-Do Item Id</param>
        public void MarkToDoItemAsCompleted(Guid itemId)
        {
            var toDoItem= _todoItems.FirstOrDefault(t => t.ItemId == itemId);

            if (toDoItem != null)
            {
                toDoItem.Completed = true;
                _console.WriteLine("The To-Do item has been marked as completed!");
            }
            else
            {
                _console.WriteLine("The To-Do item was not found.");
            }
        }

        /// <summary>
        /// Writes out the existing To-Do Items list to the console. 
        /// </summary>
        public void DisplayToDoItems()
        {
            if (_todoItems.Count == 0)
            {
                _console.WriteLine("No to-do items were found.");
                return;
            }

            foreach (var todoItem in _todoItems)
            {
                _console.WriteLine($"ID: {todoItem.ItemId}, Title: {todoItem.Title}, Description: {todoItem.Description}, Due Date: {todoItem.DueDate.ToShortDateString()}, " +
                                  $"Completed: {todoItem.Completed}");
            }
        }

        /// <summary>
        /// Sorts the existing To-Do Items list by the due date.
        /// </summary>
        public void SortToDoItemsByDueDate()
        {
            var sortedItems = _todoItems.OrderBy(t => t.DueDate).ToList();
            _console.WriteLine("The To-Do Items have been sorted by due date:");
            DisplayToDoItems();
        }

        /// <summary>
        /// Sorts the existing To-Do Items list by title.
        /// </summary>
        public void SortToDoItemsByTitle()
        {
            var sortedItems = _todoItems.OrderBy(t => t.Title).ToList();
            _console.WriteLine("The To-Do Items have been sorted by title:");
            DisplayToDoItems();
        }

        /// <summary>
        /// Filters the existing To-Do Items list by if they're completed.
        /// </summary>
        public void FilterCompletedItems()
        {
            var completedTodoItems = _todoItems.Where(t => t.Completed).ToList();
            _console.WriteLine("The To-Do items have been filtered by completed:");

            foreach (var todoItem in completedTodoItems)
            {
                _console.WriteLine($"ID: {todoItem.ItemId}, Title: {todoItem.Title}, Due Date: {todoItem.DueDate.ToShortDateString()}, Completed: {todoItem.Completed}");
            }
        }

        /// <summary>
        ///  Filters the existing To-Do Items list by if they're not completed.
        /// </summary>
        public void FilterNotCompletedItems()
        {
            var noncompletedTodoItems = _todoItems.Where(t => !t.Completed).ToList();
            _console.WriteLine("The To-Do items have been filtered by not completed:");

            foreach (var todoItem in noncompletedTodoItems)
            {
                _console.WriteLine($"ID: {todoItem.ItemId}, Title: {todoItem.Title}, Due Date: {todoItem.DueDate.ToShortDateString()}, Completed: {todoItem.Completed}");
            }
        }
    }
}
