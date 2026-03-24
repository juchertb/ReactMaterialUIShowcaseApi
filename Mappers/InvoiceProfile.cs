using AutoMapper;
using ReactMaterialUIShowcaseApi.Dtos;
using ReactMaterialUIShowcaseApi.Entities;

namespace ReactMaterialUIShowcaseApi.Mapping
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            //CreateMap<Invoice, InvoiceDto>().ReverseMap();

            CreateMap<Invoice, InvoiceDto>()
                .ForMember(dest => dest.total_ex_taxes, opt => opt.MapFrom(src => src.TotalExTaxes))
                .ForMember(dest => dest.delivery_fees, opt => opt.MapFrom(src => src.DeliveryFees))
                .ReverseMap();
        }
    }
}