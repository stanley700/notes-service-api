using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesService.Core.Data.Entities
{
    public class Role : BaseEntity<int>
    {
        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}
