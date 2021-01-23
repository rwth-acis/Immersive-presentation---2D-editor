using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Numerics;
using System.IO;
using RestSharp.Extensions;
using System.Threading.Tasks;

namespace CoordinatorConnectorLibrary
{
    public class CoordinatorConnection
    {
        private string email;
        private string password;
        private int iduser;
        private string token;
        private DateTime tokenExp;
        private bool loggedIn;

        public RestClient client;

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static DateTime FromUnixTimeMilliSek(long unixTimeMilliSek)
        {
            return epoch.AddMilliseconds(unixTimeMilliSek);
        }
        public CoordinatorConnection()
        {
            client = new RestClient("https://cloud19.dbis.rwth-aachen.de");
            //ToDo: set a good timeout value
            client.Timeout = -1;
            loggedIn = false;
        }

        public void setPassword(string pPassword)
        {
            password = pPassword;
        }
        public void setEmail(string pEmail)
        {
            email = pEmail;
        }
        public int getIdUser()
        {
            return iduser;
        }
        public bool login(string pEmail, string pPassword)
        {
            email = pEmail;
            password = pPassword;
            //Build and execute Request
            var request = new RestRequest("/auth/login", Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("email", email.ToString());
            request.AddParameter("password", password.ToString());
            IRestResponse response = client.Execute(request);
            if (!(response.StatusCode == HttpStatusCode.OK)) return false;

            //Deserialize Response
            try
            {

                JObject output = JObject.Parse(response.Content);

                JToken helpJToken;
                if (!output.TryGetValue("token", out helpJToken)) return false;
                token = helpJToken.ToString();
                if (!output.TryGetValue("exp", out helpJToken)) return false;
                long helpInt;
                if (!long.TryParse(helpJToken.ToString(), out helpInt)) return false;
                tokenExp = FromUnixTimeMilliSek(helpInt);
                if (!output.TryGetValue("user", out helpJToken)) return false;
                JObject user = JObject.Parse(helpJToken.ToString());
                if (!user.TryGetValue("iduser", out helpJToken)) return false;
                if (!int.TryParse(helpJToken.ToString(), out iduser)) return false;
                loggedIn = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool loginLearningLayers(string pEmail, string pAccessToken)
        {
            email = pEmail;
            //Build and execute Request
            var request = new RestRequest("/auth/openid", Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("email", email.ToString());
            request.AddParameter("accesstoken", pAccessToken.ToString());
            IRestResponse response = client.Execute(request);
            if (!(response.StatusCode == HttpStatusCode.OK)) return false;

            //Deserialize Response
            try
            {

                JObject output = JObject.Parse(response.Content);

                JToken helpJToken;
                if (!output.TryGetValue("token", out helpJToken)) return false;
                token = helpJToken.ToString();
                if (!output.TryGetValue("exp", out helpJToken)) return false;
                long helpInt;
                if (!long.TryParse(helpJToken.ToString(), out helpInt)) return false;
                tokenExp = FromUnixTimeMilliSek(helpInt);
                if (!output.TryGetValue("user", out helpJToken)) return false;
                JObject user = JObject.Parse(helpJToken.ToString());
                if (!user.TryGetValue("iduser", out helpJToken)) return false;
                if (!int.TryParse(helpJToken.ToString(), out iduser)) return false;
                loggedIn = true;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void logout()
        {
            loggedIn = false;
        }
        public bool checkExp()
        {
            if (!loggedIn) return false;
            if(tokenExp < DateTime.Now.AddMinutes(15))
            {
                return login(email, password);
            }
            else
            {
                return true;
            }
        }

        public string register(string pEmail, string pPassword)
        {
            //Build and execute Request
            var request = new RestRequest("/auth/register", Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("email", pEmail.ToString());
            request.AddParameter("password", pPassword.ToString());
            IRestResponse response = client.Execute(request);
            if ((response.StatusCode == HttpStatusCode.OK)) return "";

            try
            {
                //Error Handling
                JObject output = JObject.Parse(response.Content);
                JToken helpJToken;
                if (!output.TryGetValue("message", out helpJToken)) return "Intern Error. Try again later.";
                string message = helpJToken.ToString();
                //ToDo log the recieved error code

                return message;
            }
            catch
            {
                return "Intern Error. Try again later.";
            }
            
        }

        public async Task<string> uploadPresentation(string pPath, string pId)
        {
            if (!checkExp()) return "Upload failed - Session expired.";
            if (!File.Exists(pPath)) return "File not found.";

            var request = new RestRequest("/presentation/upload", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddFile("presentation", pPath);
            request.AddParameter("idpresentation", pId);
            IRestResponse response = await client.ExecuteAsync(request);
            if ((response.StatusCode == HttpStatusCode.OK)) return "";
            Console.WriteLine("Upload Error by Parameters: " + pPath + " " + pId + " " + token);
            try
            {
                //Error Handling
                JObject output = JObject.Parse(response.Content);
                JToken helpJToken;
                if (!output.TryGetValue("message", out helpJToken)) return "Intern Error. Try again later.";
                string message = helpJToken.ToString();
                //ToDo log the recieved error code

                return message;
            }
            catch
            {
                return "Internal Error.";
            }
        }

        public string newPresentation(string name)
        {
            if(!checkExp()) return "";
            var request = new RestRequest("/presentation", Method.POST);
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("name", name);
            IRestResponse response = client.Execute(request);
            if ((response.StatusCode == HttpStatusCode.OK))
            {
                //Handle presentationId
                //Deserialize Response
                try
                {

                    JObject output = JObject.Parse(response.Content);

                    JToken helpJToken;
                    if (!output.TryGetValue("idpresentation", out helpJToken)) return "";
                    string idPresentation = helpJToken.ToString();
                    Console.WriteLine("ID: " + idPresentation);
                    return idPresentation;
                }
                catch
                {
                    return "";
                }
            }
            else
            {
                //No Id recieved
                return "";
            }
        }

        public PresentationStartResponse startPresentation(string pPresentationId)
        {
            //Build and execute Request
            var request = new RestRequest("/presentation/start", Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Bearer " + token);
            request.AddParameter("idpresentation", pPresentationId);
            IRestResponse response = client.Execute(request);
            if (!(response.StatusCode == HttpStatusCode.OK)) return null;

            //Deserialize Response
            try
            {
                PresentationStartResponse res = new PresentationStartResponse();
                JObject output = JObject.Parse(response.Content);

                JToken helpJToken;
                if (!output.TryGetValue("photonRoomName", out helpJToken)) return null;
                res.photonRoomName = helpJToken.ToString();
                if (!output.TryGetValue("exp", out helpJToken)) return null;
                long helpInt;
                if (!long.TryParse(helpJToken.ToString(), out helpInt)) return null;
                res.exp = helpInt;
                if (!output.TryGetValue("shortCode", out helpJToken)) return null;
                res.shortCode = helpJToken.ToString();
                if (!output.TryGetValue("invitationToken", out helpJToken)) return null;
                res.invitationToken = helpJToken.ToString();
                if (!output.TryGetValue("message", out helpJToken)) return null;
                res.message = helpJToken.ToString();
                return res;
            }
            catch
            {
                return null;
            }
        }
    
        public ListResponse loadPresentationList()
        {
            //Build and execute Request
            var request = new RestRequest("/presentations", Method.GET);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);
            if (!(response.StatusCode == HttpStatusCode.OK)) return null;

            ListResponse listresponse = JsonConvert.DeserializeObject<ListResponse>(response.Content);
            return listresponse;
        }
        
        public bool downloadPresentation(string path, string idpresentation)
        {
            //Build and execute Request
            var request = new RestRequest(string.Format("/presentation/foreditor?idpresentation={0}", idpresentation), Method.GET);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Bearer " + token);
            //try
            //{
                byte[] fileBytes = client.DownloadData(request);
                File.WriteAllBytes(path, fileBytes);
                return true;
            //}
            //catch
            //{
            //    return false;
            //}
        }
    }

    public class PresentationStartResponse
    {
        public string photonRoomName { get; set; }
        public string shortCode { get; set; }
        public string invitationToken { get; set; }
        public long exp { get; set; }
        public string message { get; set; }
    }

    public class PresentationElement
    {
        public string iduser { get; set; }
        public string idpresentation { get; set; }
        public string filepath { get; set; }
        public object timeofcreation { get; set; }
        public string name { get; set; }
        public long? lastchange { get; set; }
    }

    public class ListResponse
    {
        public List<PresentationElement> presentations { get; set; }
    }
}
