﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Numerics;
using System.IO;

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
            client = new RestClient("http://binarybros.de");
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

        public string uploadPresentation(string pPath, string pId)
        {
            var request = new RestRequest("/presentation/upload", Method.POST);
            if (!File.Exists(pPath)) return "File not found.";

            request.AddFile("presentation", pPath);
            request.AddParameter("idpresentation", pId);
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
                return "Internal Error.";
            }
        }
    }
}