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
        public async Task<IActionResult> Index()
        {
            try
            {
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        [Route("Contact")]
        public async Task<IActionResult> Contact()
        {
            try
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
                return await Task.FromResult(View(config));
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
        [Route("About")]
        public async Task<IActionResult> About()
        {
            try
            {
                TblConfig selected = _core.Config.Get(c => c.Key == "DarBareyeMa").FirstOrDefault();
                return await Task.FromResult(View(selected));
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
        [Route("OrderDetails")]
        public async Task<IActionResult> OrderDetails()
        {
            try
            {
                TblConfig selected = _core.Config.Get(c => c.Key == "OrderDetails").FirstOrDefault();
                return await Task.FromResult(View(selected));
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
        [Route("SharyeteErsal")]
        public async Task<IActionResult> SharyeteErsal()
        {
            try
            {
                TblConfig selected = _core.Config.Get(c => c.Key == "SharyeteErsal").FirstOrDefault();
                return await Task.FromResult(View(selected));
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
        [Route("RegularQuestion")]
        public async Task<IActionResult> RegularQuestion()
        {
            try
            {
                List<TblRegularQuestion> selected = _core.RegularQuestion.Get().ToList();
                return await Task.FromResult(View(selected));
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
        [HttpPost]
        [Route("Contact")]
        public async Task<IActionResult> Contact(TblContactU contact)
        {
            try
            {
                TblContactU addContact = new TblContactU();
                addContact.Name = contact.Name;
                addContact.TellNo = contact.TellNo;
                addContact.Message = contact.Message;
                _core.ContactU.Add(addContact);
                _core.Save();
                return await Task.FromResult(Redirect("/Contact?SendMessage=true"));
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        [Route("Error")]
        public async Task<IActionResult> Error()
        {
            try
            {
                return await Task.FromResult(View());
            }
            catch (Exception)
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
    }
}
