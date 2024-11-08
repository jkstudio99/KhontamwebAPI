using System;

namespace KhontamwebAPI.DTOs.Response;

public class ProductDTO
{
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductPicture { get; set; }
    public string? CategoryName { get; set; }
    public string? ProductStatus { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
