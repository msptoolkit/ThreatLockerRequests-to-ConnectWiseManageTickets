using ManageIntegration.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageIntegration.DAL
{
    public class ThreatLockerAccess
    {
        public static List<ThreatLockerOrganization> GetOrganizations(Config config)
        {
            var client = new RestClient(config.ThreatLockerUrl);

            var request = new RestRequest("/getorganizations.ashx", DataFormat.Json);
            request.AddHeader("Authorization", config.ThreatlockerAuth);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<List<ThreatLockerOrganization>>(response.Content);

            return result;
        }

        public static List<ThreatLockerRequest> GetRequests(Config config)
        {
            var client = new RestClient(config.ThreatLockerUrl);

            var request = new RestRequest("/getrequests.ashx", DataFormat.Json);
            request.AddHeader("Authorization", config.ThreatlockerAuth);
            request.AddHeader("DateTime", config.LastSuccessRequestSent.ToString());

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<List<ThreatLockerRequest>>(response.Content);

            return result;
        }

        public static ThreatLockerAction ProcessJson(ThreatLockerRequest threatLockerRequest)
        {
            ThreatLockerAction threatLockerAction = new ThreatLockerAction();
            threatLockerAction = JsonConvert.DeserializeObject<ThreatLockerAction>(threatLockerRequest.Json);
            return threatLockerAction;
        }
    }
}
