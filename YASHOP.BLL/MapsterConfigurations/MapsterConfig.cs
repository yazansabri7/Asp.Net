using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASHOP.DAL.DTO.Response;
using YASHOP.DAL.Models;

namespace YASHOP.BLL.MapsterConfigurations
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister()
        {
            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.CraetedBy, source => source.User.UserName);

            TypeAdapterConfig<Category, CategoryUserResponse>.NewConfig()
                .Map(dest => dest.Name, source => source.Translations
                .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
                .Map(dest => dest.MainImage, source => $"https://localhost:7220/images/{source.MainImage}")
                .Map(dest => dest.CreatedBy, source => source.User.UserName);

            TypeAdapterConfig<Product, ProductUserResponse>.NewConfig()
                .Map(dest => dest.Name, source => source.Translations
                .Where(p => p.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(p => p.Name).FirstOrDefault())
                .Map(dest => dest.MainImage, source => $"https://localhost:7220/images/{source.MainImage}");


            TypeAdapterConfig<Product, ProductUserDetails>.NewConfig()
                .Map(dest => dest.Name, source => source.Translations
                .Where(p => p.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(p => p.Name).FirstOrDefault())
                .Map(dest => dest.MainImage, source => $"https://localhost:7220/images/{source.MainImage}")
                .Map(dest => dest.Description, source => source.Translations
                .Where(p => p.Language == MapContext.Current.Parameters["lang"].ToString())
                .Select(p => p.Description).FirstOrDefault())
                .Map(dest => dest.SubImages, source => source.SubImages.Select(s=> $"https://localhost:7220/images/{s.ImageName}") );







        }
    }
}
