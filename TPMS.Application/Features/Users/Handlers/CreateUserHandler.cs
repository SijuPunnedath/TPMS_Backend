using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TPMS.Application.Features.Users.Commands;
using TPMS.Domain.Entities;
using TPMS.Infrastructure.Persistence.Configurations;

namespace TPMS.Application.Features.Users.Handlers;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly TPMSDBContext _db;

    public CreateUserHandler(TPMSDBContext db)
    {
        _db = db;
    }
    
    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var dto = request.User;

        // ✅ Validate uniqueness
        if (await _db.Users.AnyAsync(u => u.Username == dto.Username, cancellationToken))
            throw new InvalidOperationException($"Username '{dto.Username}' is already taken.");

        if (await _db.Users.AnyAsync(u => u.Email == dto.Email, cancellationToken))
            throw new InvalidOperationException($"Email '{dto.Email}' is already registered.");

        // ✅ Validate Role
        var roleExists = await _db.Roles.AnyAsync(r => r.RoleID == dto.RoleID && r.IsActive, cancellationToken);
        if (!roleExists)
            throw new InvalidOperationException($"Role ID {dto.RoleID} does not exist or is inactive.");
        // ✅ Hash password
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        // ✅ Create entity
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordHash,
            RoleID = dto.RoleID,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync(cancellationToken);

        return user.UserID;
    }
}