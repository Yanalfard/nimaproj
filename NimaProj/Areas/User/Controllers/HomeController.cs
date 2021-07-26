using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using NimaProj.Utilities;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.Areas.User.Controllers
{
    [Area("User")]
    [PermissionChecker("user,admin")]
    public class HomeController : Controller
    {
        private Core _core = new Core();
        public async Task<IActionResult> Index()
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.First().Value);
                List<TblOrder> list = _core.Order.Get(i => i.IsFinaly == true
                && i.ClientId == Main.SelectUser(userId).ClientId,
                orderBy: i => i.OrderByDescending(i => i.OrdeId)).ToList();
                return await Task.FromResult(View(list));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
    }
}
