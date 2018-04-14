
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Table;

namespace bdjyghasofyug9asf
{
    public static class MyHttpDemo
    {
        [FunctionName("HttpOrderFormSave")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, 
            [Table("Orders", Connection = "TableStorage")]ICollector<Order> ordersTable,
            TraceWriter log)
        {
            Order order = null;
            try
            {
                string requestBody = new StreamReader(req.Body).ReadToEnd();
                order = JsonConvert.DeserializeObject<Order>(requestBody);
                order.PartitionKey = System.DateTime.UtcNow.Ticks.ToString();
                order.RowKey = order.PhotoName;
                ordersTable.Add(order);
            }
            catch(System.Exception)
            {
                return new BadRequestObjectResult("Wprowadzone dane s¹ nieprawid³owe.");
            }
            return (ActionResult)new OkObjectResult($"Zamówienie przyjêto.");
        }
    }

    public class Order: TableEntity
    {
        public string CustomerEmail { get; set; }
        public string PhotoName { get; set; }
        public int PhotoHeight { get; set; }
        public int PhotoWidth { get; set; }
    }
}
