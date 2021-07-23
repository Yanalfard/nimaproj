﻿using Microsoft.AspNetCore.Mvc;
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
    public class ContactUsController : Controller
    {
        Core _core = new Core();
        public IActionResult Index(int page = 1)
        {
            IEnumerable<TblContactU> contactUs = PagingList.Create(_core.ContactU.Get(), 40, page);
            return View(contactUs);
        }
        public IActionResult Info(int id)
        {
            return ViewComponent("ContactUsInfoAdmin", new { id = id });
        }
    }
}
