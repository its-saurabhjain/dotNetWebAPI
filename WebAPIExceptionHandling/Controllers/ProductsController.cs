using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebAPIExceptionHandling.Filters;
using WebAPIExceptionHandling.Models;

namespace WebAPIExceptionHandling.Controllers
{
    //[NotImplExceptionFilter] //Registered Globally
    public class ProductsController : ApiController
    {

        public IList<Product> Get()
        {
            IList<Product> _productList = new List<Product>();

            _productList.Add(new Product() { Id = "1", Name = "Apple iPhone 7+", Price = "769", Category = "Electronic" });
            _productList.Add(new Product() { Id = "2", Name = "Apple iPhone 7", Price = "650", Category = "Electronic" });
            _productList.Add(new Product() { Id = "3", Name = "Apple iPhone SE", Price = "450", Category = "Electronic" });
            _productList.Add(new Product() { Id = "4", Name = "Apple iPhone 6", Price = "600", Category = "Electronic" });
            _productList.Add(new Product() { Id = "5", Name = "Apple iPhone 6S", Price = "750", Category = "Electronic" });

            return _productList;
        }

        //[NotImplExceptionFilter] //Registered at controller
        public IList<Product> Get(string id)
        {
            IList<Product> _productList = new List<Product>();
            //Not Caught Exception
            if (id == "0")
            {
                throw new Exception();
            }
            //HTTPResponse Exception
            if (id == "1")
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            //HTTPResponse, there will be a response content returned 
            if (id == "2")
            {

                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("No product with ID = {0}", id)),
                    ReasonPhrase = "Product ID Not Found Exception"
                };
                throw new HttpResponseException(resp);
            }
            //Using Exception filters
            if (id == "3")
            {
                throw new NotImplementedException();

            }
            //similar to id=2 above just that instead of creating HttpResponse we create HttpErrorResponse
            if (id == "4")
            {
                var message = string.Format("Product with id = {0} not found", id);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            return _productList;
        }

        /*
        public HttpResponseMessage GetProduct(string id)
        {
            if (id == "1")
            {
                var message = string.Format("Product with id = {0} not found", id);
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, message);
            }
            else
            {
                var product = new Product() { Id = "1", Name = "Apple iPhone 7+", Price = "769", Category = "Electronic" };
                return Request.CreateResponse(HttpStatusCode.OK, product);
            }
        }
        */
    }
}