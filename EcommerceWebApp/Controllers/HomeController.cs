using EcommerceApp.Services;
using EcommerceWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public HomeController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllProductsAsync();
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        return View(products);
    }

    public async Task<IActionResult> Search(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return RedirectToAction(nameof(Index));
        }

        var products = await _productService.SearchProductsAsync(searchTerm);
        ViewBag.SearchTerm = searchTerm;
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        return View("Index", products);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}