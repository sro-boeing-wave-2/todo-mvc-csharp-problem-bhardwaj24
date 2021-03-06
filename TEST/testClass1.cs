﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Controllers;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace TEST
{
    public class testClass1
    {
        private readonly NotesController _controller;
        private NotesContext notecontext;
        public testClass1()
        {
            var optionBuilder = new DbContextOptionsBuilder<NotesContext>();
            optionBuilder.UseInMemoryDatabase("any string");
            notecontext = new NotesContext(optionBuilder.Options);
            _controller = new NotesController(notecontext);

            notecontext.Note.AddRange(note);
            notecontext.Note.AddRange(notebyid);
            notecontext.SaveChanges();

        }
        Note TestNotePost = new Note
        {
            NoteTitle = "Title-2",
            NoteContent = "Message-2",
            check = new List<checklist>()
                      {
                          new checklist(){ listcontent = "checklist content 1"},
                          new checklist(){ listcontent = "checklist content 2"}
                      },
            labellist = new List<Labels>()
                      {
                          new Labels(){name = "label content 1"},
                          new Labels(){ name = "label content 2"}
                      },
            pinned = false
        };

        Note notebyid = new Note()
        {
            NoteTitle = "this is test title",
            NoteContent = "some text",
            labellist = new List<Labels>
                {
                    new Labels{ name="My First Tag" },
                    new Labels{ name = "My second Tag" },
                    new Labels{ name = "My third Tag" },
                },
            check = new List<checklist>
                {
                    new checklist{listcontent="first item"},
                    new checklist{listcontent="second item"},
                    new checklist{listcontent="third item"},
                },
            pinned = true
        };

        Note TestNotePut = new Note
        {
            Id = 1,
            NoteTitle = "Title-2",
            NoteContent = "Message-2",
            check = new List<checklist>()
                      {
                          new checklist(){ listcontent = "checklist content 1"},
                          new checklist(){ listcontent = "checklist content 2"}
                      },
            labellist = new List<Labels>()
                      {
                          new Labels(){name = "label content 1"},
                          new Labels(){ name = "label content 2"}
                      },
            pinned = false
        };

        Note note = new Note()
        {
            NoteTitle = "this is test title",
            NoteContent = "some text",
            labellist = new List<Labels>
                {
                    new Labels{ name="My First Tag" },
                    new Labels{ name = "My second Tag" },
                    new Labels{ name = "My third Tag" },
                },
            check = new List<checklist>
                {
                    new checklist{listcontent="first item"},
                    new checklist{listcontent="second item"},
                    new checklist{listcontent="third item"},
                },
            pinned = true
        };

        [Fact]
        public void GetNotes()
        {
            var result = _controller.GetNoteByPrimitive(0, null, null, true);
            var objectresult = result as OkObjectResult;
            var notes = objectresult.Value as List<Note>;
            Assert.Equal(5, notes.Count);
        }

        [Fact]
        public async void TestGetByID()
        {

            var result = _controller.GetNoteByPrimitive(8, null, null, true);
            var objectresult = result as OkObjectResult;
            var notes = objectresult.Value as List<Note>;
            Console.WriteLine(notes);
            Assert.Equal(notes[0], notebyid);
        }

        [Fact]
        public async void Post()
        {
            var response = await _controller.PostNote(TestNotePost);
            var responseOkObject = response as CreatedAtActionResult;
            Note note = responseOkObject.Value as Note;
            Assert.Equal(note.NoteTitle, TestNotePost.NoteTitle);
        }

        [Fact]
        public async void Edit()
        {
            var result = await _controller.PutNote(1, TestNotePut);
            var responseOkObject = result as OkObjectResult;
            var note = responseOkObject.Value as Note;
            Assert.Equal(note.Id, TestNotePut.Id);

        }
        [Fact]
        public async void Delete()
        {
            var result = await _controller.DeleteNote(1, null, null, true);
            Assert.True(result is NoContentResult);
        }


    }
}
