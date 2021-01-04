using App.Interfaces;
using App.Models;
using Jil;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using Polly.Timeout;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Services
{
    public class HeroesService : IHeroes
    {
        const string _URL = "http://gateway.marvel.com/v1/public/characters";
        const string _APIKEY = "14c462d9e685d41ea836a2b7028b9b80";
        const string _APIKEYPRIVATE = "95177538d4435a16b3ed91862a725319119b03e6";

        public string GetUrl(string limite)
        {
            using(MD5 generateMD5 = MD5.Create())
            {
                //era para ser um datetime
                var timestamp = 1;
                
                var combinacao = $"{timestamp}{_APIKEYPRIVATE}{_APIKEY}";

                //Sem string de formato: 13
                //String de formato 'X2': 0D formato hexadecimal
                var hash = string.Concat(generateMD5.ComputeHash(Encoding.UTF8.GetBytes(combinacao)).Select(x => x.ToString("x2")));

                return $"{_URL}?limit={limite}&ts={timestamp}&apikey={_APIKEY}&hash={hash}";
            }
        }

        public AsyncRetryPolicy<HttpResponseMessage> _httpRetryPolicy;
        public AsyncTimeoutPolicy _timeoutPolicy;
        private static object _MessageLock = new object();


        //O HttpClient deve ser instanciado uma vez por aplicativo, em vez de por uso.Veja as observações.
        //Referencia
        //https://docs.microsoft.com/pt-br/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
        static readonly HttpClient _client = new HttpClient();
        public async Task<Hero> GetHeroes(string limite)
        {
            //if(_client == null)
            //{
            //_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //_client.BaseAddress
            //}


            //Posso melhorar o tratamento do handleResult
            //https://dev.to/rickystam/net-core-use-httpclientfactory-and-polly-to-build-rock-solid-services-2edh

            //referencia
            //https://www.davidbritch.com/2017/07/transient-fault-handling-in.html
            _httpRetryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                       .Or<TimeoutRejectedException>()
                       //.OrResult<HttpResponseMessage>(r => httpStatusCodesToRetry.Contains(r.StatusCode))
                       //.Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))

                       .RetryAsync
                       (
                           //Quantidade de tentaivas
                           retryCount: 3,

                           //duração entre as tentativas
                           //sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),

                           //O que será executado se der erro
                           onRetry: async (ex, time, ase) =>
                           {
                               var context = ase;
                               var rawResponse = await ex.Result.Content.ReadAsStringAsync();
                               var json = JsonConvert.DeserializeAnonymousType(rawResponse, new { code = "", status = "" });
                               var teste = ex.Result;
                               if(teste != null)
                               {
                                   Console.WriteLine($"" +
                                  $"\n\n********************************************************************************\n" +
                                  $"Ocorreu um erro ao baixar os dados: {json.status},\n\n\n tentando novamente... em {time}" +
                                  $"***********************************************************************************\n\n");
                               }
                               else
                               {
                                   Console.WriteLine($"" +
                                  $"\n\n********************************************************************************\n" +
                                  $"Ocorreu um erro ao baixar os dados: {ex.Result},\n\n\n tentando novamente... em {time}" +
                                  $"***********************************************************************************\n\n");
                               }
                              
                           }
                       );

            _timeoutPolicy = Policy.TimeoutAsync(5, TimeoutStrategy.Pessimistic);
            


                Hero heroes = null;
                try
                {
                    
                    //.ExecuteAsync(async () =>
                    //{
                    //   Console.WriteLine($"Obtendo Herois...");
                    //    var response = await client.GetAsync(GetUrl(limite));
                    //    var rawResponse = await response.Content.ReadAsStringAsync();

                    //Problemas para retornar um await direito, a melhor opção por enquantp é deixar finalizar o metodo e retornar o objeto depois
                    //heroes = JsonConvert.DeserializeObject<Hero>(rawResponse);
                    //});


                    
                    var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0,0,1);

                //Referencia
                //https://nodogmablog.bryanhogan.net/2017/12/using-the-polly-timeout-when-making-a-http-request/
                HttpResponseMessage response = await
                        _httpRetryPolicy.ExecuteAsync(() =>
                            _timeoutPolicy.ExecuteAsync(async c => 
                            

                                await _client.GetAsync(GetUrl(limite), c)
                                //var rawResponse = await response.Content.ReadAsStringAsync();

                                //Problemas para retornar um await direito, a melhor opção por enquantp é deixar finalizar o metodo e retornar o objeto depois
                                //heroes = JsonConvert.DeserializeObject<Hero>(rawResponse);
                                //return new HttpResponseMessage(); 
                            , CancellationToken.None));

                if (response.Content != null)
                {
                    if (response.StatusCode == HttpStatusCode.OK) { }
                }




                //Teste Jil
                Stopwatch TimerJill = new Stopwatch();
                TimerJill.Start();
                var employeeDeserialized = JSON.Deserialize<Hero>(await response.Content.ReadAsStringAsync(), new Options(dateFormat: DateTimeFormat.MicrosoftStyleMillisecondsSinceUnixEpoch));
                TimerJill.Stop();
                var tempoDecorridoJil = TimerJill.Elapsed.TotalSeconds;


                //Teste using newtonsof
                Stopwatch TimerNewton = new Stopwatch();
                TimerNewton.Start();
                var newtonSoft = JsonConvert.DeserializeObject<Hero>(await response.Content.ReadAsStringAsync());
                TimerNewton.Stop();
                var tempoDecorridoNewton = TimerNewton.Elapsed.TotalSeconds;


                string json = await response.Content.ReadAsStringAsync();
                //Referencia
                //https://stackoverflow.com/questions/8707755/how-to-know-the-size-of-the-string-in-bytes
                var howManyBytes = json.Length * sizeof(Char);

                return newtonSoft;


                //forma correta  de utilizar, sem jogar atribuir para uma string antes de realizar a deserialização.
                //return JsonConvert.DeserializeObject<Hero>(await response.Content.ReadAsStringAsync());






                /*  
                  HttpStatusCode[] httpStatusCodesToRetry =
                  {
                      HttpStatusCode.RequestTimeout, // 408
                      HttpStatusCode.InternalServerError, // 500
                      HttpStatusCode.BadGateway, // 502
                      HttpStatusCode.ServiceUnavailable, // 503
                      HttpStatusCode.GatewayTimeout // 504
                  };



                  await Policy
                     //Se for um erro diferente de 404 ira executar a politica de retry
                     //podemos utilizar grupo de statusCode
                     //o ideal é trabalhar em cima dos erros de internet, e continuar tratando o resto pelo statusCode

                     //.Handle<TimeoutException>()
                     .Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))
                     .Or<HttpRequestException>()
                     //.OrResult<HttpResponseMessage>(r => httpStatusCodesToRetry.Contains(r.StatusCode))
                     //.Handle<HttpRequestException>(ex => !ex.Message.ToLower().Contains("404"))

                     .WaitAndRetryAsync
                     (
                         //Quantidade de tentaivas
                         retryCount: 3,

                         //duração entre as tentativas
                         sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),

                         //O que será executado se der erro
                         onRetry: (ex, time) =>
                         {
                             lock (_MessageLock)
                             {
                                 Console.BackgroundColor = ConsoleColor.Red;
                                 Console.WriteLine($"Ocorreu um erro ao baixar os dados: {ex.Message}, tentando novamente em...{time}");
                                 Console.ResetColor();
                             }
                         }
                     )
                     .ExecuteAsync(async () =>
                     {
                         Console.BackgroundColor = ConsoleColor.Red;
                         Console.ForegroundColor = ConsoleColor.White;
                         Console.WriteLine($"Obtendo Herois...");
                         var response = await client.GetAsync(GetUrl(limite));
                         var rawResponse = await response.Content.ReadAsStringAsync();

                         //Problemas para retornar um await direito, a melhor opção por enquantp é deixar finalizar o metodo e retornar o objeto depois
                         heroes = JsonConvert.DeserializeObject<Hero>(rawResponse);
                     });

                  */
                /*
                var response = await client.GetAsync(GetUrl(limite));

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new Exception(response.Content.ReadAsStringAsync().Result);
                }
                var rawResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Hero>(rawResponse);

                */
                //return heroes;

            }
                catch (Exception ex)
                {
                    throw ex;
                }
            
        }
    }
}
