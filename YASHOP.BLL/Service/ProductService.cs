using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;
using YASHOP.DAL.Models;
using YASHOP.DAL.Repository;

namespace YASHOP.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IFileService fileService;

        private readonly IProductRepository productRepository;
        public ProductService(IProductRepository productRepository , IFileService fileService)
        {
            this.productRepository = productRepository;
            this.fileService = fileService;
        }


        public async Task<ProductResponse> CreateProduct(ProductRequest request)
        {
            var product = request.Adapt<Product>();
            if(request.MainImage != null)
            {
                product.MainImage = await fileService.UploadAsync(request.MainImage);
            }

            await productRepository.AddAsync(product);
            return product.Adapt<ProductResponse>();
        }
    }
}
