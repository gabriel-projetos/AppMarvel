using App.Interfaces;
using App.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
    public class HeroesService : IHeroes
    {
        const string _URL = "http://gateway.marvel.com/v1/public/characters";
        const string _APIKEY = "14c462d9e685d41ea836a2b7028b9b80";
        const string _APIKEYPRIVATE = "95177538d4435a16b3ed91862a725319119b03e6";

        public string GetUrl()
        {
            using(MD5 generateMD5 = MD5.Create())
            {
                var timestamp = 1;
                
                var combinacao = $"{timestamp}{_APIKEYPRIVATE}{_APIKEY}";

                //Sem string de formato: 13
                //String de formato 'X2': 0D formato hexadecimal
                var hash = string.Concat(generateMD5.ComputeHash(Encoding.UTF8.GetBytes(combinacao)).Select(x => x.ToString("x2")));


                var url = $"?limit=10&ts={timestamp}&apikey={_APIKEY}&hash={hash}";
                return $"{_URL}{url}";
            }
        }

        public async Task<Hero> GetHeroes()
        {
            Hero herois = new Hero();
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(GetUrl());

                var rawResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Hero>(rawResponse);
            }
        }
    }
}
