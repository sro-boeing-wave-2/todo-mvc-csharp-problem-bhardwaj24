﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string NoteTitle { get; set; }
        public string NoteContent { get; set; }
        public bool pinned { get; set; }
        public List<checklist> check { get; set; }
        public List<Labels> labellist { get; set; }

        public bool IsEquals(Note n)
        {

            if (NoteTitle == n.NoteTitle && NoteContent == n.NoteContent && pinned == n.pinned && labellist.All(x => n.labellist.Exists(y => y.name == x.name)) && check.All(x => n.check.Exists(y => (y.listcontent == x.listcontent))))
                return true;

            return false;


        }
    }

    public class checklist
    {
        [Key]
       public int checklistId { get; set; }
        public string listcontent { get; set; }
    }

    public class Labels
    {
        [Key]
        public int LabelId { get; set; }
        public string name { get; set; }
    }
}
