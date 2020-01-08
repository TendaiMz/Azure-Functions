using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Linq;

namespace DemoApp
{
    public static class OnDataReceived
    {
        [FunctionName("OnDataReceived")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Data received.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            try
            {

                // Call Your  API
                HttpClient newClient = new HttpClient();
                newClient.DefaultRequestHeaders.Add("X-Auth-Token", "dfcc2d28c734429f80198f1568058f9a");
                HttpRequestMessage newRequest = new HttpRequestMessage(HttpMethod.Get,
                "https://api.football-data.org/v2/competitions/PL");

                if(name == null){                   
                   
                    return new BadRequestObjectResult("Please pass a name on the query string or in the request body");
                }

                //Read Server Response
                HttpResponseMessage response = await newClient.SendAsync(newRequest);
                RootObject obj = await response.Content.ReadAsAsync<RootObject>();
                var objResult = obj.seasons.Where(x => x.startDate.ToLower().Contains(name.ToLower())).Select(x => x.winner.name).FirstOrDefault();
                var result = string.IsNullOrEmpty(objResult) ? "No data" : objResult;

                log.LogInformation("success");
                return  (ActionResult)new OkObjectResult($"The winner was ,{ result}"); 
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }



        }
    }
}


public class Area
{
    public int id { get; set; }
    public string name { get; set; }
}

public class CurrentSeason
{
    public int id { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }
    public int currentMatchday { get; set; }
    public object winner { get; set; }
}

public class Winner
{
    public int id { get; set; }
    public string name { get; set; }
    public string shortName { get; set; }
    public string tla { get; set; }
    public string crestUrl { get; set; }
}

public class Season
{
    public int id { get; set; }
    public string startDate { get; set; }
    public string endDate { get; set; }
    public int? currentMatchday { get; set; }
    public Winner winner { get; set; }
}

public class RootObject
{
    public int id { get; set; }
    public Area area { get; set; }
    public string name { get; set; }
    public string code { get; set; }
    public object emblemUrl { get; set; }
    public string plan { get; set; }
    public CurrentSeason currentSeason { get; set; }
    public List<Season> seasons { get; set; }
    public DateTime lastUpdated { get; set; }
}