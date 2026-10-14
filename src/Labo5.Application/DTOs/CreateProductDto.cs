using System.ComponentModel.DataAnnotations;

namespace Labo5.Application.DTOs;

public class CreateProductDto
{
    [Required] 
    public string Name { get; set; } = string.Empty;

    [Required] 
    public decimal Price { get; set; }

    public string Description { get; set; } = string.Empty;
}
