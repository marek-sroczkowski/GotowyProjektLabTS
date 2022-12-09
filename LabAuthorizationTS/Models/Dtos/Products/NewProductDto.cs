using AutoMapper;
using LabAuthorizationTS.Mapping.Interfaces;
using LabAuthorizationTS.Models.Entities;

namespace LabAuthorizationTS.Models.Dtos.Products
{
    public class NewProductDto : IMap
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public bool OnlyForAdults { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NewProductDto, Product>();
        }
    }
}