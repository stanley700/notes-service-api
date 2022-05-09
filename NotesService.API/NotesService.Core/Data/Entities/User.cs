using System.Collections.Generic;

namespace NotesService.Core.Data.Entities
{
    public class User : BaseEntity<string>
    {
        public User(string id, string firstname, string lastname, string username, string password, bool isActive, int roleId)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastname;
            Password = password;
            IsActive = isActive;
            RoleId = roleId;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public List<Note> Notes { get; set; }
    }
}
