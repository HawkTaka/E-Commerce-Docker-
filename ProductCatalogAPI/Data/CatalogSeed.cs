using Microsoft.EntityFrameworkCore;
using ProductCatalogAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogAPI.Data
{
    public class CatalogSeed
    {




        public static async Task SeedAsync(CatalogContext context)
        {
            string procider = context.Database.ProviderName;
            var connectionString = context.Database.GetDbConnection().ConnectionString;

            if (!context.CatalogBrands.Any())
            {
                context.CatalogBrands.AddRange(GetPreconfiguredCatalogBrands());
                await context.SaveChangesAsync();
            }

            if (!context.CatalogTypes.Any())
            {
                context.CatalogTypes.AddRange(GetPreconfiguredCatalogTypes());
                await context.SaveChangesAsync();
            }

            if (!context.CatalogItems.Any())
            {
                context.CatalogItems.AddRange(GetPreconfiguredCatalogItems());
                await context.SaveChangesAsync();
            }
        }


        static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
        {
            return new List<CatalogBrand>()
            {
                new CatalogBrand(){Brand="Addidas"},
                new CatalogBrand(){Brand="Puma"},
                new CatalogBrand(){Brand="Slazenger"}

            };
        }

        static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
        {
            return new List<CatalogType>()
            {
                new CatalogType(){Type="Running"},
                new CatalogType(){Type="Basketball"},
                new CatalogType(){Type="Tennis"}
            };
        }

        static IEnumerable<CatalogItem> GetPreconfiguredCatalogItems()
        {
            return new List<CatalogItem>
            {
                //Brand 1
                new CatalogItem(){ CatalogTypeId = 1,CatalogBrandId =1 ,Name="Shoe1",Descirption="Some Description",PictureFileName="1.jpg", Price=150M,PictureUrl="http://SomeURL/api/getImage/1"},
                new CatalogItem(){ CatalogTypeId = 2,CatalogBrandId =1 ,Name="Shoe2",Descirption="Some Description",PictureFileName="2.jpg", Price=120M,PictureUrl="http://SomeURL/api/getImage/2"},
                new CatalogItem(){ CatalogTypeId = 3,CatalogBrandId =1 ,Name="Shoe3",Descirption="Some Description",PictureFileName="3.jpg", Price=130M,PictureUrl="http://SomeURL/api/getImage/3"},

                //Brand 2
                new CatalogItem(){ CatalogTypeId = 1,CatalogBrandId =2 ,Name="Shoe4",Descirption="Some Description",PictureFileName="4.jpg", Price=140M,PictureUrl="http://SomeURL/api/getImage/4"},
                new CatalogItem(){ CatalogTypeId = 2,CatalogBrandId =2 ,Name="Shoe5",Descirption="Some Description",PictureFileName="5.jpg", Price=150M,PictureUrl="http://SomeURL/api/getImage/5"},
                new CatalogItem(){ CatalogTypeId = 3,CatalogBrandId =2 ,Name="Shoe6",Descirption="Some Description",PictureFileName="6.jpg", Price=160M,PictureUrl="http://SomeURL/api/getImage/6"},

                //Brand 3
                new CatalogItem(){ CatalogTypeId = 1,CatalogBrandId =3 ,Name="Shoe7",Descirption="Some Description",PictureFileName="7.jpg", Price=140M,PictureUrl="http://SomeURL/api/getImage/7"},
                new CatalogItem(){ CatalogTypeId = 2,CatalogBrandId =3 ,Name="Shoe8",Descirption="Some Description",PictureFileName="8.jpg", Price=150M,PictureUrl="http://SomeURL/api/getImage/8"},
                new CatalogItem(){ CatalogTypeId = 3,CatalogBrandId =3 ,Name="Shoe9",Descirption="Some Description",PictureFileName="9.jpg", Price=160M,PictureUrl="http://SomeURL/api/getImage/9"}
            };
        }

    }
}
