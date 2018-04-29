using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTfulTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MakeRequest();

            Console.ReadLine();
        }

        static async void MakeRequest()
        {
            try
            {
                var listResp = await RESTful.RequestHandler.MakeRequestForList<User>("https://jsonplaceholder.typicode.com/posts");
                var objResp = await RESTful.RequestHandler.MakeRequest<User>("https://jsonplaceholder.typicode.com/posts/1");
                var intResp = await RESTful.RequestHandler.MakeRequest<int>("http://localhost:55779/api/values/GetInt");
                var boolResp = await RESTful.RequestHandler.MakeRequest<bool>("http://localhost:55779/api/values/GetBool");
                var nullResp = await RESTful.RequestHandler.MakeRequest<bool>("http://localhost:55779/api/values/GetNull");
                var nullResp2 = await RESTful.RequestHandler.MakeRequest<User>("http://localhost:55779/api/values/GetNull");
                var formattedResponse = await RESTful.RequestHandler.MakeRequest("http://localhost:55779/api/values/GetFormattedResponse", formattedResponse: true);

                var intTest = await RESTful.RequestHandler.MakeRequest<int>("http://localhost:55779/api/values/GetInt");
                var boolTest = await RESTful.RequestHandler.MakeRequest<bool>("http://localhost:55779/api/values/GetBool");
                var nullTest = await RESTful.RequestHandler.MakeRequest<string>("http://localhost:55779/api/values/GetNull");
                var nullTest2 = await RESTful.RequestHandler.MakeRequest<int>("http://localhost:55779/api/values/GetNull");


                var binTest = await RESTful.RequestHandler.MakeRequest(
                    "http://requestbin.fullcontact.com/1oqahqk1?urlParam=test&2ndUrlParam=test2", RESTful.RequestMethod.DELETE,
                    new Dictionary<string, object>()
                {
                         {"TestBody", DateTime.Now }
                },
                    new Dictionary<string, string>()
                    {
                        {"Authentication", "auth header" },
                        {"Test", "test header" }
                    },
                    30,
                    files: new List<RESTful.FileParameter>()
                    {
                        new RESTful.FileParameter()
                        {
                            File = File.ReadAllBytes(@"C:\SpotlightImages\7809011bb4cc0a360a9146be98a87cf0b3fe561452b8456467782dc8074f684e.jpg"),
                            FileName = "Spotlightimage.jpg",
                            ParamName = "SpotlightImg"
                        }
                    }
                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public class User
        {
            public int userId { get; set; }
            public int id { get; set; }
            public string title { get; set; }
            public string body { get; set; }
        }
    }
}
