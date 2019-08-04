using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductCatalogAPI.Data;
using ProductCatalogAPI.Domain;
using ProductCatalogAPI.ViewModels;

namespace ProductCatalogAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Catalog")]
    public class CatalogController : Controller
    {
        private readonly CatalogContext catalogContext;
        private readonly IOptionsSnapshot<CatalogSettings> _settings;

        public CatalogController(CatalogContext context, IOptionsSnapshot<CatalogSettings> settings)
        {
            catalogContext = context;
            _settings = settings;
            ((DbContext)catalogContext).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogTypes()
        {
            var items = await catalogContext.CatalogTypes.ToListAsync();
            return Ok(items);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogBrands()
        {
            var items = await catalogContext.CatalogBrands.ToListAsync();
            return Ok(items);
        }


        [HttpGet]
        [Route("items/{id:int}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            if(id <=- 0)
            {
                return BadRequest();
            }

            var item = await catalogContext.CatalogItems.SingleOrDefaultAsync(c => c.Id == id);
            if (item != null)
            {
                item.PictureUrl.Replace("http://SomeURL", _settings.Value.ExternalCatalogBaseUrl);
                return Ok(item);
            }
            else
            {
                return NotFound();
            }
        }

        //Get api/Catalog/items[?pageSize=4&pageIndex=3]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items([FromQuery] int pageSize =6, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await catalogContext.CatalogItems
                .LongCountAsync();

            var itemsOnPage = await catalogContext.CatalogItems
                .OrderBy(c => c.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, ChangeUrlPlaceHolder(itemsOnPage));

            return Ok(model);
        }


        [HttpGet]
        [Route("[action]/withname/{name:minlength(1)}")]
        public async Task<IActionResult> Items(string name, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await catalogContext.CatalogItems
                .Where(w=> w.Name.StartsWith(name))
                .LongCountAsync();

            var itemsOnPage = await catalogContext.CatalogItems
                .Where(w => w.Name.StartsWith(name))
                .OrderBy(c => c.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, ChangeUrlPlaceHolder(itemsOnPage));

            return Ok(model);
        }


        //GET api/Catalog/items/type/1/brand/null[?pageSize=4&pageIndex=0]
        [HttpGet]
        [Route("[action]/type/{catalogTypeId}/brand/{catalogBrandId}")]
        public async Task<IActionResult> Items(int? catalogTypeId,int? CatalogBrandId, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {

            var root = (IQueryable<CatalogItem>)catalogContext.CatalogItems;


            if(catalogTypeId.HasValue)
            {
                root = root.Where(w => w.CatalogTypeId == catalogTypeId.Value);
            }

            if (CatalogBrandId.HasValue)
            {
                root = root.Where(w => w.CatalogBrandId == CatalogBrandId.Value);
            }

            var totalItems = await root
                .LongCountAsync();

            var itemsOnPage = await root
                .OrderBy(c => c.Name)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, ChangeUrlPlaceHolder(itemsOnPage));

            return Ok(model);
        }

        
        [HttpPost]
        [Route("items")]
        public async Task<IActionResult> CreateProduct([FromBody] CatalogItem product)
        {
            var item = new CatalogItem
            {
                CatalogBrandId = product.CatalogBrandId,
                CatalogTypeId = product.CatalogTypeId,
                Descirption = product.Descirption,
                Name = product.Name,
                PictureFileName = product.PictureFileName,
                PictureUrl = product.PictureUrl
            };

            catalogContext.CatalogItems.Add(item);
            await catalogContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemById), new { id = item.Id });

        }

        [HttpPut]
        [Route("items")]
        public async Task<IActionResult> UpdateProduct([FromBody] CatalogItem productToUpdate)
        {
            var catalogItem = await catalogContext.CatalogItems.SingleOrDefaultAsync(w => w.Id == productToUpdate.Id);
            if (catalogItem == null)
            {
                return NotFound();
            }

            catalogItem = productToUpdate;
            catalogContext.Update(catalogItem);
            await catalogContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemById), new { id = productToUpdate.Id });
        }


        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var catalogItem = await catalogContext.CatalogItems.SingleOrDefaultAsync(w => w.Id == id);
            if (catalogItem == null)
            {
                return NotFound();
            }

            catalogContext.Remove(catalogItem);
            await catalogContext.SaveChangesAsync();

            return NoContent();
        }

        private List<CatalogItem> ChangeUrlPlaceHolder(List<CatalogItem> items)
        {
            items.ForEach(i =>
            i.PictureUrl = i.PictureUrl.Replace("http://SomeURL", _settings.Value.ExternalCatalogBaseUrl)
            );

            return items;
        }

    }
}