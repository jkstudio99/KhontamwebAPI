using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KhontamwebAPI.Enums;

public enum ProductStatus
{
    [Display(Name = "แบบร่าง")]
    [Description("Draft - แบบร่าง")]
    Draft = 0,
    
    [Display(Name = "เผยแพร่")]
    [Description("Published - เผยแพร่")]
    Published = 1
} 