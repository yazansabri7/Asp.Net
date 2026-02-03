using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        public async Task<PaginatedResponse<ProductUserResponse>> GetAllProductsForUserAsync( string lang ="en"
            , int page = 1 
            ,int limit = 3 
            ,string? search = null
            ,int? categoryId = null
            ,decimal? minPrice =null
            ,decimal? maxPrice=null
            ,string? sortBy = null
            ,bool asc =true)
        {
            var query = productRepository.Query();
            if(search is not null)
            {
                //search in name or description
                query = query.Where(p => p.Translations.Any(t => t.Language == lang && t.Name.Contains(search)||t.Description.Contains(search)));
            }
            if(categoryId is not null)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }
            if(minPrice is not null)
            {
                query = query.Where(p => p.Price >= minPrice);
            }
            if(maxPrice is not null)
            {
                query = query.Where(p => p.Price <= maxPrice);
            }
            if(sortBy is not null)
            {
                sortBy = sortBy.ToLower();
                if (sortBy == "price")
                {
                    query = asc ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
                }
                else if (sortBy == "name")
                {
                    query = asc ? query.OrderBy(p=>p.Translations.FirstOrDefault(t=>t.Language==lang).Name)
                        : query.OrderByDescending(p => p.Translations.FirstOrDefault(t => t.Language == lang).Name);
                }
                else if(sortBy == "rate")
                {
                    query = asc ? query.OrderBy(p=>p.Rate) : query.OrderByDescending(p => p.Rate);
                }
            }
            var totalCount = await query.CountAsync();
            query = query.Skip((page - 1) * limit).Take(limit);
            var response = query.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<ProductUserResponse>>();
            return new PaginatedResponse<ProductUserResponse>
            {
                Page = page,
                Limit = limit,
                TotalCount = totalCount,
                Data = response
            };

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
