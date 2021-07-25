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
            List<TblOfflineOrder> list = _core.OfflineOrder.Get(i => i.ClientId == SelectUser().ClientId, orderBy: i => i.OrderByDescending(i => i.DateSubmited)).ToList();
            return View(list);
        }
        public IActionResult OfflineOrder(int id)
        {
            ViewBag.Product = _core.Product.GetById(id).Name;
            return View(new TblOfflineOrder()
            {
                ProductId = id
            });
        }
        [HttpPost]
        public async Task<IActionResult> OfflineOrder(TblOfflineOrder offlineOrder)
        {
            try
            {
                offlineOrder.ClientId = SelectUser().ClientId;
                offlineOrder.DateSubmited = DateTime.Now;
                _core.OfflineOrder.Add(offlineOrder);
                _core.Save();
                return await Task.FromResult(Redirect("/User/OfflineOrder/Index?OfflineOrder=true"));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }

        }
    }
}
