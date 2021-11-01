using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
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

                // get
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
    }
}