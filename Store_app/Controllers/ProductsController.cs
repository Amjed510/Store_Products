using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store_app.Migrations;
using Store_app.Models;
using Store_app.services;

namespace Store_app.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbcontext context;
        private readonly IWebHostEnvironment environment;

        public ProductsController(ApplicationDbcontext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()

        {
            var Products = context.Products.OrderByDescending(p => p.Id).ToList();
            return View(Products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            // save the image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                productDto.ImageFile.CopyTo(stream);
            }


            // save the new product in the database
            Product product = new Product()
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                categgory = productDto.Category,
                price = productDto.Price.ToString(),
                Descrption = productDto.Description,
                Imgaqfillname = newFileName,
                createdAt = DateTime.Now,
            };

            context.Products.Add(product);
            context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }

        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            ProductDto productDto = new ProductDto()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.categgory,
                Price = decimal.Parse(product.price),
                Description = product.Descrption
            };

            ViewBag.ProductId = product.Id;
            ViewBag.CurrentImage = product.Imgaqfillname;

            return View(productDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, ProductDto productDto, string currentImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ProductId = id;
                ViewBag.CurrentImage = currentImage;
                return View(productDto);
            }

            var product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = productDto.Name;
            product.Brand = productDto.Brand;
            product.categgory = productDto.Category;
            product.price = productDto.Price.ToString();
            product.Descrption = productDto.Description;

            // Handle image update if a new image is provided
            if (productDto.ImageFile != null)
            {
                // Save the new image file
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.ImageFile.FileName);

                string imageFullPath = environment.WebRootPath + "/products/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    productDto.ImageFile.CopyTo(stream);
                }

                // Update the image filename in the database
                product.Imgaqfillname = newFileName;
            }

            context.Update(product);
            context.SaveChanges();
            return RedirectToAction("Index", "Products");
        }

        public IActionResult Details(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            // Delete the image file if it exists
            if (!string.IsNullOrEmpty(product.Imgaqfillname))
            {
                string imageFullPath = Path.Combine(environment.WebRootPath, "products", product.Imgaqfillname);
                if (System.IO.File.Exists(imageFullPath))
                {
                    System.IO.File.Delete(imageFullPath);
                }
            }

            // Remove the product from the database
            context.Products.Remove(product);
            context.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
    }
}
