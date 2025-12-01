//using AutoMapper;
//using Contract.Abstractions.Message;
//using Contract.Abstractions.Shared;
//using Contract.Services.V1.Product;
//using Query.Domain.Abstractions.Repositories;
//using Query.Domain.Collections;

//namespace Query.Application.UseCases.V1.Queries;
//public sealed class GetProductsQueryHandler : IQueryHandler<Contract.Services.V1.Product.Query.GetProductsQuery, List<Response.ProductResponse>>
//{
//    private readonly IMongoRepository<ProductProjection> _productRepository;
//    private readonly IMapper _mapper;

//    public GetProductsQueryHandler(IMongoRepository<ProductProjection> productRepository, IMapper mapper)
//    {
//        _productRepository = productRepository;
//        _mapper = mapper;
//    }

//    public async Task<Result<List<Response.ProductResponse>>> Handle(Contract.Services.V1.Product.Query.GetProductsQuery request, CancellationToken cancellationToken)
//    {
//        var products = await _productRepository.FindAll();
//        var result = new List<Response.ProductResponse>();

//        foreach (var product in products)
//        {
//            result.Add(new Response.ProductResponse(product.DocumentId, product.Name, product.Price, product.Description));
//        }

//        return Result.Success(result);
//    }
//}
