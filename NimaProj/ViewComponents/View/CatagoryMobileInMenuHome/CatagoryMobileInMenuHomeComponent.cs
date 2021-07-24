using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.View.CatagoryMobileInMenuHome
{
    public class CatagoryMobileInMenuHomeComponent : ViewComponent
    {
        private Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<TblCatagory> list = _core.Catagory.Get(i => i.IsBlog == false && i.Parent == null).ToList();
            return await Task.FromResult((IViewComponentResult)View("~/Views" +
                "/Shared/Components/CatagoryMobileInMenuHomeComponent/" +
                "CatagoryMobileInMenuHomeComponent.cshtml"
                , list));
        }
    }
}
