﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class NotesContext : DbContext
    {
        public NotesContext (DbContextOptions<NotesContext> options)
            : base(options)
        {
        }

        public DbSet<WebApplication1.Models.Note> Note { get; set; }
        public DbSet<WebApplication1.Models.checklist> checklist { get; set; }
        public DbSet<WebApplication1.Models.Labels> Labels { get; set; }
    }
}
