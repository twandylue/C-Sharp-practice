using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
    public class RequestBodyForToken
    {
        public string grant_type { get; set; }
        public string code { get; set; }
        public string redirect_uri { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
    public class Utility
    {
        public async Task<GetTokenFromCodeResult> GetTokenFromCode(string code, string clientId, string clientSecret, string redirectURI)
        {
            try
            {
                RequestBodyForToken reqBodyForToken = new RequestBodyForToken
                {
                    grant_type = "authorization_code",
                    code = code,
                    redirect_uri = redirectURI,
                    client_id = clientId,
                    client_secret = clientSecret
                };

                var postBody = new StringContent(
                    JsonConvert.SerializeObject(reqBodyForToken),
                    Encoding.UTF8,
                    "application/json"
                );

                HttpRequestMessage req = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://www.googleapis.com/oauth2/v4/token"
                )
                {
                    Content = postBody
                };

                var response = await new HttpClient().SendAsync(req);
                var GetTokenFromCodeResult = JsonConvert.DeserializeObject<GetTokenFromCodeResult>(await response.Content.ReadAsStringAsync());
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

        public async Task<GoogleUserInfo> GetUserInfo(string token)
        {
            try
            {
                HttpRequestMessage req = new HttpRequestMessage(
                    HttpMethod.Get,
                    "https://www.googleapis.com/oauth2/v1/userinfo"
                );
                req.Headers.Clear();
                req.Headers.Add("Authorization", "Bearer  " + token);
                var response = await new HttpClient().SendAsync(req);
                GoogleUserInfo ret = JsonConvert.DeserializeObject<GoogleUserInfo>(await response.Content.ReadAsStringAsync());
                return ret;
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