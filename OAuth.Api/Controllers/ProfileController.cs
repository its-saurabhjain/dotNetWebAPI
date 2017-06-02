using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors; //Install-Package Microsoft.AspNet.WebApi.Cors
//using Thinktecture.IdentityModel.WebApi;

namespace OAuth.Api.Controllers
{
    public class ProfileController : ApiController
    {
        [HttpGet]
        [ActionName("GetName")]
        public async Task<IHttpActionResult> GetName()
        {
            return Ok(HttpStatusCode.Accepted);
        }

        [HttpGet]
        [Authorize]
        [ActionName("GetAsync")]
        [EnableCors("http://OAuthServer", "*", "GET POST")]
        //[ScopeAuthorize("read")] //install-package Thinktecture.IdentityModel.WebApi.ScopeAuthorization
        public async Task<IHttpActionResult> GetAsync()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            string userName = claimsPrincipal?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            return Ok(HttpStatusCode.Accepted);
        }

        [HttpGet]
        [Authorize]
        [ActionName("Claims")]
        //[ScopeAuthorize("read")] //install-package Thinktecture.IdentityModel.WebApi.ScopeAuthorization
        public async Task<IHttpActionResult> GetClaims()
        {
            var claimsPrincipal = User as ClaimsPrincipal;
            string userName = claimsPrincipal?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            return Ok(userName);
        }
    }
}
