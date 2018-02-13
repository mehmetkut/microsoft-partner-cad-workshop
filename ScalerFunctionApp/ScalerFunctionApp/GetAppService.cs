using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace ScalerFunctionApp
{
    public static class GetAppService
    {
        [FunctionName("GetAppService")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log, ExecutionContext executionContext)
        {
            log.Info("GetAppService started");

            var appServicePlanName = "#YOURAPPSERVICEPLANNAME#";

            var authFile = $"{Directory.GetParent(executionContext.FunctionDirectory).FullName}\\my.azureauth";

            log.Info(authFile);

            var azure = Azure.Authenticate(authFile).WithDefaultSubscription();

            var appServicePlan = azure.AppServices.AppServicePlans.List().FirstOrDefault(x => x.Name == appServicePlanName);

            var getAppServiceResponse = new GetAppServiceResponse();

            getAppServiceResponse.Capacity = appServicePlan.Capacity;

            getAppServiceResponse.SkuTier = appServicePlan.PricingTier.SkuDescription.Tier;
            getAppServiceResponse.SkuSize = appServicePlan.PricingTier.SkuDescription.Size;
            getAppServiceResponse.SkuName = appServicePlan.PricingTier.SkuDescription.Name;
            getAppServiceResponse.SkuFamily = appServicePlan.PricingTier.SkuDescription.Family;

            var appServicePlanResponseJson = JsonConvert.SerializeObject(getAppServiceResponse);

            return req.CreateResponse(HttpStatusCode.OK, appServicePlanResponseJson);
        }
    }

    public class GetAppServiceResponse
    {
        public string SkuTier { get; set; }
        public string SkuSize { get; set; }
        public string SkuName { get; set; }
        public string SkuFamily { get; set; }
        public int Capacity { get; set; }
    }

  
}
