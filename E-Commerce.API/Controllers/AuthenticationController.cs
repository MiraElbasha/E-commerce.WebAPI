using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{

    public class AuthenticationController : APIBaseController
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        #region Login
        [HttpPost("Login")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO, CancellationToken ct) => ToActionResult(await _authenticationService.LoginAsync(loginDTO, ct));
        #endregion

        #region Register
        [HttpPost("Register")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO, CancellationToken ct) =>
            ToActionResult(await _authenticationService.RegisterAsync(registerDTO , ct));
        #endregion

        #region Email Exists
        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmail([FromQuery] string email, CancellationToken ct = default)
       => ToActionResult(await _authenticationService.CheckEmailAsync(email, ct));
        #endregion

        #region Current User
        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser(CancellationToken ct = default)
            => ToActionResult(await _authenticationService.GetCurrentUserAsync(GetEmailFromToken(), ct));
        #endregion

        #region User Address
        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress(CancellationToken ct)
            => ToActionResult(await _authenticationService.GetUserAddressAsync(GetEmailFromToken(), ct));
        #endregion

        #region UpdateAddress
        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO addressDTO, CancellationToken ct)
            => ToActionResult(await _authenticationService.UpdateUserAddressAsync(addressDTO, GetEmailFromToken(), ct));
        #endregion
    }
}
