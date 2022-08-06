using Microsoft.EntityFrameworkCore;
using Notes.API.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notes.API.Data
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions options):base(options)
        {}

        public DbSet<Note> Notes { get; set; }
    }
}
