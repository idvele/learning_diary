using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;


namespace Learning_Diary_IK
{
    public static class APIHelpers
    {
        //create HTTP client
        private static HttpClient GetHttpClient(string url)
        {
            var client = new HttpClient { BaseAddress = new Uri(url) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public static async Task<T> RunAsync<T>(String url, string urlparams)
        {
            try
            {
                using (var client = GetHttpClient(url))
                {
                    HttpResponseMessage response = await client.GetAsync(urlparams);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = await response.Content.ReadAsStringAsync();

                        //JSON to an object
                        var result = JsonSerializer.Deserialize<T>(json);
                        return result;

                    }
                    return default(T);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return default(T);


            }
        }
    }
}
