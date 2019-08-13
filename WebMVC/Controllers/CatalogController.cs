﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebMvc.ViewModels;
using WebMVC.Services;

namespace WebMVC.Controllers
{
    public class CatalogController : Controller
    {
        private ICatalogService _catalogSvc;

        public CatalogController(ICatalogService catalogSvc) =>
            _catalogSvc = catalogSvc;

        public async Task<IActionResult> Index(int? BrandFilterApplied, int? TypesFilterApplied, int? page)
        {
            int itemsPage = 10;
            var catalog = await _catalogSvc.GetCatalogItems(page ?? 0, itemsPage, BrandFilterApplied, TypesFilterApplied);
            if (catalog != null)
            {
                var vm = new CatalogIndexViewModel()
                {
                    CatalogItems = catalog.Data,
                    Brands = await _catalogSvc.GetBrands(),
                    Types = await _catalogSvc.GetTypes(),
                    BrandFilterApplied = BrandFilterApplied ?? 0,
                    TypesFilterApplied = TypesFilterApplied ?? 0,
                    PaginationInfo = new PaginationInfo()
                    {
                        ActualPage = page ?? 0,
                        ItemsPerPage = itemsPage, //catalog.Data.Count,
                        TotalItems = catalog.Count,
                        TotalPages = (int)Math.Ceiling(((decimal)catalog.Count / itemsPage))
                    }
                };

                vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
                vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

                return View(vm);
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }


    }
}