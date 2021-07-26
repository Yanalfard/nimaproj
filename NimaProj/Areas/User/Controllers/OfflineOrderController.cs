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
        public async Task<IActionResult> Index()
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.First().Value);
                List<TblOfflineOrder> list = _core.OfflineOrder.Get(i => i.ClientId == Main.SelectUser(userId).ClientId, orderBy: i => i.OrderByDescending(i => i.DateSubmited)).ToList();
                return await Task.FromResult(View(list));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        public async Task<IActionResult> OfflineOrder(int id)
        {
            try
            {
                ViewBag.Product = _core.Product.GetById(id).Name;
                return await Task.FromResult(View(new TblOfflineOrder()
                {
                    ProductId = id
                }));

            }
            catch
            {
                return await Task.FromResult(View("Error"));
            }


        }
        [HttpPost]
        public async Task<IActionResult> OfflineOrder(TblOfflineOrder offlineOrder)
        {
            try
            {
                int userId = Convert.ToInt32(User.Claims.First().Value);
                offlineOrder.ClientId = Main.SelectUser(userId).ClientId;
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
