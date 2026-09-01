using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IAuthenticationService
    {
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO, CancellationToken ct = default);
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDTO, CancellationToken ct = default);
        Task<Result<bool>> CheckEmailAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> GetUserAddressAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> UpdateUserAddressAsync(AddressDTO addressDTO ,string email, CancellationToken ct = default);
        Task<Result<UserDTO>> GetCurrentUserAsync(string email, CancellationToken ct = default);

    }
}
