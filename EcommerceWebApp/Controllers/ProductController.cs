using EcommerceWebApp.Models;
using EcommerceWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public ProductsController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    public async Task<IActionResult> Category(int id)
    {
        var products = await _productService.GetProductsByCategoryAsync(id);
        var category = await _categoryService.GetCategoryByIdAsync(id);

        // Fix: If category is a string, use it directly; otherwise, access Name property
        ViewBag.CategoryName = category ?? "Category";
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();

        return View("~/Views/Home/Index.cshtml", products);
    }

    [Authorize]
    public async Task<IActionResult> Manage()
    {
        var products = await _productService.GetAllProductsAsync();
        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        return View();
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (ModelState.IsValid)
        {
            await _productService.CreateProductAsync(product);
            return RedirectToAction(nameof(Manage));
        }

        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        return View(product);
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        return View(product);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Product product)
    {
        if (ModelState.IsValid)
        {
            await _productService.UpdateProductAsync(product);
            return RedirectToAction(nameof(Manage));
        }

        ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
        return View(product);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteProductAsync(id);
        return RedirectToAction(nameof(Manage));
    }
}