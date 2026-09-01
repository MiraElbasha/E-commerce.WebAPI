using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IIdentityService identityService , ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<Result<bool>> CheckEmailAsync(string email, CancellationToken ct = default)
        => await _identityService.EmailExsitsAsync(email, ct);


        public async Task<Result<UserDTO>> GetCurrentUserAsync(string email, CancellationToken ct = default)
        {
            var result = await _identityService.FindByEmailAsyn(email, ct);
            if (!result.IsSuccess) 
            {
                return Result<UserDTO>.Fail(result.Errors);
            }
            var user = result.data;
            var rolesResults = await _identityService.GetRolesAsync(email, ct);
            if (!rolesResults.IsSuccess)
            {
                return Result<UserDTO>.Fail(rolesResults.Errors);
            }
            var roles = rolesResults.data;
            var Token = _tokenService.CreateToken(user.Id,user.Email ,user.UserName, roles);
            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = Token
            };
        }

        public async Task<Result<AddressDTO>> GetUserAddressAsync(string email, CancellationToken ct = default)
        {
            var result = await _identityService.GetAddressByEmailAsync(email, ct);
            if (!result.IsSuccess) 
            {
                return Result<AddressDTO>.Fail(result.Errors);
            }
            return result.data;
        }

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO, CancellationToken ct = default)
        {
            //get user by email
            var userResult = await _identityService.FindByEmailAsyn(loginDTO.Email, ct);
            if (!userResult.IsSuccess) 
            {
                return Result<UserDTO>.Fail(userResult.Errors);
            }

            //check password
            var passwordResult = await _identityService.CheckPasswordAsync(loginDTO.Email, loginDTO.Password, ct);
            if (!passwordResult.IsSuccess)
            {
                return Result<UserDTO>.Fail(Error.Unauthorized("Invalid Email Or Password"));
            }

            var rolesResults = await _identityService.GetRolesAsync(loginDTO.Email, ct);
            if (!rolesResults.IsSuccess)
            {
                return Result<UserDTO>.Fail(rolesResults.Errors);
            }
            var roles = rolesResults.data;
            var user = userResult.data;
            var Token = _tokenService.CreateToken(user.Id, user.Email, user.UserName, roles);
            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = Token
            };
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO, CancellationToken ct = default)
        {
            var result = await _identityService.CreateUserAsync(registerDTO, ct);
            if (!result.IsSuccess || result.data is null) 
            {
                return Result<UserDTO>.Fail(result.Errors);

            }

            return new UserDTO
            {
                Email = result.data.Email,
                DisplayName = result.data.DisplayName,
                Token = "Token"
            };
        }

        public async Task<Result<AddressDTO>> UpdateUserAddressAsync(AddressDTO addressDTO, string email, CancellationToken ct = default)
      => await _identityService.UpdateAddressAsync(email, addressDTO, ct);
    }
}
