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
    public class OfflineOrderController : Controller
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
            List<TblOfflineOrder> list = _core.OfflineOrder.Get(i => i.ClientId == SelectUser().ClientId).ToList();
            return View(list);
        }
    }
}
