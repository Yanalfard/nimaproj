using DataLayer.Models;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.ViewComponents.View.ContactFooter
{
    public class ContactFooterComponent : ViewComponent
    {
        private Core _core = new Core();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<TblConfig> selectedAll = _core.Config.Get().ToList();
            ConfigVm config = new ConfigVm();
            config.Email = selectedAll.Where(c => c.Key == "Email").FirstOrDefault().Value;
            config.Address = selectedAll.Where(c => c.Key == "Address").FirstOrDefault().Value;
            config.TellMobile = selectedAll.Where(c => c.Key == "TellMobile").FirstOrDefault().Value;
            config.TellHome = selectedAll.Where(c => c.Key == "TellHome").FirstOrDefault().Value;
            config.Telegram = selectedAll.Where(c => c.Key == "Telegram").FirstOrDefault().Value;
            config.Inista = selectedAll.Where(c => c.Key == "Inista").FirstOrDefault().Value;
            config.Whatsapp = selectedAll.Where(c => c.Key == "Whatsapp").FirstOrDefault().Value;
            config.IsInista = Convert.ToBoolean(selectedAll.Where(c => c.Key == "IsInista").FirstOrDefault().Value);
            config.IsTelegram = Convert.ToBoolean(selectedAll.Where(c => c.Key == "IsTelegram").FirstOrDefault().Value);
            config.IsWhatsapp = Convert.ToBoolean(selectedAll.Where(c => c.Key == "IsWhatsapp").FirstOrDefault().Value);
            return await Task.FromResult((IViewComponentResult)View("~/Views" +
                "/Shared/Components/ContactFooterComponent/" +
                "ContactFooterComponent.cshtml"
                , config));
        }
    }
}
