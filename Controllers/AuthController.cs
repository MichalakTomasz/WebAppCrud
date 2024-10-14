using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppCrud.Mediator;
using WebAppCrud.Models;

namespace WebAppCrud.Controllers
{
    [ApiController]
	[Route("{controller}")]
	public class AuthController : ControllerBase
	{
		private readonly IMediator _mediator;

		public AuthController(IMediator mediator)
			=> _mediator = mediator;

		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult<AuthResult>> Auth(AuthModel auth)
		{
			var authResult = await _mediator.Send(new AuthRequest { AuthModel = auth });
			if (authResult.IsAuthorized)
				return Ok(authResult);

			return BadRequest(authResult);
		}

		[HttpPost(nameof(Register))]
		[AllowAnonymous]
		public async Task<IActionResult> Register(NewAppUser newAppUser)
		{
			var registerResult = await _mediator.Send(new RegisterRequest { NewUser = newAppUser });
			if (registerResult)
				return Ok(true);

			return BadRequest(false);
		}

		[HttpDelete("deleteaccount")]
		[Authorize(Roles = CommonConsts.Admin)]
		public async Task<ActionResult<bool>> DeleteAccountAsync(GuidRequest request)
		{
			var deleteResult = await _mediator.Send(new DeleteUserRequest { UserId = request.Guid });
			if (deleteResult)
				return Ok(true);

			return BadRequest(false);
		}
	}
}
