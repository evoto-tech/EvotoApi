﻿using System;
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
            if (!ConfigurationManager.AppSettings.Get("ApiKeys").IsNullOrWhiteSpace())
                req.AddQueryParameter("key", ConfigurationManager.AppSettings.Get("ApiKeys"));
            return await client.ExecuteTaskAsync(req);
        }

        public static async Task<bool> CreateBlockchain(ManaVote model)
        {
            var req = new RestRequest("management/createblockchain");
            req.AddBody(JsonConvert.SerializeObject(model));

            var res = await MakeApiRequest(req);
            return res.StatusCode == HttpStatusCode.OK;
        }

        public static async Task<IEnumerable<SingleRegiUserResponse>> GetRegistrarUsers()
        {
            // TODO: Put in resource dictionary
            var req = new RestRequest("/users/list");
            var res = await MakeApiRequest(req);
            if (res.StatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<IEnumerable<SingleRegiUserResponse>>(res.Content);
            throw new Exception("Error retrieving registrar users");
        }

        public static async Task<SingleRegiUserResponse> GetRegistrarUser(int id)
        {
            // TODO: Put in resource dictionary
            var req = new RestRequest("/users/details/" + id);
            var res = await MakeApiRequest(req);
            if (res.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<SingleRegiUserResponse>(res.Content);
            }
            throw new Exception("Error retrieving registrar users");
        }

        public static async Task<IList<CustomUserField>> UpdateCustomFields(IList<CreateCustomUserFieldModel> models)
        {
            var req = new RestRequest("users/customFields/update");
            req.Method = Method.POST;
            req.JsonSerializer.ContentType = "application/json; charset=utf-8";
            req.AddHeader("Accept", "application/json");
            req.Parameters.Clear();
            req.AddParameter("application/json", JsonConvert.SerializeObject(models), ParameterType.RequestBody);
            var res = await MakeApiRequest(req);
            Debug.WriteLine(JsonConvert.SerializeObject(models));
            try
            {
                Debug.WriteLine(JsonConvert.DeserializeObject<IList<CreateCustomUserFieldModel>>(JsonConvert.SerializeObject(models)).ToString());
            } catch (Exception e)
            {
                throw e;
            }
            if (res.StatusCode == HttpStatusCode.OK)
            {
                return JsonConvert.DeserializeObject<IList<CustomUserField>>(res.Content);
            }
            var exception = new Exception("Error updating custom field settings");
            exception.Data["status"] = res.StatusCode;
            exception.Data["content"] = res.Content;
            throw exception;
        }

        public static async Task<SingleRegiUserResponse> CreateRegistrarUser(CreateRegiUser model)
        {
            // TODO: Put in resource dictionary
            var req = new RestRequest("/account/register");
            req.AddBody(JsonConvert.SerializeObject(model));

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
