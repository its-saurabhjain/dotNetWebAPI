using Microsoft.Owin.Security;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Clients;

namespace OAuth.Mvc.Controllers
{
    public class Home2Controller_RefreshToken : Controller
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
        public ActionResult RefreshAccessToken() {

            var claimPrincipal = User as ClaimsPrincipal;
            var client = new OAuth2Client(new System.Uri("http://OAuthServer/connect/token"), "socialnetwork_code", "secret");
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
}
