using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.View.PhotoInHome
{
    public class PhotoInHomeComponent : ViewComponent
    {
        private Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<TblConfig> selectedAll = _core.Config.Get().ToList();
            ConfigVm config = new ConfigVm();
            config.BackImgHomeUnder = selectedAll.Where(c => c.Key == "BackImgHomeUnder").FirstOrDefault().Value;
            config.BackImgHomeOn = selectedAll.Where(c => c.Key == "BackImgHomeOn").FirstOrDefault().Value;
            config.BackTextHomeOn = selectedAll.Where(c => c.Key == "BackTextHomeOn").FirstOrDefault().Value;
            return await Task.FromResult((IViewComponentResult)View("~/Views" +
                "/Shared/Components/PhotoInHomeComponent/" +
                "PhotoInHomeComponent.cshtml"
                , config));
        }
    }
}
