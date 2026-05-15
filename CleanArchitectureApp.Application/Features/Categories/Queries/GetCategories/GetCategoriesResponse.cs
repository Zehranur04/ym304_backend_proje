using System.Collections.Generic;
using CleanArchitectureApp.Application.DTOs;

namespace CleanArchitectureApp.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<CategoryDto> Data { get; set; } = new();
}
