using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using Services.Services;
using ReflectionIT.Mvc.Paging;
using NimaProj.Utilities;

namespace NimaProj.Areas.Admin.Controllers
{
    [Area("admin")]
    [PermissionChecker("admin")]
    public class OfflineOrderController : Controller
    {
        Core _core = new Core();
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                IEnumerable<TblOfflineOrder> contactUs = PagingList.Create(_core.OfflineOrder.Get(), 40, page);
                return await Task.FromResult(View(contactUs));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
            
        }
        public async Task<IActionResult> Info(int id)
        {
            try
            {
                return await Task.FromResult(ViewComponent("OfflineOrderInfoAdmin", new { id = id }));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
        }
        public void DoneOrder(int id)
        {
            TblOfflineOrder order = _core.OfflineOrder.GetById(id);
            order.IsRead = true;
            _core.OfflineOrder.Update(order);
            _core.Save();
        }
        [HttpPost]
        public void Delete(int id)
        {
            _core.OfflineOrder.DeleteById(id);
            _core.Save();
        }
    }
}
