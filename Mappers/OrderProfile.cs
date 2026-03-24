using AutoMapper;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.customer_id, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.command_id, opt => opt.MapFrom(src => src.Invoice == null ? string.Empty : src.Invoice.Id))
                .ForMember(dest => dest.total_ex_taxes, opt => opt.MapFrom(src => src.TotalExTaxes))
                .ForMember(dest => dest.delivery_fees, opt => opt.MapFrom(src => src.DeliveryFees))
                .ForMember(dest => dest.command, opt => opt.MapFrom(src => src.Invoice))
                .ReverseMap();
        }
    }
}