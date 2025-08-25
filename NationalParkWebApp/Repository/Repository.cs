using NationalParkWebApp.Repository.IRepository;
using Newtonsoft.Json;
using System.Text;

namespace NationalParkWebApp.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public Repository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> CreateAsync(string Url, T ObjToCreate)
        {
            var request =new HttpRequestMessage(HttpMethod.Post, Url);
            if (ObjToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(ObjToCreate),
                    Encoding.UTF8, "application/json"); 
            }
            var Client = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse=await Client.SendAsync(request);
            if(httpResponse.StatusCode==System.Net.HttpStatusCode.Created) 
                return true;
                return false;
        }

        public async Task<bool> DeleteAsync(string Url, int Id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, Url+"/"+Id.ToString());
            var Client = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse = await Client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                return true;
            return false;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string Url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,Url);
            var Client = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse = await Client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonstring =await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonstring);
            }
            return null;
        }

        public async Task<T> GetAsync(string Url, int Id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Url+"/"+Id.ToString());
            var Client = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse = await Client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonstring = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonstring);
            }
            return null;
        }

        public async Task<bool> UpdateAsync(string Url, T ObjToUpdate)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, Url);
            if (ObjToUpdate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(ObjToUpdate),
                    Encoding.UTF8, "application/json");
            }
            var Client = _httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse = await Client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                return true;
            return false;
        }
    }

}
