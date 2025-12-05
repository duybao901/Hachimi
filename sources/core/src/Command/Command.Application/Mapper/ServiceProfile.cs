using AutoMapper;
using Command.Domain.Entities;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;

namespace Command.Application.Mapper;
public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        // CreateMap<Source, Destination>();
        CreateMap<Posts, Response.PostResponse>().ReverseMap();

        CreateMap<PageResult<Posts>, PageResult<Response.PostResponse>>().ReverseMap();
    }
}
