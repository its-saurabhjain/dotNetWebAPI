using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace WebAPIExceptionHandling.Controllers
{
    public class CustomersController : ApiController
    {
        public CustomersController()
        {
            throw new NotSupportedException();

        }
        public HttpResponse GetCustomer()
        {
            throw new HttpResponseException(System.Net.HttpStatusCode.NotImplemented);
        }

    }
}