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
using NimaProj.Utilities;

namespace NimaProj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker("admin")]
    public class ClientController : Controller
    {
        Core _core = new Core();
        public async Task<IActionResult> Index(int page = 1, string Name = null, string TelNo = null)
        {
            try
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
                return await Task.FromResult(View(PagingList.Create(data.OrderByDescending(b => b.ClientId), 20, page)));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                return await Task.FromResult(ViewComponent("ClientEditAdmin", new { id = id }));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
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
        public async Task<IActionResult> Edit(int ClientId, string Name, int RoleId, int Balance)
        {
            try
            {

                TblClient client = _core.Client.GetById(ClientId);
                client.Name = Name;
                client.RoleId = RoleId;
                _core.Client.Update(client);
                _core.Save();
                return await Task.FromResult(Redirect("/Admin/Client"));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
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
