using AutoMapper;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Query.Domain.Collections;

namespace Query.Application.Mapper;
public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        // CreateMap<Source, Destination>();
        CreateMap<PostProjection, Response.PostResponse>().ReverseMap();

        CreateMap<PageResult<PostProjection>, PageResult<Response.PostResponse>>().ReverseMap();
    }
}
