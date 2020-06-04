using ManageIntegration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ManageIntegration.DAL
{
    public class ManageAccess
    {
        public static List<ManageCompany> GetCompanies(Config config)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest("/V4_6_release/apis/3.0/company/companies/", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<List<ManageCompany>>(response.Content);

            return result;
        }

        public static List<ManageCompany> GetCompanies(Config config, string filterUrl)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest("/V4_6_release/apis/3.0/" + filterUrl, DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<List<ManageCompany>>(response.Content);

            return result;
        }

        public static List<ManageCompanyType> GetCompanyTypes(Config config)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest("/V4_6_release/apis/3.0/company/companies/types", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<List<ManageCompanyType>>(response.Content);

            return result;
        }


        public static List<ManageCompanyStatus> GetCompanyStatuses(Config config)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest("/V4_6_release/apis/3.0/company/companies/statuses", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<List<ManageCompanyStatus>>(response.Content);

            return result;
        }

        public static string CreateCompanyFilterUrl(List<ManageCompanyStatus> manageCompanyStatuses, List<ManageCompanyType> manageCompanyTypes)
        {
            string statuses = "";
            string types = "";

            foreach (var s in manageCompanyStatuses)
            {
                if (string.IsNullOrEmpty(statuses))
                    statuses = s.Id.ToString();
                else
                    statuses = string.Concat(statuses, ",", s.Id);
            }

            foreach (var t in manageCompanyTypes)
            {
                if (string.IsNullOrEmpty(types))
                    types = t.Id.ToString();
                else
                    types = string.Concat(types, "%20or%20types%2Fid%20%3D%20", t.Id);
            }


            string url = string.Concat("company/companies?pageSize=1000&conditions=status/id%20in%20(", statuses, ")%26childconditions%3Dtypes%2Fid%20%3D%20", types, "&orderBy=name");

            var result = System.Web.HttpUtility.UrlDecode(url);
            return result;
        }
        public static List<ManageBoard> GetBoards(Config config)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest("/V4_6_release/apis/3.0/service/boards", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<List<ManageBoard>>(response.Content);

            return result;
        }

        public static ManageBoard GetBoard(Config config, int BoardId)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest($"/V4_6_release/apis/3.0/service/boards/{BoardId}", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<ManageBoard>(response.Content);

            return result;
        }
        public static List<ManageBoardType> GetBoardTypes(Config config, int BoardId)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest($"/V4_6_release/apis/3.0/service/boards/{BoardId}/types/", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<List<ManageBoardType>>(response.Content);

            return result;
        }

        public static ManageBoardType GetBoardType(Config config, int BoardId, int TypeId)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest($"/V4_6_release/apis/3.0/service/boards/{BoardId}/types/{TypeId}", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<ManageBoardType>(response.Content);

            return result;
        }

        public static List<ManageBoardSubType> GetBoardSubTypes(Config config, int BoardId)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest($"/V4_6_release/apis/3.0/service/boards/{BoardId}/subtypes", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<List<ManageBoardSubType>>(response.Content);

            return result;
        }
        public static ManageBoardSubType GetBoardSubType(Config config, int BoardId, int SubType)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest($"/V4_6_release/apis/3.0/service/boards/{BoardId}/subtypes/{SubType}", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<ManageBoardSubType>(response.Content);

            return result;
        }

        public static List<ManageBoardItem> GetBoardItems(Config config, int BoardId)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest($"/V4_6_release/apis/3.0/service/boards/{BoardId}/items", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<List<ManageBoardItem>>(response.Content);

            return result;
        }

        public static ManageBoardItem GetBoardItem(Config config, int BoardId, int ItemId)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);



            var request = new RestRequest($"/V4_6_release/apis/3.0/service/boards/{BoardId}/items/{ItemId}", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<ManageBoardItem>(response.Content);

            return result;
        }

        public static List<ManageBoardPriority> GetBoardPriorities(Config config)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest($"/V4_6_release/apis/3.0/service/priorities", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<List<ManageBoardPriority>>(response.Content);

            return result;
        }

        public static ManageBoardPriority GetBoardPriority(Config config, int PriorityId)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest($"/V4_6_release/apis/3.0/service/priorities/{PriorityId}", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<ManageBoardPriority>(response.Content);

            return result;
        }

        public static List<ManageBoardStatus> GetBoardStatuses(Config config, int BoardId)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest($"/V4_6_release/apis/3.0/service/boards/{BoardId}/statuses", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<List<ManageBoardStatus>>(response.Content);

            return result;
        }

        public static ManageBoardStatus GetBoardStatus(Config config, int BoardId, int StatusId)
        {
            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest($"/V4_6_release/apis/3.0/service/boards/{BoardId}/statuses/{StatusId}", DataFormat.Json);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            var response = client.Get(request);

            var result = JsonConvert.DeserializeObject<ManageBoardStatus>(response.Content);

            return result;
        }

        public static ManageConfig GetManageConfigNames(Config config, ManageConfig manageConfig)
        {
            manageConfig.ManageBoard = GetBoard(config, manageConfig.BoardId);
            manageConfig.ManageBoardType = GetBoardType(config, manageConfig.BoardId, manageConfig.TypeId);
            manageConfig.ManageBoardSubType = GetBoardSubType(config, manageConfig.BoardId, manageConfig.SubTypeId);
            manageConfig.ManageBoardItem = GetBoardItem(config, manageConfig.BoardId, manageConfig.ItemId);
            manageConfig.ManageBoardStatus = GetBoardStatus(config, manageConfig.BoardId, manageConfig.StatusId);
            manageConfig.ManageBoardPriority = GetBoardPriority(config, manageConfig.PriorityId);

            return manageConfig;
        }

        public static HttpStatusCode PostTicket(Config config, ManageTicket manageTicket)
        {
            string output = JsonConvert.SerializeObject(manageTicket, Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var client = new RestClient(config.ManageUrl);
            client.Authenticator = new HttpBasicAuthenticator(config.ManageCompanyName + "+" + config.ManagePubKey, config.ManagePriKey);

            var request = new RestRequest("/V4_6_release/apis/3.0/service/tickets", Method.POST);
            request.AddHeader("Accept", "application/vnd.connectwise.com+json; version=3.0.0; application/json");
            request.AddHeader("clientid", config.ManageClientId);

            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(output);

            var response = client.Post(request);

            return response.StatusCode;
        }
    }
}
