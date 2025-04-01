using AutoFixture;
using FociSolutionsTakeHomeChallenge.Interfaces;
using FociSolutionsTakeHomeChallenge.Models;
using FociSolutionsTakeHomeChallenge.Services;
using Moq;
using System.Reflection;

namespace FociSolutionsTakeHomeChallenge.Tests.Services
{
    public class ToDoListServiceUnitTests
    {
        private Mock<IConsole> _mockConsole;
        private ToDoListService _service;
        private readonly Fixture _fixture = new();

        [SetUp]
        public void SetUp()
        {
            _mockConsole = new Mock<IConsole>();
            _service = new ToDoListService(_mockConsole.Object);
        }

        [Test]
        public void Constructor_ShouldThrow_WhenConsoleIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ToDoListService(null));
        }

        [Test]
        public void AddToDoItem_ShouldAddItem_And_LogMessage()
        {
            // Arrange
            var title = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            var dueDate = _fixture.Create<DateTime>();

            // Act
            _service.AddToDoItem(title, description, dueDate);

            //Assert
            _mockConsole.Verify(c => c.WriteLine("The To-Do Item has been added successfully!"), Times.Once);
        }

        [Test]
        public void AddToDoItem_ShouldThrow_When_TitleIsNullOrWhitespace()
        {
            // Arrange
            var description = _fixture.Create<string>();
            var dueDate = _fixture.Create<DateTime>();

            // Act /  Assert
            Assert.Throws<ArgumentException>(() => _service.AddToDoItem(null, description, dueDate));
            Assert.Throws<ArgumentException>(() => _service.AddToDoItem("", description, dueDate));
            Assert.Throws<ArgumentException>(() => _service.AddToDoItem("   ", description, dueDate));
        }

        [Test]
        public void AddToDoItem_ShouldThrow_When_DescriptionIsNullOrWhitespace()
        {
            // Arrange
            var title = _fixture.Create<string>();
            var dueDate = _fixture.Create<DateTime>();

            // Act /  Assert
            Assert.Throws<ArgumentException>(() => _service.AddToDoItem(title, null, dueDate));
            Assert.Throws<ArgumentException>(() => _service.AddToDoItem(title, "", dueDate));
            Assert.Throws<ArgumentException>(() => _service.AddToDoItem(title, "   ", dueDate));
        }

        [Test]
        public void AddToDoItem_ShouldAllowPastDueDates()
        {
            // Arrange
            var title = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            var dueDate = DateTime.Today.AddDays(-5);

            // Act
            _service.AddToDoItem(title, description, dueDate);

            // Assert
            _mockConsole.Verify(c => c.WriteLine("The To-Do Item has been added successfully!"), Times.Once);
        }

        [Test]
        public void AddToDoItem_ShouldIncreaseInternalListCount()
        {
            var title = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            var dueDate = _fixture.Create<DateTime>();

            _service.AddToDoItem(title, description, dueDate);

            var todoItemsField = typeof(ToDoListService)
                .GetField("_todoItems", BindingFlags.NonPublic | BindingFlags.Instance);
            var list = todoItemsField?.GetValue(_service) as List<ToDoItem>;

            Assert.That(list?.Count, Is.EqualTo(1));
        }

        [Test]
        public void UpdateToDoItem_ShouldUpdate_When_ToDoItemExists()
        {
            // Arrange
            var title = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            var dueDate = _fixture.Create<DateTime>();
            var todoItemId = AddTestToDoItem(title, description, dueDate);

            // Act
            _service.UpdateToDoItem(todoItemId, _fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<DateTime>().AddDays(1));

            // Assert
            _mockConsole.Verify(c => c.WriteLine("The To-Do Item has been updated successfully!"), Times.Once);
        }

        [Test]
        public void UpdateToDoItem_ShouldLogNotFound_When_ToDoItemDoesNotExist()
        {
            // Arrange
            Guid todoItemId = Guid.NewGuid();
            var title = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            var dueDate = _fixture.Create<DateTime>();

            // Act
            _service.UpdateToDoItem(todoItemId, title, description, dueDate);

            // Assert
            _mockConsole.Verify(c => c.WriteLine("The To-Do Item was not found."), Times.Once);
        }

        [Test]
        public void UpdateToDoItem_ShouldThrow_When_NewTitleIsNullOrWhitespace()
        {
            // Arrange
            var title = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            var dueDate = _fixture.Create<DateTime>();

            var todoItemId = AddTestToDoItem(title, description, DateTime.Today);

            // Act /  Assert
            Assert.Throws<ArgumentException>(() => _service.UpdateToDoItem(todoItemId, null, description, dueDate));
            Assert.Throws<ArgumentException>(() => _service.UpdateToDoItem(todoItemId, "", description, dueDate));
            Assert.Throws<ArgumentException>(() => _service.UpdateToDoItem(todoItemId, "   ", description, dueDate));
        }

        [Test]
        public void UpdateToDoItem_ShouldThrow_When_NewDescriptionIsNullOrWhitespace()
        {
            // Arrange
            var title = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            var dueDate = _fixture.Create<DateTime>();

            var todoItemId = AddTestToDoItem(title, description, dueDate);

            // Act /  Assert
            Assert.Throws<ArgumentException>(() => _service.UpdateToDoItem(todoItemId, title, null, dueDate));
            Assert.Throws<ArgumentException>(() => _service.UpdateToDoItem(todoItemId, title, "", dueDate));
            Assert.Throws<ArgumentException>(() => _service.UpdateToDoItem(todoItemId, title, "   ", dueDate));
        }

        [Test]
        public void UpdateToDoItem_ShouldChangeItemProperties()
        {
            var title = "Original Title";
            var description = "Original Description";
            var dueDate = DateTime.Today;

            var itemId = AddTestToDoItem(title, description, dueDate);

            var newTitle = "Updated Title";
            var newDescription = "Updated Description";
            var newDueDate = dueDate.AddDays(3);

            _service.UpdateToDoItem(itemId, newTitle, newDescription, newDueDate);

            var todoItemsField = typeof(ToDoListService)
                .GetField("_todoItems", BindingFlags.NonPublic | BindingFlags.Instance);
            var list = todoItemsField?.GetValue(_service) as List<ToDoItem>;
            var updatedItem = list?.FirstOrDefault(i => i.ItemId == itemId);

            Assert.That(updatedItem?.Title, Is.EqualTo(newTitle));
            Assert.That(updatedItem?.Description, Is.EqualTo(newDescription));
            Assert.That(updatedItem?.DueDate, Is.EqualTo(newDueDate));
        }

        [Test]
        public void DeleteToDoItem_ShouldDelete_When_ItemExists()
        {
            // Arrange
            var title = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            var dueDate = _fixture.Create<DateTime>();
            var itemId = AddTestToDoItem(title, description, dueDate);

            // Act
            _service.DeleteToDoItem(itemId);

            // Assert
            _mockConsole.Verify(c => c.WriteLine("The To-Do item has been deleted successfully!"), Times.Once);
        }

        [Test]
        public void DeleteToDoItem_ShouldLogNotFound_When_ToDoItemDoesNotExist()
        {
            // Arrange
            Guid todoItemId = Guid.NewGuid();

            // Act
            _service.DeleteToDoItem(todoItemId);

            // Assert
            _mockConsole.Verify(c => c.WriteLine("The To-Do item was not found."), Times.Once);
        }

        [Test]
        public void MarkToDoItemAsCompleted_ShouldMark_When_ToDoItemExists()
        {
            // Arrange
            var title = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            var dueDate = _fixture.Create<DateTime>();
            var todoItemId = AddTestToDoItem(title, description, dueDate);

            // Act
            _service.MarkToDoItemAsCompleted(todoItemId);

            // Assert
            _mockConsole.Verify(c => c.WriteLine("The To-Do item has been marked as completed!"), Times.Once);
        }

        [Test]
        public void MarkToDoItemAsCompleted_ShouldLogNotFound_When_ToDoItemDoesNotExist()
        {
            // Arrange
            Guid todoItemId = Guid.NewGuid();

            // Act
            _service.MarkToDoItemAsCompleted(todoItemId);

            // Assert
            _mockConsole.Verify(c => c.WriteLine("The To-Do item was not found."), Times.Once);
        }

        [Test]
        public void DisplayToDoItems_ShouldLogNoToDoItems_When_Empty()
        {
            // Act
            _service.DisplayToDoItems();

            // Assert
            _mockConsole.Verify(c => c.WriteLine("No to-do items were found."), Times.Once);
        }

        [Test]
        public void DisplayToDoItems_ShouldLogEachToDoItem_When_ToDoItemsExist()
        {
            // Arrange
            var title = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            var dueDate = _fixture.Create<DateTime>();
            AddTestToDoItem(title, description, dueDate);

            // Act
            _service.DisplayToDoItems();

            // Assert
            _mockConsole.Verify(c => c.WriteLine(It.Is<string>(msg => msg.Contains(title))), Times.Once);
        }

        [Test]
        public void SortToDoItemsByDueDate_ShouldSort_And_LogHeading()
        {
            // Arrange
            var firstTodoItemTitle = _fixture.Create<string>();
            var firstTodoItemDescription = _fixture.Create<string>();
            var firstTodoItemDueDate = _fixture.Create<DateTime>().AddDays(2);
            var secondTodoItemTitle = _fixture.Create<string>();
            var secondTodoItemDescription = _fixture.Create<string>();
            var secondtodoItemDueDate = _fixture.Create<DateTime>().AddDays(1);

            AddTestToDoItem(firstTodoItemTitle, firstTodoItemDescription, firstTodoItemDueDate);
            AddTestToDoItem(secondTodoItemTitle, secondTodoItemDescription, secondtodoItemDueDate);

            // Act
            _service.SortToDoItemsByDueDate();

            // Assert
            _mockConsole.Verify(c => c.WriteLine("The To-Do Items have been sorted by due date:"), Times.Once);
        }

        [Test]
        public void SortToDoItemsByDueDate_ShouldDisplayToDoItemsInOrder()
        {
            // Arrange
            var firstTodoItemTitle = _fixture.Create<string>();
            var firstTodoItemDescription = _fixture.Create<string>();
            DateTime firsttodoItemDueDate = _fixture.Create<DateTime>();
            var secondTodoItemTitle = _fixture.Create<string>();
            var secondTodoItemDescription = _fixture.Create<string>();
            DateTime secondTodoItemDueDate = _fixture.Create<DateTime>();

            AddTestToDoItem(firstTodoItemTitle, firstTodoItemDescription, firsttodoItemDueDate);
            AddTestToDoItem(secondTodoItemTitle, secondTodoItemDescription, secondTodoItemDueDate);

            // Act
            _service.SortToDoItemsByDueDate();

            // Assert
            _mockConsole.Verify(c => c.WriteLine("The To-Do Items have been sorted by due date:"), Times.Once);
            _mockConsole.Verify(c => c.WriteLine(It.Is<string>(msg => msg.Contains(firstTodoItemTitle))), Times.AtLeastOnce);
            _mockConsole.Verify(c => c.WriteLine(It.Is<string>(msg => msg.Contains(secondTodoItemTitle))), Times.AtLeastOnce);
        }

        [Test]
        public void SortToDoItemsByTitle_ShouldDisplayToDoItemsInOrder()
        {
            // Arrange
            var firstTodoItemTitle = "Attend meeting";
            var firstTodoItemDescription = _fixture.Create<string>();
            var firstTodoItemDueDate = _fixture.Create<DateTime>();

            var secondTodoItemTitle = "Buy groceries";
            var secondTodoItemDescription = _fixture.Create<string>();
            var secondTodoItemDueDate = _fixture.Create<DateTime>();

            AddTestToDoItem(secondTodoItemTitle, secondTodoItemDescription, secondTodoItemDueDate);
            AddTestToDoItem(firstTodoItemTitle, firstTodoItemDescription, firstTodoItemDueDate);

            // Act
            _service.SortToDoItemsByTitle();

            // Assert
            _mockConsole.Verify(c => c.WriteLine("The To-Do Items have been sorted by title:"), Times.Once);
            _mockConsole.Verify(c => c.WriteLine(It.Is<string>(msg => msg.Contains(firstTodoItemTitle))), Times.AtLeastOnce);
            _mockConsole.Verify(c => c.WriteLine(It.Is<string>(msg => msg.Contains(secondTodoItemTitle))), Times.AtLeastOnce);
        }

        [Test]
        public void FilterCompletedToDoItems_ShouldLogHeading_AndCompletedOnly()
        {
            // Arrange
            var title = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            var dueDate = _fixture.Create<DateTime>();

            var todoItemId = AddTestToDoItem(title, description, dueDate);
            _service.MarkToDoItemAsCompleted(todoItemId);

            // Act
            _service.FilterCompletedItems();

            // Assert
            _mockConsole.Verify(c => c.WriteLine("The To-Do items have been filtered by completed:"), Times.Once);
            _mockConsole.Verify(c => c.WriteLine(It.Is<string>(msg => msg.Contains("Completed: True"))), Times.AtLeastOnce);
        }

        [Test]
        public void FilterNotCompletedToDoItems_ShouldLogHeading_And_NotCompletedOnly()
        {
            // Arrange
            var title = _fixture.Create<string>();
            var description = _fixture.Create<string>();
            var dueDate = _fixture.Create<DateTime>();

            AddTestToDoItem(title, description, dueDate);

            // Act
            _service.FilterNotCompletedItems();

            // Assert
            _mockConsole.Verify(c => c.WriteLine("The To-Do items have been filtered by not completed:"), Times.Once);
            _mockConsole.Verify(c => c.WriteLine(It.Is<string>(msg => msg.Contains("Completed: False"))), Times.AtLeastOnce);
        }

        [Test]
        public void DisplayToDoItems_ShouldLogMultipleToDoItems_When_ToDoItemsExist()
        {
            // Arrange
            var firstTodoItemTitle = _fixture.Create<string>();
            var firstTodoItemDescription = _fixture.Create<string>();
            DateTime firstTodoItemDueDate = _fixture.Create<DateTime>();
            var secondTodoItemTitle = _fixture.Create<string>();
            var secondTodoItemDescription = _fixture.Create<string>();
            DateTime secondTodoItemDueDate = _fixture.Create<DateTime>();

            AddTestToDoItem(firstTodoItemTitle, firstTodoItemDescription, firstTodoItemDueDate);
            AddTestToDoItem(secondTodoItemTitle, firstTodoItemDescription, firstTodoItemDueDate);

            // Act
            _service.DisplayToDoItems();

            // Assert
            _mockConsole.Verify(c => c.WriteLine(It.Is<string>(msg => msg.Contains(firstTodoItemTitle))), Times.Once);
            _mockConsole.Verify(c => c.WriteLine(It.Is<string>(msg => msg.Contains(secondTodoItemTitle))), Times.Once);
        }

        private Guid AddTestToDoItem(string title, string description, DateTime? dueDate = null)
        {
            Guid itemId = Guid.NewGuid();

            var todoItem = new ToDoItem
            {
                ItemId = itemId,
                Title = title,
                Description = description,
                DueDate = dueDate ?? DateTime.Today,
                Completed = false
            };

            var todoItemsListField = typeof(ToDoListService)
                .GetField("_todoItems", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var list = todoItemsListField?.GetValue(_service) as List<ToDoItem>;
            list?.Add(todoItem);

            return itemId;
        }
    }
}
