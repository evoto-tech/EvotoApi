using System.Configuration;
using System.Net;
using System.Threading.Tasks;
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

        private const string ApiBase = "management";

        public static async Task<bool> CreateBlockchain(ManaVote model)
        {
            var client = new RestClient(RegistrarUrl);

            // TODO: Put in resource dictionary
            var req = new RestRequest(ApiBase + "/createblockchain");
            req.AddBody(JsonConvert.SerializeObject(model));

            var res = await client.ExecuteTaskAsync(req);
            return (res.StatusCode == HttpStatusCode.OK);
        } 
    }
}