using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotesService.API.Models
{
    public class AddNoteModel
    {
        public string Id { get; set; }
        [Required]
        public string Tile { get; set; }
        [Required]
        public string Body { get; set; }
        public string Tags { get; set; }
        public bool IsPublic { get; set; }
    }
}
