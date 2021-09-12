using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductShop.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public List<ProductImageItemViewModel> Images { get; set; }
    }

    public class ProductImageItemViewModel
    {
        public string Path { get; set; }
    }

    /// <summary>
    /// Модель для зміни товару
    /// </summary>
    public class ProductEditViewModel
    {
        public int Id { get; set; }
        [Display(Name="Оберіть товар")]
        public ProductOptionViewModel SelectGroup { get; set; }
        [Display(Name = "Назва"),Required(ErrorMessage = "Поле 'Назва' не може бути пустим!")]
        public string Name { get; set; }
        [Display(Name="Ціна"), Required(ErrorMessage = "Поле 'Ціна' не може бути пустим!")]
        public decimal Price { get; set; }
        [Display(Name = "Фотографії")]
        public List<IFormFile> Images { get; set; }
        public List<string> deletedImages { get; set; }
    }

    /// <summary>
    /// Модель для створення товару
    /// </summary>
    public class ProductAddViewModel
    {
        [Display(Name = "Назва"), Required(ErrorMessage = "Поле 'Назва' не може бути пустим!")]
        public string Name { get; set; }
        [Display(Name = "Ціна"), Required(ErrorMessage = "Поле 'Ціна' не може бути пустим!")]
        public decimal Price { get; set; }
        [Display(Name = "Фотографії")]
        public List<IFormFile> Images { get; set; }
    }

    public class ProductOptionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
