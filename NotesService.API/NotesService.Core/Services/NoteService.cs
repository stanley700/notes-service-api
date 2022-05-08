using Microsoft.EntityFrameworkCore;
using NotesService.Core.Data;
using NotesService.Core.Data.Entities;
using NotesService.Core.Exceptions;
using NotesService.Core.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesService.Core.Services
{
    public class NoteService : INoteService
    {
        private NotesServiceDbContext _dbContext;
        public NoteService(NotesServiceDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Note> Add(Note note)
        {
            await ValidateNoteDetails(note);

            var noteObj = await _dbContext.AddAsync(note);
            await _dbContext.SaveChangesAsync();

            return noteObj?.Entity;
        }

        private async Task ValidateNoteDetails(Note note)
        {
            if (note == null)
            {
                throw new MessageException(SystemCodes.InvalidRequest, "Note details is required");
            }

            if (string.IsNullOrEmpty(note.UserId))
            {
                throw new MessageException(SystemCodes.InvalidRequest, "User Id is required");
            }

            var existingNote = await _dbContext.Notes.AnyAsync(n => n.UserId == note.UserId && n.Title == n.Title && !note.IsDeleted);
            if (existingNote)
            {
                throw new MessageException(SystemCodes.NoteAlreadyExist, "Note with the same title already exist");
            }
        }

        public async Task<bool> Delete(string noteId)
        {
            var noteObj = await _dbContext.Notes.Where(n => !n.IsDeleted && n.Id == noteId).FirstOrDefaultAsync();

            if(noteObj == null)
            {
                throw new MessageException(SystemCodes.DataNotFound, "Note not found");
            }

            noteObj.IsDeleted = true;
            _dbContext.Notes.Update(noteObj);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<List<Note>>> Find(string[] tags)
        {
            //Todo::Find by tags
            throw new NotImplementedException();
        }

        public async Task<List<Note>> GetAll(string userId)
        {
            var notes = await _dbContext.Notes.Where(n => n.UserId == userId).ToListAsync();

            return notes;
        }

        public async Task<Note> Update(Note note)
        {
            await ValidateNoteDetails(note);


            var noteObj = _dbContext.Notes.Update(note);
            await _dbContext.SaveChangesAsync();

            return noteObj?.Entity;
        }
    }
}
