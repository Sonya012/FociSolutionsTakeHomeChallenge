namespace FociSolutionsTakeHomeChallenge.Models
{
    public class ToDoItem
    {
        public Guid ItemId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
    }
}
