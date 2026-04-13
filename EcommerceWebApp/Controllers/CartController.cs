using EcommerceWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceApp.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IProductService _productService;

    public CartController(ICartService cartService, IProductService productService)
    {
        _cartService = cartService;
        _productService = productService;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim ?? "0");
    }

    public async Task<IActionResult> Index()
    {
        var userId = GetUserId();
        var cart = await _cartService.GetCartByUserIdAsync(userId);
        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        var userId = GetUserId();
        var product = await _productService.GetProductByIdAsync(productId);

        if (product == null)
        {
            return NotFound();
        }

        if (product.Stock < quantity)
        {
            TempData["Error"] = "Not enough stock available.";
            return RedirectToAction("Details", "Products", new { id = productId });
        }

        await _cartService.AddToCartAsync(userId, productId, quantity);
        TempData["Success"] = "Product added to cart!";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
    {
        if (quantity <= 0)
        {
            return await RemoveItem(cartItemId);
        }

        await _cartService.UpdateCartItemAsync(cartItemId, quantity);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> RemoveItem(int cartItemId)
    {
        await _cartService.RemoveFromCartAsync(cartItemId);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Clear()
    {
        var userId = GetUserId();
        await _cartService.ClearCartAsync(userId);
        return RedirectToAction(nameof(Index));
    }
}