using Domain.Interfaces;
using Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppCrud.Mediator;

namespace WebAppCrud.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PersonController : ControllerBase
	{
		private readonly IGenericRepository<Person> _appRepository;
		private readonly IValidator<Person> _personValidator;
		private readonly IMediator _mediator;

		public PersonController(
			IGenericRepository<Person> appRepository,
			IValidator<Person> personValidator,
			IMediator mediator)
        {
			_appRepository = appRepository;
			_personValidator = personValidator;
			_mediator = mediator;
		}

		[HttpGet]
		[Authorize(policy: CommonConsts.GuestPolicy)]
		public async Task<ActionResult<IReadOnlyList<Person>>> GetPeopleFull()
			=> await _mediator.Send(new GetGenericRequest<Person> { Includes = new[] { nameof(Address)} });

		[HttpGet("{id}")]
		public async Task<ActionResult<Person>> GetPersonAsync(int id)
			=> await _mediator.Send(new GetGenericByIdRequest<Person> { Id = id, Includes = new[] { nameof(Address) } } );

		[HttpPost]
		public async Task<ActionResult<Person>> AddPersonAsync(Person person)
		{

			var validationResult = _personValidator.Validate(person);
			if (!validationResult.IsValid)
				return BadRequest(validationResult.Errors);

			var newPerson = await _appRepository.AddAsync(person);
			if (newPerson == default)
			{
				return BadRequest();
			}

			return newPerson;
		}

		[HttpPut]
		public async Task<ActionResult<Person>> UpdatePersonAsync(Person person)
		{

			var updatedPerson = await _appRepository.UpdateAsync(person);
			if (updatedPerson == default)
			{
				return default;
			}
				
			return updatedPerson;			
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult<bool>> DeletePersonAsync(int id)
		{
			var person =  await _appRepository.DeleteAsync(id);
			return await Task.FromResult(true);
		}
	}
}
