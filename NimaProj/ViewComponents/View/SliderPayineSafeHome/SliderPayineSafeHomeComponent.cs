using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.View.BanerPayineSafeHomeComponent
{
    public class SliderPayineSafeHomeComponent : ViewComponent
    {
        private Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            List<TblBannerAndSlide> list = _core.BannerAndSlide.Get(i => i.IsBanner == true && i.ActiveTill >= dt).ToList();
            list.ShuffleList();
            return await Task.FromResult((IViewComponentResult)View("~/Views/Shared/Components/SliderPayineSafeHomeComponent/SliderPayineSafeHomeComponent.cshtml", list.Take(4)));
        }
    }
}
