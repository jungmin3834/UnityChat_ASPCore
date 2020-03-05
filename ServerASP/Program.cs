using System;

namespace ServerASP
{
    class Program
    {
        [Route("api/[controller]")]
        public class ProductsController : Controller
        {
            [HttpGet("{id}")]
            public IActionResult GetProduct(int id)
            {
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
