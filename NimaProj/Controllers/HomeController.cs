using DataLayer.Models;
using DataLayer.ViewModels;
using DataLayer.ViewModels.View;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using ReflectionIT.Mvc.Paging;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.Controllers
{
    public class HomeController : Controller
    {
        private Core _core = new Core();
        public IActionResult Index()
        {
            return View();
        }
        [Route("Contact")]
        public IActionResult Contact()
        {
            List<TblConfig> selectedAll = _core.Config.Get().ToList();
            ConfigVm config = new ConfigVm();
            config.Address = selectedAll.Where(c => c.Key == "Address").FirstOrDefault().Value;
            config.Email = selectedAll.Where(c => c.Key == "Email").FirstOrDefault().Value;
            config.Inista = selectedAll.Where(c => c.Key == "Inista").FirstOrDefault().Value;
            config.TellHome = selectedAll.Where(c => c.Key == "TellHome").FirstOrDefault().Value;
            config.TellMobile = selectedAll.Where(c => c.Key == "TellMobile").FirstOrDefault().Value;
            config.Whatsapp = selectedAll.Where(c => c.Key == "Whatsapp").FirstOrDefault().Value;
            config.Telegram = selectedAll.Where(c => c.Key == "Telegram").FirstOrDefault().Value;
            return View(config);
        }
        [Route("About")]
        public IActionResult About()
        {
            TblConfig selected = _core.Config.Get(c => c.Key == "DarBareyeMa").FirstOrDefault();
            return View(selected);
        } 
        [Route("OrderDetails")]
        public IActionResult OrderDetails()
        {
            TblConfig selected = _core.Config.Get(c => c.Key == "OrderDetails").FirstOrDefault();
            return View(selected);
        } 
        [Route("SharyeteErsal")]
        public IActionResult SharyeteErsal()
        {
            TblConfig selected = _core.Config.Get(c => c.Key == "SharyeteErsal").FirstOrDefault();
            return View(selected);
        } 
        [Route("RegularQuestion")]
        public IActionResult RegularQuestion()
        {
            List<TblRegularQuestion> selected = _core.RegularQuestion.Get().ToList();
            return View(selected);
        } 
        [HttpPost]
        [Route("Contact")]
        public async Task<IActionResult> Contact(TblContactU contact)
        {
            TblContactU addContact = new TblContactU();
            addContact.Name = contact.Name;
            addContact.TellNo = contact.TellNo;
            addContact.Message = contact.Message;
            _core.ContactU.Add(addContact);
            _core.Save();
            return await Task.FromResult(Redirect("/Contact?SendMessage=true"));
        }

    }
}
