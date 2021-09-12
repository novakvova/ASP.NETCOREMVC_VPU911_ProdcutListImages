using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public EFAppContext _context { get; set; }
        public ProductController(EFAppContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("get/{id}")]
        public IActionResult GetData(int id)
        {
            var product = _context.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == id);
            ProductViewModel model = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Images = product.Images.Select(x => new ProductImageItemViewModel
                {
                    Path = x.Name
                }).ToList()
            };
            return Ok(model);
        }
    }
}
