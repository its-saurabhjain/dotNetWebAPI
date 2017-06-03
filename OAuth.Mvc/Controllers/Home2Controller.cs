using Microsoft.Owin.Security;
using Newtonsoft.Json;
using OAuth.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Clients;

namespace OAuth.Mvc.Controllers
{
    public class Home2Controller : Controller
    {
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Login(){

            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public void Register(string username, string password)
        {
            var userRepository = new UserRepository(
                () => new SqlConnection(ConfigurationManager.ConnectionStrings["User.DB"].ConnectionString));
            userRepository.RegisterUser(username, Sha512(password));
        }
        public static string Sha512(string input)
        {
            using (var sha = SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        [HttpPost]
        public async Task<ActionResult> Login(string username, string password)
        {
            /*
            var client = new OAuth2Client(new System.Uri("http://oauthserver/connect/token"), "socialnetwork", "sceret");
            var requestResponse = client.RequestAccessTokenUserName(username, password, "openid profile");
            */
            var requestResponse = await GetTokenAsync(username, password);
            var claims = new[]
                {
                    new Claim("access_token", requestResponse.AccessToken),
                    new Claim("refresh_token", requestResponse.RefreshToken)
                };
            var claimIdentity = new ClaimsIdentity(claims, "Cookies");
            HttpContext.GetOwinContext().Authentication.SignIn(claimIdentity);

            //await GetTokenAsync();
            return Redirect("/Home2/Private");
        }
        private async Task<TokenResponse> GetTokenAsync(string username, string password)
        {
            using (var client = new HttpClient())
            {
                var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes("socialnetwork:secret"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
                var rawResult = await client.PostAsync("http://localhost:15638/connect/token",
                        new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string,string>(
                                "grant_type",
                                "password"),
                            new KeyValuePair<string,string>(
                                "scope",
                                "openid profile offline_access"),
                            new KeyValuePair<string,string>(
                                "username",
                                username),
                            new KeyValuePair<string,string>(
                                "password",
                                 password)
                        }));
                var dat = await rawResult.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TokenResponse>(dat);
            }
        }
        public ActionResult RefreshAccessToken() {

            var claimPrincipal = User as ClaimsPrincipal;
            var client = new OAuth2Client(new System.Uri("http://OAuthServer/connect/token"), "socialnetwork", "secret");
            var requestResponse = client.RequestAccessTokenRefreshToken(
                claimPrincipal.FindFirst("refresh_token").Value);
            var manager = HttpContext.GetOwinContext().Authentication;
            var refreshedIdentity = new ClaimsIdentity(User.Identity);
            refreshedIdentity.RemoveClaim(refreshedIdentity.FindFirst("access_token"));
            refreshedIdentity.RemoveClaim(refreshedIdentity.FindFirst("refresh_token"));

            refreshedIdentity.AddClaim(new Claim("access_token", requestResponse.AccessToken));
            refreshedIdentity.AddClaim(new Claim("refresh_token", requestResponse.RefreshToken));

            manager.AuthenticationResponseGrant = new AuthenticationResponseGrant(
                                                                new ClaimsPrincipal(refreshedIdentity),
                                                                new AuthenticationProperties { IsPersistent = true});

            return Redirect("/Home2/Private");
        }
        [Authorize]
        public async Task<ActionResult> Private()
        {
            //return Content("OK");
            var claimsPrincipal = User as ClaimsPrincipal;
            using (HttpClient client = new HttpClient())
            {
                
                //set authorization header
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", claimsPrincipal.FindFirst("access_token").Value);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                HttpResponseMessage response = await client.GetAsync("http://localhost:26654/api/profile/GetAsync");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var table = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);
                }
            }
          
            return View(claimsPrincipal.Claims);
            //api call
            /*
            using (HttpClient client = new HttpClient())
            {
                //set authorization header
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", claimsPrincipal.FindFirst("access_token").Value);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
                HttpResponseMessage response = await client.GetAsync("http://localhost:7825/api/products/findall");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    //var table = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);
                }
            }
            */
        }
        [Authorize]
        public ActionResult LogOut()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
