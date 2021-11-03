using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace MVC.Models
{
    public class GetTokenFromCodeResult
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public string token_type { get; set; }
        public string id_token { get; set; }
    }
    public class Utility
    {
        public static GetTokenFromCodeResult GetTokenFromCode(string code, string clientId, string clientSecret, string redirectURI)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = System.Text.Encoding.UTF8;
                wc.Headers.Clear();
                // wc.Headers.Add("Content-Type", "application/json");

                var data = new System.Collections.Specialized.NameValueCollection();
                data["grant_type"] = "authorization_code";
                data["code"] = code;
                data["redirect_uri"] = redirectURI;
                data["client_id"] = clientId;
                data["client_secret"] = clientSecret;

                // post
                byte[] bResult = wc.UploadValues("https://www.googleapis.com/oauth2/v4/token", data);

                // get result
                string jsonString = System.Text.Encoding.UTF8.GetString(bResult);

                var GetTokenFromCodeResult = JsonConvert.DeserializeObject<GetTokenFromCodeResult>(jsonString);
                return GetTokenFromCodeResult;

            }
            catch (WebException ex)
            {
                using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream()))
                {
                    var responseText = reader.ReadToEnd();
                    throw new Exception("GetTokenFromCode: " + responseText, ex);
                }
            }
        }

        public static GoogleUserInfo GetUserInfo(string token)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.Encoding = System.Text.Encoding.UTF8;
                wc.Headers.Clear();
                wc.Headers.Add("Authorization", "Bearer  " + token);

                // get user info 
                string jsonString = wc.DownloadString("https://www.googleapis.com/oauth2/v1/userinfo");

                // parsing Json
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleUserInfo>(jsonString);
                return result;
            }
            catch (WebException ex)
            {
                using (var reader = new System.IO.StreamReader(ex.Response.GetResponseStream()))
                {
                    var responseText = reader.ReadToEnd();
                    throw new Exception("GetUserInfo: " + responseText, ex);
                }
            }
        }

        public static string GetGoogleSSOUrl(StateInfo stateInfo)
        {
            /*
                To pass several parameters to your redirect uri, have them stored in state parameter before calling Oauth url, the url after authorization will send the same parameters to your redirect uri as state=THE_STATE_PARAMETERS
                ref: https://stackoverflow.com/questions/7722062/google-oauth-2-0-redirect-uri-with-several-parameters
            */

            // TODO 可以移到env
            string state = JsonConvert.SerializeObject(stateInfo);
            string client_id = "536062935773-e1hvscne4ead0kk62fho999kc179rhhj.apps.googleusercontent.com";
            // string redirect_Uri = "https://localhost:5001/api/v1/checkPortalSSO";
            string redirect_Uri = "https://localhost:5001/";

            string url = "https://accounts.google.com/o/oauth2/v2/auth?";
            url += "scope=email profile&";
            url += $"redirect_uri={redirect_Uri}&";
            url += "response_type=code&";
            // url += "response_type=token&"; //* for token
            url += $"client_id={client_id}&";
            url += $"state={state}&";

            return url;
        }
    }
}