using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using Services.Services;
using ReflectionIT.Mvc.Paging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace NimaProj.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[PermissionChecker("admin,employee")]
    public class ClientController : Controller
    {
        Core _core = new Core();
        public IActionResult Index(int page = 1, string Name = null, string TelNo = null)
        {
            IEnumerable<TblClient> data = _core.Client.Get();

            if (!string.IsNullOrEmpty(Name))
            {
                data = data.Where(c => c.Name.Contains(Name));
            }

            if (!string.IsNullOrEmpty(TelNo))
            {
                data = data.Where(c => c.TellNo.Contains(TelNo));
            }

            ViewBag.name = Name;
            ViewBag.tellno = TelNo;
            return View(PagingList.Create(data.OrderByDescending(b => b.ClientId), 20, page));


        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return ViewComponent("ClientEditAdmin", new { id = id });
        }

        [HttpPost]
        public string IsActive(int id)
        {
            TblClient client = _core.Client.GetById(id);
            client.IsActive = !client.IsActive;
            _core.Client.Update(client);
            _core.Save();
            return "true";
        }

        [HttpPost]
        public IActionResult Edit(int ClientId, string Name, int RoleId, int Balance)
        {
            TblClient client = _core.Client.GetById(ClientId);
            client.Name = Name;
            client.RoleId = RoleId;
            _core.Client.Update(client);
            _core.Save();
            return Redirect("/Admin/Client");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _core.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
