using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoreWeb_lesson1
{
    internal class Program
    {
        private static readonly string _url = "https://jsonplaceholder.typicode.com/posts/";
        private static readonly string _fileName = "result.txt";
        private static async Task Main(string[] args)
        {
            var tasks = new List<Task<Post>>();
            for (int i = 4; i <= 13; i++)
            {
                tasks.Add(GetPostAsync(i));
            }

            await Task.WhenAll(tasks);

            string startupPath = System.IO.Directory.GetCurrentDirectory();
            string path = startupPath+"\\"+_fileName;
  
                using (var sw = new StreamWriter(path, true))
                {
                    
                    foreach (var post in tasks)
                    {
                        sw.WriteLine("");
                        sw.WriteLine(post.Result.userId);
                        sw.WriteLine(post.Result.id);
                        sw.WriteLine(post.Result.title);
                        sw.WriteLine(post.Result.body);
                    }
                }
          


            
        }

        private static async Task<Post> GetPostAsync(int id)
        {
            string data = null;
            Post post = new Post();

            using (WebClient client = new WebClient())
            {
                try
                {
                    data = await client.DownloadStringTaskAsync(new Uri(_url + id));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            if (data != null)
            {
                Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
                try
                {
                    post = await JsonSerializer.DeserializeAsync<Post>(stream);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            return post;
        }
    }
}
