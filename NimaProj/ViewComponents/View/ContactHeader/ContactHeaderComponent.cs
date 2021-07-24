using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.View.ContactHeader
{
    public class ContactHeaderComponent : ViewComponent
    {
        private Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<TblCatagory> list = _core.Catagory.Get(i => i.IsBlog == false).ToList();



            List<TblConfig> selectedAll = _core.Config.Get().ToList();
            ConfigVm config = new ConfigVm();
            config.Email = selectedAll.Where(c => c.Key == "Email").FirstOrDefault().Value;
            config.TellMobile = selectedAll.Where(c => c.Key == "TellMobile").FirstOrDefault().Value;
            return await Task.FromResult((IViewComponentResult)View("~/Views" +
                "/Shared/Components/ContactHeaderComponent/" +
                "ContactHeaderComponent.cshtml"
                , config));
        }
    }
}
