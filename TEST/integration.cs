using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Xml;
using WebApplication1;
using WebApplication1.Models;
using FluentAssertions;
using Xunit;

namespace XUnitTestProject1
{
    public class IntegrationTest
    {
        private HttpClient _client;
        private NotesContext _context;

        public IntegrationTest()
        {
            var host = new TestServer(
                new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>());

            _context = host.Host.Services.GetService(typeof(NotesContext)) as NotesContext;
            _client = host.CreateClient();

            List<Note> TestNoteProper1 = new List<Note> { new Note()
            {

                NoteTitle = "this is test title",
                NoteContent = "some text",
                check = new List<checklist>()
                      {
                          new checklist(){ listcontent = "checklist-1"},
                          new checklist(){ listcontent = "checklist-2"}
                      },
                labellist = new List<Labels>()
                      {
                          new Labels(){name = "Label-1-Deletable"},
                          new Labels(){name = "Label-2-Deletable"}
                      },
                pinned = true
            } };
            _context.Note.AddRange(TestNoteProper1);
            _context.Note.AddRange(TestNoteDelete);
            _context.SaveChanges();

        }
        Note TestNotePut = new Note
        {

            NoteTitle = "First Title",
            NoteContent = "Message-1-Deletable",
            check = new List<checklist>()
                      {
                          new checklist(){ listcontent = "checklist-1"},
                          new checklist(){ listcontent = "checklist-2"}
                      },
            labellist = new List<Labels>()
                      {
                         new Labels(){name = "Label-1-Deletable"},
                          new Labels(){name = "Label-2-Deletable"}
                      },
            pinned = false
        };
        Note TestNotePost1 = new Note
        {
            
            NoteTitle = "Second Title",
            NoteContent = "Message-1-Deletable",
            check = new List<checklist>()
                      {
                          new checklist(){ listcontent = "checklist-1"},
                          new checklist(){ listcontent = "checklist-2"}
                      },
            labellist = new List<Labels>()
                      {
                          new Labels(){name = "Label-1-Deletable"},
                          new Labels(){name = "Label-2-Deletable"}
                      },
            pinned = false
        };

        Note TestNoteDelete = new Note
        {

            NoteTitle = "this is deleted title",
            NoteContent = "Message-1-Deletable",
            check = new List<checklist>()
                      {
                          new checklist(){ listcontent = "checklist-1"},
                          new checklist(){ listcontent = "checklist-2"}
                      },
            labellist = new List<Labels>()
                      {
                          new Labels(){name = "Label-1-Deletable"},
                          new Labels(){name = "Label-2-Deletable"}
                      },
            pinned = false
        };
        [Fact]
        public async void TestGetRequest()
        {
            var response = await _client.GetAsync("/api/Notes");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseBody);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var responseString = await response.Content.ReadAsStringAsync();
            //var notes = JsonConvert.DeserializeObject<List<Note>>(responseString);
            responseString.Should().Contain("this is test title")
              .And.Contain("some text")
              .And.Contain("true");
        }

        [Fact]
        public async void PostData()
        {
            HttpRequestMessage postMessage = new HttpRequestMessage(HttpMethod.Post, "api/Notes")
            {
                Content = new StringContent(JsonConvert.SerializeObject(TestNotePost1), UnicodeEncoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(postMessage);



            var responseString = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Note>(responseString);



            response.EnsureSuccessStatusCode();

            Assert.True(TestNotePost1.IsEquals(obj));

        }

        [Fact]
        public async void Testput()
        {
            HttpRequestMessage putMessage = new HttpRequestMessage(HttpMethod.Put, "api/Notes/1")
            {
                Content = new StringContent(JsonConvert.SerializeObject(TestNotePut), UnicodeEncoding.UTF8, "application/json")
            };
            var response = await _client.SendAsync(putMessage);
            var responseString = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Note>(responseString);

            //var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            //var Response = await _client.PutAsync("api/Notes/1", stringContent);
            //var responsedata = r.StatusCode;
            Assert.True(TestNotePut.IsEquals(obj));
        }
        [Fact]
        public async void TestDelete()
        {
            var response = await _client.DeleteAsync("api/Notes?Title=this is deleted title");
            var responsecode = response.StatusCode;
            Assert.Equal(HttpStatusCode.NoContent, responsecode);
            _context.Note.Should().NotContain("this is deleted title");

        }

    }
}
