using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class NoteService
    {
        //private field
        private readonly Guid _userId;

        //CONSTRUCTOR
        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        //=======CREATE NOTE=======//
        public bool CreateNote(NoteCreate model)
        {
            var entity =
                new Note()
                {
                    OwnerId = _userId,
                    Title = model.Title,
                    Content = model.Content,
                    CreatedUtc = DateTimeOffset.Now
                };

            using(var ctx = new ApplicationDbContext())
            {
                ctx.Notes.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        //=====GET ALL NOTES======//
        public IEnumerable<NoteListItem> GetNotes()
        {
            using(var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                    .Notes
                    .Where(i => i.OwnerId == _userId)
                    .Select(
                        n =>
                        new NoteListItem
                        {
                            NoteId = n.NoteId,
                            Title = n.Title,
                            CreatedUtc = n.CreatedUtc
                        }
                        );
                return query.ToArray();
            }
        }

    }
}
