using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Infrastructure
{
    public class ApiPaths
    {
        public static string GetAllCatalogItems(string baseUri, int page, int take,int? brandId, int? typeId)
        {
            var filterQs = "";

            if(brandId.HasValue || typeId.HasValue)
            {
                var brandQs = (brandId.HasValue) ? brandId.Value.ToString() : "null";
                var typeQs = (typeId.HasValue) ? typeId.Value.ToString() : "null";
                filterQs = $"type/{typeQs}/brand/{brandQs}";
            }

            return $"{baseUri}items/{filterQs}?pageIndex={page}&pageSize={take}";
        }

        public static string GetCatalogItem(string baseUri, int Id)
        {
            return $"{baseUri}items/{Id}";
        }

        public static string GetAllTypes(string baseUri)
        {
            return $"{baseUri}catalogTypes";
        }

        public static string GetAllBrands(string baseUri)
        {
            return $"{baseUri}catalogBrands";
        }
    }
}
