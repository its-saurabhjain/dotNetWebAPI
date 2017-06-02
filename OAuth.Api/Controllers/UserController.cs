using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;

namespace OAuth.Api.Controllers
{

    public class UserController : ApiController
    {
        //install-package Thinktecture.IdentityModel.WebApi.ScopeAuthorization
        [ScopeAuthorize("read")]
        //[Authorize]
        [HttpGet]
        public string Get()
        {
            return "Saurabh";
        }
    }
}
