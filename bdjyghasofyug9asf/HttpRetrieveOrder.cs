using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace bdjyghasofyug9asf
{
    public static class HttpRetrieveOrder
    {
        [FunctionName("HttpRetrieveOrder")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
            [Table("Orders", Connection = "TableStorage")]CloudTable ordersTable, TraceWriter log)
        {
            string fileName = req.Query["fileName"];
            if (string.IsNullOrWhiteSpace(fileName))
                return new BadRequestResult();
            TableQuery<Order> query = new TableQuery<Order>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, fileName));
            TableQuerySegment<Order> tableQueryResult = await ordersTable.ExecuteQuerySegmentedAsync(query, null);
            var resultList = tableQueryResult.Results;

            if (resultList.Any())
            {
                var firstElement = resultList.First();
                return new JsonResult(new
                {
                    firstElement.CustomerEmail,
                    firstElement.PhotoName,
                    firstElement.PhotoHeight,
                    firstElement.PhotoWidth
                });
            }

            return new NotFoundResult();
        }
    }
}