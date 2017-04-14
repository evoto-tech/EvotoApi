using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Management.Models;
using Management.Models.Exceptions;
using Management.Models.Request;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Registrar.Models.Request;
using Registrar.Models.Response;
using RestSharp;

namespace EvotoApi.Connections
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
                request.AddParameter(
                    "application/json",
                    JsonConvert.SerializeObject(data),
                    ParameterType.RequestBody);

            return request;
        }

        public static async Task<bool> CreateBlockchain(ManaVote model)
        {
            var publishableVote = new PublishManaVote(model);
            var req = CreateRequest(RegistrarUris.createBlockchain, Method.POST, publishableVote);
            var res = await MakeApiRequest(req);

            return res.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<IEnumerable<SingleRegiUserResponse>> GetRegistrarUsers()
        {
            var req = CreateRequest(RegistrarUris.getUsers, Method.GET);
            var res = await MakeApiRequest(req);

            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IEnumerable<SingleRegiUserResponse>>(res.Content);

            throw new RegistrarConnectionException("Error retrieving registrar users");
        }

        public static async Task<SingleRegiUserResponse> GetRegistrarUser(int id)
        {
            var req = CreateRequest(string.Format(RegistrarUris.getUserById, id), Method.GET);
            var res = await MakeApiRequest(req);

            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<SingleRegiUserResponse>(res.Content);

            throw new RegistrarConnectionException("Error retrieving registrar users");
        }

        public static async Task<IList<SingleCustomUserFieldResponse>> GetCustomFields()
        {
            var req = CreateRequest(RegistrarUris.getCustomFields, Method.GET);
            var res = await MakeApiRequest(req);

            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IList<SingleCustomUserFieldResponse>>(res.Content);

            throw new RegistrarConnectionException("Error getting custom field settings");
        }

        public static async Task<IList<SingleCustomUserFieldResponse>> UpdateCustomFields(
            IList<CreateCustomUserFieldModel> models)
        {
            var req = CreateRequest(RegistrarUris.updateCustomFields, Method.POST, models);
            var res = await MakeApiRequest(req);

            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IList<SingleCustomUserFieldResponse>>(res.Content);

            throw new RegistrarConnectionException("Error updating custom field settings");
        }

        public static async Task<SingleRegiUserResponse> CreateRegistrarUser(CreateRegiUser model)
        {
            var req = CreateRequest(RegistrarUris.createUser, Method.POST, model);
            var res = await MakeApiRequest(req);

            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<SingleRegiUserResponse>(res.Content);

            throw new RegistrarConnectionException("Error registering registrar user");
        }

        public static async Task SetEmailConfirmed(int id)
        {
            var req = CreateRequest(string.Format(RegistrarUris.setEmailConfirmed, id), Method.POST);
            var res = await MakeApiRequest(req);

            if (res.StatusCode != HttpStatusCode.OK)
                throw new RegistrarConnectionException("Could not confirm email");
        }

        public static async Task ChangePassword(ChangePasswordModel model)
        {
            var req = CreateRequest(RegistrarUris.changePassword, Method.POST, model);
            var res = await MakeApiRequest(req);

            if (res.StatusCode != HttpStatusCode.OK)
                throw new RegistrarConnectionException("Could not change password");
        }

        public static async Task DeleteUser(int id)
        {
            var req = CreateRequest(RegistrarUris.deleteUser, Method.DELETE);
            var res = await MakeApiRequest(req);

            if (res.StatusCode != HttpStatusCode.OK)
                throw new RegistrarConnectionException("Could not delete user");
        }

        public static async Task<IList<SingleRegiSettingResponse>> ListRegistrarSettings()
        {
            var req = CreateRequest(RegistrarUris.listSettings, Method.GET);
            req.JsonSerializer.ContentType = "application/json; charset=utf-8";
            req.AddHeader("Accept", "application/json");
            var res = await MakeApiRequest(req);

            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IList<SingleRegiSettingResponse>>(res.Content);

            throw new RegistrarConnectionException("Error listing registrar settings");
        }

        public static async Task<SingleRegiSettingResponse> UpdateRegistrarSetting(UpdateRegiSetting model)
        {
            var req = CreateRequest(RegistrarUris.updateSetting, Method.POST, model);
            req.JsonSerializer.ContentType = "application/json; charset=utf-8";
            req.AddHeader("Accept", "application/json");
            var res = await MakeApiRequest(req);

            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<SingleRegiSettingResponse>(res.Content);

            throw new RegistrarConnectionException("Error updating registrar settings");
        }

        public static async Task<object> GetResults(string blockchainName)
        {
            var req = CreateRequest(RegistrarUris.getResults, Method.GET);
            req.AddQueryParameter("chainString", blockchainName);
            req.AddHeader("Accept", "application/json");
            var res = await MakeApiRequest(req);

            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IEnumerable<BlockchainQuestionResultsResponse>>(res.Content);
            else if (res.StatusCode == HttpStatusCode.Unauthorized)
                throw new RegistrarConnectionException("Encrypted results");

            throw new RegistrarConnectionException("Error getting blockchain results");
        }
    }
}