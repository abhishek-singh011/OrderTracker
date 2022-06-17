using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using OrderTracking.Mvc.Models;
using TrackingService;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace OrderTracking.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            var address = new EndpointAddress("https://devwebservices.purolator.com/EWS/V1/Tracking/TrackingService.asmx");

            TrackingServiceContractClient client = new TrackingServiceContractClient(binding, address);
            client.ClientCredentials.UserName.Password = "somepassowrd";
            client.ClientCredentials.UserName.UserName = "13ad23ef17555290920a729dd6c924";

            ((BasicHttpBinding)client.Endpoint.Binding).Security.Mode = BasicHttpSecurityMode.Transport;
            ((BasicHttpBinding)client.Endpoint.Binding).Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;

            TrackPackagesByReferenceRequest referenceRequest = new TrackPackagesByReferenceRequest();
            referenceRequest.RequestContext = new RequestContext();
            referenceRequest.RequestContext.GroupID = "1.2";
            referenceRequest.RequestContext.Language = Language.en;
            referenceRequest.RequestContext.Version = "1.2";
            referenceRequest.RequestContext.RequestReference = "RequestReference";
            referenceRequest.RequestContext.UserToken = "13ad23ef17555290920a729dd6c924";
           


            referenceRequest.TrackPackagesByReferenceRequest1 = new TrackPackagesByReferenceRequestContainer();
            referenceRequest.TrackPackagesByReferenceRequest1.TrackPackageByReferenceSearchCriteria = new TrackPackageByReferenceSearchCriteria();
            referenceRequest.TrackPackagesByReferenceRequest1.TrackPackageByReferenceSearchCriteria.Reference = "ref1";
            referenceRequest.TrackPackagesByReferenceRequest1.TrackPackageByReferenceSearchCriteria.DestinationPostalCode = "V2S8B7";
            referenceRequest.TrackPackagesByReferenceRequest1.TrackPackageByReferenceSearchCriteria.BillingAccountNumber = "9999999999";

            TrackPackagesByReferenceResponse referenceResponse = new TrackPackagesByReferenceResponse();


            

            try
            {
                referenceResponse = client.TrackPackagesByReference(referenceRequest);
            }
            catch (AggregateException ex)
            {

            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
