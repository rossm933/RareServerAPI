using AutoMapper;
using RareServerAPI.Models;
using RareServerAPI.DTOs;
namespace RareServerAPI.Mapper
{
    public class UsersMapper : Profile
    {
        public UsersMapper()
        {

            CreateMap<UserDetailsDTO, Users>();
            // work to be done still

        }
    }
}