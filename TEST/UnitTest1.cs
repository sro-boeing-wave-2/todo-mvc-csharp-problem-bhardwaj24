//using System;
//using Xunit;
//using NSuperTest;
//using Microsoft.AspNetCore.Hosting.Server;

//namespace TEST
//{

//    public class UnitTest1
//    {
//        static Server server;

//        [Fact]
//        public void Test1()
//        {
//            server = new Server("https://localhost:44397/api/Notes");
//        }
//        object TestNoteOne = new
//        {
//            Title = "Note One Title",
//            Text = "This is the text for the Test Note One",
//            Pinned = "True"
//        };

//        object TestNoteTwo = new
//        {
//            Title = "Note Two Title",
//            Text = "This is the text for the Test Note Two",
//            Pinned = "True"
//        };
//        [Fact]
//        public void GetTest()
//        {
//            server
//              .Get("/api/Notes")
//              .Expect(200)
//              .End();
//        }

//        [Fact]
//        public void testnotecreate()
//        {
//            server.Post("/api/Notes")
//                .Send(TestNoteOne)
//                .Expect(201)
//                .End();

//        }
//    }
//}
