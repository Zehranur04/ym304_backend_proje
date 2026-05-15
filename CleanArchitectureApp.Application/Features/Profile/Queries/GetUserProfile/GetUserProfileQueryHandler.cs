using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.DTOs;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Profile.Queries.GetUserProfile;

public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, GetUserProfileResponse>
{
    private readonly IApplicationDbContext _context;

    public GetUserProfileQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetUserProfileResponse> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.UserProfile)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("UserId", "Kullanıcı bulunamadı.")
            };
            throw new ValidationException(failures);
        }

        var dto = new UserProfileDto
        {
            UserId = user.Id,
            Name = user.Name,
            Surname = user.Surname,
            FullName = $"{user.Name} {user.Surname}",
            Email = user.Email,
            Age = user.Age,
            Gender = user.Gender.ToString(),
            Bio = user.UserProfile?.Bio,
            ProfilePictureUrl = user.UserProfile?.ProfilePictureUrl,
            TotalScore = user.UserProfile?.TotalScore ?? 0
        };

        return new GetUserProfileResponse
        {
            Success = true,
            Message = "Profil başarıyla getirildi.",
            Data = dto
        };
    }
}
