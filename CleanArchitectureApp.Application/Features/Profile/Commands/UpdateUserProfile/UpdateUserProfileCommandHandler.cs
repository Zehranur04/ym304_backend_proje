using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitectureApp.Application.Interfaces.Persistence;
using CleanArchitectureApp.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureApp.Application.Features.Profile.Commands.UpdateUserProfile;

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UpdateUserProfileResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateUserProfileCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateUserProfileResponse> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
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

        user.Name = request.Name;
        user.Surname = request.Surname;

        if (user.UserProfile == null)
        {
            user.UserProfile = new UserProfile
            {
                UserId = user.Id,
                TotalScore = 0,
                Bio = request.Bio,
                ProfilePictureUrl = request.ProfilePictureUrl
            };
        }
        else
        {
            user.UserProfile.Bio = request.Bio;
            user.UserProfile.ProfilePictureUrl = request.ProfilePictureUrl;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateUserProfileResponse
        {
            Success = true,
            Message = "Profil başarıyla güncellendi."
        };
    }
}
