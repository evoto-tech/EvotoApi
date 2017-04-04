using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.Models;
using EvotoApi.Areas.ManagementApi.Models.Response;
using Management.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using RestSharp;

namespace EvotoApi.Areas.Management.Connections
{
    public class RegistrarConnection
    {
#if DEBUG
        private const string RegistrarUrl = "http://localhost:15893";
#else
        private const string RegistrarUrl = "http://api.evoto.tech";
#endif

        private static async Task<IRestResponse> MakeApiRequest(IRestRequest req)
        {
            var client = new RestClient(RegistrarUrl);
            if (!ConfigurationManager.AppSettings.Get("ApiKeys").IsNullOrWhiteSpace())
            {
                req.AddQueryParameter("key", ConfigurationManager.AppSettings.Get("ApiKeys"));
            }
            return await client.ExecuteTaskAsync(req);
        }

        public static async Task<bool> CreateBlockchain(ManaVote model)
        {
            var req = new RestRequest("management/createblockchain");
            req.AddBody(JsonConvert.SerializeObject(model));

            var res = await MakeApiRequest(req);
            return (res.StatusCode == HttpStatusCode.OK);
        }

        public static async Task<IEnumerable<SingleRegiUserResponse>> GetRegistrarUsers()
        {
            // TODO: Put in resource dictionary
            var req = new RestRequest("/users/list");
            var res = await MakeApiRequest(req);
            if (res.StatusCode == HttpStatusCode.OK)
            {
                var users = JsonConvert.DeserializeObject<IEnumerable<RegiUser>>(res.Content).Select((v) => new SingleRegiUserResponse(v));
                return users;
            }
            throw new Exception("Error retrieving registrar users");
        }

        public static async Task<IList<CustomUserField>> UpdateCustomFields(dynamic model)
        {
            var req = new RestRequest("users/customFields/update");
            req.AddBody(JsonConvert.SerializeObject(model));

            var res = await MakeApiRequest(req);
            if (res.StatusCode == HttpStatusCode.OK)
            {
                var fields = JsonConvert.DeserializeObject<IList<CustomUserField>>(res.Content);
                return fields;
            }
            throw new Exception("Error updating custom field settings");
        }
    }
}