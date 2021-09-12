﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Data.Entities;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public EFAppContext _context { get; set; }

        public HomeController(ILogger<HomeController> logger, EFAppContext context)
        {
            _logger = logger;
            _context = context;
        }
        #region Default
        public IActionResult Index()
        {
            return View(_context.Products.Include(x => x.Images).Select(x => new ProductViewModel { 
                Name = x.Name,
                Price = x.Price,
                Id = x.Id,
                Images = x.Images.Select(x => new ProductImageItemViewModel { 
                Path = x.Name,
                }).ToList()
            }).ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(ProductAddViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Дані введено не коректно!");
                return View(model);

            }

            var product = new Product();
            product.Name = model.Name;
            product.Price = model.Price;
            product.Images = new List<ProductImage>();
            _context.Products.Add(product);
            _context.SaveChanges();

            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "images");

            if (model.Images != null)
            {
                foreach (var newImages in model.Images)
                {
                    string ext = Path.GetExtension(newImages.FileName);
                    string fileName = Path.GetRandomFileName() + ext;

                    string filePath = Path.Combine(dirPath, fileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        newImages.CopyTo(stream);
                    }

                    _context.ProductImages.Add(new Data.Entities.ProductImage
                    {
                        Name = fileName,
                        ProductId = product.Id
                    });
                }
            }

            _context.SaveChanges();


            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult Edit() 
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Edit(ProductEditViewModel model) 
        {
            if (!ModelState.IsValid) { 
                ModelState.AddModelError("", "Дані введено не коректно!");
                return View(model);
            
            }

            if (model.Id == 0) 
            {
                ModelState.AddModelError("", "Оберіть продукт видалення!");
                return View(model);
            }

            var product = _context.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == model.Id);
            if (product != null) 
            {
                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "images");
                product.Name = model.Name;
                product.Price = model.Price;
                if (model.deletedImages != null) { 
                    foreach (var delProduct in model.deletedImages) 
                {
                    var delProductImage = product.Images.SingleOrDefault(x => delProduct.Contains(x.Name));
                    string imgPath = Path.Combine(dirPath, delProductImage.Name);
                    if (System.IO.File.Exists(imgPath)) 
                    {
                        System.IO.File.Delete(imgPath);
                    }
                    _context.ProductImages.Remove(delProductImage);
                }
                }
                if (model.Images != null) 
                {
                    foreach (var newImages in model.Images)
                {
                    string ext = Path.GetExtension(newImages.FileName);
                    string fileName = Path.GetRandomFileName() + ext;

                    string filePath = Path.Combine(dirPath, fileName);
                    using (var stream = System.IO.File.Create(filePath)) 
                    {
                        newImages.CopyTo(stream);
                    }

                    _context.ProductImages.Add(new Data.Entities.ProductImage { 
                        Name = fileName,
                        ProductId = product.Id
                    });
                }
                }

                _context.SaveChanges();
            }
            else {
                ModelState.AddModelError("", "Помилка коду! Зверніться до сервісного центру!");
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult GetData(int id) 
        {
            var product = _context.Products.Include(x => x.Images).FirstOrDefault(x => x.Id == id);
            ProductViewModel model = new ProductViewModel { 
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Images = product.Images.Select(x => new ProductImageItemViewModel { 
                Path = x.Name
            }).ToList()
            };
            return Ok(JsonConvert.SerializeObject(model));
        }


        [HttpPost]
        public IActionResult Delete(int id) 
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);
            if (product != null)
            {
                List<ProductImage> images = _context
                    .ProductImages.Where(x => x.ProductId == product.Id).ToList();

                foreach (var image in images)
                {
                    string imageName = Path.Combine(Directory.GetCurrentDirectory(), "images", image.Name);
                    if (System.IO.File.Exists(imageName))
                    {
                        System.IO.File.Delete(imageName);
                    }

                    _context.ProductImages.Remove(image);
                }
                _context.SaveChanges();

                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return Ok();
        }
        
    }

}
