using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppCrud.Mediator;
using WebAppCrud.Models;
using WebAppCrud.Services;

namespace WebAppCrud.Controllers
{
	[ApiController]
	[Route("{controller}")]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IConfiguration _configuration;
		private readonly ITokenService _tokenService;
		private readonly IMediator _mediator;

		public AuthController(
			UserManager<IdentityUser> userManager, 
			RoleManager<IdentityRole> roleManager, 
			IConfiguration configuration,
			ITokenService tokenService,
			IMediator mediator)
        {
			_userManager = userManager;
			_roleManager = roleManager;
			_configuration = configuration;
			_tokenService = tokenService;
			_mediator = mediator;
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult<AuthResult>> Auth(AuthViewModel auth)
		{
			switch (auth.AuthType)
			{
				case AuthType.LogIn:
					var user = await _userManager.FindByEmailAsync(auth.Credentials.Email);
					if (user == null)
						return Unauthorized();

					var isValidPassword = await _userManager.CheckPasswordAsync(user, auth.Credentials.Password);
					if (!isValidPassword)
						return Unauthorized();

					var roles = await _userManager.GetRolesAsync(user);

					AuthResult loginResult = new()
					{
						UserId = user.Id,
						Token = await _mediator.Send(new GenerateTokenRequest { InputRoles = new InputRoles { Roles = roles as List<string> } }),
						//Token = _tokenService.GenerateToken(new InputRoles { Roles = roles as List<string> }),
						Roles = roles.ToList()
					};
					return Ok(loginResult);

				default:
					AuthResult authResult = new()
					{
						Token = await _mediator.Send(new GenerateTokenRequest()),
						//Token = _tokenService.GenerateToken(),
						Roles = new List<string> { CommonConsts.Guest }
					};
					return Ok(authResult);
			}
			
		}

		[HttpPost(nameof(Register))]
		[AllowAnonymous]
		public async Task<IActionResult> Register(NewAppUser newAppUser)
		{
			IdentityUser identityUser = new()
			{
				UserName = newAppUser.Email,
				Email = newAppUser.Email,
				PasswordHash = newAppUser.Password,
			};

			var registerResult = await _userManager.CreateAsync(identityUser, newAppUser.Password);

			if (!registerResult.Succeeded)
			{
				return BadRequest(registerResult.Errors);
			}

			var addRolesResult = await _userManager.AddToRolesAsync(identityUser, Roles.List);
			if (!addRolesResult.Succeeded)
				return BadRequest(addRolesResult.Errors);

			return Ok(registerResult.Succeeded);
		}

		[HttpDelete("deleteaccount")]
		[Authorize(Roles = CommonConsts.Admin)]
		public async Task<ActionResult<bool>> DeleteAccountAsync(GuidRequest request)
		{
			var loggedUser = await _userManager.FindByIdAsync(request.Guid.ToString());
			if (loggedUser == default)
				return NotFound();

			var deleteUserResult = await _userManager.DeleteAsync(loggedUser);
			if (!deleteUserResult.Succeeded)
				return BadRequest(false);

			return Ok(true);
		}
	}
}
