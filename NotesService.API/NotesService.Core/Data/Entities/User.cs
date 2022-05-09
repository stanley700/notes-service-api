using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NotesService.Core.Data.Entities
{
    public class User : BaseEntity<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public List<Note> Notes { get; set; }
    }
}
