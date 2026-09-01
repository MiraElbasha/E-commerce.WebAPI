using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IIdentityService
    {
        Task<Result<IdentityUserResult>> FindByEmailAsyn(string email, CancellationToken ct = default);
        Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDTO registerDTO, CancellationToken ct = default);
        Task<Result<bool>> CheckPasswordAsync(string email, string password , CancellationToken ct = default);
        Task<Result<IReadOnlyList<string>>> GetRolesAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> GetAddressByEmailAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> UpdateAddressAsync(string email,AddressDTO addressDTO, CancellationToken ct = default);
        Task<Result<bool>> EmailExsitsAsync(string email, CancellationToken ct = default);
        

    }
}
