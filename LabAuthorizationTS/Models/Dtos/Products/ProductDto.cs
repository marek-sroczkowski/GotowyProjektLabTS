using AutoMapper;
using LabAuthorizationTS.Mapping.Interfaces;
using LabAuthorizationTS.Models.Entities;

namespace LabAuthorizationTS.Models.Dtos.Products
{
    public class ProductDto : IMap
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool OnlyForAdults { get; set; }
        public long UserId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductDto>();
        }
    }
}