using Microsoft.AspNetCore.Mvc;
using ProductShop.Data;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductShop.ViewComponents
{
    public class ShowProductViewComponent : ViewComponent
    {
        public EFAppContext _context { get; set; }
        public ShowProductViewComponent(EFAppContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync() 
        {
            return await Task.Run(() => {
                return View("_ShowProduct", _context.Products.Select(x => new ProductOptionViewModel { 
                    Id = x.Id,
                    Name = x.Name
                }).ToList());
            });
        }
    }
}
