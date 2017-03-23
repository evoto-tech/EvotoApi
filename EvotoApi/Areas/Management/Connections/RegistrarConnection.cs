using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Common.Models;
using Management.Models;
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

        public static async Task<bool> CreateBlockchain(ManaVote model)
        {
            var client = new RestClient(RegistrarUrl);

            // TODO: Put in resource dictionary
            var req = new RestRequest("management/createblockchain");
            req.AddBody(JsonConvert.SerializeObject(model));

            var res = await client.ExecuteTaskAsync(req);
            return res.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<IEnumerable<RegiUser>> GetRegistrarUsers()
        {
            var client = new RestClient(RegistrarUrl);

            // TODO: Put in resource dictionary
            var req = new RestRequest("/users/list");

            var res = await client.ExecuteTaskAsync(req);
            if (res.StatusCode == HttpStatusCode.OK)
            {
                var users = JsonConvert.DeserializeObject<IEnumerable<RegiUser>>(res.Content);
                return users;
            }
            throw new Exception("Error retrieving registrar users");
        }
    }
}