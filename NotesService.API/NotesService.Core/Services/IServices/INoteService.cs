using NotesService.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesService.Core.Services.IServices
{
    public interface INoteService
    {
        Task<Note> Add(Note note);
        Task<bool> Delete(string noteId);
        Task<Note> Update(Note note);
        Task<List<Note>> GetAll(string userId);
        Task<List<List<Note>>> Find(string[] tags);
    }
}
