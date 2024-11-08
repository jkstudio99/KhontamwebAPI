using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KhontamwebAPI.DTOs.Request;
using KhontamwebAPI.DTOs.Response;
using KhontamwebAPI.Models;
using KhontamwebAPI.Services;
using Microsoft.AspNetCore.Cors;

namespace KhontamwebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors("MyCors")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ICloudinaryService _cloudinaryService;

    public ProductController(AppDbContext context, ICloudinaryService cloudinaryService)
    {
        _context = context;
        _cloudinaryService = cloudinaryService;
    }

    // GET: api/Product
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagingDTO<ProductDTO>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PagingDTO<ProductDTO>>> GetProducts([FromQuery] GetProductDTO request)
    {
        var query = _context.Products.AsQueryable();
        // เพิ่มส่วนนี้สำหรับ filter ตาม Status
        if (request.Status.HasValue)
        {
            query = query.Where(p => p.ProductStatus == request.Status.Value);
        }

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(p =>
                p.ProductName.Contains(request.Keyword) ||
                p.CategoryName.Contains(request.Keyword));
        }

        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new ProductDTO
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                ProductPicture = p.ProductPicture,
                CategoryName = p.CategoryName,
                ProductStatus = p.ProductStatus.ToString(),
                CreatedDate = p.CreatedDate,
                ModifiedDate = p.ModifiedDate
            })
            .ToListAsync();

        return Ok(new PagingDTO<ProductDTO>
        {
            TotalItems = totalItems,
            Items = items
        });
    }

    // GET: api/Product/5
    [HttpGet("{id}")]
    [ProducesResponseType(statusCode: StatusCodes.Status200OK, type: typeof(ProductDetailDTO))]
    [ProducesResponseType(statusCode: StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(int id)
    {
        var curProduct = await _context.Products.FindAsync(id);

        if (curProduct == null)
        {
            return NotFound();
        }

        var result = new ProductDetailDTO
        {
            ProductId = curProduct.ProductId,
            ProductName = curProduct.ProductName,
            ProductPicture = curProduct.ProductPicture,
            CategoryName = curProduct.CategoryName,
            ProductStatus = curProduct.ProductStatus.ToString(),
            Description = curProduct.Description,
            CreatedDate = curProduct.CreatedDate,
            ModifiedDate = curProduct.ModifiedDate
        };

        return Ok(result);
    }

    // POST: api/Product
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ProductDTO>> CreateProduct([FromForm] AddProductDTO request)
    {
        // Add file validation
        if (Request.Form.Files.Count > 0)
        {
            var file = Request.Form.Files[0];
            if (!IsValidImageFile(file))
            {
                return BadRequest("Invalid file. Only JPG, PNG, or GIF images up to 5MB are allowed.");
            }
        }

        var curProduct = new ProductModel
        {
            ProductName = request.ProductName!,
            CategoryName = request.CategoryName!,
            ProductStatus = request.ProductStatus!,
            Description = request.Description
        };

        if (Request.Form.Files.Count > 0)
        {
            var file = Request.Form.Files[0];
            curProduct.ProductPicture = await _cloudinaryService.UploadImageAsync(file);
        }

        _context.Products.Add(curProduct);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = curProduct.ProductId },
            new ProductDTO
            {
                ProductId = curProduct.ProductId,
                ProductName = curProduct.ProductName,
                ProductPicture = curProduct.ProductPicture,
                CategoryName = curProduct.CategoryName,
                ProductStatus = curProduct.ProductStatus.ToString(),
                CreatedDate = curProduct.CreatedDate,
                ModifiedDate = curProduct.ModifiedDate
            });
    }

    // PUT: api/Product/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateProduct(int id, [FromForm] EditProductDTO request)
    {
        try
        {
            var curProduct = await _context.Products.FindAsync(id);
            if (curProduct == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }

            // Add file validation
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                if (!IsValidImageFile(file))
                {
                    return BadRequest("Invalid file. Only JPG, PNG, or GIF images up to 5MB are allowed.");
                }

                // Delete old image if exists
                if (!string.IsNullOrEmpty(curProduct.ProductPicture))
                {
                    await _cloudinaryService.DeleteImageAsync(curProduct.ProductPicture);
                }
                // Upload new image
                curProduct.ProductPicture = await _cloudinaryService.UploadImageAsync(file);
            }

            curProduct.ProductName = request.ProductName!;
            curProduct.CategoryName = request.CategoryName!;
            curProduct.ProductStatus = request.ProductStatus;
            curProduct.Description = request.Description;

            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException) when (!ProductExists(id))
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            // Log the error
            return StatusCode(500, $"An error occurred while updating the product: {ex.Message}");
        }
    }

    // DELETE: api/Product/5
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var curProduct = await _context.Products.FindAsync(id);
        if (curProduct == null)
        {
            return NotFound();
        }

        if (!string.IsNullOrEmpty(curProduct.ProductPicture))
        {
            await _cloudinaryService.DeleteImageAsync(curProduct.ProductPicture);
        }

        _context.Products.Remove(curProduct);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.ProductId == id);
    }

    private bool IsValidImageFile(IFormFile file)
    {
        // ตรวจสอบนามสกุลไฟล์
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(fileExtension))
        {
            return false;
        }

        // ตรวจสอบ MIME type
        if (!file.ContentType.StartsWith("image/"))
        {
            return false;
        }

        // ตรวจสอบขนาดไฟล์ (5MB)
        if (file.Length > 5 * 1024 * 1024)
        {
            return false;
        }

        return true;
    }
}
