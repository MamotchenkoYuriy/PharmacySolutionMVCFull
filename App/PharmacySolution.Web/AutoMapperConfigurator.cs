using AutoMapper;
using PharmacySolution.Core;
using PharmacySolution.Web.Core.Models;

namespace PharmacySolution.Web
{
    public class AutoMapperConfigurator
    {
        public void Configure()
        {
            // Для Pharmacy
            Mapper.CreateMap<Pharmacy, PharmacyViewModel>();
            Mapper.CreateMap<PharmacyViewModel, Pharmacy>();

            // Для Medicament
            Mapper.CreateMap<Medicament, MedicamentViewModel>();
            Mapper.CreateMap<MedicamentViewModel, Medicament>();

            // для MedicamentPriceHistory
            Mapper.CreateMap<MedicamentPriceHistory, MedicamentPriceHistoryViewModel>();
            Mapper.CreateMap<MedicamentPriceHistoryViewModel, MedicamentPriceHistory>();

            // для Order
            Mapper.CreateMap<Order, OrderCreateViewModel>();
            Mapper.CreateMap<OrderCreateViewModel, Order>();
            Mapper.CreateMap<Order, OrderListViewModel>();
            Mapper.CreateMap<OrderListViewModel, Order>();

            // для OrderDetails
            Mapper.CreateMap<OrderDetails, OrderDetailsCreateViewModel>();
            Mapper.CreateMap<OrderDetailsCreateViewModel, OrderDetails>();
            Mapper.CreateMap<OrderDetailsListViewModel, OrderDetails>();
            Mapper.CreateMap<OrderDetails, OrderDetailsListViewModel>();

            // для Storage
            Mapper.CreateMap<Storage, StorageCreateViewModel>();
            Mapper.CreateMap<Storage, StorageViewModel>();
            Mapper.CreateMap<StorageCreateViewModel, Storage>();
            Mapper.CreateMap<StorageViewModel, Storage>();
        }
    }
}