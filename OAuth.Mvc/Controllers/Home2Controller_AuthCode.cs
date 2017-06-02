using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Clients;

namespace OAuth.Mvc.Controllers
{
    public class Home2Controller_AuthCodeFlow : Controller
    {
        public ActionResult Index()
        {
            //return View();
            if (User.Identity.IsAuthenticated) {
                var claimPrincipal = User as ClaimsPrincipal;
                return Content(claimPrincipal.FindFirst("access_token").Value);
            }
            //Setting up for AuthorizationCode flow manually
            string url = "http://OAuthserver/connect/authorize" +
                            "?client_id=socialnetwork_code" +
                            "&redirect_uri=http://localhost:26654/Home2/AuthorizationCallback/" +
                            "&response_type=code" +
                            "&scope=openid+profile" +
                            "&response_mode=form_post";
            return Redirect(url);
        }

        public ActionResult AuthorizationCallback(string code, string state, string error) {
            var tokenUrl = "http://OAuthserver/connect/token";
            //install-package thinktecture.identitymodel
            var client = new OAuth2Client(new System.Uri(tokenUrl), "socialnetwork_code", "secret");
            var requestResult = client.RequestAccessTokenCode(code, new System.Uri("http://localhost:26654/Home2/AuthorizationCallback/"));

            var claims = new[]
            {
                new Claim("access_token", requestResult.AccessToken)
            };
            var identity = new ClaimsIdentity(claims, "Cookies");
            Request.GetOwinContext().Authentication.SignIn(identity);
            return Redirect("/");
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
