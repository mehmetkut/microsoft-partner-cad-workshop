using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace ScalerFunctionApp
{
    public static class SetAppService
    {
        [FunctionName("SetAppService")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log, ExecutionContext executionContext)
        {
            log.Info("SetAppService started");

            string setAppServiceRequestJson = await req.Content.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(setAppServiceRequestJson))
            {
                var setAppServiceRequest = JsonConvert.DeserializeObject<SetAppServicePlanRequest>(setAppServiceRequestJson);

                var appServicePlanName = "#YOURAPPSERVICEPLANNAME#";

                var authFile = $"{Directory.GetParent(executionContext.FunctionDirectory).FullName}\\my.azureauth";

                var azure = Azure.Authenticate(authFile).WithDefaultSubscription();

                var appServicePlan = azure.AppServices.AppServicePlans.List().FirstOrDefault(x => x.Name == appServicePlanName);
                log.Info(executionContext.FunctionDirectory);

                PricingTier pricingTier = new PricingTier(setAppServiceRequest.Tier, setAppServiceRequest.Size); //Basic, B1

                int capacity = setAppServiceRequest.Capacity;

                appServicePlan?.Update()
                    .WithPricingTier(pricingTier)
                    .WithCapacity(capacity)
                    .Apply();

                return req.CreateResponse(HttpStatusCode.OK, "SetAppService Successfully Competed");

            }
            else
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }
    }

    public class SetAppServicePlanRequest
    {
        public string Tier { get; set; }
        public string Size { get; set; }
        public int Capacity { get; set; }
    }
}
