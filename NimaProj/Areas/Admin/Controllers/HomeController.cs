using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models;
using Services.Services;
using DataLayer.ViewModels;
using NimaProj.Utilities;

namespace NimaProj.Areas.Admin.Controllers
{
    [Area("Admin")]
    [PermissionChecker("admin")]
    public class HomeController : Controller
    {
        Core _core = new Core();
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                IEnumerable<TblClient> clients = _core.Client.Get(v => v.IsActive);
                IEnumerable<TblOrder> orders = _core.Order.Get();
                AdminDashboardVm adminDashboardVm = new AdminDashboardVm();
                return await Task.FromResult(View(new AdminDashboardVm()
                {
                    CustomersCount = clients.Where(c => c.RoleId == 2).Count(),
                    EmployeesCount = clients.Where(c => c.RoleId == 1).Count(),
                    BrandsCount = _core.Brand.Get().Count(),
                    AllOrderCount = orders.Count(),
                    OrderSucssesCount = orders.Where(o => o.IsFinaly).Count(),
                    OnlineOrderCount = _core.OfflineOrder.Get().Count(),
                    AllPostCount = _core.Blog.Get().Count(),
                    AllRegularQuestionCount = _core.RegularQuestion.Get().Count(),
                }));
            }
            catch
            {
                return await Task.FromResult(Redirect("Error"));
            }
         
        }
    }
}
