namespace NotesService.Core.Data.Entities
{
    public class Note : BaseEntity<string>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Tags { get; set; }
        public bool IsDeleted { get; set; }
    }
}
