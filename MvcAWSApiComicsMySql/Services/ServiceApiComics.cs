using MvcAWSApiComicsMySql.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcAWSApiComicsMySql.Services
{
    public class ServiceApiComics
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApi;

        public ServiceApiComics(IConfiguration configuration)
        {
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApi =
                configuration.GetValue<string>("ApiUrls:ApiComics");
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                //LO UNICO QUE DEBEMOS TENER EN CUENTA ES 
                //QUE LAS PETICIONES, A VECES SE QUEDAN ATASCADAS
                //SI LAS HACEMOS MEDIANTE .BaseAddress + Request
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = this.UrlApi + request;
                HttpResponseMessage response =
                    await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task CreateComic(Comic comic)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/comics";
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string jsonComic = JsonConvert.SerializeObject(comic);
                StringContent content =
                    new StringContent(jsonComic, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(this.UrlApi + request, content);
            }
        }

        public async Task<List<Comic>> GetComicsAsync()
        {
            string request = "api/comics";
            List<Comic> comics = await this.CallApiAsync<List<Comic>>(request);
            return comics;
        }

        public async Task<Comic> FindComicAsync(int id)
        {
            string request = "api/comics/" + id;
            Comic comic = await this.CallApiAsync<Comic>(request);
            return comic;
        }





    }
}
