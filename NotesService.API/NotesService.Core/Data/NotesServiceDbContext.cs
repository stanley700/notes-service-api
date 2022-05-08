using Microsoft.EntityFrameworkCore;
using NotesService.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesService.Core.Data
{
    public class NotesServiceDbContext : DbContext
    {
        public NotesServiceDbContext(DbContextOptions<NotesServiceDbContext> options)
            :base(options)
        {
            
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
    }
}
