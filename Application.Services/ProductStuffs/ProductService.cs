using AutoMapper;
using Infrastructure.DAL.Contracts;

namespace Application.Services.ProductStuffs;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public ProductService(
        IProductRepository productRepository,
        IMapper mapper
        )
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> GetAll()
    {
        //assumption this is filtered products that need to be retrieved to server other purposes.
        int[] requiredProductIds = [1, 2, 5, 7, 8, 10];

        var allProduct = await _productRepository.GetAllAsync();
        var listProductNeedForOtherProcess = allProduct.Where(p => requiredProductIds.Contains(p.ProductId));


        var productList = _mapper.Map<IEnumerable<ProductDto>>(allProduct);

        return productList;
    }
}
