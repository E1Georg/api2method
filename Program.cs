using API_T2.Data;
using API_T2.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;


namespace API_T2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var data = getDataFromSite();
            sendDataToBD(data);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static List<Person> getDataFromSite()
        {
            var data = new List<Person>();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://testlodtask20172.azurewebsites.net/task");
            httpWebRequest.ContentType = "text/json";
            httpWebRequest.Method = "GET";          
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            // Получение списка жителей, но без возраста каждого жителя. Используется для получения id каждого жителя.
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var answer = streamReader.ReadToEnd();             
                var result = JsonConvert.DeserializeObject<List<ConvertPerson>>(answer);                
                string url;

                foreach (var item in result)
                {
                    url = "http://testlodtask20172.azurewebsites.net/task/" + item.id;
                    httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.ContentType = "text/json";
                    httpWebRequest.Method = "GET";
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    // Получение списка жителей, но уже со всеми данными, с указанием возраста(по id). 
                    using (var streamReaderFullList = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var answerWithFullData = streamReaderFullList.ReadToEnd();
                        var resultWithFullData = JsonConvert.DeserializeObject<ConvertPerson>(answerWithFullData);
                        data.Add(new Person { name = resultWithFullData.name, Age = resultWithFullData.Age, sex = resultWithFullData.sex });
                    }                      
                }  
            }
            return data;
        }

        public static void sendDataToBD(List<Person> data)
        {          
            using (MyDBContext context = new MyDBContext())
            {   
                foreach (var item in data)
                {
                    context.Persons.Add(item);
                }
                
                context.SaveChanges();
            }
        }

    }
}
