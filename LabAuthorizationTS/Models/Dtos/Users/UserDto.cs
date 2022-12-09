using AutoMapper;
using LabAuthorizationTS.Mapping.Interfaces;
using LabAuthorizationTS.Models.Entities;
using LabAuthorizationTS.Models.Enums;

namespace LabAuthorizationTS.Models.Dtos.Users
{
    public class UserDto : IMap
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime BirthDate { get; set; }
        public UserRole Role { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserDto>();
        }
    }
}