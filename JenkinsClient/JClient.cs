using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System.Collections.Generic;

namespace JenkinsClient
{
    public class JClient : IJClient
    {
        public RestClient Client { get; private set; }

        public JClient(string baseUri, string username, string password)
        {
            Client = new RestClient(baseUri)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };
        }

        public Jobs GetJobs()
        {
            var endpoint = "/api/json";
            var request = new RestRequest(endpoint, Method.GET).AddParameter("tree", "jobs[name,color]");
            var resp = Client.Execute(request);

            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                return new Jobs() { jobs = new List<Job>() };

            return JsonConvert.DeserializeObject<Jobs>(resp.Content);
        }

        public Builds GetBuilds(string jobname, int take = 5)
        {
            var endpoint = $"/job/{jobname}/api/json";
            var request = new RestRequest(endpoint, Method.GET).AddParameter("tree", $"builds[number,status,timestamp,id,result]{{,{take}}}");
            var resp = Client.Execute(request);

            if (resp.StatusCode != System.Net.HttpStatusCode.OK)
                return new Builds() { builds = new List<Build>() };

            return JsonConvert.DeserializeObject<Builds>(resp.Content);
        }

        public bool Test()
        {
            var endpoint = $"/api/json";
            var request = new RestRequest(endpoint, Method.GET).AddParameter("json", $"pretty");
            var resp = Client.Execute(request);
            return resp.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
