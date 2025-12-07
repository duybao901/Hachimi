using AutoMapper;
using Contract.Abstractions.Shared;
using Contract.Services.V1.Posts;
using Contract.Services.V1.Posts.ViewModels;
using Query.Domain.Collections;

namespace Query.Application.Mapper;
public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        // CreateMap<Source, Destination>();
        CreateMap<PostProjection, Response.PostResponse>().ReverseMap();

        CreateMap<PostProjection, Response.PostResponse>()
            .ForCtorParam("Id", opt => opt.MapFrom(src => src.DocumentId))
            .ForCtorParam("Title", opt => opt.MapFrom(src => src.Title))
            .ForCtorParam("Content", opt => opt.MapFrom(src => src.Content));

        //CreateMap<PostTagViewModel, TagReadViewModel>().ReverseMap();
    }
}
