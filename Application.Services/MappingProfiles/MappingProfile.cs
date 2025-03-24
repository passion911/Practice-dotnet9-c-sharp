using Application.Services.OrderStuffs;
using Application.Services.ProductStuffs;
using AutoMapper;
using Infrastructure.DAL.Entities;

namespace Application.Services.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<OrderItemDto, OrderDetail>()
           .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
           .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
           .ForMember(dest => dest.UnitPrice, opt => opt.Ignore()); // Set explicitly later


        CreateMap<PlaceOrderDto, Order>()
          .ForMember(dest => dest.OrderDetails, opt => opt.Ignore()) // Map separately
          .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.ProductCategory, opt => opt.MapFrom(src => src.ProductCategory));
    }

}
