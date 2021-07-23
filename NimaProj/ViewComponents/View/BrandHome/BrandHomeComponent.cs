using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.View.BrandHome
{
    public class BrandHomeComponent : ViewComponent
    {
        private Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<TblBrand> list = _core.Brand.Get().ToList();
            list.ShuffleList();
            return await Task.FromResult((IViewComponentResult)View("~/Views/Shared/Components/BrandHomeComponent/BrandHomeComponent.cshtml", list));
        }
    }
}
