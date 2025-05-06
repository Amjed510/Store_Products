using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store_app.Models;
using Store_app.services;

namespace Store_app.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbcontext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbcontext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        // Get featured products (most recent 4 products)
        var featuredProducts = _context.Products
            .OrderByDescending(p => p.createdAt)
            .Take(4)
            .ToList();

        // Get products by categories
        var categories = _context.Products
            .Select(p => p.categgory)
            .Distinct()
            .Take(3)
            .ToList();

        var productsByCategory = new Dictionary<string, List<Product>>();
        
        foreach (var category in categories)
        {
            var products = _context.Products
                .Where(p => p.categgory == category)
                .Take(4)
                .ToList();
                
            productsByCategory.Add(category, products);
        }

        ViewBag.FeaturedProducts = featuredProducts;
        ViewBag.ProductsByCategory = productsByCategory;
        
        return View();
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
}
