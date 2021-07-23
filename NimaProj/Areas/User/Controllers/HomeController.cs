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
    [PermissionChecker("user")]
    public class HomeController : Controller
    {
        private Core _core = new Core();
        TblClient SelectUser()
        {
            int userId = Convert.ToInt32(User.Claims.First().Value);
            TblClient selectUser = _core.Client.GetById(userId);
            return selectUser;
        }
        public IActionResult Index()
        {
            List<TblOrder> list = _core.Order.Get(i => i.IsFinaly == true
            && i.ClientId == SelectUser().ClientId,
            orderBy: i => i.OrderByDescending(i => i.OrdeId)).ToList();
            return View(list);
        }
    }
}
