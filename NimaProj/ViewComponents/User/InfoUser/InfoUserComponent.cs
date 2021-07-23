using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace NimaProj.ViewComponents.User.InfoUser
{
    public class InfoUserComponent : ViewComponent
    {
        private Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync(string tell)
        {
            TblClient selected = _core.Client.Get(i => i.TellNo == tell).FirstOrDefault();
            InfoVm info = new InfoVm()
            {
                Name = selected.Name,
                ImageUrl = selected.MainImage
            };
            return await Task.FromResult((IViewComponentResult)View("~/Areas/User/Views/Shared/Components/InfoUserComponent/InfoUserComponent.cshtml", info));
        }
    }
}
