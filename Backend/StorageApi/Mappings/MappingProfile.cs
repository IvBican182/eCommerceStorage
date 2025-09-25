using AutoMapper;
using StorageApi.Core.Models;
using StorageApi.Core.ModelsDTO; 

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        
        CreateMap<Cart, CartDTO>();
        CreateMap<CartItem, CartItemDto>();
        CreateMap<Order, OrderDTO>();

        CreateMap<AddRemoveCartItemDto, CartItem>();
    }
}