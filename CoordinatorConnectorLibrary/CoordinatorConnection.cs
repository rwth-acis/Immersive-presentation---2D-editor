using System;
using RestSharp;

namespace CoordinatorConnectorLibrary
{
    public class CoordinatorConnection
    {
        private string email;
        private string password;
        private string token;
        private DateTime tokenExp;
        public CoordinatorConnection(string pEmail, string pPassword)
        {
            email = pEmail;
            password = pPassword;
        }

        public void setPassword(string pPassword)
        {
            password = pPassword;
        }
        public void setEmail(string pEmail)
        {
            email = pEmail;
        }
        public bool login()
        {
            var client = new RestClient("http://binarybros.de/auth/login");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("email", email.ToString());
            request.AddParameter("password", password.ToString());
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
            if(response.StatusCode.ToString() == "200")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
