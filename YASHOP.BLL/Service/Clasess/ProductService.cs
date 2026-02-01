using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.BLL.Service.Interfaces;
using YASHOP.DAL.DTO.Request;
using YASHOP.DAL.DTO.Response;
using YASHOP.DAL.Models;
using YASHOP.DAL.Repository;

namespace YASHOP.BLL.Service.Clasess
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
        public async Task<List<ProductResponse>> GetAllProductsForAdminAsync()
        {
            var products = await productRepository.GetAllForAdminAsync();
            
            return products.Adapt<List<ProductResponse>>();
        }
        public async Task<List<ProductUserResponse>> GetAllProductsForUserAsync(string lang ="en")
        {
            var products = await productRepository.GetAllForUserAsync();
            var response = products.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<ProductUserResponse>>();
            return response;

        }


        public async Task<ProductResponse> CreateProduct(ProductRequest request)
        {
            var product = request.Adapt<Product>();
            if(request.MainImage != null)
            {
                product.MainImage = await fileService.UploadAsync(request.MainImage);
            }
            if(request.SubImages != null)
            {
                product.SubImages = new List<ProductImage>();
                foreach (var file in request.SubImages)
                {
                    var imagePath = await fileService.UploadAsync(file);
                    product.SubImages.Add(new ProductImage
                    {
                        ImageName = imagePath

                    });
                }
            }

            await productRepository.AddAsync(product);
            
            return product.Adapt<ProductResponse>();
        }

        public async Task<ProductUserDetails> GetProductDetailsForUserAsync(int id , string lang ="en")
        {
            var product = await productRepository.FindByIdAsync(id);
            var response = product.BuildAdapter().AddParameters("lang",lang).AdaptToType<ProductUserDetails>();
            return response;
        }
    }
}
