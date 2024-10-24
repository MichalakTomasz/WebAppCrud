using AutoMapper;
using Domain.Models;

namespace WebAppCrud.AutoMapper
{
	public class AutoMapperProfile : Profile
	{
        public AutoMapperProfile()
        {
            CreateMap<InputProduct, Product>();
        }
    }
}
