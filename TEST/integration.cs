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
using Xunit;

namespace XUnitTestProject1
{
    public class IntegrationTest
    {
        private HttpClient _client;

        public IntegrationTest()
        {
            var host = new TestServer(
                new WebHostBuilder()
                .UseEnvironment("Testing")
                .UseStartup<Startup>());

            _client = host.CreateClient();

        }

        Note TestNotePost1 = new Note
        {
            
            NoteTitle = "Title-1-Deletable",
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
            var responseBody = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async void Testpost()
        {
            var json = JsonConvert.SerializeObject(TestNotePost1);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            var Response = await _client.PostAsync("api/Notes", stringContent);
            // var ResponseGet = await _client.GetAsync("/api/Notes");
            Response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, Response.StatusCode);
        }

        //[Fact]
        //public async void Testput()
        //{
        //    var json=JsonConvert.SerializeObject()
        //    TestNotePost1.NoteTitle = "edit_title";
        //    var json = JsonConvert.SerializeObject(TestNotePost1);

        //}

    }
}
