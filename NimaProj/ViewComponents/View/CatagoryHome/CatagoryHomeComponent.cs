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
    public class CatagoryHomeComponent : ViewComponent
    {
        private Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<TblCatagory> list = _core.Catagory.Get(i => i.IsBlog == false && i.ImageUrl != null).ToList();
            list.ShuffleList();
            return await Task.FromResult((IViewComponentResult)View("~/Views/Shared/Components/CatagoryHomeComponent/CatagoryHomeComponent.cshtml", list.Take(3)));
        }
    }
}
