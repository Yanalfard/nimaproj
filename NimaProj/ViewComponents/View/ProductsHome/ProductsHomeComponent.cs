using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.View.ProductsHomeComponent
{
    public class ProductsHomeComponent : ViewComponent
    {
        private Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<TblProduct> list = _core.Product.Get(i => !i.TblSpecialOffers.Any() && i.IsDeleted == false, orderBy: i => i.OrderByDescending(i => i.ProductId)).ToList();
            list.ShuffleList();
            return await Task.FromResult((IViewComponentResult)View("~/Views" +
                "/Shared/Components/ProductsHomeComponent/" +
                "ProductsHomeComponent.cshtml"
                , list));
        }
    }
}
