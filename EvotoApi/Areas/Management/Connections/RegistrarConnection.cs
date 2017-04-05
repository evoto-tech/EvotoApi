using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Common.Models;
using Registrar.Models.Request;
using Registrar.Models.Response;
using Management.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using RestSharp;
using System.Diagnostics;

namespace EvotoApi.Areas.Management.Connections
{
    public class RegistrarConnection
    {
#if DEBUG
        private const string RegistrarUrl = "http://localhost:15893";
#else
        private const string RegistrarUrl = "https://api.evoto.tech";
#endif

        private static async Task<IRestResponse> MakeApiRequest(IRestRequest req)
        {
            var client = new RestClient(RegistrarUrl);
            return await client.ExecuteTaskAsync(req);
        }

        private static IRestRequest CreateRequest(string uri, Method method, object data = null)
        {
            var request = new RestRequest(uri, method);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");

            if (!ConfigurationManager.AppSettings.Get("ApiKeys").IsNullOrWhiteSpace())
                request.AddQueryParameter("key", ConfigurationManager.AppSettings.Get("ApiKeys"));

            if (data != null)
            {
                request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);
            }

            return request;
        }

        public static async Task<bool> CreateBlockchain(ManaVote model)
        {
            var req = CreateRequest("management/createblockchain", Method.GET, model);
            var res = await MakeApiRequest(req);

            return res.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<IEnumerable<SingleRegiUserResponse>> GetRegistrarUsers()
        {
            // TODO: Put in resource dictionary
            var req = CreateRequest("/users/list", Method.GET);
            var res = await MakeApiRequest(req);
            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IEnumerable<SingleRegiUserResponse>>(res.Content);
            throw new Exception("Error retrieving registrar users");
        }

        public static async Task<SingleRegiUserResponse> GetRegistrarUser(int id)
        {
            // TODO: Put in resource dictionary
            var req = CreateRequest("/users/details/" + id, Method.GET);
            var res = await MakeApiRequest(req);
            if (res.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<SingleRegiUserResponse>(res.Content);
            }
            throw new Exception("Error retrieving registrar users");
        }

        public static async Task<IList<CustomUserField>> UpdateCustomFields(IList<CreateCustomUserFieldModel> models)
        {
            var req = CreateRequest("users/customFields/update", Method.POST, models);
            var res = await MakeApiRequest(req);

            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IList<CustomUserField>>(res.Content);

            var exception = new Exception("Error updating custom field settings");
            exception.Data["status"] = res.StatusCode;
            exception.Data["content"] = res.Content;
            throw exception;
        }

        public static async Task<SingleRegiUserResponse> CreateRegistrarUser(CreateRegiUser model)
        {
            // TODO: Put in resource dictionary
            var req = CreateRequest("/account/register", Method.POST, model);
            var res = await MakeApiRequest(req);

            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<SingleRegiUserResponse>(res.Content);

            var exception = new Exception("Error registering registrar user");
            exception.Data["status"] = res.StatusCode;
            exception.Data["content"] = res.Content;
            throw exception;
        }
    }
}
