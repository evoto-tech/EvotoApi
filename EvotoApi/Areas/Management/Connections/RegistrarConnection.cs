using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Common.Models;
using Registrar.Models.Request;
using Registrar.Models.Response;
using Management.Models;
using EvotoApi.Areas.ManagementApi.Models.Request;
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
            var publishableVote = new PublishManaVote(model);
            var req = CreateRequest("management/createblockchain", Method.POST, publishableVote);
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

        public static async Task<IList<SingleCustomUserFieldResponse>> GetCustomFields()
        {
            var req = CreateRequest("account/customFields", Method.GET);
            var res = await MakeApiRequest(req);

            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IList<SingleCustomUserFieldResponse>>(res.Content);

            var exception = new Exception("Error getting custom field settings");
            exception.Data["status"] = res.StatusCode;
            exception.Data["content"] = res.Content;
            throw exception;
        }

        public static async Task<IList<SingleCustomUserFieldResponse>> UpdateCustomFields(IList<CreateCustomUserFieldModel> models)
        {
            var req = CreateRequest("users/customFields/update", Method.POST, models);
            var res = await MakeApiRequest(req);

            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IList<SingleCustomUserFieldResponse>>(res.Content);

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

        public static async Task<IList<SingleRegiSettingResponse>> ListRegistrarSettings()
        {
            var req = CreateRequest("/settings/list", Method.GET);
            req.JsonSerializer.ContentType = "application/json; charset=utf-8";
            req.AddHeader("Accept", "application/json");
            var res = await MakeApiRequest(req);

            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IList<SingleRegiSettingResponse>>(res.Content);

            throw new Exception("Error listing registrar settings");
        }

        public static async Task<SingleRegiSettingResponse> UpdateRegistrarSettings(UpdateRegiSetting model)
        {
            var req = CreateRequest("/settings", Method.POST, model);
            req.JsonSerializer.ContentType = "application/json; charset=utf-8";
            req.AddHeader("Accept", "application/json");
            var res = await MakeApiRequest(req);

            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<SingleRegiSettingResponse>(res.Content);

            throw new Exception("Error updating registrar settings");
        }
    }
}
