using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Common.Exceptions;
using Microsoft.AspNet.Identity;
using RestSharp;
using RestSharp.Authenticators;

namespace Registrar.Api.Auth
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var client = new RestClient
            {
                BaseUrl = new Uri("https://api.mailgun.net/v3"),
                Authenticator = new HttpBasicAuthenticator("api",
                    ConfigurationManager.AppSettings["MailGunKey"])
            };

            var domain = ConfigurationManager.AppSettings["SupportEmailAddr"].Split('@')[1];

            var request = new RestRequest();
            request.AddParameter("domain", domain, ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", $"Evoto <{ConfigurationManager.AppSettings["SupportEmailAddr"]}>");
            request.AddParameter("to", message.Destination);
            request.AddParameter("subject", message.Subject);
            request.AddParameter("html", message.Body);
            request.Method = Method.POST;
            var res = await client.ExecuteTaskAsync(request);

            if (res.StatusCode != HttpStatusCode.OK)
                throw new CouldNotSendEmailException();
        }
    }
}