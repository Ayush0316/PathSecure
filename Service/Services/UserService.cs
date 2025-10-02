using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Crypto;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Mailer;
using Infrastructure.Session;
using Service.DTO;

namespace Service.Services;

public class UserService : IUserService
{
    private readonly ISessionContext _sessionContext;
    private readonly IUserRepository _userRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IEmailSender _emailSender;
    private readonly string EmailRegex = "^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,}$";
    private readonly string PasswordSetUrl = "https://localhost:44353/PasswordSet.aspx";
    public UserService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, ISessionContext context ,IEmailSender sender)
    {
        _sessionContext = context;
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _emailSender = sender;
    }
    public async Task<List<UserDto>> GetAll()
    {
        var users = await _userRepository.Query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).ToListAsync();
        var userDtos = users.Select(user => new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            Type = user.Type.ToString(),
            Roles = user.UserRoles.Select(ur => new RoleDto(ur.RoleId, ur.Role.Name)).ToList()
        }).ToList();

        return userDtos;
    }
    public async Task<UserDto> GetUser(string userId)
    {
        var userEntity = await _userRepository.Query.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).Where(u => u.Id == userId).FirstOrDefaultAsync();

        if (userEntity == null)
            throw new InvalidOperationException("User not found");

        var userDto = new UserDto()
        {
            Id = userEntity.Id,
            Email = userEntity.Email,
            Username = userEntity.Username,
            Type = userEntity.Type.ToString(),
            Roles = userEntity.UserRoles.Select(ur => new RoleDto(ur.RoleId, ur.Role.Name)).ToList()
        };
        
        return userDto;
    }

    public async Task<string> CreateUser(string username, string email, string type)
    {
        username.Trim();
        email.Trim();
        if (String.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username missing");

        if (!IsValidEmail(email))
            throw new ArgumentException("UserEmail is invalid");

        var exists = await _userRepository.Query
            .Where(u => (u.Email.ToLower() == email.ToLower() || u.Username.Equals(username)))
            .AnyAsync();

        if (exists)
            throw new InvalidOperationException($"User with username: {username} or email: {email} already exists");

        if (Enum.TryParse<UserType>(type, true, out var userType) == false)
            throw new ArgumentException("User type is invalid");

        Domain.Entities.User user = new Domain.Entities.User
        {
            Username = username,
            Email = email,
            Type = userType,
            Status = UserStatus.NOT_VERIFIED
        };
        if (userType == UserType.OAUTH2)
        {
            HandleOauthPassword(user);
            user.Status = UserStatus.VERIFIED;
        }
        else
            GeneratePasswordSetLink(user);

        await _userRepository.Create(user, false);
        await _userRepository.SaveChanges();
        return user.Id;
    }

    public async Task AssignRoles(string userId, ICollection<string> roleIds)
    {
        var user = await _userRepository.FindByPrimaryKey(userId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (roleIds == null || roleIds.Count == 0)
            throw new InvalidOperationException("No roles provided");

        var userRoleExists = await _userRoleRepository.Query
            .Where(ur => ur.UserId == userId && roleIds.Contains(ur.RoleId))
            .Select(ur => ur.RoleId)
            .ToListAsync();

        var rolesToAdd = roleIds.Except(userRoleExists).ToList();
        var rolesToRemove = userRoleExists.Except(roleIds).ToList();

        if (rolesToRemove.Count > 0)
        {
            await _userRoleRepository.RemoveUserRole(userId, rolesToRemove, false);
        }

        if (rolesToAdd.Count > 0)
        {
            await _userRoleRepository.AssignUserRole(userId, rolesToAdd, false);
        }

        await _userRoleRepository.SaveChanges();
    }

    public async Task SpecialUpdateUser(string userId, string? username, string? email)
    {
        var user = await UpdateUserHelper(userId, username, email);
        await _userRepository.Update(user);
    }

    public async Task UpdateUser(string userId, string? username, string? email, string? password)
    {
        string? sessionUserId = _sessionContext.UserContext?.UserId;
        if (sessionUserId == null || !String.Equals(sessionUserId, userId, StringComparison.OrdinalIgnoreCase))
            throw new UnauthorizedAccessException("You are not authorized to update this user");

        var user = await UpdateUserHelper(userId, username, email);

        if (!String.IsNullOrWhiteSpace(password))
            if (!BCryptService.IsValidPassword(password))
                throw new ArgumentException("Password is not strong enough");
            else
                user.PasswordHash = BCryptService.HashPassword(password);

        await _userRepository.Update(user);
    }
    public async Task DeleteUser(string userId)
    {
        var user = await _userRepository.FindByPrimaryKey(userId);

        if (user == null)
            throw new InvalidOperationException("User not found");

        await _userRepository.Delete(user);
    }

    private void HandleOauthPassword(Domain.Entities.User user)
    {
        var token = CryptoHelpers.GenerateRandomToken(12);
        token = ToUrlSafeToken(token);
        user.PasswordHash = BCryptService.HashPassword(token);
        OAuthMail(user, token);
    }

    private void GeneratePasswordSetLink(Domain.Entities.User user)
    {
        var token = CryptoHelpers.GenerateRandomToken();
        token = ToUrlSafeToken(token);
        var hashedToken = CryptoHelpers.HashToken(token);
        var passwordSetLink = new PasswordSetLinks
        {
            User = user,
            TokenHash = hashedToken,
            Expiry = DateTime.UtcNow.AddDays(1),
            Used = false
        };
        user.PasswordSetLinks.Add(passwordSetLink);
        PasswordSetMailer(user, token);
    }

    private void OAuthMail(Domain.Entities.User user, string token)
    {
        string mailBody = $"Your account has been created on PathSecure. Your user id is: {Uri.EscapeDataString(user.Id)} and token is: {Uri.EscapeDataString(token)}";
        Mailer(user, mailBody);
    }

    private void PasswordSetMailer(Domain.Entities.User user, string token)
    {
        string setLink = $"{PasswordSetUrl}?token={Uri.EscapeDataString(token)}&userId={Uri.EscapeDataString(user.Id)}";
        string mailBody = $"Your account has been created on PathSecure. Please set your password using the link:{setLink}";
        Mailer(user, mailBody);
    }

    private void Mailer(Domain.Entities.User user, string mailBody)
    {
        _emailSender.SendEmailAsync(user.Email, "PathSecure Account Created", mailBody);
    }

    private static string ToUrlSafeToken(string token)
    {
        if (string.IsNullOrEmpty(token)) return token;
        return token.Replace('+', '-').Replace('/', '_').TrimEnd('=');
    }

    private bool IsValidEmail(string email)
    {
        return Regex.IsMatch(email, EmailRegex, RegexOptions.IgnoreCase);
    }

    private async Task<User> UpdateUserHelper(string userId, string? username, string? email)
    {
        var user = await _userRepository.FindByPrimaryKey(userId);
        if (user == null)
            throw new InvalidOperationException("User not found");

        username = username?.Trim();
        email = email?.Trim();

        var query = _userRepository.Query.Where(u => u.Id != userId);
        if ((!String.IsNullOrEmpty(username)) && (!String.IsNullOrEmpty(email)))
        {
            query = query.Where(u => u.Email.ToLower() == email!.ToLower() || u.Username.Equals(username));
        }else if (!String.IsNullOrEmpty(username))
        {
            query = query.Where(u => u.Username.Equals(username));
        } else if (!String.IsNullOrEmpty(email))
        {
            query = query.Where(u => u.Email.ToLower() == email!.ToLower());
        }

        var exists = await query.AnyAsync();

        if (exists)
            throw new InvalidOperationException($"User with username: {username} or email: {email} already exists");

        if (!String.IsNullOrWhiteSpace(username))
            user.Username = username;

        if (!String.IsNullOrWhiteSpace(email))
            if (!IsValidEmail(email))
                throw new ArgumentException("UserEmail is invalid");
            else
                user.Email = email;

        return user;
    }
}
