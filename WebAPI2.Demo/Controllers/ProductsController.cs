using System.Collections.Generic;
using System.Web.Http;
using WebAPI2.Demo.Models;

namespace WebAPI2.Demo.Controllers
{
    public class ProductsController: ApiController
    {
        [HttpGet]
        [Authorize]
        //[AcceptVerbs("Get", "Head")]
        [ActionName("FindAll")]
        public IList<Product> FindAllProduct()
        {
            IList<Product> _productList = new List<Product>();
            _productList.Add(new Product() {Id ="1", ProductName ="Apple iPhone", Category = new Category() { Id="1", CategoryName= "Electronics"}, Price = 769.99});
            _productList.Add(new Product() { Id = "2", ProductName = "Samsung Galaxy Edge", Category = new Category() { Id = "1", CategoryName = "Electronics" }, Price = 699.99 });
            _productList.Add(new Product() { Id = "3", ProductName = "LG Note", Category = new Category() { Id = "1", CategoryName = "Electronics" }, Price = 500.99 });
            _productList.Add(new Product() { Id = "4", ProductName = "Lego Blocks", Category = new Category() { Id = "2", CategoryName = "Toys" }, Price = 100 });
            _productList.Add(new Product() { Id = "5", ProductName = "Barbie Dolls", Category = new Category() { Id = "2", CategoryName = "Toys" }, Price = 100 });
            return _productList;
        }
        [HttpGet]
        [NonAction]
        public IList<Product> GetAllProduct()
        {
            IList<Product> _productList = new List<Product>();
            _productList.Add(new Product() { Id = "1", ProductName = "Apple iPhone", Category = new Category() { Id = "1", CategoryName = "Electronics" }, Price = 769.99 });
            _productList.Add(new Product() { Id = "2", ProductName = "Samsung Galaxy Edge", Category = new Category() { Id = "1", CategoryName = "Electronics" }, Price = 699.99 });
            _productList.Add(new Product() { Id = "3", ProductName = "LG Note", Category = new Category() { Id = "1", CategoryName = "Electronics" }, Price = 500.99 });
            return _productList;
        }
    }
}