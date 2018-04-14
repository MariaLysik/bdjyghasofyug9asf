
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace bdjyghasofyug9asf
{
    public static class MyHttpDemo
    {
        [FunctionName("HttpOrderFormSave")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            Order order = null;
            try
            {
                string requestBody = new StreamReader(req.Body).ReadToEnd();
                order = JsonConvert.DeserializeObject<Order>(requestBody);
            }
            catch(System.Exception)
            {
                return new BadRequestObjectResult("Wprowadzone dane s¹ nieprawid³owe.");
            }
            return (ActionResult)new OkObjectResult($"Zamówienie przyjêto.");
        }
    }

    public class Order
    {
        string Name { get; set; }
        string EmailAddress { get; set; }
        string PhotoSize { get; set; }
        string PhotoName { get; set; }
    }
}
