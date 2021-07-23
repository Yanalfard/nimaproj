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
        [HttpGet]
        public IActionResult Index()
        {
            AdminDashboardVm adminDashboardVm = new AdminDashboardVm();
            return View(adminDashboardVm);
        }
    }
}
