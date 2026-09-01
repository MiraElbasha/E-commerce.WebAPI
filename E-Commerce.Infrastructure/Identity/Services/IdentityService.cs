using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Authentication;
using E_Commerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManger;

        public IdentityService(UserManager<ApplicationUser> userManger)
        {
            _userManger = userManger;
        }
        public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await _userManger.FindByEmailAsync(email);
            if (user is null) 
            {
                return Result<bool>.Fail(Error.NotFound("User Not Found"));
            }

            var IsValid = await _userManger.CheckPasswordAsync(user, password);
            return IsValid;
        }

        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDTO registerDTO, CancellationToken ct = default)
        {
            var user = new ApplicationUser
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
                PhoneNumber = registerDTO.PhoneNumber,
                DisplayName = registerDTO.DisplayName,
            };

            var result = await _userManger.CreateAsync(user , registerDTO.Password);
            if (!result.Succeeded) 
            {
                var errors = result.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
            }
            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id , user.Email , user.UserName , user.DisplayName));
        }


        public async Task<Result<IdentityUserResult>> FindByEmailAsyn(string email, CancellationToken ct = default)
        {
            var user = await _userManger.FindByEmailAsync(email);
            if (user is null)
            {
                return Result<IdentityUserResult>.Fail(Error.NotFound("User Not Found"));
            }
            else 
            {
                return new IdentityUserResult(user.Id, user.Email , user.UserName , user.DisplayName);
            }
        }
        public async Task<Result<bool>> EmailExsitsAsync(string email, CancellationToken ct = default)
        
          =>  await _userManger.FindByEmailAsync (email) is not null;
        

        public async Task<Result<AddressDTO>> GetAddressByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManger.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email, ct);

            if (user is null)
            {
                return Result<AddressDTO>.Fail(Error.NotFound($"User, {email} Not Found"));
            }

            if (user?.Address == null)
            {
                return Result<AddressDTO>.Fail(Error.NotFound($"Address Not Found"));
            }
            return new AddressDTO
            {
                FirstName = user.Address.FirstName,
                LastName = user.Address.LastName,
                City = user.Address.City,
                Street = user.Address.Street,
                Country = user.Address.Country,

            };
        }

        public async Task<Result<IReadOnlyList<string>>> GetRolesAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManger.FindByEmailAsync(email);
            if(user is null)
            {
                return Result<IReadOnlyList<string>>.Fail(Error.NotFound($"User, {email} Not Found"));
            }
            var roles = await _userManger.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<Result<AddressDTO>> UpdateAddressAsync(string email, AddressDTO addressDTO, CancellationToken ct = default)
        {
            var user = await _userManger.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email, ct);

            if (user is null)
            {
                return Result<AddressDTO>.Fail(Error.NotFound($"User, {email} Not Found"));
            }
            if (user.Address == null)
            {
                user.Address = new Address
                {
                    FirstName = addressDTO.FirstName,
                    LastName = addressDTO.LastName,
                    City = addressDTO.City,
                    Street = addressDTO.Street,
                    Country = addressDTO.Country,
                };
            }

            else
            {
                user.Address.FirstName = addressDTO.FirstName;
                user.Address.LastName = addressDTO.LastName;
                user.Address.City = addressDTO.City;
                user.Address.Street = addressDTO.Street;
                user.Address.Country = addressDTO.Country;

            }
            var result = await _userManger.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return Result<AddressDTO>.Fail(Error.Failure("Failed" , string.Join(";" , result.Errors.Select(e => e.Description))));
            }

            return addressDTO;
        }
    }
}
