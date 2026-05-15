using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, GetCategoriesResponse>
{
    private readonly IApplicationDbContext _context;

    public GetCategoriesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetCategoriesResponse> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _context.Categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                IconUrl = c.IconUrl
            })
            .ToListAsync(cancellationToken);

        return new GetCategoriesResponse
        {
            Success = true,
            Message = "Categories retrieved successfully.",
            Data = categories
        };
    }
}
