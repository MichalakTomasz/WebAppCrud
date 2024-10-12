using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppCrud.Mediator;
using WebAppCrud.Models;
using WebAppCrud.Notifications;

namespace WebAppCrud.Controllers
{

	[ApiController]
	[Route("[Controller]")]
	public class ProductController : ControllerBase
	{
		private readonly IMediator _mediator;

		public ProductController(IMediator mediator)
		{
			_mediator = mediator;
		}

		[HttpGet]
		[Authorize(policy: CommonConsts.GuestPolicy)]
		public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
		{
			var products = await _mediator.Send(new GetAllProductsRequest());

			await _mediator.Publish(new LoggerNotification
			{
				NotificationType = NotificationType.Get,
				Message = nameof(GetAllProducts)
			});

			return Ok(products);
		}

		[HttpGet("{id}")]
		[Authorize(policy: CommonConsts.GuestPolicy)]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var getProductResult =  await _mediator.Send(new GetGenericByIdRequest<Product> { Id = id });
			if (getProductResult == default)
				return NotFound();

			await _mediator.Publish(new LoggerNotification
			{
				NotificationType = NotificationType.Get,
				Message = nameof(GetProduct)
			});

			return Ok(getProductResult);
		}

		[HttpPut]
		[Authorize(policy: CommonConsts.AdminPolicy)]
		public async Task<ActionResult<Product>> UpdateProduct(InputProduct product)
		{
			var validationResult = await _mediator.Send(new ProductValidationRequest { ProductDto = product });
			if (!validationResult.IsValid)
				return BadRequest(validationResult.Errors);

			var findProductResult = await _mediator.Send(new GetGenericByIdRequest<Product> { Id = product.Id });
			if (findProductResult == default)
				return NotFound();

			findProductResult.UrlPicture = product.UrlPicture;
			findProductResult.Price = product.Price;
			findProductResult.Name = product.Name;
			findProductResult.Description = product.Description;
			findProductResult.Code = product.Code;

			UpdateProductRequest updateProduct = new() { Product = findProductResult };
			var updateResult = await _mediator.Send(updateProduct);
			if (updateResult == default)
				return NotFound();

			await _mediator.Publish(new LoggerNotification
			{
				NotificationType = NotificationType.Update,
				Message = nameof(UpdateProduct)
			});

			return Ok(updateResult);
		}

		[HttpPost]
		[Authorize(policy: CommonConsts.AdminPolicy)]
		public async Task<ActionResult<Product>> AddProduct(InputProduct product)
		{
			var validationResult = await _mediator.Send(new ProductValidationRequest { ProductDto = product });
			if (!validationResult.IsValid)
				return BadRequest(validationResult.Errors);

			AddProductRequest addProduct = new() { Product = product };
			var addProductResult = await _mediator.Send(addProduct);
			if (addProductResult == default)
				return BadRequest();
			
			await _mediator.Publish(new LoggerNotification
			{
				NotificationType = NotificationType.Create,
				Message = nameof(AddProduct)
			});

			return Ok(addProductResult);
		}

		[HttpDelete("{id}")]
		[Authorize(policy: CommonConsts.AdminPolicy)]
		public async Task<ActionResult<bool>> DeleteProduct(int id)
		{
			var deleteProductResult = await _mediator.Send(new DeleteProductRequest { Id = id });
			if (deleteProductResult == default)
				return NotFound();

			await _mediator.Publish(new LoggerNotification
			{
				NotificationType = NotificationType.Delete,
				Message = nameof(DeleteProduct)
			});

			return Ok(deleteProductResult);
		}
	}
}
