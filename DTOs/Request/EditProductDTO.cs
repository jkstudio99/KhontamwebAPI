using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using KhontamwebAPI.Enums;

namespace KhontamwebAPI.DTOs.Request;

public class EditProductDTO
{
    [Required(ErrorMessage = "Product name is required")]
    public string? ProductName { get; set; }

    public IFormFile? ProductPicture { get; set; }

    [Required(ErrorMessage = "Category name is required")]
    public string? CategoryName { get; set; }

    /// <summary>
    /// Product status: 0 = Draft, 1 = Published
    /// </summary>
    [Required(ErrorMessage = "Product status is required")]
    [EnumDataType(typeof(ProductStatus))]
    public ProductStatus ProductStatus { get; set; }

    public string? Description { get; set; }
}
