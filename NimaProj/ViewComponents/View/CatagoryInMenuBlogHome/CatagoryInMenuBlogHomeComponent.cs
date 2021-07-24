using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.View.CatagoryInMenuBlogHome
{
    public class CatagoryInMenuBlogHomeComponent : ViewComponent
    {
        private Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<TblCatagory> list = _core.Catagory.Get(i => i.IsBlog == true && i.Parent == null).ToList();
            return await Task.FromResult((IViewComponentResult)View("~/Views" +
                "/Shared/Components/CatagoryInMenuBlogHomeComponent/" +
                "CatagoryInMenuBlogHomeComponent.cshtml"
                , list));
        }
    }
}
