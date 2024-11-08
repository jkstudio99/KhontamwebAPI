using System;
using System.ComponentModel.DataAnnotations;
using KhontamwebAPI.Enums;

namespace KhontamwebAPI.DTOs.Request;

public class GetProductDTO
{
    [Range(1, 10000)]
    public int PageIndex { get; set; } = 1;

    [Range(1, 100)]
    public int PageSize { get; set; } = 5;

    public string? Keyword { get; set; }
    public ProductStatus? Status { get; set; }
}
