using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KhontamwebAPI.Enums;

namespace KhontamwebAPI.Models;

[Table("Products")]
public class ProductModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductId { get; set; }

    [Required]
    [StringLength(255)]
    public string ProductName { get; set; } = string.Empty;

    [StringLength(255)]
    public string? ProductPicture { get; set; }

    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [Required]
    [EnumDataType(typeof(ProductStatus))]
    public ProductStatus ProductStatus { get; set; }

    public string? Description { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedDate { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime ModifiedDate { get; set; }
}
