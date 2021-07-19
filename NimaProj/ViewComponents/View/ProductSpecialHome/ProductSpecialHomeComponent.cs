using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.View.CatagoryHome
{
    public class ProductSpecialHomeComponent : ViewComponent
    {
        private Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<TblSpecialOffer> list = _core.SpecialOffer.Get(i => i.ValidTill > DateTime.Now && i.Product.IsDeleted == false).ToList();
            list.ShuffleList();
            return await Task.FromResult((IViewComponentResult)View("~/Views" +
                "/Shared/Components/ProductSpecialHomeComponent/" +
                "ProductSpecialHomeComponent.cshtml"
                , list));
        }
    }
}